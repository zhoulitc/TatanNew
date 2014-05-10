namespace Tatan.Common
{
    /// <summary>
    /// 可识别的对象接口
    /// </summary>
    public interface IDentityObject : IDentifiable
    {
        /// <summary>
        /// 标识对象的名称
        /// </summary>
        string Name { get; set; }
    }

    /// <summary>
    /// 可识别的对象接口
    /// </summary>
    public interface IDentityObject<out T> : IDentifiable<T>
    {
        /// <summary>
        /// 标识对象的名称
        /// </summary>
        string Name { get; set; }
    }
}