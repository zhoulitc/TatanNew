namespace Tatan.Common.Extension.Object
{
    using Serialization;

    #region 提供将对象序列化成字符串的扩展方法

    /// <summary>
    /// 提供将对象序列化成字符串的扩展方法
    /// <para>author:zhoulitcqq</para>
    /// <para>此方法组不会抛出异常</para>
    /// </summary>
    public static class ObjectExtension
    {
        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToJsonString(this object value)
        {
            if (value == null) return string.Empty;
            return Serializers.Json.Serialize(value);
        }

        /// <summary>
        /// 将对象序列化成Xml字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToXmlString(this object value)
        {
            if (value == null) return string.Empty;
            return Serializers.Xml.Serialize(value);
        }
    }

    #endregion
}