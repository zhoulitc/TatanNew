// ReSharper disable once CheckNamespace
namespace Tatan.Data
{
    using System.Collections.Generic;
    using Common;

    /// <summary>
    /// 数据文档
    /// </summary>
    public interface IDataDocument : IObject, IEnumerable<IDataRecord>
    {
        /// <summary>
        /// 获取数据集中的项目个数
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 根据索引获取内部文档
        /// </summary>
        /// <param name="index">索引</param>
        /// <exception cref="System.IndexOutOfRangeException">索引越界时抛出</exception>
        /// <returns>数据文档</returns>
        IDataRecord this[int index] { get; }
    }
}