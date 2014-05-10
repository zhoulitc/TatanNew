namespace Tatan.Common.Serialization
{
    /// <summary>
    /// 串行接口，提供序列化和反序列化方法
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="text">字符串</param>
        /// <exception cref="System.Text.DecoderExceptionFallback">回退时</exception>
        /// <returns>指定对象</returns>
        T Deserialize<T>(string text);

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">指定对象</param>
        /// <exception cref="System.ArgumentException">包含无效Unicode码时</exception>
        /// <exception cref="System.Runtime.Serialization.InvalidDataContractException">正在序列化类型不符合协议规定时</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">正在写入的实例出现问题</exception>
        /// <exception cref="System.ServiceModel.QuotaExceededException">已超出序列化最大数量时</exception>
        /// <exception cref="System.Text.EncoderExceptionFallback">回退时</exception>
        /// <exception cref="System.InvalidOperationException">序列化期间发生错误时</exception>
        /// <returns>json字符串</returns>
        string Serialize(object obj);
    }
}