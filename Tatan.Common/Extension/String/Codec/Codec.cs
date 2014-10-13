namespace Tatan.Common.Extension.String.Codec
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;

    using Cryptography;

    /// <summary>
    /// 编解码器，统一为UTF8格式
    /// </summary>
    public static class Codec
    {
        private static readonly IDictionary<string, Func<string, string, string>> _encodes = GetEncodes();
        private static IDictionary<string, Func<string, string, string>> GetEncodes()
        {
            var codes = new Dictionary<string, Func<string, string, string>>
            {
                { "md5", (s, k) => CipherFactory.GetCipher("md5").Encrypt(s, k) },
                { "sha1", (s, k) => CipherFactory.GetCipher("sha1").Encrypt(s, k) },
                { "des", (s, k) => CipherFactory.GetCipher("des").Encrypt(s, k) },
                { "aes", (s, k) => CipherFactory.GetCipher("aes").Encrypt(s, k) },
                { "base64", (s, k) => CipherFactory.GetCipher("base64").Encrypt(s, k) },

                { "html", (s, k) => HttpUtility.HtmlEncode(s) },
                { "url", (s, k) => HttpUtility.UrlEncode(s, Encoding.UTF8) },
            };

            return codes;
        }

        private static readonly IDictionary<string, Func<string, string, string>> _decodes = GetDecodes();
        private static IDictionary<string, Func<string, string, string>> GetDecodes()
        {
            var codes = new Dictionary<string, Func<string, string, string>>
            {
                { "md5", (s, k) => CipherFactory.GetCipher("md5").Decrypt(s, k) },
                { "sha1", (s, k) => CipherFactory.GetCipher("sha1").Decrypt(s, k) },
                { "des", (s, k) => CipherFactory.GetCipher("des").Decrypt(s, k) },
                { "aes", (s, k) => CipherFactory.GetCipher("aes").Decrypt(s, k) },
                { "base64", (s, k) => CipherFactory.GetCipher("base64").Decrypt(s, k) },

                { "html", (s, k) => HttpUtility.HtmlDecode(s) },
                { "url", (s, k) => HttpUtility.UrlDecode(s, Encoding.UTF8) },
            };

            return codes;
        }

        /// <summary>
        /// 将字符串编码
        /// </summary>
        /// <param name="value"></param>
        /// <param name="format">编码表达式</param>
        /// <param name="key"></param>
        /// <returns>输出文本</returns>
        public static string AsEncode(this string value, string format = null, string key = null)
        {
            if (string.IsNullOrEmpty(format) || !_encodes.ContainsKey(format.ToLower()))
                return value;

            try
            {
                return _encodes[format.ToLower()](value, key);
            }
            catch
            {
                return value;
            }
        }

        /// <summary>
        /// 将字符串编码
        /// </summary>
        /// <param name="value"></param>
        /// <param name="format">编码表达式</param>
        /// <param name="key"></param>
        /// <returns>输出文本</returns>
        public static string AsDecode(this string value, string format = null, string key = null)
        {
            if (string.IsNullOrEmpty(format) || !_decodes.ContainsKey(format.ToLower()))
                return value;

            try
            {
                return _decodes[format.ToLower()](value, key);
            }
            catch
            {
                return value;
            }
        }
    }
}