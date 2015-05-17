namespace Tatan.Common.Configuration
{
    using System;
    using System.Collections.Generic;
    using Exception;

    /// <summary>
    /// 集成信息管理者
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public static class Integrations
    {
        private static readonly IDictionary<string, Integration> _integrations;
        private static readonly object _lock;

        static Integrations()
        {
            _lock = new object();
            _integrations = new Dictionary<string, Integration>();
        }

        private class Integration : IIntegration
        {
            public string Name { get; set; }

            public Uri Uri { get; set; }

            public ICertification Certification { get; set; }

            public IDictionary<string, string> Properties { get; set; }
        }

        /// <summary>
        /// 增加或维护一个集成信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="uri"></param>
        /// <param name="certification"></param>
        /// <param name="properties"></param>
        public static void Put(string name, Uri uri, ICertification certification = null,
            IDictionary<string, string> properties = null)
        {
            lock (_lock)
            {
                if (!_integrations.ContainsKey(name))
                {
                    _integrations.Add(name, new Integration {Name = name});
                }
                var integration = _integrations[name];
                integration.Uri = uri;
                if (certification != null)
                    integration.Certification = certification;
                if (properties != null)
                {
                    if (integration.Properties == null)
                    {
                        integration.Properties = properties;
                    }
                    else
                    {
                        foreach (var property in properties)
                        {
                            integration.Properties[property.Key] = property.Value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 判断集成对象是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool Contains(string name)
        {
            return _integrations.ContainsKey(name);
        }

        /// <summary>
        /// 获取一个集成信息对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IIntegration Get(string name)
        {
            Assert.KeyFound(_integrations, name);
            return _integrations[name];
        }

        /// <summary>
        /// 获取一个集成信息的Uri
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Uri GetUri(string name)
        {
            return Get(name).Uri;
        }

        /// <summary>
        /// 获取一个集成信息的认证
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ICertification GetCertification(string name)
        {
            return Get(name).Certification;
        }

        /// <summary>
        /// 获取一个集成信息的扩展属性
        /// </summary>
        /// <param name="name"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string GetProperty(string name, string property)
        {
            var intergration = Get(name);
            Assert.KeyFound(intergration.Properties, property);
            return intergration.Properties[property];
        }

        /// <summary>
        /// 移除一个集成信息
        /// </summary>
        /// <param name="name"></param>
        public static void Remove(string name)
        {
            _integrations.Remove(name);
        }
    }
}