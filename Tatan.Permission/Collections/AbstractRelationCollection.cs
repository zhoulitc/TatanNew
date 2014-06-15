namespace Tatan.Permission.Collections
{
    using Common;
    using Common.Collections;
    using Common.Exception;
    using Data;

    /// <summary>
    /// 关联集合
    /// </summary>
    public abstract class AbstractRelationCollection<T> where T : IDentifiable, INameable, IDataEntity, new()
    {
// ReSharper disable once StaticFieldInGenericType
        /// <summary>
        /// 
        /// </summary>
        protected static readonly ListMap<string, string> Sqls;

        /// <summary>
        /// 
        /// </summary>
        protected readonly IDentifiable Identity;

        /// <summary>
        /// 
        /// </summary>
        protected readonly string TableName;

        /// <summary>
        /// 
        /// </summary>
        protected readonly string FieldName;

        /// <summary>
        /// 
        /// </summary>
        protected readonly string ThisName;

        /// <summary>
        /// 
        /// </summary>
        protected readonly string TypeName;

        static AbstractRelationCollection()
        {
            Sqls = new ListMap<string, string>(4)
            {
                {"constains", "SELECT COUNT(1) FROM {0} WHERE {5}={1}{6} AND {3}={2}{4}"},
                {"add", "INSERT INTO {0}({5}, {3}) VALUES({1}{6}, {2}{4}})"},
                {"remove", "DELETE FROM {0} WHERE {5}={1}{6} AND {3}={2}{4}"},
                {"getById", "SELECT * FROM {0} WHERE Id = (SELECT {8} FROM {1} WHERE {6}={2}{7} AND {4}={3}{5})"}
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="fieldName"></param>
        /// <param name="tableName"></param>
        /// <param name="thisName"></param>
        protected AbstractRelationCollection(IDentifiable identity, string fieldName, string tableName, string thisName)
        {
            TableName = tableName;
            FieldName = fieldName;
            Identity = identity;
            ThisName = thisName;
            TypeName = typeof(T).Name;
        }

        /// <summary>
        /// 设置数据源，只有设置了源才能做关联操作
        /// </summary>
        public DataSource Source { protected get; set; }

        /// <summary>
        /// 判断是否包含关联
        /// </summary>
        /// <param name="relation"></param>
        /// <returns></returns>
        public virtual bool Contains(T relation)
        {
            ExceptionHandler.ArgumentNull("Source", Source);
            ExceptionHandler.ArgumentNull("relation", relation);
            var sql = string.Format(Sqls["contains"], TableName,
                Source.Provider.ParameterSymbol, Source.Provider.ParameterSymbol, 
                FieldName, FieldName,
                ThisName, ThisName
                );
            return Source.UseSession(TableName, session => session.GetScalar<int>(sql, parameters =>
            {
                parameters[ThisName] = relation.Id;
                parameters[FieldName] = Identity.Id;
            }) == 1);
        }

        /// <summary>
        /// 添加一个关联对象
        /// </summary>
        /// <param name="relation"></param>
        /// <returns></returns>
        public bool Add(T relation)
        {
            ExceptionHandler.ArgumentNull("Source", Source);
            if (Contains(relation))
                ExceptionHandler.DuplicateRecords();
            var sql = string.Format(Sqls["add"], TableName, 
                Source.Provider.ParameterSymbol, Source.Provider.ParameterSymbol,
                FieldName, FieldName,
                ThisName, ThisName);
            return Source.UseSession(sql, session => session.Execute(sql, parameters =>
            {
                parameters[ThisName] = relation.Id;
                parameters[FieldName] = Identity.Id;
            }) == 1);
        }

        /// <summary>
        /// 移除一个关联对象
        /// </summary>
        /// <param name="relation"></param>
        /// <returns></returns>
        public bool Remove(T relation)
        {
            ExceptionHandler.ArgumentNull("Source", Source);
            if (!Contains(relation))
                ExceptionHandler.NotExistRecords();
            var sql = string.Format(Sqls["remove"], TableName,
                Source.Provider.ParameterSymbol, Source.Provider.ParameterSymbol,
                FieldName, FieldName,
                ThisName, ThisName);
            return Source.UseSession(TableName, session => session.Execute(sql, parameters =>
            {
                parameters[ThisName] = relation.Id;
                parameters[FieldName] = Identity.Id;
            }) == 1);
        }

        /// <summary>
        /// 根据Id获取关联对象
        /// </summary>
        /// <param name="id"></param>
        public virtual T GetById(int id)
        {
            ExceptionHandler.ArgumentNull("Source", Source);
            var sql = string.Format(Sqls["getById"], 
                TypeName, TableName, 
                Source.Provider.ParameterSymbol, Source.Provider.ParameterSymbol,
                FieldName, FieldName,
                ThisName, ThisName, ThisName);
            return Source.UseSession(TableName, session =>
            {
                var entities = session.GetEntities<T>(sql, parameters =>
                {
                    parameters[ThisName] = id;
                    parameters[FieldName] = Identity.Id;
                });
                if (entities == null || entities.Count != 1) 
                    return default(T);
                return entities[0];
            });
        }
    }
}