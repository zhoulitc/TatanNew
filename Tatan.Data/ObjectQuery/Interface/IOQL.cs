// ReSharper disable once CheckNamespace
namespace Tatan.Data.ObjectQuery
{
    /// <summary>
    /// 对象语言字符串化接口
    /// </summary>
// ReSharper disable once InconsistentNaming
    public interface IOQL
    {
        /// <summary>
        /// 获取OQL对应的SQL
        /// </summary>
        /// <returns>SQL语句</returns>
        string ToString();
    }
}