using System.Runtime.Serialization;

namespace Tatan._12306Logic.Common
{
    [DataContract]
    public class CommonResult
    {
        [DataMember(Name = "msg")]
        public string Message { get; set; }

        [DataMember(Name = "result")]
        public string Result { get; set; }
    }
}
