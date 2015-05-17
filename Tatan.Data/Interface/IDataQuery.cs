// ReSharper disable once CheckNamespace
namespace Tatan.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// 实体查询接口
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IDataQuery<T> : IDisposable
        where T : IDataEntity, new()
    {
        /// <summary>
        /// 设置where条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IDataQuery<T> Where(Expression<Func<T, bool>> condition);

        /// <summary>
        /// 设置排序规则
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        IDataQuery<T> OrderBy(string name, DataSort sort = DataSort.Ascending);

        /// <summary>
        /// 执行并获取返回结果
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<T> Execute();
    }
}