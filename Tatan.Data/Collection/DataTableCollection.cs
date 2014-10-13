// ReSharper disable once CheckNamespace
namespace Tatan.Data
{
    using System;
    using Common.Collections;
    using Common.Exception;

    /// <summary>
    /// 数据记录
    /// </summary>
    public sealed class DataTableCollection : ReadOnlyCollection<IDataTable>
    {
        private readonly DataSource _source;
        internal DataTableCollection(DataSource source)
        {
            _source = source;
        }

        internal void Clear()
        {
            Collection.Clear();
        }

        /// <summary>
        /// 根据表名获取数据表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IDataTable this[string name]
        {
            get
            {
                Assert.ArgumentNotNull("name", name);
                Assert.KeyFound(Collection, name);
                return Collection[name];
            }
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