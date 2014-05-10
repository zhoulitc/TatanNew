namespace Tatan.Web
{
    using System;
    using Tatan.Data.DAO;

    public static class UserManager
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static UserState Login(string username, string password)
        {
            UserState us = UserHandler.Login(username, password, Http.Session.Id, _GetLoginIP());
            if (us.IsLogin)
            {
                Http.Cache[username] = us;
                Http.Cookies["username"] = us.UserName;
                Http.Cookies["loginOrder"] = us.LoginOrder;
                Http.Cookies["loginState"] = us.LoginState;
            }
            return us;
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static bool Logout(string username)
        {
            return UserHandler.Logout(username, (name) => {
                try
                {
                    //UserState us = Web.Cache.Get<UserState>(value);
                    Http.Cache[name] = null;
                    Http.Cookies["username"] = null;
                    Http.Cookies["loginOrder"] = null;
                    Http.Cookies["loginState"] = null;
                    Http.Session.Abandon();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        public static bool ChangedPassword(string id, string password, string newPassword)
        {
            return UserHandler.ChangedPassword(id, password, newPassword, Http.Session.Id);
        }

        public static bool Delete(string id)
        {
            return UserHandler.Delete(id, Http.Session.Id);
        }

        /// <summary>
        /// 验证用户是否合法
        /// </summary>
        /// <param name="username"></param>
        /// <param name="loginOrder"></param>
        /// <param name="loginState"></param>
        /// <returns></returns>
        public static bool Auth(string username, string loginOrder, string loginState)
        {
            return UserHandler.Authentication(username, (name) =>
            {
                var us = Http.Cache.Get<UserState>(username);
                return us.IsLogin && us.UserName == username && us.LoginOrder == loginOrder && us.LoginState == loginState;
            });
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool Register(string username, string password)
        {
            return UserHandler.Register(username, password);
        }

        private static string _GetLoginIP()
        {
            var ip = string.Empty;
            try
            {
                if (Http.Context == null || 
                    Http.Request == null ||
                    Http.Request.ServerVariables == null)
                    return string.Empty;

                //CDN加速后取到的IP simone 090805
                ip = Http.Request.Headers["Cdn-Src-Ip"];
                if (!string.IsNullOrEmpty(ip))
                    return ip;

                ip = Http.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!String.IsNullOrEmpty(ip))
                    return ip;

                if (Http.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    ip = Http.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (String.IsNullOrEmpty(ip))
                        ip = Http.Request.ServerVariables["REMOTE_ADDR"];
                }
                else
                    ip = Http.Request.ServerVariables["REMOTE_ADDR"];

                if (string.Compare(ip, "unknown", true) == 0)
                    return Http.Request.UserHostAddress;
                return ip;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
    }
}