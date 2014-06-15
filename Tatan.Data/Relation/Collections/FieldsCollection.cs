namespace Tatan.Data.Relation.Collections
{
    using System.Collections;
    using System.Collections.Generic;
    using Common;

    /// <summary>
    /// Fields集合
    /// </summary>
    public sealed class FieldsCollection : IEnumerable<Fields>, ICountable
    {
        private readonly ICollection<Fields> _fields; 

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fields"></param>
        public FieldsCollection(IEnumerable<Fields> fields)
        {
            _fields = new List<Fields>(fields);
        }

        /// <summary>
        /// 返回一个循环访问集合的枚举器。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Fields> GetEnumerator()
        {
            return _fields.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count 
        {
            get { return _fields.Count; }
        }
    }
}
