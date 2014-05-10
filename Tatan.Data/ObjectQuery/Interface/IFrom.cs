// ReSharper disable once CheckNamespace
namespace Tatan.Data.ObjectQuery
{
    /// <summary>
    /// 集合接口
    /// <para>处理查询语句中的from子句</para>
    /// </summary>
    public interface IFrom : IOQL
    {
        /// <summary>
        /// 追加表
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="tables">其他表名</param>
        /// <returns>集合接口</returns>
        IFrom _(string table, params string[] tables);

        /// <summary>
        /// 追加数据查询集合
        /// </summary>
        /// <param name="select">数据查询集合</param>
        /// <param name="selects">其他数据查询集合</param>
        /// <returns>集合接口</returns>
        IFrom _(IOQL select, params IOQL[] selects);

        /// <summary>
        /// 连接两个集合
        /// </summary>
        /// <param name="left">左表</param>
        /// <param name="join">连接类型枚举</param>
        /// <param name="right">右表</param>
        /// <param name="expression">ON表达式</param>
        /// <returns>集合接口</returns>
        IFrom Join(string left, Join join, string right, IExpression expression);

        /// <summary>
        /// 检索条件
        /// </summary>
        /// <param name="expression">表达式对象</param>
        /// <returns>数据查询接口</returns>
        IWhere Where(IExpression expression);
    }
}