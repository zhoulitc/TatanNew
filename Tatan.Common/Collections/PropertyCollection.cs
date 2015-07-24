namespace Tatan.Common.Collections
{
    using System;
    using System.Reflection;
    using Exception;
    using Extension.Reflect;



    /// <summary>
    /// 属性集合
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public sealed class PropertyCollection : ReadOnlyCollection<PropertyCollection.Entry>, IDisposable
    {
        private static readonly Type _stringType = typeof (string);

        /// <summary>
        /// 条目
        /// </summary>
        public struct Entry
        {
            /// <summary>
            /// 属性的get访问器
            /// </summary>
            public Func<object, object> Get;

            /// <summary>
            /// 属性的set访问器
            /// </summary>
            public Action<object, object> Set;

            /// <summary>
            /// 属性的类型
            /// </summary>
            public Type Type;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="System.ArgumentNullException">类型为空时抛出</exception>
        /// <exception cref="System.Reflection.AmbiguousMatchException">找到多个有指定名称且与指定绑定约束匹配的属性时抛出</exception>
        public PropertyCollection(Type type)
        {
            Assert.ArgumentNotNull(nameof(type), type);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var property in properties)
            {
                AddTuple(type, property);
            }
        }

        /// <summary>
        /// 判断属性集合中某个属性是否为字符串类型
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="System.ArgumentNullException">参数为空时抛出</exception>
        /// <returns></returns>
        public bool IsString(string name)
        {
            Assert.ObjectNotDisposed(_isDisposed, nameof(PropertyCollection));
            Assert.ArgumentNotNull(nameof(name), name);
            return Contains(name) && Collection[name].Type.IsAssignableFrom(_stringType);
        }

        #region IDisposable

        /// <summary>
        /// 析构函数
        /// </summary>
        ~PropertyCollection()
        {
            Dispose(false);
        }

        private bool _isDisposed;

        /// <summary>
        /// 销毁属性集合
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposable)
        {
            if (_isDisposed)
                return;
            if (disposable)
            {
                Collection.Clear();
            }
            _isDisposed = true;
        }

        #endregion

        /// <summary>
        /// 获取或设置属性集合中的某个属性值
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <exception cref="System.ArgumentException">属性无法设置或获取时抛出</exception>
        /// <exception cref="System.ArgumentNullException">参数为空时抛出</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">键未找到时抛出</exception>
        /// <returns></returns>
        public object this[object instance, string name]
        {
            get
            {
                Assert.ObjectNotDisposed(_isDisposed, nameof(PropertyCollection));
                Assert.ArgumentNotNull(nameof(instance), instance);
                Assert.ArgumentNotNull(nameof(name), name);
                Assert.KeyFound(Collection, name);
                return Collection[name].Get(instance);
            }
            set
            {
                Assert.ObjectNotDisposed(_isDisposed, nameof(PropertyCollection));
                Assert.ArgumentNotNull(nameof(instance), instance);
                Assert.ArgumentNotNull(nameof(name), name);
                Assert.ArgumentNotNull(nameof(value), value);
                Assert.KeyFound(Collection, name);
                Collection[name].Set(instance, value);
            }
        }

        private void AddTuple(Type type, PropertyInfo property)
        {
            Collection.Add(property.Name, new Entry()
            {
                Get = type.GetGetMethod(property),
                Set = type.GetSetMethod(property),
                Type = property.PropertyType
            });
        }
    }
}