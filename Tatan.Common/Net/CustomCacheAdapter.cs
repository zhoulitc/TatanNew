namespace Tatan.Common.Net
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Component;
    using Exception;

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

            private static readonly string _group = Common.Guid.New();
            private static readonly TimeSpan _timeout = new TimeSpan(0, 20, 0);

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
                        {
                            if (pair.Value.Value is CacheGroup)
                            {
                                ((CacheGroup)pair.Value.Value).Clear();
                            }
                            removes.Add(pair.Key);
                        }
                    }
                    foreach (var key in removes)
                    {
                        RemoveItem(key);
                    }
                }
            }, null, 1000 * _timeout.Minutes, 1000 * _timeout.Minutes);

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

            public void Clear(string group)
            {
                Assert.ObjectNotDisposed(_isDisposed, nameof(InternalCustomCache));
                if (string.IsNullOrEmpty(group) || !_caches.ContainsKey(group))
                    return;

                var groupObject = _caches[group].Value as CacheGroup;
                groupObject.Clear();
                _caches.Remove(group);
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
                if (string.IsNullOrEmpty(key)) return false;
                return _caches.ContainsKey(key);
            }

            public bool Contains(string group, string key)
            {
                Assert.ObjectNotDisposed(_isDisposed, nameof(InternalCustomCache));
                if (string.IsNullOrEmpty(group) || string.IsNullOrEmpty(key)) return false;
                if (!_caches.ContainsKey(group)) return false;
                var groupObject = _caches[group].Value as CacheGroup;
                return groupObject.ContainsKey(key);
            }

            public T Get<T>(string key)
            {
                Assert.ObjectNotDisposed(_isDisposed, nameof(InternalCustomCache));
                Assert.ArgumentNotNull(nameof(key), key);
                Assert.KeyFound(_caches, key);
                var item = _caches[key];
                if (item == null || !(item.Value is T))
                    Assert.NotExistRecords("cache", key);

                lock (_lock)
                {
                    // ReSharper disable once PossibleNullReferenceException
                    if (item.Sliding != System.Web.Caching.Cache.NoSlidingExpiration)
                        item.ExpireTime = DateTime.Now + item.Sliding;
                }
                return (T)item.Value;
            }

            public T Get<T>(string group, string key)
            {
                Assert.ObjectNotDisposed(_isDisposed, nameof(InternalCustomCache));
                Assert.ArgumentNotNull(nameof(group), group);
                Assert.ArgumentNotNull(nameof(key), key);
                Assert.KeyFound(_caches, group);
                var item = _caches[group];
                lock (_lock)
                {
// ReSharper disable once PossibleNullReferenceException
                    if (item.Sliding != System.Web.Caching.Cache.NoSlidingExpiration)
                        item.ExpireTime = DateTime.Now + item.Sliding;
                }
                var groupObject = item.Value as CacheGroup;
                return (T)groupObject[key];
            }

            public void Set<T>(string key, T value, Action<string, object> removeCallback = null)
            {
                Assert.ObjectNotDisposed(_isDisposed, nameof(InternalCustomCache));
                Assert.ArgumentNotNull(nameof(key), key);
                lock (_lock)
                {
                    SetCacheItem(key, value, _timeout, removeCallback);
                }
            }

            public void Set<T>(string group, string key, T value)
            {
                Assert.ObjectNotDisposed(_isDisposed, nameof(InternalCustomCache));
                Assert.ArgumentNotNull(nameof(group), group);
                Assert.ArgumentNotNull(nameof(key), key);
                lock (_lock)
                {
                    SetCacheItem(group, key, value, _timeout);
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

            public void Set<T>(string group, string key, T value, TimeSpan slidingExpiration)
            {
                Assert.ObjectNotDisposed(_isDisposed, nameof(InternalCustomCache));
                Assert.ArgumentNotNull(nameof(group), group);
                Assert.ArgumentNotNull(nameof(key), key);
                lock (_lock)
                {
                    SetCacheItem(group, key, value, slidingExpiration);
                }
            }

            public void Remove(string key)
            {
                Assert.ObjectNotDisposed(_isDisposed, nameof(InternalCustomCache));
                Assert.ArgumentNotNull(nameof(key), key);
                if (!_caches.ContainsKey(key))
                    return;

                lock (_lock)
                {
                    RemoveItem(key);
                }
            }

            public void Remove(string group, string key)
            {
                Assert.ObjectNotDisposed(_isDisposed, nameof(InternalCustomCache));
                Assert.ArgumentNotNull(nameof(group), group);
                Assert.ArgumentNotNull(nameof(key), key);
                if (!Contains(group, key))
                    return;

                lock (_lock)
                {
                    RemoveItem(group, key);
                }
            }

            private static void RemoveItem(string key)
            {
                var callback = _caches[key].RemoveCallback;
                var value = _caches[key].Value;
                var groupObject = value as CacheGroup;
                groupObject?.Clear();
                if (_caches.Remove(key) && callback != null)
                    callback(key, value);
            }

            private static void RemoveItem(string group, string key)
            {
                var groupObject = _caches[group].Value as CacheGroup;
                groupObject?.Remove(key);
            }

            private void SetCacheItem(string key, object value, TimeSpan sliding, Action<string, object> removeCallback)
            {
                if (!_caches.ContainsKey(key))
                {
                    _caches.Add(key, new Item());
                }
                _caches[key].Sliding = sliding <= TimeSpan.Zero ? _timeout : sliding;
                _caches[key].ExpireTime = DateTime.Now + _caches[key].Sliding;
                _caches[key].Value = value;
                _caches[key].RemoveCallback = removeCallback;
            }

            private void SetCacheItem(string group, string key, object value, TimeSpan sliding)
            {
                if (!_caches.ContainsKey(group))
                {
                    _caches.Add(group, new Item());
                }
                _caches[group].Sliding = sliding <= TimeSpan.Zero ? _timeout : sliding;
                _caches[group].ExpireTime = DateTime.Now + _caches[key].Sliding;
                _caches[group].Value = value;

                var groupObject = _caches[group].Value as CacheGroup;
                if (groupObject == null)
                {
                    groupObject = new CacheGroup { Name = group };
                    _caches[group].Value = groupObject;
                }
                groupObject[key] = value;
            }
        }

        #endregion
    }
}