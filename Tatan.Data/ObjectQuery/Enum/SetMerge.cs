// ReSharper disable once CheckNamespace
namespace Tatan.Data.ObjectQuery
{
    /// <summary>
    /// 集合合并操作枚举
    /// </summary>
    public enum SetMerge
    {
        /// <summary>
        /// 差集
        /// </summary>
        Except,

        /// <summary>
        /// 交集
        /// </summary>
        Intersect,

        /// <summary>
        /// 并集，剔除重复
        /// </summary>
        Union,

        /// <summary>
        /// 并集，不剔除重复
        /// </summary>
        UnionAll
    }
}