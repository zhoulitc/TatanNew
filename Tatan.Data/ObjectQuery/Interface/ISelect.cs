// ReSharper disable once CheckNamespace
namespace Tatan.Data.ObjectQuery
{
    /// <summary>
    /// 选择接口
    /// <para>处理查询语句中的select子句</para>
    /// </summary>
    public interface ISelect
    {
        /// <summary>
        /// 追加字段名
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="fields">其他字段名</param>
        /// <returns>选择接口</returns>
        ISelect _(string field, params string[] fields);

        /// <summary>
        /// 追加CaseWhen表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="expressions">其他表达式</param>
        /// <returns>选择接口</returns>
        ISelect _(CaseWhenExpression expression, params CaseWhenExpression[] expressions);

        /// <summary>
        /// 追加函数表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="expressions">其他表达式</param>
        /// <returns>选择接口</returns>
        ISelect _(FunctionExpression expression, params FunctionExpression[] expressions);

        /// <summary>
        /// 设置包含集合
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="tables">其他表名</param>
        /// <returns>集合接口</returns>
        IFrom From(string table, params string[] tables);

        /// <summary>
        /// 设置Distinct语句
        /// </summary>
        /// <param name="field">字段名</param>
        /// <returns>选择接口</returns>
        ISelect Distinct(string field);

        /// <summary>
        /// 设置Top语句
        /// </summary>
        /// <param name="n">前n行或者n%行</param>
        /// <param name="isPercent">是否为百分比</param>
        /// <returns>选择接口</returns>
        ISelect Top(uint n, bool isPercent = false);

        /// <summary>
        /// 返回Empty，没有表名无法查询
        /// </summary>
        /// <returns>返回空字符串</returns>
        string ToString();
    }
}