namespace Tatan.Data.Relation.Collections
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Common.Collections;

    /// <summary>
    /// Fields集合
    /// </summary>
    public sealed class FieldsCollection : AbstractReadOnlyCollection<Fields>
    {
        private readonly ICollection<Fields> _fields; 

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fields"></param>
        public FieldsCollection(params Fields[] fields)
        {
            _fields = new Collection<Fields>(fields);
        }

        public override IEnumerator<Fields> GetEnumerator()
        {
            return _fields.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool Contains(Fields item)
        {
            return item != null && _fields.Contains(item);
        }

        /// <summary>
        /// 
        /// </summary>
        public override int Count 
        {
            get { return _fields.Count; }
        }

        internal void Add(Fields field)
        {
            _fields.Add(field);
        }
    }
}
