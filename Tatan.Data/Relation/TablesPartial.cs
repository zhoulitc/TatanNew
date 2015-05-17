namespace Tatan.Data.Relation
{
    using System;
    using Collections;
    using Common.Exception;

    #region Tables的实体类，无法继承
    /// <summary>
    /// Tables的实体类，无法继承。此实体保存了表的信息
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public partial class Tables
    {
        [NonSerialized]
        private FieldsCollection _fields;

        /// <summary>
        /// 从属表的字段集合
        /// </summary>
        public FieldsCollection GetFields(IDataSource source)
        {
            Assert.ArgumentNotNull(nameof(source), source);
            if (_fields != null)
                return _fields;

            var entities = source.Tables.Get<Fields>().Query<Fields>(q => q.Where(f => f.TableId.ToString() == Id));
            _fields = new FieldsCollection(entities);
            return _fields;
        }
    }
    #endregion
}
