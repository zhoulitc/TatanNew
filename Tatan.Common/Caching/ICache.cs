namespace Tatan.Common.Caching
{
    using System;

    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICache : IClearable, ICountable
    {
        /// <summary>
        /// 缓存是否包含指定项
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        bool Contains(string key);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// 设置缓存，使用默认的相对过期时间
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="removeCallback">自动/手动移除时的回调函数</param>
        void Set<T>(string key, T value, Action<string, object> removeCallback = null);

        /// <summary>
        /// 设置缓存，同时包含过期时间
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expire">相对过期时间，指从最后一次从缓存中调用起，经过多长时间没调用就自动移除</param>
        /// <param name="removeCallback">自动/手动移除时的回调函数</param>
        void Set<T>(string key, T value, TimeSpan expire, Action<string, object> removeCallback = null);

        /// <summary>
        /// 移除指定缓存项，若存在回调函数，则执行回调
        /// </summary>
        /// <param name="key">键</param>
        void Remove(string key);
    }
}