namespace Tatan.Common.Collections
{
    using System.Collections.Generic;
    using System.Collections;

    /// <summary>
    /// 只读抽象集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractReadOnlyCollection<T> : IEnumerable<T>, ICountable
    {
        /// <summary>
        /// 确定集合是否包含指定项
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public abstract bool Contains(T item);

        /// <summary>
        /// 获取集合中对象个数
        /// </summary>
        public abstract int Count { get; }

        #region IEnumerable
        /// <summary>
        /// 获取此集合的迭代
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerator<T> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
