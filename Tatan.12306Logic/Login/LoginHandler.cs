using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Tatan.Common.Extension.Net;
using Tatan._12306Logic.Common;

namespace Tatan._12306Logic.Login
{
    public static class LoginHandler
    {
        public static event Action<IDictionary<string, string>> InitBefore;

        public static event Action<IDictionary<string, string>, HttpWebResponse> InitAfter;

        public static event Action<IDictionary<string, string>> RequestBefore;

        public static event Action<IDictionary<string, string>, HttpWebResponse> RequestAfter;

        /// <summary>
        /// 初始化请求Cookie，产生会话Id
        /// </summary>
        /// <param name="input"></param>
        public static void Init(IDictionary<string, string> input)
        {
            if (InitBefore != null)
                InitBefore(input);

            var response = CommonHandler.Request(@"Login\Init");
            CommonHandler.SetCookie(input, response);

            if (InitAfter != null)
                InitAfter(input, response);
        }

        /// <summary>
        /// 用于获取验证码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetCode(IDictionary<string, string> input)
        {
            var response = CommonHandler.Request(@"Login\GetCode", input);
            CommonHandler.SetCookie(input, response);

            var directory = AppDomain.CurrentDomain.BaseDirectory + "Codes";
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            var file = directory + "\\" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".png";
            var buffer = new byte[response.ContentLength];
            using (var stream = response.GetResponseStream())
            {
                if (stream == null) return string.Empty;
                stream.Read(buffer, 0, buffer.Length);
            }
            using (var f = new FileStream(file, FileMode.OpenOrCreate))
            {
                f.Write(buffer, 0, buffer.Length);
            }
            return file;
        }

        public static bool Request(IDictionary<string, string> input)
        {
            if (RequestBefore != null)
                RequestBefore(input);

            CheckCode(input);

            var response = Login(input);
            if (response == null)
                return false;

            if (RequestAfter != null)
                RequestAfter(input, response);

            return true;
        }

        private static void CheckCode(IDictionary<string, string> input)
        {
            var response = CommonHandler.Request(@"Login\CheckCode", input);
            var commonResponse = response.GetJsonObject<CommonResponse<CommonResult>>();
            if (commonResponse.Data.Result != "1")
            {
                throw new Exception("check code error.");
            }
        }

        private static HttpWebResponse Login(IDictionary<string, string> input)
        {
            var response = CommonHandler.Request(@"Login\Login", input);
            var commonResponse = response.GetJsonObject<CommonResponse<LoginResult>>();
            if (!commonResponse.Status)
                return null;

            CommonHandler.SetCookie(input, response);
            return response;
        }
    }
}