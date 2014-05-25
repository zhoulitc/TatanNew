using Tatan.Common.Expression;

// ReSharper disable once CheckNamespace


namespace Tatan.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Text;
    using Common.Exception;

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
                return session.Execute(_count + where.Condition, p =>
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
        #endregion
    }
}