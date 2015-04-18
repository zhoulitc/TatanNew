namespace Tatan.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using Common.Exception;
    using Common.Configuration;

    /// <summary>
    /// 数据源
    /// </summary>
    public sealed class DataSource : IDataSource
    {
        private readonly DataTableCollection _tables;
        private readonly DataProvider _dataProvider;
        private readonly DataSession _session;

        internal readonly IDictionary<string, DataSession> Sessions; //会话集

        private static readonly IDictionary<DataProvider, IDataSource> _sources; //数据源集合
        private static readonly object _lock = new object();

        internal static readonly Dictionary<string, DbProviderFactory> DbFactories; //工厂集

        static DataSource()
        {
            _sources = new Dictionary<DataProvider, IDataSource>();
            DbFactories = new Dictionary<string, DbProviderFactory>(50);
        }

        /// <summary>
        /// 连接一个数据源对象
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IDataSource Connect(string providerName, string connectionString)
        {
            Assert.ArgumentNotNull("providerName", providerName);
            Assert.ArgumentNotNull("connectionString", connectionString);
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

        /// <summary>
        /// 连接一个数据源对象
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static IDataSource Connect(string configName)
        {
            Assert.ArgumentNotNull("configName", configName);
            var config = Configurations.Connection[configName, "ConnectionString"];
            var provider = Configurations.Connection[configName, "ProviderName"];
            return Connect(provider, config);
        }

        private DataSource(DataProvider provider)
        {
            _dataProvider = provider;
            _tables = new DataTableCollection(this);
            Sessions = new Dictionary<string, DataSession>();
            _session = new DataSession("0", _dataProvider);
        }

        #region IDisposable
        /// <summary>
        /// 析构函数
        /// </summary>
        ~DataSource()
        {
            Dispose(false);
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposable)
        {
            if (disposable)
            {
                _tables.Clear();
                foreach (var session in Sessions.Values)
                {
                    ((IDisposable)session).Dispose();
                }
                Sessions.Clear();
                if (_session != null)
                {
                    ((IDisposable)_session).Dispose();
                }
            }
        }
        #endregion

        internal static DbProviderFactory Get(string name)
        {
            if (!DbFactories.ContainsKey(name))
            {
                lock (_lock)
                {
                    DbFactories.Add(name, DbProviderFactories.GetFactory(name));
                }
            }
            return DbFactories[name];
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
        /// <param name="function"></param>
        /// <returns></returns>
        public T UseSession<T>(Func<IDataSession, T> function)
        {
            return UseSession(null, function);
        }

        /// <summary>
        /// 使用数据会话对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identity"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public T UseSession<T>(string identity, Func<IDataSession, T> function)
        {
            Assert.ArgumentNotNull("function", function);
            if (string.IsNullOrEmpty(identity) || identity.Length > 128)
                return function(_session);
            if (!Sessions.ContainsKey(identity))
            {
                Sessions.Add(identity, new DataSession(identity, _dataProvider));
            }
            using (var session = Sessions[identity])
            {
                return function(session);
            }
        }
        #endregion
    }
}