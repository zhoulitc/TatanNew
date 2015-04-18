using System.Runtime.Serialization;

namespace Tatan._12306Logic.Query
{
    [DataContract]
    public class CheckResult
    {
        [DataMember(Name = "flag")]
        public bool Flag { get; set; }
    }

}
