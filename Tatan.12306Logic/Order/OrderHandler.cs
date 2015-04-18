using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Tatan.Common.Extension.Net;
using Tatan._12306Logic.Common;
using Tatan._12306Logic.Login;

namespace Tatan._12306Logic.Order
{
    public static class OrderHandler
    {
        public static event Action<IDictionary<string, string>> RequestBefore;

        public static event Action<IDictionary<string, string>, HttpWebResponse> RequestAfter;

        /// <summary>
        /// 初始化之前的操作
        /// </summary>
        public static event Action<IDictionary<string, string>> InitBefore;

        /// <summary>
        /// 初始化之后的操作
        /// </summary>
        public static event Action<IDictionary<string, string>, HttpWebResponse> InitAfter;

        /// <summary>
        /// 请求火车票
        /// </summary>
        /// <param name="input"></param>
        public static void Init(IDictionary<string, string> input)
        {
            if (InitBefore != null)
                InitBefore(input);

            var response = CommonHandler.Request(@"Order\Init", input);

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
            var response = CommonHandler.Request(@"Order\GetCode", input);
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
            CheckOrder(input);

            if (!HasCount(input))
                return false;

            var response = Submit(input);
            if (response == null)
                return false;

            if (RequestAfter != null)
                RequestAfter(input, response);

            return true;
        }

        private static void CheckCode(IDictionary<string, string> input)
        {
            var response = CommonHandler.Request(@"Order\CheckCode", input);
            var commonResponse = response.GetJsonObject<CommonResponse<CommonResult>>();
            if (commonResponse.Data.Result != "1")
            {
                throw new Exception("提交订单时检查验证码出错");
            }
        }

        private static void CheckOrder(IDictionary<string, string> input)
        {
            var response = CommonHandler.Request(@"Order\CheckOrder", input);
            var commonResponse = response.GetJsonObject<CommonResponse<OrderResult>>();
            if (!commonResponse.Data.Status)
            {
                throw new Exception("检查订单时出错");
            }
        }

        private static bool HasCount(IDictionary<string, string> input)
        {
            var response = CommonHandler.Request(@"Order\GetCount", input);
            var commonResponse = response.GetJsonObject<CommonResponse<CountResult>>();
            return !commonResponse.Data.Ticket.EndsWith("0000");
        }

        private static HttpWebResponse Submit(IDictionary<string, string> input)
        {
            var response = CommonHandler.Request(@"Order\Submit", input);
            var commonResponse = response.GetJsonObject<CommonResponse<LoginResult>>();
            if (!commonResponse.Status)
                return null;

            CommonHandler.SetCookie(input, response);
            return response;
        }
    }
}