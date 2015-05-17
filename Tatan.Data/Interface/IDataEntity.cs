// ReSharper disable once CheckNamespace
namespace Tatan.Data
{
    using System.Collections.Generic;
    using Common;

    /// <summary>
    /// 数据实体接口
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public interface IDataEntity : IDentifiable, IEnumerable<string>, IReplicable<IDataEntity>, IPropertyChanged, IDataCreated
    {
        /// <summary>
        /// 清空实体记录
        /// </summary>
        void Clear();

        /// <summary>
        /// 通过name来获取或者设置value
        /// </summary>
        /// <param name="name">实体名</param>
        /// <returns>数据值</returns>
        object this[string name] { get; set; }
    }
}