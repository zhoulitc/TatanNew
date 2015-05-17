namespace Tatan.Common.Net
{
    using System;
    using System.Web;
    using Caching;
    using Exception;

    /// <summary>
    /// Http操作类
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public static class Http
    {
        #region HttpContext

        /// <summary>
        /// 获取或设置HTTP的上下文
        /// </summary>
        public static HttpContext Context
        {
            get
            {
                var context = HttpContext.Current;
                Assert.ArgumentNotNull("context", context);
                return context;
            }
        }

        /// <summary>
        /// 获取Request对象
        /// </summary>
        public static HttpRequest Request
        {
            get { return Context.Request; }
        }

        /// <summary>
        /// 获取Response对象
        /// </summary>
        public static HttpResponse Response
        {
            get { return Context.Response; }
        }
        
        /// <summary>
        /// 获取Server对象
        /// </summary>
        public static HttpServerUtility Server
        {
            get { return Context.Server; }
        }

        #endregion

        #region Cache

        /// <summary>
        /// 获取Cache对象
        /// </summary>
        public static ICache Cache
        {
            get { return Caching.Cache.GetCache(); }
        }

        #endregion

        #region Cookies

        /// <summary>
        /// 获取Cookies对象
        /// </summary>
        public static ICookies Cookies
        {
            get { return InternalCookies.Instance; }
        }

        private sealed class InternalCookies : ICookies
        {
            #region 单例

            private static readonly InternalCookies _instance = new InternalCookies();

            private InternalCookies()
            {
            }

            public static InternalCookies Instance
            {
                get { return _instance; }
            }

            #endregion

            public void Clear()
            {
                Context.Response.Cookies.Clear();
            }

            public int Count
            {
                get
                {
                    return Context.Request.Cookies.Count;
                }
            }

            public string this[string key]
            {
                get //从Request中读取
                {
                    Assert.ArgumentNotNull(nameof(key), key);
                    var cookie = Context.Request.Cookies[key];
                    if (cookie == null)
                        return string.Empty;
                    return cookie.Value;
                }
                set //从Response中写入
                {
                    Assert.ArgumentNotNull(nameof(key), key);
                    var cookie = Context.Response.Cookies[key];
                    if (cookie == null) //Add
                    {
                        if (!string.IsNullOrEmpty(value))
                            Context.Response.Cookies.Add(new HttpCookie(key, value));
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(value)) //Delete
                        {
                            cookie.Expires = DateTime.Now.AddYears(-2);
                        }
                        else //Edit
                        {
                            cookie.Value = value;
                        }
                        Context.Response.Cookies.Set(cookie);
                    }
                }
            }

            /// <summary>
            /// 根据key设置Cookie的过期时间
            /// </summary>
            /// <param name="key">键</param>
            /// <param name="expires">过期时间，单位分钟</param>
            public void SetExpires(string key, double expires)
            {
                Assert.ArgumentNotNull(nameof(key), key);
                var cookie = Context.Response.Cookies[key];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddMinutes(expires);
                    Context.Response.Cookies.Set(cookie);
                }
            }
        }

        #endregion

        #region Session

        /// <summary>
        /// 获取Session对象
        /// </summary>
        public static ISession Session
        {
            get { return InternalSession.Instance; }
        }

        private sealed class InternalSession : ISession
        {
            #region 单例

            private static readonly InternalSession _instance = new InternalSession();

            private InternalSession()
            {
            }

            public static InternalSession Instance
            {
                get { return _instance; }
            }

            #endregion

            public void Abandon()
            {
                Context.Session.Abandon();
            }

            public void Clear()
            {
                lock (Context.Session.SyncRoot)
                {
                    Context.Session.Clear();
                }
            }

            public int Count
            {
                get
                {
                    return Context.Session.Count;
                }
            }

            public string Id
            {
                get
                {
                    return Context.Session.SessionID;
                }
            }

            public bool IsNew
            {
                get
                {
                    return Context.Session.IsNewSession;
                }
            }

            public T Get<T>(string key)
            {
                Assert.ArgumentNotNull(nameof(key), key);
                return (T) Context.Session[key];
            }

            public object this[string key]
            {
                set
                {
                    Assert.ArgumentNotNull(nameof(key), key);
                    var oldValue = Context.Session[key];
                    lock (Context.Session.SyncRoot)
                    {
                        if (oldValue == null) //Add
                        {
                            if (value != null)
                                Context.Session.Add(key, value);
                        }
                        else
                        {
                            if (value == null) //Delete
                            {
                                Context.Session.Remove(key);
                            }
                            else //Edit
                            {
                                Context.Session[key] = value;
                            }
                        }
                    }
                }
            }

            public int Timeout
            {
                get
                {
                    return Context.Session.Timeout;
                }
                set
                {
                    lock (Context.Session.SyncRoot)
                    {
                        Context.Session.Timeout = value;
                    }
                }
            }
        }

        #endregion
    }
}