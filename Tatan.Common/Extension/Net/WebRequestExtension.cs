using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Tatan.Common.Extension.Net
{
    /// <summary>
    /// Http请求扩展方法
    /// </summary>
    public static class WebRequestExtension
    {
        /// <summary>
        /// 获取请求结果
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Get(this WebRequest value)
        {
            var response = GetResponse(value, string.Empty);
            if (response == null) return string.Empty;

            return response.GetContent();
        }

        /// <summary>
        /// 获取请求结果
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task<string> GetAsync(this WebRequest value)
        {
            var response = await GetResponseAsync(value, string.Empty);
            if (response == null) return string.Empty;

            return await response.GetContentAsync();
        }

        /// <summary>
        /// 提交数据并获取响应结果
        /// </summary>
        /// <param name="value"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Post(this WebRequest value, string data)
        {
            if (string.IsNullOrEmpty(data))
                return string.Empty;

            var response = GetResponse(value, data);
            if (response == null) return string.Empty;

            return response.GetContent();
        }

        /// <summary>
        /// 提交数据并获取响应结果
        /// </summary>
        /// <param name="value"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<string> PostAsync(this WebRequest value, string data)
        {
            if (string.IsNullOrEmpty(data))
                return string.Empty;

            var response = await GetResponseAsync(value, data);
            if (response == null) return string.Empty;

            return await response.GetContentAsync();
        }

        /// <summary>
        /// 获取响应对象
        /// </summary>
        /// <param name="value"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static WebResponse GetResponse(this WebRequest value, string data = null)
        {
            if (value == null) return null;
            if (string.IsNullOrEmpty(data))
            {
                value.Method = "GET";
                return value.GetResponse();
            }

            value.Method = "POST";
            var buffer = Encoding.UTF8.GetBytes(data);
            using (var stream = value.GetRequestStream())
            {
                stream.Write(buffer, 0, buffer.Length);
            }
            return value.GetResponse();
        }

        /// <summary>
        /// 获取响应对象
        /// </summary>
        /// <param name="value"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<WebResponse> GetResponseAsync(this WebRequest value, string data)
        {
            if (value == null) return null;
            if (string.IsNullOrEmpty(data))
            {
                value.Method = "GET";
                return await value.GetResponseAsync();
            }

            value.Method = "POST";
            using (var wirter = new StreamWriter(value.GetRequestStream(), Encoding.UTF8))
            {
                wirter.Write(data);
            }
            return await value.GetResponseAsync();
        }
    }
}