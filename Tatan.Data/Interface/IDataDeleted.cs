// ReSharper disable once CheckNamespace
namespace Tatan.Data
{
    /// <summary>
    /// 可删除数据接口
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public interface IDataDeleted
    {
        /// <summary>
        /// 数据是否被软删除
        /// </summary>
        bool IsDelete { get; }
    }
}