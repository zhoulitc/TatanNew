using System.Runtime.Serialization;

namespace Tatan._12306Logic.Login
{
    [DataContract]
    public class LoginResult
    {
        [DataMember(Name = "loginAddress")]
        public string LoginAddress { get; set; }

        [DataMember(Name = "loginCheck")]
        public string LoginCheck { get; set; }
    }
}
