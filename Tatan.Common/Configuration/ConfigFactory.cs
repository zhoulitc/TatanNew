namespace Tatan.Common.Configuration
{
    using System;
    using System.Text;
    using Serialization;
    using Exception;
    using IO;
    using SystemFile = System.IO.File;
    using System.Configuration;
    using System.Collections.Generic;

    /// <summary>
    /// 配置文件工厂
    /// </summary>
    public static class ConfigFactory
    {
        /// <summary>
        /// 获取配置文件
        /// <para>请使用System.Xml.Serialization空间中的特性标明</para>
        /// </summary>
        /// <typeparam name="T">具体的配置文件类型</typeparam>
        /// <param name="fileName">XML配置文件名</param>
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
        public static T GetXmlConfig<T>(string fileName)
        {
            ExceptionHandler.ArgumentNull("fileName", fileName);
            var path = String.Format("{0}{1}.xml", Runtime.Root, fileName);
            ExceptionHandler.FileNotFound(path);
            var content = SystemFile.ReadAllText(path, Encoding.UTF8);
            return Serializer.Xml.Deserialize<T>(content);
        }

        private static readonly AppSettingsConfig _appConfig = new AppSettingsConfig();

        /// <summary>
        /// 获取默认配置文件项
        /// </summary>
        /// <returns></returns>
        public static IConfig AppConfig
        {
            get { return _appConfig; }
        }

        private static readonly ConnectionStringsConfig _connectionConfig = new ConnectionStringsConfig();

        /// <summary>
        /// 获取默认配置文件项
        /// </summary>
        /// <returns></returns>
        public static IConfig<DbConfig> ConnectionConfig
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
                    ExceptionHandler.ArgumentNull("key", key);
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
                    ExceptionHandler.ArgumentNull("key", key);
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