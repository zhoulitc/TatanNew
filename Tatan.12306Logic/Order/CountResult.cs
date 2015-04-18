using System.Runtime.Serialization;

namespace Tatan._12306Logic.Order
{
    [DataContract]
    public class CountResult
    {
        [DataMember(Name = "count")]
        public string Count { get; set; }

        [DataMember(Name = "ticket")]
        public string Ticket { get; set; }

        [DataMember(Name = "op_2")]
        public string Op2 { get; set; }

        [DataMember(Name = "countT")]
        public string CountT { get; set; }

        [DataMember(Name = "op_1")]
        public string Op1 { get; set; }
    }

}
