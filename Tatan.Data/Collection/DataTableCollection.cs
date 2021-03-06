﻿// ReSharper disable once CheckNamespace
namespace Tatan.Data
{
    using System;
    using Common.Collections;
    using Common.Exception;

    /// <summary>
    /// 数据记录
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public sealed class DataTableCollection : ReadOnlyCollection<IDataTable>
    {
        private readonly DataSource _source;
        private static readonly object _lock = new object();
        internal DataTableCollection(DataSource source)
        {
            _source = source;
        }

        internal void Clear() => Collection.Clear();

        /// <summary>
        /// 根据表名获取数据表，如果不存在则会抛出异常
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IDataTable this[string name]
        {
            get
            {
                Assert.ArgumentNotNull(nameof(name), name);
                Assert.KeyFound(Collection, name);
                return Collection[name];
            }
        }

        /// <summary>
        /// 根据表名获取数据表，如果不存在则会加入数据表集合中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IDataTable Get<T>() where T : IDataEntity
        {
            var type = typeof(T);
            var name = type.Name;
            if (!Collection.ContainsKey(name))
            {
                lock (_lock)
                {
                    Add(type);
                }
            }
            return Collection[name];
        }

        /// <summary>
        /// 添加一个表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IDataTable Add(Type type)
        {
            var name = type.Name;
            if (Collection.ContainsKey(name)) return Collection[name];
            var table = new DataTable(_source, name, type);
            Collection.Add(name, table);
            var dataProvider = _source.Provider as DataProvider;
            if (dataProvider != null)
                _source.Sessions.Add(name, new DataSession(name, dataProvider));
            return table;
        }
    }
}