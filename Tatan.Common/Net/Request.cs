namespace Tatan.Common.Net
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// 从Json文件中读取的请求对象
    /// <para>author:zhoulitcqq</para>
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
        /// 请求体
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

        /// <summary>
        /// 获取请求方法
        /// </summary>
        public string Method
        {
            get
            {
                if (Content == null || Content.Count <= 0)
                    return "GET";

                if (Headers.ContainsKey("Method"))
                {
                    var method = Headers["Method"];
                    if (_methods.Contains(method))
                        return method;
                }

                return "POST";
            }
        }

        private static readonly HashSet<string> _methods;

        static Request()
        {
            _methods = new HashSet<string> {"GET", "POST", "PUT", "DELETE"};
        }
    }
}