namespace Tatan.Web
{
    using System;
    using Common;

    /// <summary>
    /// 服务器缓存接口
    /// </summary>
    public interface ICache : IClearable, ICountable
    {
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        object this[string key] { set; }

        /// <summary>
        /// 设置缓存，同时包含过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="slidingExpiration"></param>
        object this[string key, DateTime absoluteExpiration, TimeSpan slidingExpiration] { set; }
    }
}