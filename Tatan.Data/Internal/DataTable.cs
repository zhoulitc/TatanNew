// ReSharper disable once CheckNamespace
namespace Tatan.Data
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using Common.Exception;
    using Common.Expression;
    using Builder;

    /// <summary>
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    internal class DataTable : IDataTable
    {
        public string Name { get; internal set; }

        public SqlBuilder SqlBuilder { get; }

        public IDataSource DataSource { get; internal set; }

        #region SQL
        //实体原型
        private readonly IDataEntity _entityPrototype;

        //唯一标识符名称
        private const string _identityName = "Id";

        //SELECT * FROM [tableName] WHERE [identity]
        private readonly string _selectIdentity;

        //INSERT INTO [tableName](...) VALUES(...)
        private readonly string _insert;

        //DELETE FROM [tableName] WHERE [identity]
        private readonly string _deleteIdentity;

        //DELETE FROM [tableName] WHERE [condition]
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly string _delete;

        //UPDATE [tableName] SET [entity] WHERE [identity]
        private readonly string _updateIdentity;

        //UPDATE [tableName] SET [expression] WHERE [condition]
        private readonly string _update;

        //SELECT COUNT(1) FROM [tableName]
        private readonly string _counts;

        //SELECT COUNT(1) FROM [tableName] WHERE [condition]
        private readonly string _count;
        #endregion

        public DataTable(IDataSource dataSource, string tableName, Type type)
        {
            Name = tableName;
            DataSource = dataSource;
            var constructorInfo = type.GetConstructor(new[] { typeof(string) });
            if (constructorInfo != null)
                _entityPrototype = (IDataEntity)constructorInfo.Invoke(new object[] { DataEntity.DefaultId });

            //初始化SQL语句
            SqlBuilder = new SqlBuilder(Name, _identityName, _entityPrototype.ToArray(), DataSource.Provider);
            _insert = SqlBuilder.GetInsertStatement();
            _delete = SqlBuilder.GetDeleteStatement();
            _deleteIdentity = SqlBuilder.GetDeleteStatement();
            _update = SqlBuilder.GetUpdateStatement(new[] {"{0}"});
            _updateIdentity = SqlBuilder.GetUpdateStatement(new[] {"{0}"});
            _counts = SqlBuilder.GetCountStatement("1=1");
            _count = SqlBuilder.GetCountStatement();
            _selectIdentity = SqlBuilder.GetSelectStatement();
        }

        #region IDataTable
        public bool Insert<T>(T entity)
            where T : class, IDataEntity
        {
            Assert.ArgumentNotNull(nameof(entity), entity);
            return DataSource.UseSession(Name, session => session.Execute(_insert, p =>
            {
                foreach (var name in entity)
                    p[name] = entity[name];
            }) == 1);
        }

        public bool Delete(string id)
        {
            Assert.ArgumentNotNull(nameof(id), id);
            return DataSource.UseSession(Name, session => session.Execute(_deleteIdentity, p =>
            {
                p[_identityName] = id;
            }) == 1);
        }

        public bool Delete<T>(T entity)
            where T : class, IDataEntity
        {
            Assert.ArgumentNotNull(nameof(entity), entity);
            return Delete(entity.Id);
        }

        public int Delete<T>(Expression<Func<T, bool>> condition)
            where T : class, IDataEntity
        {
            Assert.ArgumentNotNull(nameof(condition), condition);
            return DataSource.UseSession(Name, session =>
            {
                var where = ExpressionParser.Parse(condition, DataSource.Provider.ParameterSymbol);
                return session.Execute(_delete + where.Condition, p =>
                {
                    foreach (var pair in where.Parameters)
                        p[pair.Key] = pair.Value;
                });
            });
        }

        public bool Update<T>(T entity)
            where T : class, IDataEntity
        {
            Assert.ArgumentNotNull(nameof(entity), entity);
            var set = new StringBuilder();
            foreach (var name in entity)
            {
                set.AppendFormat("{0}={1}{2},", name, DataSource.Provider.ParameterSymbol, name);
            }
            if (set.Length > 0)
                set.Remove(set.Length - 1, 1);
            return DataSource.UseSession(Name, session => session.Execute(string.Format(_updateIdentity, set), p =>
            {
                p[_identityName] = entity.Id;
                foreach (var name in entity)
                {
                    p[name] = entity[name];
                }
            }) == 1);
        }

        public int Update<T>(object entity, Expression<Func<T, bool>> condition)
            where T : class, IDataEntity
        {
            Assert.ArgumentNotNull(nameof(entity), entity);
            Assert.ArgumentNotNull(nameof(condition), condition);
            return DataSource.UseSession(Name, session =>
            {
                var set = ExpressionParser.Parse(entity, DataSource.Provider.ParameterSymbol);
                var where = ExpressionParser.Parse(condition, DataSource.Provider.ParameterSymbol);
                return session.Execute(string.Format(_update, set.Condition) + where.Condition, p =>
                {
                    foreach (var pair in set.Parameters)
                        p[pair.Key] = pair.Value;
                    foreach (var pair in where.Parameters)
                        p[pair.Key] = pair.Value;
                });
            });
        }

        public int Update<T>(IDictionary<string, object> sets, Expression<Func<T, bool>> condition)
            where T : class, IDataEntity
        {
            Assert.ArgumentNotNull(nameof(sets), sets);
            Assert.ArgumentNotNull(nameof(condition), condition);
            return DataSource.UseSession(Name, session =>
            {
                var set = ExpressionParser.Parse(sets, DataSource.Provider.ParameterSymbol);
                var where = ExpressionParser.Parse(condition, DataSource.Provider.ParameterSymbol);
                return session.Execute(string.Format(_update, set.Condition) + where.Condition, p =>
                {
                    foreach (var pair in set.Parameters)
                        p[pair.Key] = pair.Value;
                    foreach (var pair in where.Parameters)
                        p[pair.Key] = pair.Value;
                });
            });
        }

        public long Count() => DataSource.UseSession(Name, session => session.ExecuteScalar<long>(_counts));

        public long Count<T>(Expression<Func<T, bool>> condition)
            where T : class, IDataEntity
        {
            Assert.ArgumentNotNull(nameof(condition), condition);
            return DataSource.UseSession(Name, session =>
            {
                var where = ExpressionParser.Parse(condition, DataSource.Provider.ParameterSymbol);
                return session.ExecuteScalar<long>(_count + where.Condition, p =>
                {
                    foreach (var pair in where.Parameters)
                        p[pair.Key] = pair.Value;
                });
            });
        }

        public T NewEntity<T>(string id = null)
            where T : class, IDataEntity
        {
            var entity = (DataEntity)_entityPrototype.Clone();
            entity.Id = id ?? Common.Guid.New();
            return entity as T;
        }

        public IReadOnlyList<T> Query<T>(Func<IDataQuery<T>, IDataQuery<T>> queryFunction) 
            where T : class, IDataEntity, new()
        {
            if (queryFunction == null)
                return new ReadOnlyCollection<T>(new List<T>(0));
            using (var query = queryFunction(new DataQuery<T>(DataSource, Name)))
            {
                return query.Execute();
            }
        }

        public T Query<T>(string id)
            where T : class, IDataEntity, new()
        {
            Assert.ArgumentNotNull(nameof(id), id);
            return DataSource.UseSession(Name, session =>
            {
                var entities = session.GetEntities<T>(_selectIdentity, p =>
                {
                    p[_identityName] = id;
                });

                if (entities == null || entities.Count < 1)
                    return null;
                return entities[0];
            });
        }
        #endregion

        #region IDataQuery
        private class DataQuery<T> : IDataQuery<T>
            where T : IDataEntity, new()
        {
            private string _select;

            private string _from;

            private Expression<Func<T, bool>> _where;

            private Dictionary<string, DataSort> _orderBy;

            private IDataSource _source;

            public DataQuery(IDataSource source, string name)
            {
                _source = source;
                _from = name;
                _select = string.Format("SELECT * FROM {0}", name);
                _orderBy = new Dictionary<string, DataSort>();
            }

            public void Dispose()
            {
                _source = null;
                _from = null;
                _select = null;
                _where = null;
                _orderBy.Clear();
                _orderBy = null;
            }

            public IDataQuery<T> Where(Expression<Func<T, bool>> condition)
            {
                _where = condition;
                return this;
            }

            public IDataQuery<T> OrderBy(string name, DataSort sort = DataSort.Ascending)
            {
                _orderBy[name] = sort;
                return this;
            }

            public IReadOnlyList<T> Execute()
            {
                return _source.UseSession(_from, session =>
                {
                    var where = ExpressionParser.Parse(_where, _source.Provider.ParameterSymbol);

                    var query = string.Format("{0} {1} {2}",
                            _select, GetWhere(where.Condition), GetOrderBy()).Trim();
                    return session.GetEntities<T>(query, p =>
                    {
                        foreach (var pair in where.Parameters)
                            p[pair.Key] = pair.Value;
                    });
                });
            }

            private static string GetWhere(string condition)
            {
                if (string.IsNullOrEmpty(condition))
                    return string.Empty;
                return " WHERE " + condition;
            }

            private string GetOrderBy()
            {
                if (_orderBy.Count == 0)
                    return string.Empty;
                var builder = new StringBuilder(" ORDER BY ");
                foreach (var order in _orderBy)
                {
                    builder.AppendFormat("{0} {1},", order.Key, (order.Value == DataSort.Ascending ? "ASC" : "DESC"));
                }
                if (builder.Length > 0)
                    builder.Remove(builder.Length - 1, 1);
                return builder.ToString();
            }
        }
        #endregion
    }
}