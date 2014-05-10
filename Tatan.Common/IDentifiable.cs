namespace Tatan.Common
{
    /// <summary>
    /// 可识别的对象接口
    /// </summary>
    public interface IDentifiable : IDentifiable<int>
    {
    }

    /// <summary>
    /// 可识别的对象接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDentifiable<out T>
    {
        /// <summary>
        /// 获取对象的唯一标识符
        /// </summary>
        T Id { get; }
    }
}