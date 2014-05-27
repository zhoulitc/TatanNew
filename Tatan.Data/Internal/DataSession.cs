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

    /// <summary>
    /// 数据库抽象会话类，处理一些通用的会话操作
    /// </summary>
    internal sealed class DataSession : IDataSession, IDisposable
    {
        #region 私有变量
        private DbCommand _command;
        private readonly IDataParameters _parameters;

        #endregion

        #region 构造函数
        public DataSession(DataSource source, string id, string connectionString)
        {
            ExceptionHandler.ArgumentNull("source", source);
            ExceptionHandler.ArgumentNull("connectionString", connectionString);

            if (string.IsNullOrEmpty(id) || id.Length > 128)
                id = "0";
            Id = id;

            var dbFactory = DataSource.Get(source.Provider.Name);
            var conn = dbFactory.CreateConnection();
            ExceptionHandler.ArgumentNull("conn", conn);
// ReSharper disable once PossibleNullReferenceException
            conn.ConnectionString = connectionString;
            _command = conn.CreateCommand();
            _parameters = new DataParameters(_command, source);
        }
        #endregion

        #region IDataSession
        #region 数据处理
        public IDataDocument GetData(string request, Action<IDataParameters> action = null)
        {
            using (var reader = _Execute(request, action, () => _command.ExecuteReader()))
            {
                var document = new DataDocument(reader);
                return document;
            }
        }

        public async Task<IDataDocument> GetDataAsync(string request, Action<IDataParameters> action = null)
        {

            using (var reader = await _Execute(request, action, () => _command.ExecuteReaderAsync()))
            {
                var document = new DataDocument(reader);
                return document;
            }
        }

        public IDataEntities<T> GetEntities<T>(string request, Action<IDataParameters> action = null, int begin = 0, int end = 0)
            where T : IDataEntity, new()
        {
            DbDataReader reader;
            if (begin == 0 && end == 0)
                reader = _Execute(request, action, () => _command.ExecuteReader());
            else
                reader = _Execute(request, action, () => _command.ExecuteReader(), begin, end);
            var entities = _GetEntities<T>(reader, begin, end);
            reader.Close();
            return entities;
        }

        public async Task<IDataEntities<T>> GetEntitiesAsync<T>(string request, Action<IDataParameters> action = null, int begin = 0, int end = 0)
            where T : IDataEntity, new()
        {
            DbDataReader reader;
            if (begin == 0 && end == 0)
                reader = await _Execute(request, action, () => _command.ExecuteReaderAsync());
            else
                reader = await _Execute(request, action, () => _command.ExecuteReaderAsync(), begin, end);
            var entities = _GetEntities<T>(reader, begin, end);
            reader.Close();
            return entities;
        }

        public T GetScalar<T>(string request, Action<IDataParameters> action = null)
        {
            var value = _Execute(request, action, () => _command.ExecuteScalar());
            if (value == null)
                return default(T);
            return (T)value;
        }

        public async Task<T> GetScalarAsync<T>(string request, Action<IDataParameters> action = null)
        {
            var value = await _Execute(request, action, () => _command.ExecuteScalarAsync());
            if (value == null)
                return default(T);
            return (T)value;
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
        internal sealed class DataParameters : IDataParameters
        {
            private readonly IDbCommand _command;
            private readonly string _symbol;
            private static readonly IDictionary<DataType, DbType> _mapping;
            static DataParameters()
            {
                _mapping = new Dictionary<DataType, DbType>
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

            public DataParameters(IDbCommand command, IDataSource source)
            {
                _command = command;
                _symbol = source.Provider.ParameterSymbol;
            }

            public object this[int index, DataType type = DataType.Object, int size = 0] 
            {
                set
                {
                    if (index < 0 || index > _command.Parameters.Count)
                        throw new IndexOutOfRangeException(ExceptionHandler.GetText("IndexOutOfRange"));
                    IDbDataParameter param;
                    if (index == _command.Parameters.Count)
                    {
                        param = _command.CreateParameter();
                        _command.Parameters.Add(param);
                    }
                    param = _command.Parameters[index] as IDbDataParameter;
                    if (param != null)
                    {
                        param.DbType = _mapping[type];
                        param.Size = size;
                        param.Value = value;
                    }
                }
            }

            public object this[int index, byte precision, byte scale = 0] 
            {
                set
                {
                    if (index < 0 || index > _command.Parameters.Count)
                        throw new IndexOutOfRangeException(ExceptionHandler.GetText("IndexOutOfRange"));
                    IDbDataParameter param;
                    if (index == _command.Parameters.Count)
                    {
                        param = _command.CreateParameter();
                        _command.Parameters.Add(param);          
                    }
                    param = _command.Parameters[index] as IDbDataParameter;
                    if (param != null)
                    {
                        param.DbType = DbType.Double;
                        param.Precision = precision;
                        param.Scale = scale;
                        param.Value = value;
                    }
                }
            }

            public object this[string name, DataType type = DataType.Object, int size = 0] 
            {
                set
                {
                    ExceptionHandler.ArgumentNull("name", name);
                    name = _symbol + name;
                    IDbDataParameter param;
                    if (!_command.Parameters.Contains(name))
                    {
                        param = _command.CreateParameter();
                        param.ParameterName = name;
                        _command.Parameters.Add(param);
                    }
                    param = _command.Parameters[name] as IDbDataParameter;
                    if (param != null)
                    {
                        param.DbType = _mapping[type];
                        param.Size = size;
                        param.Value = value;
                    }
                }
            }

            public object this[string name, byte size, byte scale = 0] 
            {
                set 
                {
                    ExceptionHandler.ArgumentNull("name", name);
                    name = _symbol + name;
                    IDbDataParameter param;
                    if (!_command.Parameters.Contains(name))
                    {
                        param = _command.CreateParameter();
                        param.ParameterName = name;
                        _command.Parameters.Add(param);
                    }
                    param = _command.Parameters[name] as IDbDataParameter;
                    if (param != null)
                    {
                        param.DbType = DbType.Double;
                        param.Precision = size;
                        param.Scale = scale;
                        param.Value = value;
                    }
                }
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
            if (_command.Parameters.Count > 0)
                _command.Parameters.Clear();
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
            public int TotalCount { get; internal set; }
            public DataEntities(int capacity)
            {
                Entities = new List<T>(capacity);
                TotalCount = 0;
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
        private bool _IsStoredProcedure(string text)
        {
            return !(text.Trim().Contains(" "));
        }

        private void _PrepareCommand(string text)
        {
            _command.CommandText = text; //TODO 检查SQL
            _command.CommandType = _IsStoredProcedure(text) ? CommandType.StoredProcedure : CommandType.Text;
            if (_command.Parameters.Count > 0)
                _command.Parameters.Clear();
            if (_command.Connection.State != ConnectionState.Open)
                _command.Connection.Open();
            _command.Prepare();
        }

        private T _Execute<T>(string text, Action<IDataParameters> action, Func<T> function)
        {
            var result = default(T);
            if (string.IsNullOrEmpty(text))
                return result;
            try
            {
                _PrepareCommand(text);
                if (action != null)
                    action(_parameters);
                result = function();
            }
            catch (Exception ex)
            {
                Log.Default.Error(typeof(DataSession), ex.Message, ex);
                _command.Cancel();
                ExceptionHandler.DatabaseError(ex);
            }
            finally
            {
                if (_command.Parameters.Count > 0)
                    _command.Parameters.Clear();
            }
            return result;
        }

        private T _Execute<T>(string text, Action<IDataParameters> action, Func<T> function, int begin, int end)
        {
            var result = default(T);
            if (string.IsNullOrEmpty(text))
                return result;
            try
            {
                //_PrepareCommand(_PagerName);
                if (action != null)
                    action(_parameters);
                _parameters["select_sql", DataType.String, 8000] = text;
                _parameters["table_name", DataType.String, 100] = _GetTableName(text);
                _parameters["begin", DataType.Integer] = begin;
                _parameters["end", DataType.Integer] = end;
                result = function();
            }
            catch (Exception ex)
            {
                Log.Default.Error(typeof(DataSession), ex.Message, ex);
                _command.Cancel();
                ExceptionHandler.DatabaseError(ex);
            }
            finally
            {
                if (_command.Parameters.Count > 0)
                    _command.Parameters.Clear();
            }
            return result;
        }

        private string _GetTableName(string text)
        {
            string table;
            var sql = text.ToUpper();
            var stop = 0;
            const string from = " FROM ";
            const string trim = " ";
            do
            {
                int start = sql.IndexOf(from, stop, StringComparison.Ordinal);
                stop = sql.IndexOf(trim, start + @from.Length, StringComparison.Ordinal);
// ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                if (stop == -1)
                    table = text.Substring(start + from.Length);
                else
                    table = text.Substring(start + from.Length, stop - start - from.Length);
            }
            while (table.StartsWith("("));
            table = table.Replace(")", "").Trim();
            return table;
        }

        private DataEntities<T> _GetEntities<T>(IDataReader reader, int begin, int end)
            where T : IDataEntity, new()
        {
            if (reader == null)
                return new DataEntities<T>(0);
            var entities = new DataEntities<T>(end - begin + 1);
            while (reader.Read())
            {
                var entity = new T();
                for (var j = 0; j < reader.FieldCount; j++)
                {
                    if (reader.GetName(j) == "Id")
                        SetId(entity, reader.GetInt32(j));
                    else if (!reader.IsDBNull(j))
                        entity[reader.GetName(j)] = reader.GetValue(j);
                }
                entities.Entities.Add(entity);
            }
            if (reader.NextResult() && reader.Read() && reader.FieldCount == 1)
            {
                entities.TotalCount = reader.GetInt32(0);
            }
            return entities;
        }

        private void SetId(object obj, int id)
        {
            if (obj is DataEntity)
            {
                obj.GetType().GetProperty("Id").SetValue(obj, id);
            }
        }
        #endregion
    }
}