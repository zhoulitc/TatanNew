using System;

namespace Tatan.Data.Interface
{
    /// <summary>
    /// 可修改数据接口
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
        DateTime ModifiedDate { get; set; }

        /// <summary>
        /// 数据版本号，每次修改后加一
        /// </summary>
        uint Version { get; set; }
    }
}