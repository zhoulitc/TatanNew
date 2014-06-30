// ReSharper disable once CheckNamespace
namespace Tatan.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Common.Exception;
    using Common.Logging;
    using Common.Collections;

    /// <summary>
    /// 数据库抽象会话类，处理一些通用的会话操作
    /// </summary>
    internal sealed class DataSession : IDataSession, IDisposable
    {
        #region 私有变量
        private readonly DbCommand _command;
        private readonly IDataProvider _provider;

        private static readonly object _lock = new object();
        private static readonly ListMap<string, DbProviderFactory> _dbFactories; //工厂集

        #endregion

        #region 构造函数
        public DataSession(string id, IDataProvider provider)
        {
            ExceptionHandler.ArgumentNull("provider", provider);
            if (string.IsNullOrEmpty(id) || id.Length > 128)
                id = "0";
            Id = id;

            var dbFactory = Get(provider.Name);
            var connection = dbFactory.CreateConnection();
            ExceptionHandler.ArgumentNull("connection", connection);
// ReSharper disable once PossibleNullReferenceException
            connection.ConnectionString = provider.ConnectionString;
            _command = connection.CreateCommand();
            _provider = provider;
        }

        static DataSession()
        {
            _dbFactories = new ListMap<string, DbProviderFactory>(20);
        }
        #endregion

        #region IDataSession

        public int Timeout { private get; set; }

        #region 数据处理
        public IDataEntities<T> GetEntities<T>(string request, Action<IDataParameters> action = null)
            where T : IDataEntity, new()
        {
            using (var reader = _Execute(request, action, () => _command.ExecuteReader()))
            {
                return _GetEntities<T>(reader);
            }
        }

        public async Task<IDataEntities<T>> GetEntitiesAsync<T>(string request, Action<IDataParameters> action = null)
            where T : IDataEntity, new()
        {
            using (var reader = await _Execute(request, action, () => _command.ExecuteReaderAsync()))
            {
                return _GetEntities<T>(reader);
            }
        }

        public T ExecuteScalar<T>(string request, Action<IDataParameters> action = null)
        {
            var value = _Execute(request, action, () => _command.ExecuteScalar());
            if (value == null)
                return default(T);
            return (T)value;
        }

        public async Task<T> ExecuteScalarAsync<T>(string request, Action<IDataParameters> action = null)
        {
            var value = await _Execute(request, action, () => _command.ExecuteScalarAsync());
            if (value == null)
                return default(T);
            return (T)value;
        }

        public T ExecuteReader<T>(string request, Func<IDataReader, T> function)
        {
            using (var reader = _Execute(request, null, () => _command.ExecuteReader()))
            {
                return function(reader);
            }
        }

        public async Task<T> ExecuteReaderAsync<T>(string request, Func<IDataReader, Task<T>> function)
        {
            using (var reader = await _Execute(request, null, () => _command.ExecuteReaderAsync()))
            {
                return await function(reader);
            }
        }

        public T ExecuteReader<T>(string request, Action<IDataParameters> action, Func<IDataReader, T> function)
        {
            using (var reader = _Execute(request, action, () => _command.ExecuteReader()))
            {
                return function(reader);
            }
        }

        public async Task<T> ExecuteReaderAsync<T>(string request, Action<IDataParameters> action, Func<IDataReader, Task<T>> function)
        {
            using (var reader = await _Execute(request, action, () => _command.ExecuteReaderAsync()))
            {
                return await function(reader);
            }
        }

        public int Execute(string command, Action<IDataParameters> action = null)
        {
            return _Execute(command, action, () => _command.ExecuteNonQuery());
        }

        public async Task<int> ExecuteAsync(string command, Action<IDataParameters> action = null)
        {
            return await _Execute(command, action, () => _command.ExecuteNonQueryAsync());
        }
        #endregion

        #region 事务处理
        public IDbTransaction BeginTransaction(IsolationLevel lockLevel = IsolationLevel.Unspecified)
        {
            if (_command.Connection.State != ConnectionState.Open)
                _command.Connection.Open();
            _command.Transaction = _command.Connection.BeginTransaction(lockLevel);
            return _command.Transaction;
        }
        #endregion

        #region 参数设置
        internal sealed class DataParameters : IDataParameters, IDisposable
        {
            private IDataParameterCollection _parameters;
            private Func<IDbDataParameter> _createParamter;
            private readonly string _symbol;
            private static readonly ListMap<DataType, DbType> _mapping;

            static DataParameters()
            {
                _mapping = new ListMap<DataType, DbType>
                {
                    { DataType.Binary, DbType.Binary },
                    { DataType.Boolean, DbType.Boolean },
                    { DataType.Date, DbType.DateTime },
                    { DataType.Integer, DbType.Int32 },
                    { DataType.Number, DbType.Double },
                    { DataType.String, DbType.String },
                    { DataType.Object, DbType.Object }
                };
            }

            public DataParameters(IDataParameterCollection parameters, Func<IDbDataParameter> createParamter, string symbol)
            {
                _parameters = parameters;
                _parameters.Clear();
                _createParamter = createParamter;
                _symbol = symbol;
            }
            public void Dispose()
            {
                _parameters.Clear();
                _parameters = null;
                _createParamter = null;
            }

            public object this[int index, DataType type = DataType.Object, int size = 0] 
            {
                set
                {
                    if (index < 0 || index > _parameters.Count)
                        throw new IndexOutOfRangeException(ExceptionHandler.GetText("IndexOutOfRange"));
                    SetOtherValue(TryCreate(index), type, size, value);
                }
            }

            public object this[int index, byte size, byte scale = 0] 
            {
                set
                {
                    if (index < 0 || index > _parameters.Count)
                        throw new IndexOutOfRangeException(ExceptionHandler.GetText("IndexOutOfRange"));
                    SetNumberValue(TryCreate(index), size, scale, value);
                }
            }

            public object this[string name, DataType type = DataType.Object, int size = 0] 
            {
                set
                {
                    ExceptionHandler.ArgumentNull("name", name);
                    SetOtherValue(TryCreate(name), type, size, value);
                }
            }

            public object this[string name, byte size, byte scale = 0] 
            {
                set 
                {
                    ExceptionHandler.ArgumentNull("name", name);
                    SetNumberValue(TryCreate(name), size, scale, value);
                }
            }

            private IDbDataParameter TryCreate(int index)
            {
                IDbDataParameter parameter = null;
                if (index == _parameters.Count)
                {
                    parameter = _createParamter();
                    _parameters.Add(parameter);
                }
                return parameter;
            }

            private IDbDataParameter TryCreate(string name)
            {
                name = _symbol + name;
                IDbDataParameter parameter = null;
                if (!_parameters.Contains(name))
                {
                    parameter = _createParamter();
                    parameter.ParameterName = name;
                    _parameters.Add(parameter);
                }
                return parameter;
            }

            private static void SetNumberValue(IDbDataParameter parameter, byte size, byte scale, object value)
            {
                if (parameter != null)
                {
                    if (value == null)
                    {
                        SetNullValue(parameter);
                    }
                    else
                    {
                        parameter.DbType = DbType.Double;
                        parameter.Precision = size;
                        parameter.Scale = scale;
                        parameter.Value = value;
                    }
                }
            }

            private static void SetOtherValue(IDbDataParameter parameter, DataType type, int size, object value)
            {
                if (parameter != null)
                {
                    if (value == null)
                    {
                        SetNullValue(parameter);
                    }
                    else
                    {
                        parameter.DbType = _mapping[type];
                        parameter.Size = size;
                        parameter.Value = value;
                    }
                }
            }

            private static void SetNullValue(IDataParameter parameter)
            {
                parameter.DbType = DbType.Object;
                parameter.Value = DBNull.Value;
            }
        }
        #endregion
        #endregion

        #region IDentifiable
        public string Id { get; internal set; }
        #endregion

        #region IDisposable
        ~DataSession()
        {
            if (_command == null)
                return;
            _command.Dispose();
        }

        /// <summary>
        /// 销毁属性集合
        /// </summary>
        void IDisposable.Dispose()
        {
            if (_command == null)
                return;
            if (_command.Transaction != null)
            {
                _command.Transaction.Dispose();
                _command.Transaction = null;
            }
            _command.Connection.Close();
        }
        #endregion

        #region IDataEntities
        private class DataEntities<T> : IDataEntities<T>
            where T : IDataEntity, new()
        {
            public readonly IList<T> Entities;
            public DataEntities(int capacity)
            {
                Entities = new List<T>(capacity);
            }

            #region IReadOnlyList
            public int Count
            {
                get
                {
                    return Entities.Count;
                }
            }
            public T this[int index] 
            {
                get
                {
                    ExceptionHandler.IndexOutOfRange(index, Count);
                    return Entities[index];
                }
            }

            public IEnumerator<T> GetEnumerator()
            {
                return Entities.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return Entities.GetEnumerator();
            }
            #endregion
        }
        #endregion

        #region Protected Method
        private void _PrepareCommand(string text)
        {
            text = text.Trim();
            _command.CommandText = _Check(text, _provider.CallStoredProcedure);
            _command.CommandType = _SetCommandType(text, _provider);
            _command.CommandTimeout = Timeout <= 0 ? 15 : Timeout;
            if (_command.Connection.State != ConnectionState.Open)
                _command.Connection.Open();
            _command.Prepare();
        }

        private static CommandType _SetCommandType(string text, IDataProvider provider)
        {
            if (string.IsNullOrEmpty(provider.CallStoredProcedure))
                return CommandType.Text;
            return (text.Contains(" ")) ? CommandType.Text : CommandType.StoredProcedure;
        }

        private static string _Check(string text, string call)
        {
            ExceptionHandler.IllegalSql(text, call);
            return text;
        }

        private T _Execute<T>(string text, Action<IDataParameters> action, Func<T> function)
        {
            var result = default(T);
            ExceptionHandler.ArgumentNull("text", text);
            try
            {
                _PrepareCommand(text);
                if (action != null)
                    action(new DataParameters(_command.Parameters, _command.CreateParameter, _provider.ParameterSymbol));
                result = function();
            }
            catch (Exception ex)
            {
                Log.Default.Error(typeof(DataSession), ex.Message, ex);
                _command.Cancel();
                ExceptionHandler.DatabaseError(ex);
            }
            return result;
        }

        private static DataEntities<T> _GetEntities<T>(IDataReader reader)
            where T : IDataEntity, new()
        {
            if (reader == null)
                return new DataEntities<T>(0);
            var entities = new DataEntities<T>(20);
            while (reader.Read())
            {
                var entity = new T();
                for (var j = 0; j < reader.FieldCount; j++)
                {
                    if (reader.GetName(j) == "Id")
                        SetId(entity, reader.GetInt64(j));
                    else if (!reader.IsDBNull(j))
                        entity[reader.GetName(j)] = reader.GetValue(j);
                }
                entities.Entities.Add(entity);
            }
            return entities;
        }

        private static void SetId(object obj, long id)
        {
            var entity = obj as DataEntity;
            if (entity != null)
            {
                entity.Id = id;
            }
        }

        private static DbProviderFactory Get(string name)
        {
            if (!_dbFactories.Contains(name))
            {
                lock (_lock)
                {
                    _dbFactories.Add(name, DbProviderFactories.GetFactory(name));
                }
            }
            return _dbFactories[name];
        }
        #endregion
    }
}