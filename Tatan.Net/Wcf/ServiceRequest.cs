namespace Tatan.Net.Wcf
{
    using System.Runtime.Serialization;

    /// <summary>
    /// 服务请求对象
    /// </summary>
    [DataContract]
    public class ServiceRequest
    {
        /// <summary>
        /// 在服务器端保存的服务对象全名
        /// </summary>
        [DataMember]
        public string Service { get; set; }

        /// <summary>
        /// 服务的行为名，默认为Call
        /// </summary>
        [DataMember]
        public string Action { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public object[] Arguments { get; set; }
    }
}