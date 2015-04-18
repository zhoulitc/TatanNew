using System.Text;
using Tatan.Common.Exception;

namespace Tatan.Common
{
    /// <summary>
    /// Sql构建器
    /// </summary>
    public class SqlBuilder
    {
        private readonly string _keyCondition;
        private readonly string[] _fields;
        private readonly string _table;
        private readonly string _symbol;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fields"></param>
        /// <param name="symbol"></param>
        /// <param name="key"></param>
        public SqlBuilder(string table, string key, string[] fields, string symbol)
        {
            Assert.ArgumentNotNull("table", table);
            Assert.ArgumentNotNull("fields", fields);
            if (fields.Length <= 0)
                throw new System.Exception("field is empty.");

            _table = table;
            _keyCondition = string.IsNullOrEmpty(key) ? "1=1" : string.Format("{0}={1}{0}", key, _symbol);
            _symbol = string.IsNullOrEmpty(symbol) ? "@" : symbol;
            _fields = fields;
        }

        /// <summary>
        /// 获取删除语句
        /// </summary>
        /// <returns></returns>
        public string GetDeleteStatement(string condition = null)
        {
            condition = condition ?? _keyCondition;

            return string.Format("DELETE {0} WHERE {1}", _table, condition);
        }

        /// <summary>
        /// 获取新增语句
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public string GetInsertStatement(string[] fields = null)
        {
            fields = fields ?? _fields;
            var columns = string.Join(",", fields);
            var parameters = _symbol + string.Join("," + _symbol, _fields);

            return string.Format("INSERT INTO {0}({1}) VALUES({2})",
               _table, columns, parameters);
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
                sets.AppendFormat(",{0}={1}{0}", field, _symbol);
            }

            return string.Format("UPDATE {0} SET {1} WHERE {2}",
                _table, sets.Remove(0, 1).ToString(), condition);
        }

        /// <summary>
        /// 获取统计语句
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public string GetCountStatement(string condition = null)
        {
            condition = condition ?? _keyCondition;

            return string.Format("SELECT COUNT(1) FROM {0} WHERE {1}",
                _table, condition);
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

            var columns = string.Join(",", fields);

            return string.Format("SELECT {0} FROM {1} WHERE {2}",
                _table, columns, condition);
        }
    }
}