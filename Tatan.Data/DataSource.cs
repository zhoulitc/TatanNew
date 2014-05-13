// ReSharper disable once CheckNamespace
namespace Tatan.Data
{
    using System;
    using System.Collections.Generic;
    using Common.Exception;

    /// <summary>
    /// 数据源
    /// </summary>
    public sealed class DataSource : IDataSource
    {
        private readonly DataTableCollection _tables;
        private readonly DataProvider _dataProvider;
        internal readonly IDictionary<string, DataSession> Sessions; //会话集
        private readonly DataSession _session;

        private static readonly IDictionary<DataProvider, IDataSource> _sources; //数据源集合
        private static readonly object _lock = new object();
        static DataSource()
        {
            _sources = new Dictionary<DataProvider, IDataSource>();
        }

        /// <summary>
        /// 连接一个数据源对象
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IDataSource Connect(string providerName, string connectionString)
        {
            var provider = new DataProvider(providerName, connectionString);
            if (!_sources.ContainsKey(provider))
            {
                lock (_lock)
                {
                    if (!_sources.ContainsKey(provider))
                    {
                        _sources.Add(provider, new DataSource(provider));
                    }
                }
            }
            return _sources[provider];
        }

        private DataSource(DataProvider provider)
        {
            _dataProvider = provider;
            _tables = new DataTableCollection(this);
            Sessions = new Dictionary<string, DataSession>();
            _session = new DataSession(this, "0", _dataProvider.Name, _dataProvider.Connection);
        }

        /// <summary>
        /// 析构
        /// </summary>
        ~DataSource()
        {
            Dispose();
        }

        public void Dispose()
        {
            _tables.Clear();
            foreach (var session in Sessions.Values)
            {
                session.Dispose();
            }
            Sessions.Clear();
            if (_session != null)
            {
                _session.Dispose();
            }
        }

        #region IDataSource
        /// <summary>
        /// 获取数据供应者
        /// </summary>
        public IDataProvider Provider { get { return _dataProvider; } }

        /// <summary>
        /// 获取数据表集合
        /// </summary>
        public DataTableCollection Tables { get { return _tables; } }

        /// <summary>
        /// 使用数据会话对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identity"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public T UseSession<T>(string identity, Func<IDataSession, T> function)
        {
            ExceptionHandler.ArgumentNull("function", function);
            if (string.IsNullOrEmpty(identity) || identity.Length > 128)
                return function(_session);
            if (!Sessions.ContainsKey(identity))
            {
                Sessions.Add(identity, new DataSession(this, identity, _dataProvider.Name, _dataProvider.Connection));
            }
            using (IDataSession session = Sessions[identity])
            {
                return function(session);
            }
        }
        #endregion
    }
}