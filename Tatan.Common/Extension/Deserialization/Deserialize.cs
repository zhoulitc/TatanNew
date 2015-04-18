namespace Tatan.Common.Extension.Deserialization
{
    using Serialization;

    #region 提供枚举的转换扩展方法

    /// <summary>
    /// 提供枚举的转换扩展方法
    /// <para>此方法组不会抛出异常</para>
    /// </summary>
    public static class Deserialization
    {
        /// <summary>
        /// 将枚举类型转换为int
        /// </summary>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public static T Deserialize<T>(this string value, ISerializer serializer) where T : class
        {
            if (serializer == null || string.IsNullOrEmpty(value)) return null;

            return serializer.Deserialize<T>(value);
        }
    }

    #endregion
}