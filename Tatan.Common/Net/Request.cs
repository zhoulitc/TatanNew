using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Tatan.Common.Net
{
    /// <summary>
    /// 从Json文件中读取的请求对象
    /// </summary>
    [DataContract]
    public class Request
    {
        /// <summary>
        /// 请求连接
        /// </summary>
        [DataMember]
        public string Url { get; set; }

        /// <summary>
        /// 请求头
        /// </summary>
        [DataMember]
        public IDictionary<string, string> Headers { get; set; }

        /// <summary>
        /// 请求体，没有请求体时，Method为GET。否则会去Headers中寻找Method，如果Method错误或者没有，则设置Method为POST
        /// </summary>
        [DataMember]
        public IDictionary<string, string> Content { get; set; }

        /// <summary>
        /// 获取真正的请求体
        /// </summary>
        public string Body
        {
            get
            {
                var s = string.Empty;
                if (Content == null || Content.Count <= 0)
                    return s;

                foreach (var p in Content)
                {
                    s += string.Format("&{0}={1}", p.Key, p.Value);
                }
                return s.Substring(1);
            }
        }
    }
}