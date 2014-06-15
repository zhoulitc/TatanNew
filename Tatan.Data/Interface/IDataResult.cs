// ReSharper disable once CheckNamespace
namespace Tatan.Data
{
    using System.Collections.Generic;

    /// <summary>
    /// 实体结果接口
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IDataResult<out T> : IEnumerable<T> 
        where T : IDataEntity, new()
    {
        /// <summary>
        /// 实体集合
        /// </summary>
        IDataEntities<T> Entities { get; }

        /// <summary>
        /// 总数据量
        /// </summary>
        long TotalCount { get; }
    }
}