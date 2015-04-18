using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;

namespace Tatan.Common.Soa
{
    /// <summary>
    /// 时间操作
    /// </summary>
    public class SoaRequest
    {
        
    }

    /// <summary>
    /// 时间操作
    /// </summary>
    public class SoaResponse
    {

    }

    /// <summary>
    /// 时间操作
    /// </summary>
    [Serializable]
    public class SoapRequest : SoaRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, string> Arguments { get; set; }
    }

    /// <summary>
    /// 时间操作
    /// </summary>
    [Serializable]
    public class RestRequest : SoaRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> Arguments { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class RestResponse : SoaResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public List<List<string>> Results { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class SoapResponse : SoaResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public List<List<string>> Results { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CommonSoapHeader : SoapHeader
    {
        /// <summary>
        /// 
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }

        internal void SetContext(HttpContext context)
        {
            Username = string.IsNullOrEmpty(Username) ? context.Request.Headers["Username"] : Username;
            Password = string.IsNullOrEmpty(Password) ? context.Request.Headers["Password"] : Password;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ISoapAction
    {
        /// <summary>
        /// 调用
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        SoapResponse Call(SoapRequest request);

        /// <summary>
        /// 认证
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool Authenticate(string username, string password);
    }

    /// <summary>
    /// 
    /// </summary>
    public class CommonService : WebService
    {
        /// <summary>
        /// 
        /// </summary>
        public CommonSoapHeader Header { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [SoapHeader("Header")]
        [WebMethod]
        public SoapResponse Call(SoapRequest request)
        {
            if (Header == null)
            {
                throw new SoapException("header is not exist.", XmlQualifiedName.Empty);
            }
            if (string.IsNullOrEmpty(Header.Action))
            {
                throw new SoapException("action is empty.", XmlQualifiedName.Empty);
            }
            if (!CommonSoapManager.Actions.ContainsKey(Header.Action))
            {
                throw new SoapException("action is not exist.", XmlQualifiedName.Empty);
            }

            var action = CommonSoapManager.Actions[Header.Action];
            Header.SetContext(Context);
            if (!action.Authenticate(Header.Username, Header.Password))
            {
                throw new SoapException("client is not authentication.", XmlQualifiedName.Empty);
            }

            return action.Call(request);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class CommonSoapManager
    {
        /// <summary>
        /// 
        /// </summary>
        public static IDictionary<string, ISoapAction> Actions { get; private set; }
    }
}