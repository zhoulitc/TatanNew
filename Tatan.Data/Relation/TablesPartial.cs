namespace Tatan.Data.Relation
{
    using Collections;
    using Common.Exception;

    #region Tables的实体类，无法继承
    /// <summary>
    /// Tables的实体类，无法继承。此实体保存了表的信息
    /// </summary>
    public partial class Tables
    {
        private const string _getFields = "SELECT * FROM Fields WHERE TableId={0}TableId";
        private FieldsCollection _fields = null;

        /// <summary>
        /// 从属表的字段集合
        /// </summary>
        public FieldsCollection GetFields(IDataSource source)
        {
            ExceptionHandler.ArgumentNull("source", source);
            if (_fields != null)
                return _fields;
            var entities = source.UseSession("Fields", session => session.GetEntities<Fields>(
                string.Format(_getFields, source.Provider.ParameterSymbol), p =>
            {
                p["TableId"] = Id;
            }));
            _fields = new FieldsCollection(entities);
            return _fields;
        }
    }
    #endregion
}
