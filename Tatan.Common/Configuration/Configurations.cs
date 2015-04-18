using System.IO;
using Tatan.Common.Extension.String.IO;

namespace Tatan.Common.Configuration
{
    using System;
    using System.Text;
    using Serialization;
    using Exception;
    using System.Configuration;
    using System.Collections.Generic;

    /// <summary>
    /// 配置文件管理者
    /// </summary>
    public static class Configurations
    {
        private static readonly IDictionary<string, string> _files;
        private static readonly IDictionary<string, DateTime> _fileDateTimes;
        private static readonly IDictionary<string, IConfiguration> _configures;
        private static readonly IDictionary<string, object> _xmlConfigures;
        private static readonly AppSettingsConfiguration _appConfiguration;
        private static readonly ConnectionStringsConfiguration _connectionConfiguration;
        private static readonly NullConfig _nullConfig;
        private static IConfiguration _currentConfiguration;
        private static readonly object _lock;

        static Configurations()
        {
            _lock = new object();
            _files = new Dictionary<string, string>();
            _fileDateTimes = new Dictionary<string, DateTime>();
            _configures = new Dictionary<string, IConfiguration>();
            _xmlConfigures = new Dictionary<string, object>();
            _nullConfig = new NullConfig();
            _appConfiguration = new AppSettingsConfiguration();
            _currentConfiguration = _appConfiguration;
            _connectionConfiguration = new ConnectionStringsConfiguration();
        }

        /// <summary>
        /// 注册一个配置文件到IConfiguration集合中
        /// </summary>
        /// <param name="path">配置文件路径</param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        public static void Register(string path)
        {
            Assert.ArgumentNotNull("path", path);
            Assert.FileFound(path);

            var name = path.GetFileName(false);

            Assert.KeyFound(_configures, name);
            lock (_lock)
            {
                if (!_configures.ContainsKey(name))
                {
                    _currentConfiguration = Load(path);
                    _configures.Add(name, _currentConfiguration);
                    if (!_files.ContainsKey(name))
                        _files.Add(name, path);
                    if (!_fileDateTimes.ContainsKey(name))
                        _fileDateTimes.Add(name, File.GetLastWriteTime(path));
                }
            }
        }

        /// <summary>
        /// 注册一个配置文件到一个实体类中
        /// </summary>
        /// <param name="path">配置文件路径</param>
        /// <param name="serializer">序列器对象</param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        public static void Register<T>(string path, ISerializer serializer = null)
        {
            Assert.ArgumentNotNull("path", path);
            Assert.FileFound(path);
            if (string.Compare(path.GetExtension(), "xml", true) != 0)
                return;

            var name = path.GetFileName(false);

            Assert.KeyFound(_xmlConfigures, name);
            lock (_lock)
            {
                if (!_xmlConfigures.ContainsKey(name))
                {
                    var content = File.ReadAllText(path, Encoding.UTF8);
                    var config = (serializer ?? Serializers.Xml).Deserialize<T>(content);
                    _xmlConfigures.Add(name, config);
                    if (!_files.ContainsKey(name))
                        _files.Add(name, path);
                    if (!_fileDateTimes.ContainsKey(name))
                        _fileDateTimes.Add(name, File.GetLastWriteTime(path));

                }
            }
        }

        /// <summary>
        /// 配置文件的具体加载逻辑
        /// </summary>
        public static Action<string, IDictionary<string, string>, IDictionary<string, IDictionary<string, string>>> OnLoading { private get; set; }

        /// <summary>
        /// 获取指定配置文件(不是路径)，如果文件被更新过，则会重新加载
        /// <para>请使用System.Xml.Serialization空间中的特性标明</para>
        /// </summary>
        /// <typeparam name="T">具体的配置文件类型</typeparam>
        /// <param name="name">配置文件名(不是路径)</param>
        /// <param name="serializer">序列器对象</param>
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
        public static T Get<T>(string name = null, ISerializer serializer = null) where T : class
        {
            if (string.IsNullOrEmpty(name))
                name = typeof (T).Name;
            Assert.KeyFound(_xmlConfigures, name);

            Reload<T>(name, serializer);
            return _xmlConfigures[name] as T;
        }

        /// <summary>
        /// 获取指定配置文件(不是路径)，如果文件被更新过，则会重新加载
        /// </summary>
        /// <param name="name">配置文件名(不是路径)</param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        public static IConfiguration Get(string name)
        {
            Assert.ArgumentNotNull("name", name);
            Reload(name);
            if (!_configures.ContainsKey(name))
                return _nullConfig;
            return _configures[name];
        }

        /// <summary>
        /// 获取当前自定义配置文件项
        /// </summary>
        /// <returns></returns>
        public static IConfiguration Current
        {
            get { return _currentConfiguration; }
        }

        /// <summary>
        /// 获取默认App配置文件项
        /// </summary>
        /// <returns></returns>
        public static IConfiguration App
        {
            get { return _appConfiguration; }
        }

        /// <summary>
        /// 获取默认Conection配置文件项
        /// </summary>
        /// <returns></returns>
        public static IConfiguration Connection
        {
            get { return _connectionConfiguration; }
        }

        private static void Reload(string name)
        {
            if (!_configures.ContainsKey(name))
                return;

            var path = _files[name];
            if (File.Exists(path) && File.GetLastWriteTime(path) > _fileDateTimes[name]) //如果文件有更新，则重新载入
            {
                lock (_lock)
                {
                    _configures[name] = Load(path);
                }
            }
        }

        private static void Reload<T>(string name, ISerializer serializer)
        {
            if (!_configures.ContainsKey(name))
                return;

            var path = _files[name];
            if (File.Exists(path) && File.GetLastWriteTime(path) > _fileDateTimes[name])
            {
                lock (_lock)
                {
                    var content = File.ReadAllText(path, Encoding.UTF8);
                    var config = (serializer ?? Serializers.Xml).Deserialize<T>(content);
                    _xmlConfigures[name] = config;
                }
            }
        }

        private static IConfiguration Load(string path)
        {
            return new DefaultConfig(path);
        }

        private class NullConfig : IConfiguration
        {
            public string this[string name]
            {
                get { return string.Empty; }
            }

            public string this[string section, string name]
            {
                get { return string.Empty; }
            }
        }

        private class DefaultConfig : IConfiguration
        {
            private readonly IDictionary<string, string> _configs;
            private readonly IDictionary<string, IDictionary<string, string>> _sections;

            public DefaultConfig(string path)
            {
                _configs = new Dictionary<string, string>();
                _sections = new Dictionary<string, IDictionary<string, string>>();
                if (OnLoading != null)
                    OnLoading(path, _configs, _sections);
            }

            public string this[string name]
            {
                get
                {
                    Assert.ArgumentNotNull("name", name);
                    return !_configures.ContainsKey(name) ? string.Empty : _configs[name];
                }
            }

            public string this[string section, string name]
            {
                get
                {
                    Assert.ArgumentNotNull("section", section);
                    Assert.ArgumentNotNull("name", name);
                    if (!_sections.ContainsKey(section)) return string.Empty;
                    var s = _sections[section];
                    return !s.ContainsKey(name) ? string.Empty : s[name];
                }
            }
        }

        private class AppSettingsConfiguration : IConfiguration
        {
            public string this[string name] 
            {
                get
                {
                    Assert.ArgumentNotNull("name", name);
                    return ConfigurationManager.AppSettings[name];
                }
            }

            public string this[string section, string name] 
            {
                get { return this[name]; }
            }
        }

        private class ConnectionStringsConfiguration : IConfiguration
        {
            public string this[string name]
            {
                get
                {
                    Assert.ArgumentNotNull("name", name);
                    var config = ConfigurationManager.ConnectionStrings[name];
                    return config == null ? string.Empty : config.ConnectionString;
                }
            }

            public string this[string section, string name]
            {
                get
                {
                    Assert.ArgumentNotNull("section", section);
                    Assert.ArgumentNotNull("name", name);
                    var config = ConfigurationManager.ConnectionStrings[section];
                    if (string.Compare(name, "ConnectionString", true) == 0)
                    {
                        return config.ConnectionString;
                    }
                    if (string.Compare(name, "ProviderName", true) == 0)
                    {
                        return config.ProviderName;
                    }
                    return string.Empty;
                }
            }
        }
    }
}