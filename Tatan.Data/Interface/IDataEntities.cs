// ReSharper disable once CheckNamespace
namespace Tatan.Data
{
    using System.Collections.Generic;

    /// <summary>
    /// 实体集合接口
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IDataEntities<out T> : IReadOnlyList<T>
        where T : IDataEntity, new()
    {
    }
}