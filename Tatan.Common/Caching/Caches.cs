using System.Collections.Generic;
using System.Threading;

namespace Tatan.Common.Caching
{
    using System;
    using System.Web;
    using System.Web.Caching;
    using Exception;

    /// <summary>
    ///Http操作类
    /// </summary>
    public static class Caches
    {
        //默认间隔时间，为半小时
        internal static readonly TimeSpan SlidingExpiration = new TimeSpan(0, 30, 0);

        #region WebCache
        /// <summary>
        /// 获取Web服务器缓存对象
        /// </summary>
        public static ICache WebCache
        {
            get { return InternalWebCache.Instance; }
        }

        /// <summary>
        /// 获取有效的缓存字节
        /// </summary>
        public static long WebCacheEffectiveBytes
        {
            get { return InternalWebCache.Instance.EffectiveBytes; }
        }

        private sealed class InternalWebCache : ICache
        {
            #region 单例
            private static readonly InternalWebCache _instance = new InternalWebCache();
            private InternalWebCache() { }
            public static InternalWebCache Instance { get { return _instance; } }
            #endregion

            public void Clear()
            {
                var enumerator = HttpRuntime.Cache.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    HttpRuntime.Cache.Remove(enumerator.Key.ToString());
                }
            }

            public int Count
            {
                get { return HttpRuntime.Cache.Count; }
            }

            public long EffectiveBytes 
            {
                get { return HttpRuntime.Cache.EffectivePrivateBytesLimit; }
            }

            public bool Contains(string key)
            {
                return HttpRuntime.Cache.Get(key) != null;
            }

            public T Get<T>(string key)
            {
                ExceptionHandler.ArgumentNull("key", key);
                var value = HttpRuntime.Cache.Get(key);
                if (!(value is T)) 
                    ExceptionHandler.NotExistRecords();


                return (T) value;
            }

            public void Set<T>(string key, T value, Action<string, object> removeCallback = null)
            {
                ExceptionHandler.ArgumentNull("key", key);
                if (removeCallback == null)
                    HttpRuntime.Cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, SlidingExpiration);
                else
                    HttpRuntime.Cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, SlidingExpiration,
                        CacheItemPriority.Normal, (k, v, r) => removeCallback(k, v));
            }

            public void Set<T>(string key, T value, TimeSpan slidingExpiration, Action<string, object> removeCallback = null)
            {
                ExceptionHandler.ArgumentNull("key", key);
                if (removeCallback == null)
                    HttpRuntime.Cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, slidingExpiration);
                else
                    HttpRuntime.Cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, slidingExpiration, 
                        CacheItemPriority.Normal, (k, v, r) => removeCallback(k, v));
            }

            public void Remove(string key)
            {
                ExceptionHandler.ArgumentNull("key", key);
                HttpRuntime.Cache.Remove(key);
            }
        }
        #endregion

        #region CustomCache
        /// <summary>
        /// 获取自定义缓存对象
        /// </summary>
        public static ICache CustomCache
        {
            get { return InternalCustomCache.Instance; }
        }

        /// <summary>
        /// 销毁自定义缓存，销毁后不可用
        /// </summary>
        public static void CustomDispose()
        {
            InternalCustomCache.Instance.Dispose();
        }

        private sealed class InternalCustomCache : ICache
        {
            #region 单例
            private static readonly InternalCustomCache _instance = new InternalCustomCache();
            private InternalCustomCache() { }
            public static InternalCustomCache Instance { get { return _instance; } }
            #endregion

            private class CacheItem
            {
                public TimeSpan Sliding { get; set; }
                public DateTime ExpireTime { get; set; }
                public Action<string, object> RemoveCallback { get; set; }
                public object Value { get; set; }
            }

            private static readonly object _lock = new object();
            private static readonly IDictionary<string, CacheItem> _caches = new Dictionary<string, CacheItem>();
            private static bool _isDisposed = false;

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
                        var callback = _caches[key].RemoveCallback;
                        var value = _caches[key].Value;
                        if (_caches.Remove(key) && callback != null)
                            callback(key, value);
                    }
                }
            }, null, 1000*1, 1000*1);

            public void Dispose()
            {
                _timer.Dispose();
                Clear();
                _isDisposed = true;
            }

            public void Clear()
            {
                ExceptionHandler.ObjectDisposed(_isDisposed);
                lock (_lock)
                {
                    _caches.Clear();
                }
            }

            public int Count
            {
                get
                {
                    ExceptionHandler.ObjectDisposed(_isDisposed);
                    return _caches.Count;
                }
            }

            public bool Contains(string key)
            {
                ExceptionHandler.ObjectDisposed(_isDisposed);
                return _caches.ContainsKey(key);
            }

            public T Get<T>(string key)
            {
                ExceptionHandler.ObjectDisposed(_isDisposed);
                ExceptionHandler.ArgumentNull("key", key);
                if (!Contains(key))
                    ExceptionHandler.KeyNotFound(key);
                var item = _caches[key];
                if (item == null || !(item.Value is T)) 
                    ExceptionHandler.NotExistRecords();

                lock (_lock)
                {
                    if (item.Sliding != Cache.NoSlidingExpiration)
                        item.ExpireTime = DateTime.Now + item.Sliding;
                }
                return (T) item.Value;
            }

            public void Set<T>(string key, T value, Action<string, object> removeCallback = null)
            {
                ExceptionHandler.ObjectDisposed(_isDisposed);
                ExceptionHandler.ArgumentNull("key", key);
                lock (_lock)
                {
                    SetCacheItem(key, value, SlidingExpiration, removeCallback);
                }
            }

            public void Set<T>(string key, T value, TimeSpan slidingExpiration, Action<string, object> removeCallback = null)
            {
                ExceptionHandler.ObjectDisposed(_isDisposed);
                ExceptionHandler.ArgumentNull("key", key);
                lock (_lock)
                {
                    SetCacheItem(key, value, slidingExpiration, removeCallback);
                }
            }

            public void Remove(string key)
            {
                ExceptionHandler.ObjectDisposed(_isDisposed);
                ExceptionHandler.ArgumentNull("key", key);
                if (!Contains(key))
                    return;

                lock (_lock)
                {
                    var callback = _caches[key].RemoveCallback;
                    var value = _caches[key].Value;
                    if (_caches.Remove(key) && callback != null)
                        callback(key, value);
                }
            }

            private void SetCacheItem(string key, object value, TimeSpan sliding, Action<string, object> removeCallback)
            {
                if (!Contains(key))
                {
                    _caches.Add(key, new CacheItem());
                }
                _caches[key].Sliding = sliding <= TimeSpan.Zero ? SlidingExpiration : sliding;
                _caches[key].ExpireTime = DateTime.Now + _caches[key].Sliding;
                _caches[key].Value = value;
                _caches[key].RemoveCallback = removeCallback;
            }
        }
        #endregion
    }
}