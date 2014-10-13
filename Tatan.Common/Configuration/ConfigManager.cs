using System.IO;

namespace Tatan.Common.Configuration
{
    using System;
    using System.Text;
    using Serialization;
    using Exception;
    using SystemFile = File;
    using System.Configuration;
    using System.Collections.Generic;

    /// <summary>
    /// 配置文件管理者
    /// </summary>
    public static class ConfigManager
    {
        private static readonly IDictionary<string, string> _files;
        private static readonly IDictionary<string, DateTime> _fileDateTimes;
        private static readonly IDictionary<string, IConfig> _configures;
        private static readonly IDictionary<string, object> _xmlConfigures;
        private static readonly AppSettingsConfig _appConfig;
        private static readonly ConnectionStringsConfig _connectionConfig;
        private static readonly NullConfig _nullConfig;
        private static readonly object _lock;

        static ConfigManager()
        {
            _lock = new object();
            _files = new Dictionary<string, string>();
            _fileDateTimes = new Dictionary<string, DateTime>();
            _configures = new Dictionary<string, IConfig>();
            _xmlConfigures = new Dictionary<string, object>();
            _nullConfig = new NullConfig();
            _appConfig = new AppSettingsConfig();
            _connectionConfig = new ConnectionStringsConfig();
        }

        /// <summary>
        /// 注册一个配置文件到IConfig集合中
        /// </summary>
        /// <param name="path">配置文件路径</param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        public static void Register(string path)
        {
            Assert.FileFound(path);

            var name = Path.GetFileNameWithoutExtension(path);
            lock (_lock)
            {
                if (!_configures.ContainsKey(name))
                {
                    _configures.Add(name, Load(path));
                    if (!_files.ContainsKey(name))
                        _files.Add(name, path);
                    if (!_fileDateTimes.ContainsKey(name))
                        _fileDateTimes.Add(name, SystemFile.GetLastWriteTime(path));
                }
            }
        }

        /// <summary>
        /// 注册一个配置文件到一个实体类中
        /// </summary>
        /// <param name="path">配置文件路径</param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        public static void Register<T>(string path)
        {
            Assert.ArgumentNotNull("path", path);
            Assert.FileFound(path);
            if (string.Compare(Path.GetExtension(path), "xml", true) != 0)
                return;

            var name = Path.GetFileNameWithoutExtension(path);
            lock (_lock)
            {
                if (!_xmlConfigures.ContainsKey(name))
                {
                    var content = SystemFile.ReadAllText(path, Encoding.UTF8);
                    var config = Serializer.Xml.Deserialize<T>(content);
                    _xmlConfigures.Add(name, config);
                    if (!_files.ContainsKey(name))
                        _files.Add(name, path);
                    if (!_fileDateTimes.ContainsKey(name))
                        _fileDateTimes.Add(name, SystemFile.GetLastWriteTime(path));

                }
            }
        }

        /// <summary>
        /// 配置文件的具体加载逻辑
        /// </summary>
        public static Func<string, IDictionary<string, string>> OnLoading { private get; set; }

        /// <summary>
        /// 获取指定配置文件(不是路径)，如果文件被更新过，则会重新加载
        /// <para>请使用System.Xml.Serialization空间中的特性标明</para>
        /// </summary>
        /// <typeparam name="T">具体的配置文件类型</typeparam>
        /// <param name="name">配置文件名(不是路径)</param>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.FileNotFoundException">文件没有找到时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.IO.IOException">发生I/O错误时</exception>
        /// <exception cref="System.NotSupportedException">文件格式无效时</exception>
        /// <exception cref="System.Security.SecurityException">没有权限时</exception>
        /// <exception cref="System.Text.DecoderExceptionFallback">回退时</exception>
        /// <returns>配置文件对象</returns>
        public static T Get<T>(string name = null) where T : class
        {
            if (string.IsNullOrEmpty(name))
                name = typeof (T).Name;
            Assert.KeyFound(_xmlConfigures, name);

            Reload<T>(name);
            return _xmlConfigures[name] as T;
        }

        /// <summary>
        /// 获取指定配置文件(不是路径)，如果文件被更新过，则会重新加载
        /// </summary>
        /// <param name="name">配置文件名(不是路径)</param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        public static IConfig Get(string name)
        {
            Assert.ArgumentNotNull("name", name);
            Reload(name);
            if (!_configures.ContainsKey(name))
                return _nullConfig;
            return _configures[name];
        }

        /// <summary>
        /// 获取默认配置文件项
        /// </summary>
        /// <returns></returns>
        public static IConfig AppConfig
        {
            get { return _appConfig; }
        }

        /// <summary>
        /// 获取默认配置文件项
        /// </summary>
        /// <returns></returns>
        public static IConfig<DbConfig> ConnectionConfig
        {
            get { return _connectionConfig; }
        }

        private static void Reload(string name)
        {
            if (!_configures.ContainsKey(name))
                return;

            var path = _files[name];
            if (SystemFile.Exists(path) && SystemFile.GetLastWriteTime(path) > _fileDateTimes[name])
            {
                lock (_lock)
                {
                    _configures[name] = Load(path);
                }
            }
        }

        private static void Reload<T>(string name)
        {
            if (!_configures.ContainsKey(name))
                return;

            var path = _files[name];
            if (SystemFile.Exists(path) && SystemFile.GetLastWriteTime(path) > _fileDateTimes[name])
            {
                lock (_lock)
                {
                    var content = SystemFile.ReadAllText(path, Encoding.UTF8);
                    var config = Serializer.Xml.Deserialize<T>(content);
                    _xmlConfigures[name] = config;
                }
            }
        }

        private static IConfig Load(string path)
        {
            return new DefaultConfig(path);
        }

        private class NullConfig : IConfig
        {
            public string this[string key]
            {
                get { return string.Empty; }
            }
        }

        private class DefaultConfig : IConfig
        {
            private readonly IDictionary<string, string> _configs;

            public DefaultConfig(string path)
            {
                _configs = OnLoading(path);
            }

            public string this[string key]
            {
                get
                {
                    Assert.ArgumentNotNull("key", key);
                    if (!_configs.ContainsKey(key)) return string.Empty;
                    return _configs[key];
                }
            }
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
                    Assert.ArgumentNotNull("key", key);
                    if (!_configs.ContainsKey(key)) return string.Empty;
                    return _configs[key];
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
                    Assert.ArgumentNotNull("key", key);
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