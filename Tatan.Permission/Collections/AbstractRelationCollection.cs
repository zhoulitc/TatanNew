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
        protected readonly string ThatName;

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
                {"contains", "SELECT COUNT(1) FROM [{0}] WHERE {2}={1}{2} AND {3}={1}{3}"},
                {"add", "INSERT INTO [{0}]({2},{3}) VALUES({1}{2},{1}{3})"},
                {"remove", "DELETE FROM [{0}] WHERE {2}={1}{2} AND {3}={1}{3}"},
                {"getById", "SELECT * FROM [{0}] WHERE Id=(SELECT {3} FROM [{1}] WHERE {3}={2}{3} AND {4}={2}{4})"}
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="tableName"></param>
        /// <param name="thatName"></param>
        /// <param name="thisName"></param>
        protected AbstractRelationCollection(IDentifiable identity, string tableName, string thatName, string thisName)
        {
            TableName = tableName;
            ThatName = thatName;
            Identity = identity;
            ThisName = thisName;
            TypeName = typeof(T).Name;
        }

        /// <summary>
        /// 设置数据源，只有设置了源才能做关联操作
        /// </summary>
        public IDataSource Source { protected get; set; }

        /// <summary>
        /// 判断是否包含关联
        /// </summary>
        /// <param name="relation"></param>
        /// <returns></returns>
        public virtual bool Contains(T relation)
        {
            ExceptionHandler.ArgumentNull("Source", Source);
            ExceptionHandler.ArgumentNull("relation", relation);
            var sql = string.Format(Sqls["contains"], TableName, Source.Provider.ParameterSymbol, ThisName, ThatName);
            return Source.UseSession(TableName, session => session.ExecuteScalar<long>(sql, parameters =>
            {
                parameters[ThisName] = relation.Id;
                parameters[ThatName] = Identity.Id;
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
            var sql = string.Format(Sqls["add"], TableName, Source.Provider.ParameterSymbol, ThisName, ThatName);
            return Source.UseSession(sql, session => session.Execute(sql, parameters =>
            {
                parameters[ThisName] = relation.Id;
                parameters[ThatName] = Identity.Id;
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
            var sql = string.Format(Sqls["remove"], TableName, Source.Provider.ParameterSymbol, ThisName, ThatName);
            return Source.UseSession(TableName, session => session.Execute(sql, parameters =>
            {
                parameters[ThisName] = relation.Id;
                parameters[ThatName] = Identity.Id;
            }) == 1);
        }

        /// <summary>
        /// 根据Id获取关联对象
        /// </summary>
        /// <param name="id"></param>
        public virtual T GetById(long id)
        {
            ExceptionHandler.ArgumentNull("Source", Source);
            var sql = string.Format(Sqls["getById"], TypeName, TableName, Source.Provider.ParameterSymbol, ThisName, ThatName);
            return Source.UseSession(TableName, session =>
            {
                var entities = session.GetEntities<T>(sql, parameters =>
                {
                    parameters[ThisName] = id;
                    parameters[ThatName] = Identity.Id;
                });
                if (entities == null || entities.Count != 1) 
                    return default(T);
                return entities[0];
            });
        }
    }
}