namespace Tatan.Data.Relation
{
    using System;
    using Collections;
    using Common.Exception;
    using Common.Extension.String.Convert;
    using Common.Collections;

    #region Tables的实体类，无法继承
    /// <summary>
    /// Tables的实体类，无法继承。此实体保存了表的信息
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    [Serializable]
    public partial class Tables : DataEntity
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">唯一标识符</param>
        /// <param name="creator"></param>
        /// <param name="createTime">创建时间</param>
        public Tables(string id = null, string creator = null, string createTime = null)
            : base(id, creator, createTime)
        {
        }
        #endregion

        #region Static Properties
        [NonSerialized]
        private static readonly PropertyCollection _perproties;

        static Tables()
        {
            _perproties = new PropertyCollection(typeof(Tables));
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// 获取属性集合
        /// </summary>
        protected override PropertyCollection Properties => _perproties;

        /// <summary>
        /// 清理属性
        /// </summary>
        public override void Clear()
        {
            Name = default(string);
            Title = default(string);
            Remark = default(string);
        }
        #endregion


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

            var id = Id.AsValue<int>();
            var entities = source.Tables.Get<Fields>().Query<Fields>(q => q.Where(f => id == f.TableId));
            _fields = new FieldsCollection(entities);
            return _fields;
        }
    }
    #endregion
}