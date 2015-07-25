namespace Tatan.Common.Net
{
    using System.Web;
    using Component;
    using Exception;
    using System;

    /// <summary>
    /// 自定义Cookies适配接口
    /// </summary>
    public class CookiesAdapter : IAdaptable
    {
        /// <summary>
        /// 
        /// </summary>
        public CookiesAdapter(ICookies cookies = null)
        {
            Http.Cookies = (cookies ?? InternalCookies.Instance);
        }

        #region WebSession

        private sealed class InternalCookies : ICookies
        {
            #region 单例

            private static readonly InternalCookies _instance = new InternalCookies();

            private InternalCookies() { }

            public static InternalCookies Instance => _instance;

            #endregion

            public void Clear() => HttpContext.Current?.Response.Cookies.Clear();

            public int Count => HttpContext.Current.Request.Cookies.Count;

            public string this[string key]
            {
                get //从Request中读取
                {
                    Assert.ArgumentNotNull(nameof(key), key);
                    var cookie = HttpContext.Current?.Request.Cookies[key];
                    if (cookie == null)
                        return string.Empty;
                    return cookie.Value;
                }
                set //从Response中写入
                {
                    Assert.ArgumentNotNull(nameof(key), key);
                    var context = HttpContext.Current;
                    var cookie = context?.Response.Cookies[key];
                    if (cookie == null) //Add
                    {
                        if (!string.IsNullOrEmpty(value))
                            context?.Response.Cookies.Add(new HttpCookie(key, value));
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
                        context?.Response.Cookies.Set(cookie);
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
                var context = HttpContext.Current;
                var cookie = context?.Response.Cookies[key];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddMinutes(expires);
                    context?.Response.Cookies.Set(cookie);
                }
            }
        }

        #endregion
    }
}