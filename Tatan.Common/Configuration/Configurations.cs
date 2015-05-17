namespace Tatan.Common.Configuration
{
    using System;
    using System.Configuration;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Xml.Serialization;
    using Exception;
    using Extension.String.Deserialization;
    using Extension.String.IO;
    using Serialization;

    /// <summary>
    /// 配置文件管理者
    /// <para>author:zhoulitcqq</para>
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
            Current = _appConfiguration;
            _connectionConfiguration = new ConnectionStringsConfiguration();
        }

        /// <summary>
        /// 注册一个配置文件到IConfiguration集合中
        /// </summary>
        /// <param name="path">配置文件路径</param>
        /// <param name="serializer">序列器对象</param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        public static void Register(string path, ISerializer serializer = null)
        {
            Assert.ArgumentNotNull(nameof(path), path);
            Assert.FileFound(path);

            var name = path.GetFileName(false);

            lock (_lock)
            {
                if (!_configures.ContainsKey(name))
                {
                    Current = new DefaultConfig(path, serializer);
                    _configures.Add(name, Current);
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
        public static void Register<T>(string path, ISerializer serializer = null) where T : class
        {
            Assert.ArgumentNotNull(nameof(path), path);
            Assert.FileFound(path);

            var name = path.GetFileName(false);

            lock (_lock)
            {
                if (!_xmlConfigures.ContainsKey(name))
                {
                    var content = File.ReadAllText(path, Encoding.UTF8);
                    var config = content.Deserialize<T>(serializer);
                    _xmlConfigures.Add(name, config);
                    if (!_files.ContainsKey(name))
                        _files.Add(name, path);
                    if (!_fileDateTimes.ContainsKey(name))
                        _fileDateTimes.Add(name, File.GetLastWriteTime(path));
                }
            }
        }

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
        /// <param name="serializer">序列器对象</param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        public static IConfiguration Get(string name, ISerializer serializer = null)
        {
            Assert.ArgumentNotNull(nameof(name), name);
            Reload(name, serializer);
            if (!_configures.ContainsKey(name))
                return _nullConfig;
            return _configures[name];
        }

        /// <summary>
        /// 获取当前自定义配置文件项
        /// </summary>
        /// <returns></returns>
        public static IConfiguration Current { get; private set; }

        /// <summary>
        /// 获取默认App配置文件项
        /// </summary>
        /// <returns></returns>
        public static IConfiguration App => _appConfiguration;

        /// <summary>
        /// 获取默认Conection配置文件项
        /// </summary>
        /// <returns></returns>
        public static IConfiguration Connection => _connectionConfiguration;

        private static void Reload(string name, ISerializer serializer = null)
        {
            if (!_configures.ContainsKey(name))
                return;

            var path = _files[name];
            if (File.Exists(path) && File.GetLastWriteTime(path) > _fileDateTimes[name]) //如果文件有更新，则重新载入
            {
                lock (_lock)
                {
                    _configures[name] = new DefaultConfig(path, serializer);
                }
            }
        }

        private static void Reload<T>(string name, ISerializer serializer) where T : class
        {
            if (!_xmlConfigures.ContainsKey(name))
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

        private class NullConfig : IConfiguration
        {
            public string this[string name] => string.Empty;

            public string this[string section, string name] => string.Empty;
        }

        private class DefaultConfig : IConfiguration
        {
            private const string _defaultConfigName = "default";
            private readonly IDictionary<string, IDictionary<string, string>> _sections;

            public DefaultConfig(string path, ISerializer serializer)
            {
                _sections = new Dictionary<string, IDictionary<string, string>>
                {
                    {_defaultConfigName, new Dictionary<string, string>()}
                };
                var content = File.ReadAllText(path);
                var entity = content.Deserialize<ConfigurationEntity>(serializer);
                foreach (var config in entity.Configures)
                {
                    _sections[_defaultConfigName].Add(config.Name, config.Value);
                }
                foreach (var section in entity.ConfigureSections)
                {
                    if (_sections.ContainsKey(section.Name))
                        continue;

                    var configs = new Dictionary<string, string>(section.Configures.Count + 1);
                    foreach (var config in section.Configures)
                    {
                        configs.Add(config.Name, config.Value);
                    }
                    _sections.Add(section.Name, configs);
                }
            }

            public string this[string name]
            {
                get
                {
                    Assert.ArgumentNotNull(nameof(name), name);
                    return !_sections[_defaultConfigName].ContainsKey(name)
                        ? string.Empty
                        : _sections[_defaultConfigName][name];
                }
            }

            public string this[string section, string name]
            {
                get
                {
                    Assert.ArgumentNotNull(nameof(section), section);
                    Assert.ArgumentNotNull(nameof(name), name);
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
                    return ConfigurationManager.AppSettings[name] ?? string.Empty;
                }
            }

            public string this[string section, string name] => this[name];
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
                    if (config == null) return string.Empty;
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

        /// <summary>
        /// 
        /// </summary>
        [XmlRoot(ElementName = "Configurations")]
        [DataContract]
        public sealed class ConfigurationEntity
        {
            /// <summary>
            /// 
            /// </summary>
            [XmlElement(ElementName = "Configure")]
            [DataMember(Name = "Configure")]
            public List<Configure> Configures { get; set; }

            /// <summary>
            /// 
            /// </summary>
            [XmlElement(ElementName = "ConfigureSection")]
            [DataMember(Name = "ConfigureSection")]
            public List<ConfigureSection> ConfigureSections { get; set; }

            /// <summary>
            /// 
            /// </summary>
            [DataContract(Name = "Configure")]
            public class Configure
            {
                /// <summary>
                /// 
                /// </summary>
                [XmlAttribute(AttributeName = "name")]
                [DataMember(Name = "name")]
                public string Name { get; set; }

                /// <summary>
                /// 
                /// </summary>
                [XmlAttribute(AttributeName = "value")]
                [DataMember(Name = "value")]
                public string Value { get; set; }
            }

            /// <summary>
            /// 
            /// </summary>
            [DataContract(Name = "ConfigureSection")]
            public class ConfigureSection
            {
                /// <summary>
                /// 
                /// </summary>
                [XmlAttribute(AttributeName = "name")]
                [DataMember(Name = "name")]
                public string Name { get; set; }

                /// <summary>
                /// 
                /// </summary>
                [XmlElement(ElementName = "Configure")]
                [DataMember(Name = "Configure")]
                public List<Configure> Configures { get; set; }
            }
        }
    }
}