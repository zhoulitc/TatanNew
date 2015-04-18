using Tatan.Common.Net;

namespace Tatan.Web.User
{
    using System;
    using Common;
    using Common.Extension.String.Codec;
    using Common.Exception;
    using Common.Logging;
    using Data;
    using Net;

    /// <summary>
    /// 
    /// </summary>
    public static class UserHandler
    {
        private const string _userLogin = "UserLogin";

        private const string _checkSql = "SELECT Id FROM UserLogin WHERE Password={0}Password AND Name={0}Name";

        private const string _changedPasswordSql = "UPDATE UserLogin SET Password={0}New_Password WHERE Id={0}Id AND Password={0}Password";

        private const string _logoutSql = "UPDATE UserLogin SET LastLogoutTime={0}LastLogoutTime WHERE Id={0}Id";

        private const string _updateSql = "UPDATE UserLogin SET LoginCount=LoginCount+1, LastLoginTime={0}LastLoginTime, LastLoginIp={0}LastLoginIp WHERE Id={0}Id";

        /// <summary>
        /// 设置数据源，只有设置了源才能做关联操作
        /// </summary>
        public static DataSource Source { private get; set; }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>用户状态</returns>
        public static UserInfo Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
                Assert.ArgumentNotNull("username", username);
            Assert.ArgumentNotNull("password", password);

            var us = new UserInfo {IsLogin = false, Name = username};
            var result = Source.UseSession(Http.Session.Id, session =>
            {
                var flag = false;
                var trans = session.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var id = session.ExecuteScalar<long>(string.Format(_checkSql, Source.Provider.ParameterSymbol), p =>
                {
                    p["Name"] = username;
                    p["Password"] = password.AsEncode("md5");
                });
                if (id > 0)
                {
                    us.Id = id;
                    us.Guid = (id + username).AsEncode("md5");
                    flag = session.Execute(string.Format(_updateSql, Source.Provider.ParameterSymbol), p =>
                    {
                        p["Id"] = id;
                        p["LastLoginTime", typeof(DateTime)] = DateTime.Now;
                        p["LastLoginIP"] = _GetIp();

                    }) == 1;
                }
                if (flag)
                {
                    trans.Commit();
                }
                else
                {
                    trans.Rollback();
                }
                return flag;
            });
            if (!result) return us;
            us.IsLogin = true;
            us.Token = password.AsEncode("md5");
            us.State = Http.Session.Id.AsEncode("md5");

            Http.Cache.Set(us.Guid, us);
            Http.Cookies[us.Guid] = us.ToString();
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
                    string.Format(_logoutSql, Source.Provider.ParameterSymbol), p =>
                            {
                                p["Id"] = user.Id;
                                p["LastLogoutTime", typeof(DateTime)] = Date.Now();
                            }) == 1))
                {
                    Http.Cache.Remove(guid);
                    Http.Cookies[guid] = null;
                    Http.Session[guid] = null;
                    Http.Session.Abandon();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
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
            Assert.ArgumentNotNull("guid", guid);
            Assert.ArgumentNotNull("password", password);
            Assert.ArgumentNotNull("newPassword", newPassword);

            var user = Http.Cache.Get<UserInfo>(guid);
            if (user == null) return false;
            return Source.UseSession(Http.Session.Id, session => session.Execute(
                string.Format(_changedPasswordSql, Source.Provider.ParameterSymbol), p => 
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
            Assert.ArgumentNotNull("Source", Source);
            Assert.ArgumentNotNull("username", username);
            Assert.ArgumentNotNull("password", password);

            var user = new UserLogin { Name = username, Password = password, RegisterTime = DateTime.Now };
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
            catch (Exception ex)
            {
                Log.Warn(ex.Message, ex);
                return string.Empty;
            }
        }
    }
}