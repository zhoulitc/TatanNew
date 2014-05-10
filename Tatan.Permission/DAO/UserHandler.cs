namespace Tatan.Data.DAO
{
    using System;
    using Tatan.Data.DB;
    using Tatan.Common;
    using Tatan.Common.Extension.String.Regex;
    using Tatan.Common.Cryptography;
    using Tatan.Common.Internationalization;

    public static class UserHandler
    {
        private static readonly string _checkSQL = 
            "SELECT ID" + 
            " FROM Users" + 
            " WHERE password = @:password" +
            " AND username = @:username";

        private static readonly string _changedPasswordSQL =
            "UPDATE Users" +
            " SET password = @:new_password" +
            " WHERE id = @:id AND password = @:password";

        private static readonly string _deleteSQL =
            "DELETE FROM Users WHERE id = @:id";

        private static readonly string _updateSQL =
            "UPDATE Users" +
            " SET loginCount = loginCount + 1," +
            " lastLoginTime = @:lastLoginTime," +
            " lastLoginIP = @:lastLoginIP" +   
            " WHERE id = @:id";

        /// <summary>
        /// 检查用户密码是否正确(参数符为@)
        /// </summary>
        /// <param name="username">用户名(可以是邮箱或手机号)</param>
        /// <param name="password">密码(密文)</param>
        /// <returns>用户ID</returns>
        private static string Check(IDataSession session, string username, string password)
        {
            return session.GetScalar<string>(_checkSQL, p => 
            { 
                p["username"] = username;
                p["password"] = password;
            });
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="sessionId">会话标识</param>
        /// <param name="loginIP">登录IP</param>
        /// <returns>用户状态</returns>
        public static UserState Login(string username, string password, string sessionId, string loginIP)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException("username", ExceptionHandler.GetMessage("ArgumentNull"));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password", ExceptionHandler.GetMessage("ArgumentNull"));
            if (string.IsNullOrEmpty(sessionId))
                throw new ArgumentNullException("sessionId", ExceptionHandler.GetMessage("ArgumentNull"));
            if (string.IsNullOrEmpty(loginIP))
                throw new ArgumentNullException("loginIP", ExceptionHandler.GetMessage("ArgumentNull"));

            UserState us = new UserState();
            us.IsLogin = false;
            bool result = DataSessionFactory.UseSession<bool>(sessionId, (session) =>
            {
                string id = Check(session, username, password);
                if (!string.IsNullOrEmpty(id))
                {
                    return session.Execute(_updateSQL, p =>
                    {
                        p["id"] = id;
                        p["lastLoginTime"] = Date.Now;
                        p["lastLoginIP"] = loginIP;

                    }) == 1;
                }
                return false;
            });
            if (result)
            {
                ICipher cipher = CipherFactory.GetCipher("md5");
                us.IsLogin = true;
                us.UserName = username;
                us.LoginOrder = cipher.Encrypt(password);
                us.LoginState = cipher.Encrypt(sessionId);
            }
            return us;
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="function">具体的登出委托</param>
        /// <returns>是否登出成功</returns>
        public static bool Logout(string username, Func<string, bool> function)
        {
            return function(username);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public static bool Delete(string id, string sessionId)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id", ExceptionHandler.GetMessage("ArgumentNull"));
            if (string.IsNullOrEmpty(sessionId))
                throw new ArgumentNullException("sessionId", ExceptionHandler.GetMessage("ArgumentNull"));

            return DataSessionFactory.UseSession<bool>(sessionId, (session) =>
            {
                return session.Execute(_deleteSQL, p => { p["id"] = id; }) == 1;
            });
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="password">老密码</param>
        /// <param name="newPassword">新密码</param>
        /// <param name="sessionId">会话ID</param>
        /// <returns></returns>
        public static bool ChangedPassword(string id, string password, string newPassword, string sessionId)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id", ExceptionHandler.GetMessage("ArgumentNull"));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password", ExceptionHandler.GetMessage("ArgumentNull"));
            if (string.IsNullOrEmpty(sessionId))
                throw new ArgumentNullException("sessionId", ExceptionHandler.GetMessage("ArgumentNull"));
            if (string.IsNullOrEmpty(newPassword))
                throw new ArgumentNullException("newPassword", ExceptionHandler.GetMessage("ArgumentNull"));

            return DataSessionFactory.UseSession<bool>(sessionId, (session) =>
            {
                return session.Execute(_changedPasswordSQL, p => 
                {
                    p["id"] = id;
                    p["password"] = password;
                    p["new_password"] = newPassword;
                }) == 1;
            });
        }

        /// <summary>
        /// 验证用户是否合法
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="function">具体的验证委托</param>
        /// <returns></returns>
        public static bool Authentication(string username, Func<string, bool> function)
        {
            return function(username);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool Register(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException("username", ExceptionHandler.GetMessage("ArgumentNull"));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password", ExceptionHandler.GetMessage("ArgumentNull"));

            Users user = new Users();
            user.Username = username;
            if (username.IsPhone())
                user.Phone = username;
            else if (username.IsEmail())
                user.Email = username;

            user.Password = password;
            user.GroupID = 0; //默认用户组
            user.RegisterTime = Date.Now;
            return DataAccess.Add(user);
        }
    }
}