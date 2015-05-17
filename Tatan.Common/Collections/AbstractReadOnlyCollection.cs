namespace Tatan.Common.Collections
{
    using System.Collections.Generic;
    using System.Collections;

    /// <summary>
    /// ֻ�����󼯺�
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractReadOnlyCollection<T> : IEnumerable<T>, ICountable
    {
        /// <summary>
        /// ȷ�������Ƿ����ָ����
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public abstract bool Contains(T item);

        /// <summary>
        /// ��ȡ�����ж������
        /// </summary>
        public abstract int Count { get; }

        #region IEnumerable

        /// <summary>
        /// ��ȡ�˼��ϵĵ���
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