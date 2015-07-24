namespace Tatan.Data.Relation
{
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
        public string Name { get; set; }

        /// <summary>
        /// 表显示名
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 表备注
        /// </summary>
        public string Remark { get; set; }

        #endregion
    }
    #endregion
}
