namespace Tatan.Common
{
    /// <summary>
    /// 可计数接口
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public interface ICountable
    {
        /// <summary>
        /// 获取可计数对象的计数值
        /// </summary>
        int Count { get; }
    }
}