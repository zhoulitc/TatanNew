namespace Tatan.Data.Relation
{
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