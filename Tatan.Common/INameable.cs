namespace Tatan.Common
{
    /// <summary>
    /// 可命名的对象接口
    /// </summary>
    public interface INameable
    {
        /// <summary>
        /// 标识对象的名称
        /// </summary>
        string Name { get; set; }
    }
}