namespace Tatan.Common.Net
{
    using System.Web;
    using Component;
    using Exception;

    /// <summary>
    /// 自定义会话适配接口
    /// </summary>
    public class SessionAdapter : IAdaptable
    {
        /// <summary>
        /// 
        /// </summary>
        public SessionAdapter(ISession session = null)
        {
            Http.Session = (session ?? InternalSession.Instance);
        }

        #region WebSession

        private sealed class InternalSession : ISession
        {
            #region 单例

            private static readonly InternalSession _instance = new InternalSession();

            private InternalSession() { }

            public static InternalSession Instance => _instance;

            #endregion

            public void Abandon() => HttpContext.Current?.Session.Abandon();

            public void Clear()
            {
                HttpContext.Current?.Session.Clear();
            }

            public int Count => HttpContext.Current.Session.Count;

            public string Id => HttpContext.Current?.Session.SessionID;

            public bool IsNew => HttpContext.Current.Session.IsNewSession;

            public T Get<T>(string key)
            {
                Assert.ArgumentNotNull(nameof(key), key);
                return (T)HttpContext.Current?.Session[key];
            }

            public object this[string key]
            {
                set
                {
                    Assert.ArgumentNotNull(nameof(key), key);
                    var context = HttpContext.Current;
                    Assert.ArgumentNotNull(nameof(context), context);
                    var oldValue = HttpContext.Current.Session[key];
                    lock (context.Session.SyncRoot)
                    {
                        if (oldValue == null) //Add
                        {
                            if (value != null)
                                context.Session.Add(key, value);
                        }
                        else
                        {
                            if (value == null) //Delete
                            {
                                context.Session.Remove(key);
                            }
                            else //Edit
                            {
                                context.Session[key] = value;
                            }
                        }
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