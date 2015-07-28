namespace Tatan.Common.Net
{
    using System;
    using System.Web;
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
        /// <para>异步调用时，上下文为null</para>
        /// </summary>
        public static HttpContextBase Context
        {
            get
            {
                var context = HttpContext.Current;
                Assert.ArgumentNotNull(nameof(context), context);
                return new HttpContextWrapper(context);
            }
        }

        /// <summary>
        /// 获取Request对象
        /// </summary>
        public static HttpRequestBase Request => Context.Request;

        /// <summary>
        /// 获取Response对象
        /// </summary>
        public static HttpResponseBase Response => Context.Response;

        /// <summary>
        /// 获取Server对象
        /// </summary>
        public static HttpServerUtilityBase Server => Context.Server;

        #endregion

        /// <summary>
        /// 获取Cache对象
        /// </summary>
        public static ICache Cache { get; internal set; }

        /// <summary>
        /// 获取Cookies对象
        /// </summary>
        public static ICookies Cookies { get; internal set; }

        /// <summary>
        /// 获取Session对象
        /// </summary>
        public static ISession Session { get; internal set; }
    }
}