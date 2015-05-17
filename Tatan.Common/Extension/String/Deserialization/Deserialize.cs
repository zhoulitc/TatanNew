namespace Tatan.Common.Extension.String.Deserialization
{
    using Serialization;

    #region 提供枚举的转换扩展方法

    /// <summary>
    /// 提供枚举的转换扩展方法
    /// <para>author:zhoulitcqq</para>
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
            if (string.IsNullOrEmpty(value)) return null;

            value = value.Trim();
            if (serializer == null)
            {
                if (SameJson(value))
                {
                    serializer = Serializers.Json;
                }
                else if (SameXml(value))
                    serializer = Serializers.Xml;
                else
                    return null;
            }
            return serializer.Deserialize<T>(value);
        }

        private static bool SameJson(string value)
        {
            return (value.StartsWith("{") && value.EndsWith("}")) || (value.StartsWith("[") && value.EndsWith("]"));
        }

        private static bool SameXml(string value)
        {
            return (value.StartsWith("<?xml") && value.EndsWith(">"));
        }
    }

    #endregion
}