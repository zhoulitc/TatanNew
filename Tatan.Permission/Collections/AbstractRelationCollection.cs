namespace Tatan.Permission.Collections
{
    using System.Collections.Generic;
    using Common;
    using Common.Exception;
    using Data;

    /// <summary>
    /// 关联集合
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public abstract class AbstractRelationCollection<T> where T : class, IDentifiable, INameable, IDataEntity, new()
    {
        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once StaticFieldInGenericType
        protected static readonly Dictionary<string, string> Sqls;

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
            Sqls = new Dictionary<string, string>(5)
            {
                {nameof(Contains), "SELECT COUNT(1) FROM {4}{0}{5} WHERE {4}{2}{5}={1}{2} AND {4}{3}{5}={1}{3}"},
                {nameof(Add), "INSERT INTO {4}{0}{5}({4}{2}{5},{4}{3}{5}) VALUES({1}{2},{1}{3})"},
                {nameof(Remove), "DELETE FROM {4}{0}{5} WHERE {4}{2}{5}={1}{2} AND {4}{3}{5}={1}{3}"},
                {nameof(GetById), "SELECT * FROM {5}{0}{6} WHERE {5}Id{6}=(SELECT {5}{3}{6} FROM {5}{1}{6} WHERE {5}{3}{6}={2}{3} AND {5}{4}{6}={2}{4})"}
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
            Assert.ArgumentNotNull(nameof(Source), Source);
            Assert.ArgumentNotNull(nameof(relation), relation);
            var sql = string.Format(Sqls[nameof(Contains)], 
                TableName, Source.Provider.ParameterSymbol, ThisName, ThatName,
                Source.Provider.LeftSymbol, Source.Provider.RightSymbol);
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
            Assert.ArgumentNotNull(nameof(Source), Source);
            if (Contains(relation))
                Assert.DuplicateRecords(relation.Id, relation.Name);
            var sql = string.Format(Sqls[nameof(Add)], 
                TableName, Source.Provider.ParameterSymbol, ThisName, ThatName,
                Source.Provider.LeftSymbol, Source.Provider.RightSymbol);
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
            Assert.ArgumentNotNull(nameof(Source), Source);
            if (!Contains(relation))
                Assert.NotExistRecords(relation.Id, relation.Name);
            var sql = string.Format(Sqls[nameof(Remove)], 
                TableName, Source.Provider.ParameterSymbol, ThisName, ThatName,
                Source.Provider.LeftSymbol, Source.Provider.RightSymbol);
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
        public virtual T GetById(string id)
        {
            Assert.ArgumentNotNull(nameof(Source), Source);
            var sql = string.Format(Sqls[nameof(GetById)], 
                TypeName, TableName, Source.Provider.ParameterSymbol, ThisName, ThatName,
                Source.Provider.LeftSymbol, Source.Provider.RightSymbol);
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