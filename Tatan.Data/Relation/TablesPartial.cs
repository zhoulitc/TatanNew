namespace Tatan.Data.Relation
{
    using Collections;

    #region Tables的实体类，无法继承
    /// <summary>
    /// Tables的实体类，无法继承。此实体保存了表的信息
    /// </summary>
    public partial class Tables
    {
        /// <summary>
        /// 从属表的字段集合
        /// </summary>
        public FieldsCollection Fields { get; set; }
    }
    #endregion
}
