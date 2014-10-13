namespace Tatan.Net.Wcf
{
    using System.Runtime.Serialization;

    /// <summary>
    /// 命令结果
    /// </summary>
    [DataContract]
    public class ServiceResponse
    {
        /// <summary>
        /// 响应状态
        /// <para>等于0:成功</para>
        /// <para>小于0:失败</para>
        /// <para>大于0:异常</para>
        /// </summary>
        [DataMember]
        public int Status { get; set; }

        /// <summary>
        /// 响应内容，处理成功后返回的数据
        /// </summary>
        [DataMember]
        public object Context { get; set; }

        /// <summary>
        /// 响应消息，处理失败后返回的文本
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        private ServiceResponse()
        {
            Status = -1;
            Context = string.Empty;
            Message = string.Empty;
        }

        /// <summary>
        /// 表示一个成功的响应
        /// </summary>
        /// <param name="context">成功返回的内容，json格式</param>
        /// <returns></returns>
        public static ServiceResponse Success(object context = null)
        {
            return new ServiceResponse { Status = 0, Context = context ?? string.Empty };
        }

        /// <summary>
        /// 表示一个失败的响应
        /// </summary>
        /// <param name="status"></param>
        /// <param name="message">失败或异常返回的消息</param>
        /// <returns></returns>
        public static ServiceResponse Fail(string message = null, int status = -1)
        {
            return new ServiceResponse { Status = status < 0 ? status : -1, Message = message ?? string.Empty };
        }

        /// <summary>
        /// 表示一个异常的响应
        /// </summary>
        /// <param name="status"></param>
        /// <param name="message">失败或异常返回的消息</param>
        /// <returns></returns>
        public static ServiceResponse Exception(string message = null, int status = 1)
        {
            return new ServiceResponse { Status = status > 0 ? status : 1, Message = message ?? string.Empty };
        }
    }
}