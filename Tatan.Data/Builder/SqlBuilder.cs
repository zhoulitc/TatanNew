namespace Tatan.Data.Builder
{
    using System.Linq;
    using System.Text;
    using Common.Exception;

    /// <summary>
    /// Sql构建器
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public class SqlBuilder
    {
        private readonly string _keyCondition;
        private readonly string[] _fields;
        private readonly string _table;
        private readonly IDataProvider _provider;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fields"></param>
        /// <param name="key"></param>
        /// <param name="provider"></param>
        public SqlBuilder(string table, string key, string[] fields, IDataProvider provider)
        {
            Assert.ArgumentNotNull(nameof(table), table);
            Assert.ArgumentNotNull(nameof(fields), fields);
            Assert.ArgumentNotNull(nameof(provider), provider);
            if (fields.Length <= 0)
                throw new System.Exception("field is empty.");

            _table = table;
            _provider = provider;
            _fields = fields;
            _keyCondition = string.IsNullOrEmpty(key) ? "1=1" : string.Format("{2}{0}{3}={1}{0}",
                key, _provider.ParameterSymbol, _provider.LeftSymbol, _provider.RightSymbol);
        }

        /// <summary>
        /// 获取删除语句
        /// </summary>
        /// <returns></returns>
        public string GetDeleteStatement(string condition = null)
        {
            condition = condition ?? _keyCondition;

            return string.Format("DELETE FROM {0}{1}{2} WHERE {3}",
                _provider.LeftSymbol, _table, _provider.RightSymbol, condition);
        }

        /// <summary>
        /// 获取新增语句
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public string GetInsertStatement(string[] fields = null)
        {
            var properties = (fields ?? _fields).ToList();
            properties.Add(nameof(IDataEntity.Creator));
            properties.Add(nameof(IDataEntity.CreatedTime));

            var columns = _provider.LeftSymbol + 
                string.Join(_provider.RightSymbol + "," + _provider.LeftSymbol, properties) + 
                _provider.RightSymbol;

            var parameters = _provider.ParameterSymbol + 
                string.Join("," + _provider.ParameterSymbol, properties);

            return string.Format("INSERT INTO {3}{0}{4}({1}) VALUES({2})",
                _table, columns, parameters, _provider.LeftSymbol, _provider.RightSymbol);
        }

        /// <summary>
        /// 获取更新语句
        /// </summary>
        /// <returns></returns>
        public string GetUpdateStatement(string[] fields = null, string condition = null)
        {
            fields = fields ?? _fields;
            condition = condition ?? _keyCondition;
            var sets = new StringBuilder(_fields.Length*20);
            foreach (var field in fields)
            {
                sets.AppendFormat(",{2}{0}{3}={1}{0}", 
                    field, _provider.ParameterSymbol, _provider.LeftSymbol, _provider.RightSymbol);
            }

            return string.Format("UPDATE {3}{0}{4} SET {1} WHERE {2}",
                _table, sets.Remove(0, 1).ToString(), condition, _provider.LeftSymbol, _provider.RightSymbol);
        }

        /// <summary>
        /// 获取更新语句
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public string GetUpdateStatement(string condition = null)
        {
            condition = condition ?? _keyCondition;

            return string.Format("UPDATE {2}{0}{3} SET {{0}} WHERE {1}",
                _table, condition, _provider.LeftSymbol, _provider.RightSymbol);
        }

        /// <summary>
        /// 获取统计语句
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public string GetCountStatement(string condition = null)
        {
            condition = condition ?? _keyCondition;

            return string.Format("SELECT COUNT(1) FROM {2}{0}{3} WHERE {1}",
                _table, condition, _provider.LeftSymbol, _provider.RightSymbol);
        }

        /// <summary>
        /// 获取查询语句
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public string GetSelectStatement(string[] fields = null, string condition = null)
        {
            fields = fields ?? _fields;
            condition = condition ?? _keyCondition;

            var columns = _provider.LeftSymbol + 
                string.Join(_provider.RightSymbol + "," + _provider.LeftSymbol, fields) + _provider.RightSymbol;

            return string.Format("SELECT {1} FROM {3}{0}{4} WHERE {2}",
                _table, columns, condition, _provider.LeftSymbol, _provider.RightSymbol);
        }
    }
}