// ReSharper disable once CheckNamespace
namespace Tatan.Data
{
    using System;

    /// <summary>
    /// 可创建数据接口
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public interface IDataCreated
    {
        /// <summary>
        /// 数据创建者
        /// </summary>
        string Creator { get; }

        /// <summary>
        /// 数据创建时间
        /// </summary>
        DateTime CreatedTime { get; }
    }
}