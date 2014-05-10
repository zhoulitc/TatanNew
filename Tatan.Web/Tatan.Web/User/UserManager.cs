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
            UserState us = UserHandler.Login(username, password, Net.Session.ID, _GetLoginIP());
            if (us.IsLogin)
            {
                Net.Cache[username] = us;
                Net.Cookies["username"] = us.UserName;
                Net.Cookies["loginOrder"] = us.LoginOrder;
                Net.Cookies["loginState"] = us.LoginState;
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
                    Net.Cache[name] = null;
                    Net.Cookies["username"] = null;
                    Net.Cookies["loginOrder"] = null;
                    Net.Cookies["loginState"] = null;
                    Net.Session.Abandon();
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
            return UserHandler.ChangedPassword(id, password, newPassword, Net.Session.ID);
        }

        public static bool Delete(string id)
        {
            return UserHandler.Delete(id, Net.Session.ID);
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
                UserState us = Net.Cache.Get<UserState>(username);
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
            string ip = string.Empty;
            try
            {
                if (Net.Context == null || 
                    Net.Request == null ||
                    Net.Request.ServerVariables == null)
                    return string.Empty;

                //CDN加速后取到的IP simone 090805
                ip = Net.Request.Headers["Cdn-Src-Ip"];
                if (!string.IsNullOrEmpty(ip))
                    return ip;

                ip = Net.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!String.IsNullOrEmpty(ip))
                    return ip;

                if (Net.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    ip = Net.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (String.IsNullOrEmpty(ip))
                        ip = Net.Request.ServerVariables["REMOTE_ADDR"];
                }
                else
                    ip = Net.Request.ServerVariables["REMOTE_ADDR"];

                if (string.Compare(ip, "unknown", true) == 0)
                    return Net.Request.UserHostAddress;
                return ip;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
    }
}