namespace Tatan.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using Common.Exception;
    using Common.Configuration;

    /// <summary>
    /// 数据源
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public sealed class DataSource : IDataSource
    {
        private readonly DataTableCollection _tables;
        private readonly DataProvider _dataProvider;
        private readonly DataSession _session;

        internal readonly IDictionary<string, DataSession> Sessions; //会话集

        private static readonly IDictionary<DataProvider, IDataSource> _sources; //数据源集合
        private static readonly object _lock = new object();
        private static DataProvider _defaultDataProvider;

        internal static readonly Dictionary<string, DbProviderFactory> DbFactories; //工厂集

        static DataSource()
        {
            _sources = new Dictionary<DataProvider, IDataSource>();
            DbFactories = new Dictionary<string, DbProviderFactory>(50);
        }

        /// <summary>
        /// 注册一个默认的数据源
        /// </summary>
        /// <param name="configName"></param>
        public static void Register(string configName = "Default")
        {
            var config = Configurations.Connection[configName, "ConnectionString"];
            var provider = Configurations.Connection[configName, "ProviderName"];
            _defaultDataProvider = new DataProvider(config, provider);
            Connect(_defaultDataProvider);
        }

        /// <summary>
        /// 获取默认的数据源，如果没有设置则返回null
        /// </summary>
        public static IDataSource Default
        {
            get
            {
                if (_defaultDataProvider == null) return null;
                return _sources[_defaultDataProvider];
            }
        }

        /// <summary>
        /// 连接一个数据源对象
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IDataSource Connect(string providerName, string connectionString)
        {
            Assert.ArgumentNotNull(nameof(providerName), providerName);
            Assert.ArgumentNotNull(nameof(connectionString), connectionString);
            var provider = new DataProvider(providerName, connectionString);
            return Connect(provider);
        }

        /// <summary>
        /// 连接一个数据源对象
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static IDataSource Connect(string configName)
        {
            Assert.ArgumentNotNull(nameof(configName), configName);
            var config = Configurations.Connection[configName, "ConnectionString"];
            var provider = Configurations.Connection[configName, "ProviderName"];
            return Connect(provider, config);
        }

        private static IDataSource Connect(DataProvider provider)
        {
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
        public IDataProvider Provider => _dataProvider;

        /// <summary>
        /// 获取数据表集合
        /// </summary>
        public DataTableCollection Tables => _tables;

        /// <summary>
        /// 使用数据会话对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="function"></param>
        /// <returns></returns>
        public T UseSession<T>(Func<IDataSession, T> function) => UseSession(null, function);

        /// <summary>
        /// 使用数据会话对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identity"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public T UseSession<T>(string identity, Func<IDataSession, T> function)
        {
            Assert.ArgumentNotNull(nameof(function), function);
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