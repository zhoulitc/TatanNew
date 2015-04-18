using System.Runtime.Serialization;

namespace Tatan._12306Logic.Order
{
    [DataContract]
    public class OrderResult
    {
        [DataMember(Name = "submitStatus")]
        public bool Status { get; set; }
    }

}
