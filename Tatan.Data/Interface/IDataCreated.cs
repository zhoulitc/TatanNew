using System;

namespace Tatan.Data.Interface
{
    /// <summary>
    /// 可创建数据接口
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
        DateTime CreatedDate { get; }
    }
}