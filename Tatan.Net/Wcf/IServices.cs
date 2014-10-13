namespace Tatan.Net.Wcf
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    /// <summary>
    /// 通用服务接口
    /// </summary>
    [ServiceContract(Namespace="")]
    public interface IServices
    {
        /// <summary>
        /// 通用获取唯一数据对象操作
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(UriTemplate = "Query/{request}",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponse Query(ServiceRequest request);

        /// <summary>
        /// 通用添加数据对象实体操作
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "Add/{request}", 
            Method = "POST", 
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponse Add(ServiceRequest request);

        /// <summary>
        /// 通用删除数据对象实体操作
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "Delete/{request}",
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponse Delete(ServiceRequest request);

        /// <summary>
        /// 通用修改数据对象实体操作
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "Modify/{request}",
            Method = "PUT",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        ServiceResponse Modify(ServiceRequest request);
    }
}