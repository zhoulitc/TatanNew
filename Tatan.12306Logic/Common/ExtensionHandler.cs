using System.Collections.Generic;
using System.Net;
using MSScriptControl;
using Tatan.Common.Extension.Net;
using Tatan.Common.Extension.String.Codec;

namespace Tatan._12306Logic.Common
{
    /// <summary>
    /// 用于初始化的扩展处理
    /// </summary>
    public class ExtensionHandler
    {
        /// <summary>
        /// 处理动态JS
        /// </summary>
        /// <param name="input"></param>
        /// <param name="res"></param>
        public static void HandleDynamicJs(IDictionary<string, string> input, HttpWebResponse res)
        {
            var content = res.GetContent();
            var begin = content.IndexOf("/otn/dynamicJs/");
            if (begin < 0)
                return;
            var end = content.IndexOf("\" ", begin);
            input["url"] = "https://kyfw.12306.cn" + content.Substring(begin, end - begin);

            var response = CommonHandler.Request("DynamicJs", input);

            var jsContent = response.GetContent();
            var s1 = GetFunction(jsContent, "Base32");
            var s2 = GetFunction(jsContent, "bin216");
            var s3 = GetFunction(jsContent, "encode32");
            var key = GetKey(jsContent);

            IScriptControl script = new ScriptControl { Language = "javascript", AllowUI = true };
            script.AddCode(s1);
            script.AddCode(s2);
            script.AddCode(s3);
            script.AddCode("var keyStr = \"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=\";");
            string value = script.Eval("eval(\"encode32(bin216(Base32.encrypt('1111','" + key + "')))\")").ToString();
            input["key"] = key;
            input["value"] = value.AsEncode(Coding.Url);

            begin = content.IndexOf("var globalRepeatSubmitToken = '");
            if (begin > 0)
            {
                begin += "var globalRepeatSubmitToken = '".Length;
                end = content.IndexOf("';", begin);
                input["token"] = content.Substring(begin, end - begin);
            }
            begin = content.IndexOf("'key_check_isChange':'");
            if (begin > 0)
            {
                begin += "'key_check_isChange':'".Length;
                end = content.IndexOf("',", begin);
                input["isChange"] = content.Substring(begin, end - begin);
            }
        }

        private static string GetKey(string content)
        {
            var index = content.IndexOf("function gc()");
            if (index < 0) return string.Empty;

            var left = content.IndexOf("'", index) + 1;
            if (left < 0) return string.Empty;

            var right = content.IndexOf("'", left);
            if (right < 0) return string.Empty;

            return content.Substring(left, right - left);
        }

        private static string GetFunction(string content, string name)
        {
            var index = content.IndexOf("var " + name);
            if (index < 0)
            {
                index = content.IndexOf("function " + name);
                if (index < 0)
                    return string.Empty;
            }

            var left = content.IndexOf('{', index);
            if (left < 0) return string.Empty;

            var right = -1;

            var stack = 1;
            for (var i = left + 1; i < content.Length; i++)
            {
                var c = content[i];
                if (c == '{') stack++;
                else if (c == '}') stack--;

                if (stack <= 0)
                {
                    right = i;
                    break;
                }
            }
            if (stack > 0 || right < left) return string.Empty;
            return content.Substring(index, right - index + 1);
        }
    }
}