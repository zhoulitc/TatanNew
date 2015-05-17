namespace Tatan.Common.Collections
{
    using System.Collections.Generic;

    /// <summary>
    /// 只读集合
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public class ReadOnlyCollection<T> : AbstractReadOnlyCollection<string>
    {
        /// <summary>
        /// 只读集合的内部字典
        /// </summary>
        protected readonly IDictionary<string, T> Collection;

        /// <summary>
        /// 构造函数
        /// </summary>
        protected ReadOnlyCollection(int capacity = 0)
        {
            Collection = capacity == 0
                ? new Dictionary<string, T>()
                : new Dictionary<string, T>(capacity);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected ReadOnlyCollection(IDictionary<string, T> dictionary)
        {
            Collection = dictionary == null
                ? new Dictionary<string, T>()
                : new Dictionary<string, T>(dictionary);
        }

        /// <summary>
        /// 确定集合是否包含指定项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override bool Contains(string key) => !string.IsNullOrEmpty(key) && Collection.ContainsKey(key);

        /// <summary>
        /// 获取集合中对象个数
        /// </summary>
        public override int Count => Collection.Count;

        /// <summary>
        /// 获取此集合的迭代
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<string> GetEnumerator() => Collection.Keys.GetEnumerator();
    }
}