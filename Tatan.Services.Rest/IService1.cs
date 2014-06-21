using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Tatan.Services.Rest
{
    [ServiceContract(Namespace="")]
    public interface IService1
    {
        [OperationContract]
        [WebGet(UriTemplate = "Data/{name}", 
            ResponseFormat = WebMessageFormat.Json)]
        Person GetData(string name);

        [OperationContract]
        [WebInvoke(
            UriTemplate = "Data/{name}/{age}", 
            Method = "POST", 
            ResponseFormat = WebMessageFormat.Json, 
            RequestFormat=WebMessageFormat.Json)]
        RestResult AddData(string name, string age);

        [OperationContract]
        [WebInvoke(
            UriTemplate = "Data/{name}",
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        RestResult DeleteData(string name);

        [OperationContract]
        [WebInvoke(
            UriTemplate = "Data/{name}/{age}", 
            Method = "PUT",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        RestResult EditData(string name, string age);
    }


    // 使用下面示例中说明的数据约定将复合类型添加到服务操作。
    [DataContract]
    public class RestResult
    {
        [DataMember]
        public bool IsSuccess { get; set; }

        [DataMember]
        public string Message { get; set; }

        private RestResult()
        {
            IsSuccess = false;
            Message = string.Empty;
        }

        public static RestResult Success(string message = null)
        {
            return new RestResult {IsSuccess = true, Message = message ?? string.Empty};
        }

        public static RestResult Fail(string message = null)
        {
            return new RestResult { Message = message ?? string.Empty };
        }
    }

    [DataContract]
    public class Person
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Age { get; set; }
    }
}
