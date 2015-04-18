using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tatan.Common.Serialization;

namespace Tatan.Common.Extension.Net
{
    using Deserialization;

    /// <summary>
    /// Http响应扩展方法
    /// </summary>
    public static class WebResponseExtension
    {
        /// <summary>
        /// 获取响应内容
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetContent(this WebResponse value, Encoding encoding = null)
        {
            if (value == null) return string.Empty;

            var stream = value.GetResponseStream();
            if (stream == null) return string.Empty;
            using (var reader = new StreamReader(stream, encoding ?? Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// 获取响应内容
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static T GetJsonObject<T>(this WebResponse value, Encoding encoding = null) where T : class
        {
            return GetContent(value, encoding).Deserialize<T>(Serializers.Json);
        }

        /// <summary>
        /// 获取响应内容
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static async Task<string> GetContentAsync(this WebResponse value, Encoding encoding = null)
        {
            if (value == null) return string.Empty;

            var stream = value.GetResponseStream();
            if (stream == null) return string.Empty;

            using (var reader = new StreamReader(stream, encoding ?? Encoding.UTF8))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}