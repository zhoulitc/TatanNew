namespace Tatan.Common.Configuration
{
    using System.Configuration;
    using System.Collections.Generic;

    /// <summary>
    /// 配置项
    /// </summary>
    public static class Configuration
    {
        private static readonly AppSettingsConfig _appConfig = new AppSettingsConfig();

        /// <summary>
        /// 获取默认配置文件项
        /// </summary>
        /// <returns></returns>
        public static IConfig AppSettings
        {
            get { return _appConfig; }
        }

        private static readonly ConnectionStringsConfig _connectionConfig = new ConnectionStringsConfig();

        /// <summary>
        /// 获取默认配置文件项
        /// </summary>
        /// <returns></returns>
        public static IConfig<DbConfig> ConnectionStrings
        {
            get { return _connectionConfig; }
        }

        private class AppSettingsConfig : IConfig
        {
            private readonly IDictionary<string, string> _configs;
            public AppSettingsConfig()
            {
                _configs = new Dictionary<string, string>(ConfigurationManager.AppSettings.Count);
                foreach (var key in ConfigurationManager.AppSettings.Keys)
                {
                    _configs.Add(key.ToString(), ConfigurationManager.AppSettings[key.ToString()]);
                }
            }

            public string this[string key] 
            {
                get
                {
                    if (string.IsNullOrEmpty(key))
                        return string.Empty;
                    return !_configs.ContainsKey(key) ? string.Empty : _configs[key];
                }
            }
        }

        private class ConnectionStringsConfig : IConfig<DbConfig>
        {
            private static readonly DbConfig _nullDbConfig = new DbConfig(string.Empty, string.Empty);
            private readonly IDictionary<string, DbConfig> _configs;
            public ConnectionStringsConfig()
            {
                _configs = new Dictionary<string, DbConfig>(ConfigurationManager.ConnectionStrings.Count);
                foreach (var obj in ConfigurationManager.ConnectionStrings)
                {
                    var setting = obj as ConnectionStringSettings;
                    if (setting == null) continue;
                    _configs.Add(setting.Name, new DbConfig(setting.ProviderName, setting.ConnectionString));
                }
            }

            public DbConfig this[string key]
            {
                get
                {
                    if (string.IsNullOrEmpty(key))
                        return _nullDbConfig;
                    return !_configs.ContainsKey(key) ? _nullDbConfig : _configs[key];
                }
            }
        }

        /// <summary>
        /// 数据库配置项
        /// </summary>
        public struct DbConfig
        {
            /// <summary>
            /// 连接串
            /// </summary>
            public readonly string ConnectionString;

            /// <summary>
            /// 供应商
            /// </summary>
            public readonly string ProviderName;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="providerName"></param>
            /// <param name="connectionString"></param>
            public DbConfig(string providerName, string connectionString)
            {
                ProviderName = providerName;
                ConnectionString = connectionString;
            }
        }
    }
}