namespace Tatan.Common.Collections
{
    using System.Collections.Generic;
    using System.Collections;
    using System.Text;
    using Exception;

    /// <summary>
    /// 轻量级键值对集合
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class ListMap<TKey, TValue> : IEnumerable<TKey>, ICountable, IObject
    {
        private readonly List<TKey> _keys;
        private readonly TValue[] _values;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="capacity"></param>
        public ListMap(int capacity = 0)
        {
            if (capacity <= 10)
            {
                _keys = new List<TKey>(10);
                _values = new TValue[10];
            }
            else
            {
                _keys = new List<TKey>(capacity > 100 ? 100 : capacity);
                _values = new TValue[capacity > 100 ? 100 : capacity];
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="collection"></param>
        public ListMap(IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            if (collection == null)
            {
                _keys = new List<TKey>(10);
                _values = new TValue[10];
            }
            else
            {
                _keys = new List<TKey>();
                var values = new List<TValue>();
                foreach (var element in collection)
                {
                    _keys.Add(element.Key);
                    values.Add(element.Value);
                }
                _values = values.ToArray();
            }
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
            return Find(key) >= 0;
        }

        /// <summary>
        /// 为集合添加元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Add(TKey key, TValue value)
        {
            ExceptionHandler.IsNotAllow(_keys.Count, _keys.Capacity);

// ReSharper disable once CompareNonConstrainedGenericWithNull
            if (value == null) return false;
            var index = Find(key);
            if (index >= 0) return false;
            _keys.Add(key);
            _values[_keys.Count - 1] = value;
            return true;
        }

        /// <summary>
        /// 根据键移除元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(TKey key)
        {
            var index = Find(key);
            if (index < 0) return false;
            _keys.RemoveAt(index);
            _values[index] = default(TValue);
            return true;
        }

        /// <summary>
        /// 根据键获取或设置值
        /// </summary>
        /// <param name="key"></param>
        public TValue this[TKey key]
        {
            get
            {
                var index = Find(key);
                if (index < 0)
                    ExceptionHandler.KeyNotFound<object>(null);
                return _values[index];
            }
            set
            {
// ReSharper disable once CompareNonConstrainedGenericWithNull
                if (value == null)
                    ExceptionHandler.KeyNotFound<object>(null);
                var index = Find(key);
                if (index < 0)
                    ExceptionHandler.KeyNotFound<object>(null);
                _values[_keys.Count - 1] = value;
            }
        }

        private int Find(TKey key)
        {
// ReSharper disable once CompareNonConstrainedGenericWithNull
            if (key == null)
                ExceptionHandler.KeyNotFound<object>(null);
            return _keys.IndexOf(key);
        }

        /// <summary>
        /// 获取集合中对象个数
        /// </summary>
        public int Count 
        {
            get { return _keys.Count; }
        }

        /// <summary>
        /// 清空集合
        /// </summary>
        public void Clear()
        {
           
            for (var i = 0; i < Count; i++)
                _values[i] = default(TValue);
            _keys.Clear();
        }

        #region IEnumerable

        /// <summary>
        /// 获取此集合的迭代
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TKey> GetEnumerator()
        {
            return _keys.GetEnumerator();
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
            for (var i = 0; i < Count; i++)
            {
                hash ^= _keys[i].GetHashCode();
                hash ^= _values[i].GetHashCode();
            }
            return hash;
        }

        ///
        public override string ToString()
        {
            var builder = new StringBuilder(Count * 10);
            builder.Append("{");
            for (var i = 0; i < Count; i++)
            {
                builder.AppendFormat("\"{0}\":{1},", _keys[i].ToString(), _values[i].ToString());
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
