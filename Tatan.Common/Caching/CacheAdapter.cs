namespace Tatan.Common.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Web;
    using System.Web.Caching;
    using Component;
    using Exception;

    /// <summary>
    /// 通用缓存可适配接口
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public class CacheAdapter : IAdaptable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cache"></param>
        public CacheAdapter(ICache cache = null)
        {
            Cache.Regsiter(cache);
        }
    }

    #region WebCache

    internal sealed class InternalWebCache : ICache
    {
        #region 单例

        private static readonly InternalWebCache _instance = new InternalWebCache();
        private readonly System.Web.Caching.Cache _cache;

        private InternalWebCache()
        {
            _cache = HttpRuntime.Cache;
        }

        public static InternalWebCache Instance
        {
            get { return _instance; }
        }

        #endregion

        public void Clear()
        {
            if (_cache == null)
                return;

            var enumerator = _cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                _cache.Remove(enumerator.Key.ToString());
            }
        }

        public int Count
        {
            get { return _cache == null ? 0 : _cache.Count; }
        }

        public long EffectiveBytes
        {
            get { return _cache == null ? 0 : _cache.EffectivePrivateBytesLimit; }
        }

        public bool Contains(string key)
        {
            if (_cache == null)
                throw new ObjectDisposedException(nameof(_cache));

            return _cache.Get(key) != null;
        }

        public T Get<T>(string key)
        {
            if (_cache == null)
                throw new ObjectDisposedException(nameof(_cache));
            Assert.ArgumentNotNull(nameof(key), key);

            var value = _cache.Get(key);
            if (!(value is T))
                Assert.NotExistRecords("cache", key);

            return (T) value;
        }

        public void Set<T>(string key, T value, Action<string, object> removeCallback = null)
        {
            InternalSet(key, value, Cache.Timeout, removeCallback);
        }

        public void Set<T>(string key, T value, TimeSpan slidingExpiration, Action<string, object> removeCallback = null)
        {
            InternalSet(key, value, slidingExpiration, removeCallback);
        }

        public void Remove(string key)
        {
            if (_cache == null)
                return;

            Assert.ArgumentNotNull(nameof(key), key);
            _cache.Remove(key);
        }

        private void InternalSet<T>(string key, T value, TimeSpan slidingExpiration,
            Action<string, object> removeCallback)
        {
            if (_cache == null)
                return;

            Assert.ArgumentNotNull(nameof(key), key);

            if (removeCallback == null)
                _cache.Insert(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, slidingExpiration);
            else
                _cache.Insert(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, slidingExpiration,
                    CacheItemPriority.Normal, (k, v, r) => removeCallback(k, v));
        }
    }

    #endregion

    /// <summary>
    /// 自定义缓存适配接口
    /// </summary>
    public class CustomCacheAdapter : CacheAdapter, IAdapter
    {
        /// <summary>
        /// 
        /// </summary>
        public CustomCacheAdapter()
            : base(InternalCustomCache.Instance)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            InternalCustomCache.Instance.Dispose();
        }

        #region CustomCache

        private sealed class InternalCustomCache : ICache, IDisposable
        {
            #region 单例

            private static readonly InternalCustomCache _instance = new InternalCustomCache();

            private InternalCustomCache() { }

            public static InternalCustomCache Instance => _instance;

            #endregion

            private class Item
            {
                public TimeSpan Sliding { get; set; }
                public DateTime ExpireTime { get; set; }
                public Action<string, object> RemoveCallback { get; set; }
                public object Value { get; set; }
            }

            private static readonly object _lock = new object();
            private static readonly IDictionary<string, Item> _caches = new Dictionary<string, Item>();
            private static bool _isDisposed;

            private readonly Timer _timer = new Timer(state =>
            {
                var removes = new List<string>();
                lock (_lock)
                {
                    foreach (var pair in _caches)
                    {
                        if (pair.Value.ExpireTime <= DateTime.Now)
                            removes.Add(pair.Key);
                    }
                    foreach (var key in removes)
                    {
                        RemoveItem(key);
                    }
                }
            }, null, 1000*Cache.Timeout.Minutes, 1000*Cache.Timeout.Minutes);

            public void Dispose()
            {
                _timer.Dispose();
                Clear();
                _isDisposed = true;
            }

            public void Clear()
            {
                Assert.ObjectNotDisposed(_isDisposed, nameof(InternalCustomCache));
                lock (_lock)
                {
                    _caches.Clear();
                }
            }

            public int Count
            {
                get
                {
                    Assert.ObjectNotDisposed(_isDisposed, nameof(InternalCustomCache));
                    return _caches.Count;
                }
            }

            public bool Contains(string key)
            {
                Assert.ObjectNotDisposed(_isDisposed, nameof(InternalCustomCache));
                return _caches.ContainsKey(key);
            }

            public T Get<T>(string key)
            {
                Assert.ObjectNotDisposed(_isDisposed, nameof(InternalCustomCache));
                Assert.ArgumentNotNull(nameof(key), key);
                if (!Contains(key))
                    Assert.KeyFound(key);
                var item = _caches[key];
                if (item == null || !(item.Value is T))
                    Assert.NotExistRecords("cache", key);

                lock (_lock)
                {
// ReSharper disable once PossibleNullReferenceException
                    if (item.Sliding != System.Web.Caching.Cache.NoSlidingExpiration)
                        item.ExpireTime = DateTime.Now + item.Sliding;
                }
                return (T) item.Value;
            }

            public void Set<T>(string key, T value, Action<string, object> removeCallback = null)
            {
                Assert.ObjectNotDisposed(_isDisposed, nameof(InternalCustomCache));
                Assert.ArgumentNotNull(nameof(key), key);
                lock (_lock)
                {
                    SetCacheItem(key, value, Cache.Timeout, removeCallback);
                }
            }

            public void Set<T>(string key, T value, TimeSpan slidingExpiration,
                Action<string, object> removeCallback = null)
            {
                Assert.ObjectNotDisposed(_isDisposed, nameof(InternalCustomCache));
                Assert.ArgumentNotNull(nameof(key), key);
                lock (_lock)
                {
                    SetCacheItem(key, value, slidingExpiration, removeCallback);
                }
            }

            public void Remove(string key)
            {
                Assert.ObjectNotDisposed(_isDisposed, nameof(InternalCustomCache));
                Assert.ArgumentNotNull(nameof(key), key);
                if (!Contains(key))
                    return;

                lock (_lock)
                {
                    RemoveItem(key);
                }
            }

            private static void RemoveItem(string key)
            {
                var callback = _caches[key].RemoveCallback;
                var value = _caches[key].Value;
                if (_caches.Remove(key) && callback != null)
                    callback(key, value);
            }

            private void SetCacheItem(string key, object value, TimeSpan sliding, Action<string, object> removeCallback)
            {
                if (!Contains(key))
                {
                    _caches.Add(key, new Item());
                }
                _caches[key].Sliding = sliding <= TimeSpan.Zero ? Cache.Timeout : sliding;
                _caches[key].ExpireTime = DateTime.Now + _caches[key].Sliding;
                _caches[key].Value = value;
                _caches[key].RemoveCallback = removeCallback;
            }
        }

        #endregion
    }
}