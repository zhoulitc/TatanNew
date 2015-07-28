namespace Tatan.Common.Serialization
{
    using Internal;

    /// <summary>
    /// 串行对象
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    internal static class Serializers
    {
        /// <summary>
        /// 获取json串行接口
        /// </summary>
        public static ISerializer Json { get; internal set; } = JsonSerializer.Instance;

        /// <summary>
        /// 获取xml串行接口
        /// </summary>
        public static ISerializer Xml { get; internal set; } = XmlSerializer.Instance;
    }
}