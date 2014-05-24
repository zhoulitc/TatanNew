// ReSharper disable once CheckNamespace
namespace Tatan.Data
{
    using System;
    using Common.Exception;
    using Common.Collections;

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
        
        /// <summary>
        /// 根据表名获取数据表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IDataTable this[string name]
        {
            get
            {
                ExceptionHandler.ArgumentNull("name", name);
                ExceptionHandler.KeyNotFound(Collection, name);
                return Collection[name];
            }
        }

        internal void Clear()
        {
            Collection.Clear();
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
                _source.Sessions.Add(name, new DataSession(_source, name, dataProvider.Connection));
            return table;
        }
    }
}