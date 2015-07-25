namespace Tatan.Common.Net
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using Exception;
    using Extension.Net;
    using Extension.String.Convert;
    using Extension.String.IO;
    using Extension.String.Template;
    using Serialization;

    /// <summary>
    /// 请求处理类
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public static class RequestHandler
    {
        /// <summary>
        /// 根据请求文件执行一个请求，同步返回其响应
        /// </summary>
        /// <param name="jsonFile"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static HttpWebResponse Request(string jsonFile, IDictionary<string, string> tags = null)
        {
            var entity = Create(jsonFile, tags);
            if (entity == null)
            {
                throw new Exception("request is null. jsonFile:" + jsonFile);
            }

            var request = GetRequest(entity);
            var response = request.GetResponse(entity.Body) as HttpWebResponse;
            if (response == null)
            {
                throw new Exception("response is null. url:" + entity.Url);
            }
            return response;
        }

        /// <summary>
        /// 创建一个请求对象
        /// </summary>
        /// <param name="jsonFile"></param>
        /// <param name="tags"></param>
        public static Request Create(string jsonFile, IDictionary<string, string> tags = null)
        {
            Assert.ArgumentNotNull(nameof(jsonFile), jsonFile);

            if (jsonFile.GetExtension().ToLower() != "json")
                jsonFile += ".json";

            Assert.FileFound(jsonFile);

            var content = jsonFile.ReadFile(Encoding.UTF8);
            if (tags != null)
            {
                content = content.Replace("@", "@", tags);
            }

            return Serializers.Json.Deserialize<Request>(content);
        }

        private static HttpWebRequest GetRequest(Request entity)
        {
            var request = WebRequest.CreateHttp(entity.Url);
            foreach (var header in entity.Headers)
            {
                if (header.Key == "Referer")
                    request.Referer = header.Value;
                else if (header.Key == "Host")
                    request.Host = header.Value;
                else if (header.Key == "Connection" && header.Value == "Keep-Alive")
                    request.KeepAlive = true;
                else if (header.Key == "Accept")
                    request.Accept = header.Value;
                else if (header.Key == "Content-Type")
                    request.ContentType = header.Value;
                else if (header.Key == "User-Agent")
                    request.UserAgent = header.Value;
                else if (header.Key == "If-Modified-Since")
                    request.IfModifiedSince = header.Value.As<DateTime>();
                else
                    request.Headers[header.Key] = header.Value;
            }
            return request;
        }
    }
}