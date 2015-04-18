namespace Tatan.Common.Serialization
{
    using System;
    using Internal;

    /// <summary>
    /// 串行对象
    /// </summary>
    public static class Serializers
    {
        /// <summary>
        /// 获取json串行接口
        /// </summary>
        public static ISerializer Json
        {
            get
            {
                return JsonSerializer.Instance;
            }
        }

        /// <summary>
        /// 获取一个使用自定义序列和反序列方式的json串行接口
        /// </summary>
        /// <param name="serialize"></param>
        /// <param name="deserialize"></param>
        /// <returns></returns>
        public static ISerializer CreateJsonSerializer(Func<object, string> serialize, Func<string, object> deserialize)
        {
            return new JsonSerializer(serialize, deserialize);
        }

        /// <summary>
        /// 获取xml串行接口
        /// </summary>
        public static ISerializer Xml
        {
            get
            {
                return XmlSerializer.Instance;
            }
        }

        /// <summary>
        /// 获取一个使用自定义序列和反序列方式的xml串行接口
        /// </summary>
        /// <param name="serialize"></param>
        /// <param name="deserialize"></param>
        /// <returns></returns>
        public static ISerializer CreateXmlSerializer(Func<object, string> serialize, Func<string, object> deserialize)
        {
            return new XmlSerializer(serialize, deserialize);
        }
    }
}