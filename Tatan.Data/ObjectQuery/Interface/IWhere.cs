// ReSharper disable once CheckNamespace
namespace Tatan.Data.ObjectQuery
{
    /// <summary>
    /// 数据查询语言(Data Query Language)接口
    /// <para>跨表操作时最好用试图或者存储过程</para>
    /// </summary>
    public interface IWhere : IOQL
    {							
        /// <summary>
        /// 设置分组子句
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="fields">其他字段名</param>
        /// <returns>数据查询语言接口</returns>
        IWhere GroupBy(string field, params string[] fields);

        /// <summary>
        /// 设置分组条件子句，需要先设置分组子句
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <returns>数据查询语言接口</returns>
        IWhere Having(IExpression expression);
		
		/// <summary>
		/// 设置排序子句
		/// </summary>
		/// <param name="sort">排序类型，分为asc和desc，其他任意字符将会被认为是asc</param>
        /// <param name="field">字段名</param>
        /// <param name="fields">其他字段名</param>
        /// <returns>数据查询语言接口</returns>
        IWhere OrderBy(string sort, string field, params string[] fields);

        /// <summary>
        /// 设置包含集合
        /// </summary>
        /// <param name="oper">集合操作类型</param>
        /// <param name="select">数据查询接口</param>
        /// <param name="selects">其他数据查询接口</param>
        /// <returns>集合接口</returns>
        IFrom From(SetMerge oper, IOQL select, params IOQL[] selects);
    }
}