namespace Tatan.Common.Net
{
    using System.Web;
    using Exception;
    using System;

    /// <summary>
    /// 自定义会话（缓存）适配接口
    /// </summary>
    public class SessionCacheAdapter : SessionAdapter
    {
        /// <summary>
        /// 
        /// </summary>
        public SessionCacheAdapter() : base(InternalSessionCache.Instance)
        {
        }

        #region WebSession

        private sealed class InternalSessionCache : ISession
        {
            #region 单例

            private static readonly InternalSessionCache _instance = new InternalSessionCache();

            private InternalSessionCache() { }

            public static InternalSessionCache Instance => _instance;

            #endregion

            public void Abandon() => Http.Cache.Remove(Id);

            public string Id => HttpContext.Current?.Session.SessionID;

            public bool IsNew => HttpContext.Current.Session.IsNewSession;

            public T Get<T>(string key)
            {
                Assert.ArgumentNotNull(nameof(key), key);
                return Http.Cache.Get<T>(Id, key);
            }

            public object this[string key]
            {
                set
                {
                    Assert.ArgumentNotNull(nameof(key), key);
                    var oldValue = Get<object>(key);

                    if (oldValue != null) //Add
                    {
                        if (value != null)
                            Http.Cache.Set(Id, key, value, new TimeSpan(0, Timeout, 0));
                    }
                    else
                    {
                        if (value == null) //Delete
                            Http.Cache.Remove(Id, key);
                        else //Edit
                            Http.Cache.Set(Id, key, value, new TimeSpan(0, Timeout, 0));
                    }
                }
            }

            public int Timeout
            {
                get
                {
                    return HttpContext.Current.Session.Timeout;
                }
                set
                {
                    HttpContext.Current.Session.Timeout = value;
                }
            }
        }

        #endregion
    }
}