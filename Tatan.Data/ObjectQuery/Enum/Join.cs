// ReSharper disable once CheckNamespace
namespace Tatan.Data.ObjectQuery
{
    /// <summary>
    /// 链接类型
    /// </summary>
    public enum Join
    {
        /// <summary>
        /// 交叉连接
        /// </summary>
        Cross,

        /// <summary>
        /// 内连接
        /// </summary>
        Inner,

        /// <summary>
        /// 左外连接
        /// </summary>
        Left,

        /// <summary>
        /// 右外连接
        /// </summary>
        Right,

        /// <summary>
        /// 全外连接
        /// </summary>
        Full
    }
}