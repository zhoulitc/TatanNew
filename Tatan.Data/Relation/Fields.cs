namespace Tatan.Data.Relation
{
    using Attribute;

    #region Fields的实体类，无法继承
    /// <summary>
    /// Fields的实体类，无法继承。此实体保存了表的列信息
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public partial class Fields
    {
        #region Properties

        /// <summary>
        /// 字段名
        /// </summary>
        [Field(Name = "Name", Description = "字段名", Size = 50, DefaultValue = "")]
        public string Name { get; set; }

        /// <summary>
        /// 字段显示名
        /// </summary>
        [Field(Name = "Title", Description = "字段显示名", Size = 255, DefaultValue = "")]
        public string Title { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        [Field(Name = "Type", Description = "字段类型", Size = 50, DefaultValue = "")]
        public string Type { get; set; }

        /// <summary>
        /// 字段长度/精度
        /// </summary>
        [Field(Name = "Size", Description = "字段长度/精度", Size = 20, DefaultValue = 0)]
        public long Size { get; set; }

        /// <summary>
        /// 字段小数位数
        /// </summary>
        [Field(Name = "Scale", Description = "字段小数位数", Size = 20, DefaultValue = 0)]
        public long Scale { get; set; }

        /// <summary>
        /// 字段默认值
        /// </summary>
        [Field(Name = "DefaultValue", Description = "字段默认值", Size = 50, DefaultValue = "")]
        public string DefaultValue { get; set; }

        /// <summary>
        /// 字段是否不为空
        /// </summary>
        [Field(Name = "IsNotNull", Description = "字段是否不为空", Size = 1, DefaultValue = false)]
        public bool IsNotNull { get; set; }

        /// <summary>
        /// 字段表名
        /// </summary>
        [Field(Name = "TableId", Description = "字段表名", Size = 20, DefaultValue = 0)]
        public long TableId { get; set; }

        /// <summary>
        /// 字段排序编号
        /// </summary>
        [Field(Name = "OrderId", Description = "字段排序编号", Size = 20, DefaultValue = 0)]
        public long OrderId { get; set; }

        #endregion
    }
    #endregion
}