using System.Text;

namespace Tatan.Common.Collections
{
    using System.Collections.Generic;
    using System.Collections;
    using Exception;

    /// <summary>
    /// 轻量级键值对集合
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class ListMap<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, ICountable, IObject
    {
        private readonly List<KeyValuePair<TKey, TValue>> _map;
        private static readonly KeyValuePair<TKey, TValue> _empty;

        static ListMap()
        {
            _empty = new KeyValuePair<TKey, TValue>(default(TKey), default(TValue));
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="capacity"></param>
        public ListMap(int capacity = 0)
        {
// ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
            if (capacity <= 10)
                _map = new List<KeyValuePair<TKey, TValue>>(10);
            else
                _map = new List<KeyValuePair<TKey, TValue>>(capacity > 100 ? 100 : capacity);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="collection"></param>
        public ListMap(IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            ExceptionHandler.ArgumentNull("collection", collection);
             _map = new List<KeyValuePair<TKey, TValue>>(collection);
        }

        /// <summary>
        /// 确定集合是否包含指定项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(TKey key)
        {
// ReSharper disable once CompareNonConstrainedGenericWithNull
            if (key == null) return false;
            return !_empty.Equals(Find(key));
        }

        /// <summary>
        /// 为集合添加元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Add(TKey key, TValue value)
        {
            ExceptionHandler.IsNotAllow(_map.Count, _map.Capacity);

// ReSharper disable once CompareNonConstrainedGenericWithNull
            if (value == null) return false;
            var pair = Find(key);
            if (!_empty.Equals(pair)) return false;
            _map.Add(new KeyValuePair<TKey, TValue>(key, value));
            return true;
        }

        /// <summary>
        /// 根据键移除元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(TKey key)
        {
            var pair = Find(key);
            if (_empty.Equals(pair)) return false;
            return _map.Remove(pair);
        }

        /// <summary>
        /// 根据键获取或设置值
        /// </summary>
        /// <param name="key"></param>
        public TValue this[TKey key]
        {
            get
            {
                var pair = Find(key);
                if (_empty.Equals(pair))
                    ExceptionHandler.KeyNotFound<object>(null);
                return pair.Value;
            }
            set
            {
// ReSharper disable once CompareNonConstrainedGenericWithNull
                if (value == null)
                    ExceptionHandler.KeyNotFound<object>(null);
                var pair = Find(key);
                if (!_empty.Equals(pair))
                    Remove(key);
                Add(key, value);
            }
        }

        private KeyValuePair<TKey, TValue> Find(TKey key)
        {
// ReSharper disable once CompareNonConstrainedGenericWithNull
            if (key == null)
                ExceptionHandler.KeyNotFound<object>(null);
            foreach (var pair in _map)
            {
                if (pair.Key.Equals(key))
                    return pair;
            }
            return _empty;
        }

        /// <summary>
        /// 获取集合中对象个数
        /// </summary>
        public int Count 
        {
            get { return _map.Count; }
        }

        /// <summary>
        /// 清空集合
        /// </summary>
        public void Clear()
        {
            _map.Clear();
        }

        #region IEnumerable

        /// <summary>
        /// 获取此集合的迭代
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _map.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region IObject
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return GetHashCode() == obj.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var hash = 0;
            foreach (var pair in _map)
            {
                hash ^= pair.Key.GetHashCode();
                hash ^= pair.Value.GetHashCode();
            }
            return hash;
        }

        ///
        public override string ToString()
        {
            var builder = new StringBuilder(_map.Count * 10);
            builder.Append("{");
            foreach (var pair in _map)
            {
                builder.AppendFormat("\"{0}\":{1},", pair.Key.ToString(), pair.Value.ToString());
            }
            if (builder[builder.Length - 1] == ',')
                builder[builder.Length - 1] = '}';
            else
                builder.Append("}");
            return builder.ToString();
        }
        #endregion
    }
}
