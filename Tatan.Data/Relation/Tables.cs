namespace Tatan.Data.Relation
{
    using Attribute;

    #region Tables的实体类，无法继承
    /// <summary>
    /// Tables的实体类，无法继承。此实体保存了表的信息
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public sealed partial class Tables
    {
        #region Properties

        /// <summary>
        /// 表名
        /// </summary>
        [Field(Name = "Name", Description = "表名", Size = 50, DefaultValue = "")]
        public string Name { get; set; }

        /// <summary>
        /// 表显示名
        /// </summary>
        [Field(Name = "Title", Description = "表显示名", Size = 255, DefaultValue = "")]
        public string Title { get; set; }

        /// <summary>
        /// 表备注
        /// </summary>
        [Field(Name = "Remark", Description = "表备注", Size = 255, DefaultValue = "")]
        public string Remark { get; set; }

        #endregion
    }
    #endregion
}
