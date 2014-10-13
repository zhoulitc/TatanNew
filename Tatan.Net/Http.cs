namespace Tatan.Net
{
    using System;
    using System.Web;
    using Common.Caching;
    using Common.Exception;

    /// <summary>
    ///Http操作类
    /// </summary>
    public static class Http
    {
        private static HttpContext _context;
        private static readonly object _lock = new object();

        static Http()
        {
            _context = HttpContext.Current;
        }

        #region HttpContext
        /// <summary>
        /// 获取或设置HTTP的上下文
        /// </summary>
        public static HttpContext Context
        {
            get
            {
                Assert.ArgumentNotNull("context", _context);
                return _context;
            }
            set
            {
                lock (_lock)
                {
                    _context = value;
                }
            }
        }

        /// <summary>
        /// 获取Request对象
        /// </summary>
        public static HttpRequest Request
        {
            get 
            {
                Assert.ArgumentNotNull("context", _context);
                return _context.Request;
            }
        }

        /// <summary>
        /// 获取Response对象
        /// </summary>
        public static HttpResponse Response
        {
            get
            {
                Assert.ArgumentNotNull("context", _context);
                return _context.Response;
            }
        }
        #endregion

        #region Cache
        /// <summary>
        /// 获取Cache对象
        /// </summary>
        public static ICache Cache
        {
            get { return Caches.WebCache; }
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
            private InternalCookies() { }
            public static InternalCookies Instance { get { return _instance; } }
            #endregion

            public void Clear()
            {
                Assert.ArgumentNotNull("context", _context);
                _context.Response.Cookies.Clear();
            }

            public int Count 
            {
                get
                {
                    Assert.ArgumentNotNull("context", _context);
                    return _context.Request.Cookies.Count;
                } 
            }

            public string this[string key] 
            {
                get //从Request中读取
                {
                    Assert.ArgumentNotNull("key", key);
                    Assert.ArgumentNotNull("context", _context);
                    var cookie = _context.Request.Cookies[key];
                    if (cookie == null)
                        return string.Empty;
                    return cookie.Value;
                }
                set //从Response中写入
                {
                    Assert.ArgumentNotNull("key", key);
                    Assert.ArgumentNotNull("context", _context);
                    var cookie = _context.Response.Cookies[key];
                    if (cookie == null) //Add
                    {
                        if (!string.IsNullOrEmpty(value))
                            _context.Response.Cookies.Add(new HttpCookie(key, value));
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
                        _context.Response.Cookies.Set(cookie);
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
                Assert.ArgumentNotNull("key", key);
                Assert.ArgumentNotNull("context", _context);
                var cookie = _context.Response.Cookies[key];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddMinutes(expires);
                    _context.Response.Cookies.Set(cookie);
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
            private InternalSession() { }
            public static InternalSession Instance { get { return _instance; } }
            #endregion

            public void Abandon()
            {
                Assert.ArgumentNotNull("context", _context);
                _context.Session.Abandon();
            }

            public void Clear()
            {
                Assert.ArgumentNotNull("context", _context);
                lock (_context.Session.SyncRoot)
                {
                    _context.Session.Clear();
                }
            }

            public int Count
            {
                get
                {
                    Assert.ArgumentNotNull("context", _context);
                    return _context.Session.Count;
                }
            }

            public string Id
            {
                get
                {
                    Assert.ArgumentNotNull("context", _context);
                    return _context.Session.SessionID;
                }
            }

            public bool IsNew
            {
                get
                {
                    Assert.ArgumentNotNull("context", _context);
                    return _context.Session.IsNewSession;
                }
            }

            public T Get<T>(string key)
            {
                Assert.ArgumentNotNull("key", key);
                Assert.ArgumentNotNull("context", _context);
                return (T)_context.Session[key];
            }

            public object this[string key]
            {
                set
                {
                    Assert.ArgumentNotNull("key", key);
                    Assert.ArgumentNotNull("context", _context);
                    var oldValue = _context.Session[key];
                    lock (_context.Session.SyncRoot)
                    {
                        if (oldValue == null) //Add
                        {
                            if (value != null)
                                _context.Session.Add(key, value);
                        }
                        else
                        {
                            if (value == null) //Delete
                            {
                                _context.Session.Remove(key);
                            }
                            else //Edit
                            {
                                _context.Session[key] = value;
                            }
                        }
                    }
                }
            }

            public int Timeout
            {
                get
                {
                    Assert.ArgumentNotNull("context", _context);
                    return _context.Session.Timeout;
                }
                set
                {
                    Assert.ArgumentNotNull("context", _context);
                    lock (_context.Session.SyncRoot)
                    {
                        _context.Session.Timeout = value;
                    }
                }
            }
        }
        #endregion
    }
}