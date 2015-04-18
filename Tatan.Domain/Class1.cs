using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;

namespace Tatan.Domain
{
    /// <summary>
    /// 域控对象
    /// </summary>
    public class AdObject : IEnumerable<string>
    {
        private readonly IDictionary<string, object> _properties;

        internal AdObject(SearchResult result, ICollection<string> properties)
        {
            IEnumerable names;
            int count;
            if (properties == null || properties.Count == 0)
            {
                count = result.Properties.Count;
                names = result.Properties.PropertyNames ?? new string[0];
            }
            else
            {
                count = properties.Count;
                names = properties;
            }

            _properties = new Dictionary<string, object>(count + 1);

            foreach (var name in names)
            {
                if (!result.Properties.Contains(name.ToString())) continue;

                var property = result.Properties[name.ToString()];

                if (property == null || property.Count == 0) continue;

                if (property.Count == 1)
                    _properties.Add(name.ToString(), property[0]);
                else
                {
                    var values = new object[property.Count];
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = property[i];
                    }
                    _properties.Add(name.ToString(), values);
                }
            }
        }

        internal AdObject(DirectoryEntry entry, ICollection<string> properties)
        {
            IEnumerable names;
            int count;
            if (properties == null || properties.Count == 0)
            {
                count = entry.Properties.Count;
                names = entry.Properties.PropertyNames;
            }
            else
            {
                count = properties.Count;
                names = properties;
            }

            _properties = new Dictionary<string, object>(count + 1);
            foreach (var name in names)
            {
                if (!entry.Properties.Contains(name.ToString())) continue;

                var property = entry.Properties[name.ToString()];

                if (property == null || property.Count == 0) continue;

                if (property.Count == 1)
                    _properties.Add(name.ToString(), property[0]);
                else
                {
                    var values = new object[property.Count];
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = property[i];
                    }
                    _properties.Add(name.ToString(), values);
                }
            }
        }

        public bool Contains(string name)
        {
            return _properties.ContainsKey(name);
        }

        public int Count
        {
            get { return _properties.Count; }
        }

        public T Get<T>(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            if (!_properties.ContainsKey(name))
            {
                throw new KeyNotFoundException("key:" + name + " is not found");
            }
            return (T)_properties[name];
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _properties.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    /// <summary>
    /// 域控读取器
    /// </summary>
    public class AdReader : IDisposable
    {
        private DirectoryEntry _root;

        public static AdConfig Root { private get; set; }

        public AdReader(AdConfig config = null)
        {
            config = config ?? Root;
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            _root = new DirectoryEntry(config.ToString(), config.Username, config.Password, AuthenticationTypes.Secure);
        }

        public AdObject GetObject(AdConfig config, params string[] properties)
        {
            using (var entry = new DirectoryEntry(config.ToString(), config.Username, config.Password, AuthenticationTypes.Secure))
            {
                return new AdObject(entry, properties);
            }
        }

        public AdObject GetObject(string filter, params string[] properties)
        {
            using (var sreacher = new DirectorySearcher(_root, filter, properties))
            {
                var result = sreacher.FindOne();
                if (result == null)
                    return null;

                return new AdObject(result, properties);
            }
        }

        public void Dispose()
        {
            if (_root != null)
            {
                _root.Close();
                _root.Dispose();
                _root = null;
            }
        }
    }

    public class AdConfig
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
