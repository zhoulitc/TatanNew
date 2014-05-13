namespace Tatan.Web.User
{
    using System;
    using Common;
    using Common.Cryptography;
    using Common.Exception;
    using Common.Extension.String.Convert;
    using Data;

    /// <summary>
    /// 
    /// </summary>
    public static class UserHandler
    {
        private const string _userLogin = "UserLogin";

        private const string _checkSql = "SELECT Id FROM UserLogin WHERE Password={0}Password AND Name={1}Name";

        private const string _changedPasswordSql = "UPDATE UserLogin SET Password={0}New_Password WHERE Id={1}Id AND Password={2}Password";

        private const string _logoutSql = "UPDATE UserLogin SET LastLoginTime={0}LastLoginTime WHERE Id={1}Id";

        private const string _updateSql = "UPDATE UserLogin SET LoginCount=LoginCount+1, LastLoginTime={0}LastLoginTime, LastLoginIp={1}LastLoginIp WHERE Id={2}Id";

        /// <summary>
        /// 设置数据源，只有设置了源才能做关联操作
        /// </summary>
        public static DataSource Source { private get; set; }

        /// <summary>
        /// 检查用户密码是否正确(参数符为@)
        /// </summary>
        /// <param name="username">用户名(可以是邮箱或手机号)</param>
        /// <param name="password">密码(密文)</param>
        /// <returns>用户ID</returns>
        private static string Check(string username, string password)
        {
            return Source.UseSession(_userLogin, session => session.GetScalar<string>(
                string.Format(_checkSql, 
                    Source.Provider.ParameterSymbol, 
                    Source.Provider.ParameterSymbol), p =>
            {
                p["Name"] = username;
                p["Password"] = password;
            }));
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>用户状态</returns>
        public static UserInfo Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
                ExceptionHandler.ArgumentNull("username", username);
            ExceptionHandler.ArgumentNull("password", password);

            var us = new UserInfo {IsLogin = false, Name = username};
            var cipher = CipherFactory.GetCipher("md5");
            var result = Source.UseSession(Http.Session.Id, session =>
            {
                var id = Check(username, password);
                if (!string.IsNullOrEmpty(id))
                {
                    us.Id = id.AsInt();
                    us.Guid = cipher.Encrypt(id + username);
                    return session.Execute(string.Format(_updateSql, 
                        Source.Provider.ParameterSymbol, 
                        Source.Provider.ParameterSymbol, 
                        Source.Provider.ParameterSymbol), p =>
                    {
                        p["Id"] = id;
                        p["LastLoginTime", DataType.Date] = Date.Now;
                        p["LastLoginIP"] = _GetIp();

                    }) == 1;
                }
                return false;
            });
            if (!result) return us;
            us.IsLogin = true;
            us.Token = cipher.Encrypt(password);
            us.State = cipher.Encrypt(Http.Session.Id);

            Http.Cookies[us.Guid] = us.ToString();
            Http.Cache[us.Guid] = us;
            Http.Session[us.Guid] = us;
            return us;
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="guid">用户guid</param>
        /// <returns>是否登出成功</returns>
        public static bool Logout(string guid)
        {
            if (string.IsNullOrEmpty(guid)) return false;
            var user = Http.Cache.Get<UserInfo>(guid);
            if (user == null) return false;
            try
            {
                if (Source.UseSession(Http.Session.Id, session => session.Execute(
                    string.Format(_logoutSql,
                            Source.Provider.ParameterSymbol,
                            Source.Provider.ParameterSymbol), p =>
                            {
                                p["Id"] = user.Id;
                                p["LastLogoutTime", DataType.Date] = Date.Now;
                            }) == 1))
                {
                    Http.Cache[guid] = null;
                    Http.Cookies[guid] = null;
                    Http.Session[guid] = null;
                    Http.Session.Abandon();
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="guid">用户guid</param>
        /// <param name="password">老密码</param>
        /// <param name="newPassword">新密码</param>
        /// <returns></returns>
        public static bool ChangedPassword(string guid, string password, string newPassword)
        {
            ExceptionHandler.ArgumentNull("guid", guid);
            ExceptionHandler.ArgumentNull("password", password);
            ExceptionHandler.ArgumentNull("newPassword", newPassword);

            var user = Http.Cache.Get<UserInfo>(guid);
            if (user == null) return false;
            return Source.UseSession(Http.Session.Id, session => session.Execute(
                string.Format(_changedPasswordSql,
                        Source.Provider.ParameterSymbol,
                        Source.Provider.ParameterSymbol,
                        Source.Provider.ParameterSymbol), p => 
            {
                p["Name"] = user.Name;
                p["Password"] = password;
                p["New_Password"] = newPassword;
            }) == 1);
        }

        /// <summary>
        /// 验证用户是否合法
        /// </summary>
        /// <param name="guid">用户guid</param>
        /// <param name="username">用户名</param>
        /// <param name="token">用户token</param>
        /// <param name="state">用户状态</param>
        /// <returns></returns>
        public static bool Auth(string guid, string username, string token, string state)
        {
            var us = Http.Cache.Get<UserInfo>(guid);
            if (us == null) return false;
            return us.IsLogin && us.Name == username && us.Token == token && us.State == state;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool Register(string username, string password)
        {
            ExceptionHandler.ArgumentNull("Source", Source);
            ExceptionHandler.ArgumentNull("username", username);
            ExceptionHandler.ArgumentNull("password", password);

            var user = new UserLogin(1) { Name = username, Password = password, RegisterTime = DateTime.Now };
            return Source.Tables[_userLogin].Insert(user);
        }

        private static string _GetIp()
        {
            try
            {
                if (Http.Context == null ||
                    Http.Request == null)
                    return string.Empty;

                //CDN加速后取到的IP simone 090805
                var ip = Http.Request.Headers["Cdn-Src-Ip"];
                if (!string.IsNullOrEmpty(ip))
                    return ip;

                ip = Http.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ip))
                    return ip;

                if (Http.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    ip = Http.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (string.IsNullOrEmpty(ip))
                        ip = Http.Request.ServerVariables["REMOTE_ADDR"];
                }
                else
                    ip = Http.Request.ServerVariables["REMOTE_ADDR"];

                return string.Compare(ip, "unknown", true) == 0 ? Http.Request.UserHostAddress : ip;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}