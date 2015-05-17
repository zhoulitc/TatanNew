// ReSharper disable once CheckNamespace
namespace Tatan.Data
{
    using System;

    /// <summary>
    /// 可修改数据接口
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public interface IDataModified
    {
        /// <summary>
        /// 数据最后修改人
        /// </summary>
        string Modifier { get; set; }

        /// <summary>
        /// 数据最后修改时间
        /// </summary>
        DateTime ModifiedTime { get; set; }

        /// <summary>
        /// 数据版本号，每次修改后加一
        /// </summary>
        uint Version { get; set; }
    }
}