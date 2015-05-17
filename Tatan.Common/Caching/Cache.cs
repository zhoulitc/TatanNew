namespace Tatan.Common.Caching
{
    using System;
    using Extension.Reflect;

    /// <summary>
    /// 缓存操作类
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public static class Cache
    {
        /// <summary>
        /// 缓存超时时间，默认为20分钟
        /// </summary>
        public static readonly TimeSpan Timeout = new TimeSpan(0, 20, 0);

        /// <summary>
        /// 清除所有缓存对象
        /// </summary>
        public static void Clear()
        {
            _cache.Clear();
        }

        /// <summary>
        /// 缓存是否包含指定项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Contains(string key)
        {
            return _cache.Contains(key);
        }

        /// <summary>
        /// 获取缓存的对象数量
        /// </summary>
        public static int Count
        {
            get { return _cache.Count; }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        /// <summary>
        /// 设置缓存，使用默认的相对过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="removeCallback">自动/手动移除时的回调函数</param>
        public static void Set<T>(string key, T value, Action<string, object> removeCallback = null)
        {
            _cache.Set(key, value, removeCallback);
        }

        /// <summary>
        /// 设置缓存，同时包含过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeout">相对过期时间，指从最后一次从缓存中调用起，经过多长时间没调用就自动移除</param>
        /// <param name="removeCallback">自动/手动移除时的回调函数</param>
        public static void Set<T>(string key, T value, TimeSpan timeout, Action<string, object> removeCallback = null)
        {
            _cache.Set(key, value, timeout, removeCallback);
        }

        /// <summary>
        /// 移除指定缓存项，若存在回调函数，则执行回调
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            _cache.Remove(key);
        }

        /// <summary>
        /// 调用具体子类的扩展方法
        /// </summary>
        /// <param name="method"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static object InvokeExtension(string method, params object[] arguments)
        {
            return _cache.InvokeDeclaredOnly(method, arguments);
        }

        /// <summary>
        /// 获取Web服务器缓存对象
        /// </summary>
        private static ICache _cache;

        /// <summary>
        /// 注册并替换当前缓存
        /// </summary>
        /// <param name="cache"></param>
        internal static void Regsiter(ICache cache)
        {
            _cache = cache ?? InternalWebCache.Instance;
        }

        internal static ICache GetCache()
        {
            return _cache;
        }

        static Cache()
        {
            _cache = InternalWebCache.Instance;
        }
    }
}