namespace Tatan.Common.Serialization
{
    using System;
    using Component;
    using Internal;

    /// <summary>
    /// 自定义Xml序列化适配接口
    /// </summary>
    public class XmlSerializerAdapter : IAdaptable
    {
        /// <summary>
        /// 配置一个使用自定义序列和反序列方式的xml串行接口
        /// </summary>
        /// <param name="serialize"></param>
        /// <param name="deserialize"></param>
        /// <returns></returns>
        public XmlSerializerAdapter(Func<object, string> serialize = null, Func<string, object> deserialize = null)
        {
            Serializers.Xml = new XmlSerializer(serialize, deserialize);
        }

        /// <summary>
        /// 配置一个自实现的序列化反序列化接口对象
        /// </summary>
        /// <param name="serializer"></param>
        public XmlSerializerAdapter(ISerializer serializer)
        {
            Serializers.Xml = serializer ?? XmlSerializer.Instance;
        }
    }
}