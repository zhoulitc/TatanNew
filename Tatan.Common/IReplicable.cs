namespace Tatan.Common
{
    /// <summary>
    /// 可复制的对象接口
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReplicable<out T> where T : class
    {
        /// <summary>
        /// 浅拷贝对象
        /// </summary>
        /// <returns></returns>
        T Clone();

        /// <summary>
        /// 深拷贝对象
        /// </summary>
        /// <returns></returns>
        T Copy();
    }
}