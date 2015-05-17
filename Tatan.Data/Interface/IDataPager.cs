// ReSharper disable once CheckNamespace
namespace Tatan.Data
{
    using System.Collections.Generic;

    /// <summary>
    /// 数据分页接口
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IDataPager<out T>
        where T : IDataEntity, new()
    {
        /// <summary>
        /// 是否有上一页
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        /// 是否有下一页
        /// </summary>
        bool HasNextPage { get; }

        /// <summary>
        /// 是否为第一页
        /// </summary>
        bool IsFirstPage { get; }

        /// <summary>
        /// 是否为最后一页
        /// </summary>
        bool IsLastPage { get; }

        /// <summary>
        /// 获取或设置当前页
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// 获取或设置页大小
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// 获取总页数
        /// </summary>
        int TotalPage { get; }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        /// 获取当前页数据集
        /// </summary>
        IReadOnlyList<T> Current { get; }
    }
}