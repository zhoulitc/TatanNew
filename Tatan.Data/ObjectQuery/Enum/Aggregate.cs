// ReSharper disable once CheckNamespace
namespace Tatan.Data.ObjectQuery
{
    /// <summary>
    /// 聚合函数类型
    /// </summary>
    public enum Aggregate
    {
        /// <summary>
        /// 统计
        /// </summary>
        Count,

        /// <summary>
        /// 求最小值
        /// </summary>
        Min,

        /// <summary>
        /// 求最大值
        /// </summary>
        Max,

        /// <summary>
        /// 求平均值
        /// </summary>
        Avg,

        /// <summary>
        /// 求和
        /// </summary>
        Sum
    }
}