// ReSharper disable once CheckNamespace
namespace Tatan.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Text;
    using Common.Exception;
    using Common.Expression;

    internal class DataTable : IDataTable
    {
        public string Name { get; internal set; }

        public IDataSource DataSource { get; internal set; }

        #region SQL
        //实体原型
        private readonly IDataEntity _entityPrototype;

        //唯一标识符名称
        private const string _identityName = "Id";

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
            var constructorInfo = type.GetConstructor(new[] { typeof(int) });
            if (constructorInfo != null)
                _entityPrototype = (IDataEntity)constructorInfo.Invoke(new object[] { DataEntity.DefaultId });

            //初始化SQL语句
            _insert = string.Format("INSERT INTO {0}({1}) VALUES({2})", Name, GetFields(_entityPrototype), GetParameters(_entityPrototype));
            _delete = string.Format("DELETE FROM {0} WHERE ", Name);
            _deleteIdentity = string.Format("{0}{1}={2}", _delete, _identityName, DataSource.Provider.ParameterSymbol + _identityName);
            _update = string.Format("UPDATE {0} SET {{0}} WHERE ", Name);
            _updateIdentity = string.Format("{0}{1}={2}", _update, _identityName, DataSource.Provider.ParameterSymbol + _identityName);
            _counts = string.Format("SELECT COUNT(1) FROM {0}", Name);
            _count = string.Format("{0} WHERE ", _counts);
        }

        private static string GetFields(IEnumerable<string> entityPrototype)
        {
            var builder = new StringBuilder(100);
            foreach (var name in entityPrototype)
                builder.Append(',').Append(name);
            if (builder.Length > 0)
                builder = builder.Remove(0, 1);
            return builder.ToString();
        }

        private string GetParameters(IEnumerable<string> entityPrototype)
        {
            return DataSource.Provider.ParameterSymbol +
                GetFields(entityPrototype).Replace(",", "," + DataSource.Provider.ParameterSymbol);
        }

        #region IDataTable
        public bool Insert<T>(T entity)
            where T : class, IDataEntity
        {
            ExceptionHandler.ArgumentNull("entity", entity);
            return DataSource.UseSession(Name, session => session.Execute(_insert, p =>
            {
                foreach (var name in entity)
                    p[name] = entity[name];
            }) == 1);
        }

        public bool Delete<T>(T entity)
            where T : class, IDataEntity
        {
            ExceptionHandler.ArgumentNull("entity", entity);
            return DataSource.UseSession(Name, session => session.Execute(_deleteIdentity, p =>
            {
                p[_identityName] = entity.Id;
            }) == 1);
        }

        public int Delete<T>(Expression<Func<T, bool>> condition)
            where T : class, IDataEntity
        {
            ExceptionHandler.ArgumentNull("condition", condition);
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
            ExceptionHandler.ArgumentNull("entity", entity);
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
            ExceptionHandler.ArgumentNull("entity", entity);
            ExceptionHandler.ArgumentNull("condition", condition);
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
            ExceptionHandler.ArgumentNull("sets", sets);
            ExceptionHandler.ArgumentNull("condition", condition);
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

        public long Count()
        {
            return DataSource.UseSession(Name, session => session.GetScalar<long>(_counts));
        }

        public long Count<T>(Expression<Func<T, bool>> condition)
            where T : class, IDataEntity
        {
            ExceptionHandler.ArgumentNull("condition", condition);
            return DataSource.UseSession(Name, session =>
            {
                var where = ExpressionParser.Parse(condition, DataSource.Provider.ParameterSymbol);
                return session.GetScalar<long>(_count + where.Condition, p =>
                {
                    foreach (var pair in where.Parameters)
                        p[pair.Key] = pair.Value;
                });
            });
        }

        public T NewEntity<T>(int id)
            where T : class, IDataEntity
        {
            var entity = (DataEntity)_entityPrototype.Clone();
            entity.Id = id;
            return entity as T;
        }

        public IDataResult<T> Query<T>(Func<IDataQuery<T>, IDataQuery<T>> queryFunction) 
            where T : class, IDataEntity, new()
        {
            if (queryFunction == null)
                return new DataResult<T>();
            using (var query = queryFunction(new DataQuery<T>(DataSource, Name, _counts)))
            {
                return query.Execute();
            }
        }
        #endregion

        #region IDataResult
        private class DataResult<T> : IDataResult<T> where T : IDataEntity, new()
        {
            public IDataEntities<T> Entities { get; set; }
            public long TotalCount { get; set; }
            public IEnumerator<T> GetEnumerator()
            {
                return Entities.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        #endregion

        #region IDataQuery
        private class DataQuery<T> : IDataQuery<T>
            where T : IDataEntity, new()
        {
            private string _select;

            private string _counts;

            private string _from;

            private Expression<Func<T, bool>> _where;

            private Dictionary<string, DataSort> _orderBy;

            private uint _begin;

            private uint _end;

            private IDataSource _source;

            public DataQuery(IDataSource source, string name, string counts)
            {
                _source = source;
                _from = name;
                _select = string.Format("SELECT * FROM {0}", name);
                _counts = counts;
                _orderBy = new Dictionary<string, DataSort>();
            }

            public void Dispose()
            {
                _source = null;
                _from = null;
                _select = null;
                _counts = null;
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
                if (_orderBy.ContainsKey(name))
                    _orderBy[name] = sort;
                else
                    _orderBy.Add(name, sort);
                return this;
            }

            public IDataQuery<T> Range(uint begin, uint end)
            {
                if (begin > end)
                {
                    _begin = 0;
                    _end = 0;
                }
                else
                {
                    _begin = begin < 1 ? 1 : begin;
                    _end = end;
                }
                return this;
            }

            public IDataResult<T> Execute()
            {
                return _source.UseSession(_from, session =>
                {
                    var where = ExpressionParser.Parse(_where, _source.Provider.ParameterSymbol);
                    var result = new DataResult<T>();

                    if (_begin > 0 && _end > 0)
                    {
                        var count = string.Format("{0} {1}",
                            _counts, GetWhere(where.Condition)).Trim();
                        result.TotalCount = session.GetScalar<long>(count, p =>
                        {
                            foreach (var pair in where.Parameters)
                                p[pair.Key] = pair.Value;
                        });

                        var query = string.Format("{0} {1} {2}",
                            _select, GetWhere(where.Condition), GetOrderBy()).Trim();
                        result.Entities = session.GetEntities<T>(query, p =>
                        {
                            foreach (var pair in where.Parameters)
                                p[pair.Key] = pair.Value;
                        });
                    }
                    else
                    {
                        var query = string.Format(@"{0} {1} {2}",
                            _select, GetWhere(where.Condition), GetOrderBy()).Trim();
                        result.Entities = session.GetEntities<T>(query, p =>
                        {
                            foreach (var pair in where.Parameters)
                                p[pair.Key] = pair.Value;
                        });

                        result.TotalCount = result.Entities.Count;
                    }

                    return result;
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