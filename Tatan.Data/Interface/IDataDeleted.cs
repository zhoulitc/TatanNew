namespace Tatan.Data.Interface
{
    /// <summary>
    /// 可删除数据接口
    /// </summary>
    public interface IDataDeleted
    {
        /// <summary>
        /// 数据是否被软删除
        /// </summary>
        bool IsDelete { get; }
    }
}