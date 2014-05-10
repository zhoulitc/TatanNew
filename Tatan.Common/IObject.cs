namespace Tatan.Common
{
    /// <summary>
    /// 对象接口，提供对象通用方法
    /// </summary>
    public interface IObject
    {
        /// <summary>
        /// 是否与另一个对象完全相等
        /// </summary>
        /// <param name="obj">另一个对象</param>
        /// <returns>是否相等</returns>
        bool Equals(object obj);

        /// <summary>
        /// 获取对象的hash码，如果两个对象相等，则hash码一定相等
        /// </summary>
        /// <returns>hash码</returns>
        int GetHashCode();

        /// <summary>
        /// 获取对象的字符串描述
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">当字符串超过最大长度时抛出</exception>
        /// <returns>对象的字符串描述</returns>
        string ToString();
    }
}