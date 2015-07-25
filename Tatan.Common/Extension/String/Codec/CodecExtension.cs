namespace Tatan.Common.Extension.String.Codec
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;
    using Cryptography;
    using Logging;

    /// <summary>
    /// 编解码枚举
    /// </summary>
    public enum Coding
    {
        /// <summary>
        /// 不对字符串做编解码
        /// </summary>
        None = 0,

        /// <summary>
        /// 将字符串按照MD5的方式编解码
        /// </summary>
        Md5 = 1,

        /// <summary>
        /// 将字符串按照SHA1的方式编解码
        /// </summary>
        Sha1 = 2,

        /// <summary>
        /// 将字符串按照DES的方式编解码
        /// </summary>
        Des = 3,

        /// <summary>
        /// 将字符串按照AES的方式编解码
        /// </summary>
        Aes = 4,

        /// <summary>
        /// 将字符串按照BASE64的方式编解码
        /// </summary>
        Base64 = 5,

        /// <summary>
        /// 将字符串按照HTML的方式编解码
        /// </summary>
        Html = 6,

        /// <summary>
        /// 将字符串按照URL的方式编解码
        /// </summary>
        Url = 7
    }

    /// <summary>
    /// 编解码器，默认统一为UTF8格式
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public static class CodecExtension
    {

        /// <summary>
        /// 将字符串加密/编码
        /// </summary>
        /// <param name="value"></param>
        /// <param name="format">编码格式</param>
        /// <param name="key">如果是加密，则为密匙，如果是URL编码，则为编码格式</param>
        /// <returns>输出文本</returns>
        public static string AsEncode(this string value, Coding format = Coding.None, string key = null)
        {
            return AsCoding(value, key, format, _encodes);
        }

        /// <summary>
        /// 将字符串解密/解码
        /// </summary>
        /// <param name="value"></param>
        /// <param name="format">编码表达式</param>
        /// <param name="key">如果是解密，则为密匙，如果是URL解码，则为解码格式</param>
        /// <returns>输出文本</returns>
        public static string AsDecode(this string value, Coding format = Coding.None, string key = null)
        {
            return AsCoding(value, key, format, _decodes);
        }

        private static readonly IDictionary<string, Encoding> _encodings = GetEncodings();

        private static IDictionary<string, Encoding> GetEncodings()
        {
            var infos = Encoding.GetEncodings();
            var encodings = new Dictionary<string, Encoding>(infos.Length + 1);
            foreach (var info in infos)
            {
                var encoding = info.GetEncoding();
                encodings[info.Name.ToLower()] = encoding;
            }
            return encodings;
        }

        private static Encoding GetEncoding(string name)
        {
            if (string.IsNullOrEmpty(name))
                return Encoding.UTF8;
            return !_encodings.ContainsKey(name) ? Encoding.UTF8 : _encodings[name];
        }

        private static readonly IDictionary<Coding, Func<string, string, string>> _encodes = GetEncodes();

        private static IDictionary<Coding, Func<string, string, string>> GetEncodes()
        {
            var codes = new Dictionary<Coding, Func<string, string, string>>
            {
                [Coding.Md5] = (s, k) => CipherFactory.GetCipher(Coding.Md5.ToString().ToLower()).Encrypt(s, k),
                [Coding.Sha1] = (s, k) => CipherFactory.GetCipher(Coding.Sha1.ToString().ToLower()).Encrypt(s, k),
                [Coding.Des] = (s, k) => CipherFactory.GetCipher(Coding.Des.ToString().ToLower()).Encrypt(s, k),
                [Coding.Aes] = (s, k) => CipherFactory.GetCipher(Coding.Aes.ToString().ToLower()).Encrypt(s, k),
                [Coding.Base64] = (s, k) => CipherFactory.GetCipher(Coding.Base64.ToString().ToLower()).Encrypt(s, k),
                [Coding.Html] = (s, k) => HttpUtility.HtmlEncode(s),
                [Coding.Url] = (s, k) => HttpUtility.UrlEncode(s, GetEncoding(k))
            };

            return codes;
        }

        private static readonly IDictionary<Coding, Func<string, string, string>> _decodes = GetDecodes();

        private static IDictionary<Coding, Func<string, string, string>> GetDecodes()
        {
            var codes = new Dictionary<Coding, Func<string, string, string>>
            {
                [Coding.Md5] = (s, k) => CipherFactory.GetCipher(Coding.Md5.ToString().ToLower()).Decrypt(s, k),
                [Coding.Sha1] = (s, k) => CipherFactory.GetCipher(Coding.Sha1.ToString().ToLower()).Decrypt(s, k),
                [Coding.Des] = (s, k) => CipherFactory.GetCipher(Coding.Des.ToString().ToLower()).Decrypt(s, k),
                [Coding.Aes] = (s, k) => CipherFactory.GetCipher(Coding.Aes.ToString().ToLower()).Decrypt(s, k),
                [Coding.Base64] = (s, k) => CipherFactory.GetCipher(Coding.Base64.ToString().ToLower()).Decrypt(s, k),
                [Coding.Html] = (s, k) => HttpUtility.HtmlDecode(s),
                [Coding.Url] = (s, k) => HttpUtility.UrlDecode(s, GetEncoding(k))
            };

            return codes;
        }

        private static string AsCoding(string value, string key, Coding format,
            IDictionary<Coding, Func<string, string, string>> codec)
        {
            if (!codec.ContainsKey(format))
                return value;

            try
            {
                return codec[format](value, key);
            }
            catch (Exception ex)
            {
                Log.Warn(ex.Message, ex);
                return value;
            }
        }
    }
}