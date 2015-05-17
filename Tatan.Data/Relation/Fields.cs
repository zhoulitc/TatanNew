namespace Tatan.Data.Relation
{
    using System;
    using Common.Collections;

    #region Fields的实体类，无法继承
    /// <summary>
    /// Fields的实体类，无法继承。此实体保存了表的列信息
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    [Serializable]
    public partial class Fields : DataEntity
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public Fields()
            : base(string.Empty)
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">唯一标识符</param>
        /// <param name="creator"></param>
        public Fields(string id, string creator = null)
            : base(id)
        {
        }
        #endregion

        #region Static Properties
        [NonSerialized]
        private static readonly PropertyCollection _perproties;

        static Fields()
        {
            _perproties = new PropertyCollection(typeof(Fields),
                "Name", "Title", "Type", "Size", "Scale", "DefaultValue", "IsNotNull", "TableId", "OrderId"
            );
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// 获取属性集合
        /// </summary>
        protected override PropertyCollection Properties
        {
            get { return _perproties; }
        }

        /// <summary>
        /// 清理属性
        /// </summary>
        public override void Clear()
        {
            Name = default(string);
            Title = default(string);
            Type = default(string);
            Size = default(int);
            Scale = default(int);
            DefaultValue = default(string);
            IsNotNull = default(bool);
            TableId = default(int);
            OrderId = default(int);
        }
        #endregion

        #region Properties
        /// <summary>
        /// 字段名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 字段显示名
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 字段长度/精度
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 字段小数位数
        /// </summary>
        public long Scale { get; set; }

        /// <summary>
        /// 字段默认值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 字段是否不为空
        /// </summary>
        public bool IsNotNull { get; set; }

        /// <summary>
        /// 字段表名
        /// </summary>
        public long TableId { get; set; }

        /// <summary>
        /// 字段排序编号
        /// </summary>
        public long OrderId { get; set; }

        #endregion
    }
    #endregion
}