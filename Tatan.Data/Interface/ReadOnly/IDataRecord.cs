// ReSharper disable once CheckNamespace
namespace Tatan.Data
{
    using System.Collections.Generic;
    using Common;

    /// <summary>
    /// 数据记录
    /// </summary>
    public interface IDataRecord : IObject, IEnumerable<string>
    {
        /// <summary>
        /// 获取数据记录中的数据个数
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 通过name来获取或者设置value
        /// </summary>
        /// <param name="name">数据名</param>
        /// <exception cref="System.ArgumentNullException">参数为空时抛出</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">键未找到时抛出</exception>
        /// <returns>数据值</returns>
        object this[string name] { get; }

        /// <summary>
        /// 通过索引来获取或者设置value
        /// </summary>
        /// <param name="index">数据索引</param>
        /// <exception cref="System.IndexOutOfRangeException">索引越界时抛出</exception>
        /// <returns>数据值</returns>
        object this[int index] { get; }
    }
}