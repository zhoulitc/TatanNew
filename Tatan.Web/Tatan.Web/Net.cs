namespace Tatan.Web
{
    using System;
    using System.Collections;
    using System.Web;
    using Tatan.Common.Internationalization;

    /// <summary>
    /// 网络操作类
    /// </summary>
    public static class Net
    {
        private static HttpContext _context;
        static Net()
        {
            _context = HttpContext.Current;
        }
        public static void SetContext(HttpContext context)
        {
            lock (_context)
            {
                _context = context;
            }
        }

        public static HttpContext Context
        {
            get
            {
                return _context;
            }
        }

        public static HttpRequest Request
        {
            get 
            {
                return _context.Request;
            }
        }

        public static HttpResponse Response
        {
            get
            {
                return _context.Response;
            }
        }

        #region Cache
        public static ICache Cache
        {
            get { return _Cache.Instance; }
        }

        private sealed class _Cache : ICache
        {
            #region 单例
            private static readonly _Cache _instance = new _Cache();
            private _Cache() { }
            public static _Cache Instance { get { return _instance; } }
            #endregion

            public void Clear()
            {
                IDictionaryEnumerator enumerator = HttpRuntime.Cache.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    HttpRuntime.Cache.Remove(enumerator.Key.ToString());
                }
            }

            public int Count { get { return HttpRuntime.Cache.Count; } }

            public T Get<T>(string key)
            {
                if (string.IsNullOrEmpty(key))
                    throw new ArgumentNullException("key", ExceptionMessage.Instance.ArgumentNull);
                return (T)HttpRuntime.Cache.Get(key);
            }

            public object this[string key]
            {
                set
                {
                    if (string.IsNullOrEmpty(key))
                        throw new ArgumentNullException("key", ExceptionMessage.Instance.ArgumentNull);
                    if (value == null)
                        HttpRuntime.Cache.Remove(key);
                    HttpRuntime.Cache.Insert(key, value);
                }
            }

            /// <summary>
            /// 设置一个缓存，包含过期时间和最后一次访问到过期时间的间隔
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <param name="absoluteExpiration">过期时间</param>
            /// <param name="slidingExpiration">最后一次访问到过期时间的间隔</param>
            public object this[string key, DateTime absoluteExpiration, TimeSpan slidingExpiration]
            {
                set
                {
                    if (string.IsNullOrEmpty(key))
                        throw new ArgumentNullException("key", ExceptionMessage.Instance.ArgumentNull);
                    if (value == null)
                        HttpRuntime.Cache.Remove(key);
                    HttpRuntime.Cache.Insert(key, value, null, absoluteExpiration, slidingExpiration);
                }
            }
        }
        #endregion

        #region Cookies
        public static ICookies Cookies
        {
            get { return _Cookies.Instance; }
        }

        private sealed class _Cookies : ICookies
        {
            #region 单例
            private static readonly _Cookies _instance = new _Cookies();
            private _Cookies() { }
            public static _Cookies Instance { get { return _instance; } }
            #endregion

            public void Clear()
            {
                _context.Response.Cookies.Clear();
            }

            public int Count { get { return _context.Request.Cookies.Count; } }

            public string this[string key] 
            {
                get //从Request中读取
                {
                    if (string.IsNullOrEmpty(key))
                        throw new ArgumentNullException("key", ExceptionMessage.Instance.ArgumentNull);
                    HttpCookie cookie = _context.Request.Cookies[key];
                    if (cookie == null)
                        return string.Empty;
                    return cookie.Value;
                }
                set //从Response中写入
                {
                    if (string.IsNullOrEmpty(key))
                        throw new ArgumentNullException("key", ExceptionMessage.Instance.ArgumentNull);
                    HttpCookie cookie = _context.Response.Cookies[key];
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
                if (string.IsNullOrEmpty(key))
                    throw new ArgumentNullException("key", ExceptionMessage.Instance.ArgumentNull);
                HttpCookie cookie = _context.Response.Cookies[key];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddMinutes(expires);
                    _context.Response.Cookies.Set(cookie);
                }
            }
        }
        #endregion

        #region Session
        public static ISession Session
        {
            get { return _Session.Instance; }
        }

        private sealed class _Session : ISession
        {
            #region 单例
            private static readonly _Session _instance = new _Session();
            private _Session() { }
            public static _Session Instance { get { return _instance; } }
            #endregion

            public void Abandon()
            {
                _context.Session.Abandon();
            }

            public void Clear()
            {
                lock (_context.Session.SyncRoot)
                {
                    _context.Session.Clear();
                }
            }

            public int Count
            {
                get
                {
                    return _context.Session.Count;
                }
            }

            /// <summary>
            /// 获取Session唯一标识符
            /// </summary>
            public string ID
            {
                get
                {
                    return _context.Session.SessionID;
                }
            }

            public bool IsNew
            {
                get
                {
                    return _context.Session.IsNewSession;
                }
            }

            public T Get<T>(string key)
            {
                if (string.IsNullOrEmpty(key))
                    throw new ArgumentNullException("key", ExceptionMessage.Instance.ArgumentNull);
                return (T)_context.Session[key];
            }

            public object this[string key]
            {
                set
                {
                    if (string.IsNullOrEmpty(key))
                        throw new ArgumentNullException("key", ExceptionMessage.Instance.ArgumentNull);
                    object oldValue = _context.Session[key];
                    if (oldValue == null) //Add
                    {
                        if (value != null)
                            _context.Session.Add(key, value);
                    }
                    else
                    {
                        lock (_context.Session.SyncRoot)
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
                    return _context.Session.Timeout;
                }
                set
                {
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