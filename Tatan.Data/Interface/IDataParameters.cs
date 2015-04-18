using System;
// ReSharper disable once CheckNamespace


namespace Tatan.Data
{
    /// <summary>
    /// 参数集操作接口
    /// </summary>
    public interface IDataParameters
    {
        /// <summary>
        /// 设置一个非数字型参数
        /// </summary>
        /// <param name="index">参数索引</param>
        /// <param name="type">参数类型</param>
        /// <param name="size">参数大小</param>
        /// <exception cref="System.IndexOutOfRangeException">当索引超出范围时抛出</exception>
        /// <returns>参数值</returns>
        object this[int index, Type type = null, int size = 0] { set; }

        /// <summary>
        /// 设置一个数字型参数
        /// </summary>
        /// <param name="index">参数索引</param>
        /// <param name="precision">参数精度</param>
        /// <param name="scale">参数小数位数</param>
        /// <exception cref="System.IndexOutOfRangeException">当索引超出范围时抛出</exception>
        /// <returns>参数值</returns>
        object this[int index, byte precision, byte scale = 0] { set; }

        /// <summary>
        /// 设置一个非数字型参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="type">参数类型</param>
        /// <param name="size">参数大小</param>
        /// <exception cref="System.ArgumentNullException">当参数名为空时抛出</exception>
        /// <returns>参数值</returns>
        object this[string name, Type type = null, int size = 0] { set; }

        /// <summary>
        /// 设置一个数字型参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="precision">参数精度</param>
        /// <param name="scale">参数小数位数</param>
        /// <exception cref="System.ArgumentNullException">当参数名为空时抛出</exception>
        /// <returns>参数值</returns>
        object this[string name, byte precision, byte scale = 0] { set; }
    }
}