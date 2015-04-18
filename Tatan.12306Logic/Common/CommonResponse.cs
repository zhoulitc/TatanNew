using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Tatan._12306Logic.Common
{
    [DataContract]
    public class CommonResponse<T>
    {
        [DataMember(Name = "data")]
        public T Data { get; set; }

        [DataMember(Name = "httpstatus")]
        public int HttpStatus { get; set; }

        [DataMember(Name = "status")]
        public bool Status { get; set; }

        [DataMember(Name = "validateMessagesShowId")]
        public string ValidateMessagesShowId { get; set; }

        [DataMember(Name = "messages")]
        public List<object> Messages { get; set; }

        [DataMember(Name = "validateMessages")]
        public object ValidateMessages { get; set; }
    }
}
