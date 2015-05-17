using System;
using System.Net;
using System.Text.RegularExpressions;
using Tatan.Common.Net;

namespace Tatan._12306Logic.Common
{
    using System.Collections.Generic;
    using Tatan.Common.Exception;
    using Tatan.Common.Extension.String.Regular;

    public class CommonHandler
    {
        private static readonly Regex _regex = new Regex("[Pp][Aa][Tt][Hh]=/(otn)?(,)?", RegexOptions.Compiled);
        public static HttpWebResponse Request(string name, IDictionary<string, string> tags = null)
        {
            Assert.ArgumentNotNull(nameof(name), name);

            var jsonFile = AppDomain.CurrentDomain.BaseDirectory + "Requests\\" + name;
            return RequestHandler.Request(jsonFile, tags);
        }

        public static void SetCookie(IDictionary<string, string> input, HttpWebResponse response)
        {
            if (input == null || response == null) return;
            var cookie = response.Headers["Set-Cookie"];
            if (string.IsNullOrEmpty(cookie)) return;

            cookie = cookie.Replace(_regex, "").Trim().Trim(';');
            if (!input.ContainsKey(nameof(cookie)))
            {
                input[nameof(cookie)] = cookie.Trim().Trim(';');
            }
            else if (!input[nameof(cookie)].Contains(cookie))
            {
                input[nameof(cookie)] = input[nameof(cookie)] + "; " + cookie;
            }
        }
    }
}
