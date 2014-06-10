namespace Tatan.Common.Cryptography.Internal
{
    using System;
    using System.Text;

    internal sealed class Base64Cipher : SymmetricCipher
    {
        #region 单例
        private static readonly Base64Cipher _instance = new Base64Cipher();
        private Base64Cipher()
        {
        }
        public static Base64Cipher Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        #region ICipher
        public override string Encrypt(string expressly, string key, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(expressly))
                return string.Empty;
            key = string.IsNullOrEmpty(key) ? Key : key;
            var data = (encoding ?? Encoding.Default).GetBytes(expressly + key);
            return Convert.ToBase64String(data);
        }

        public override string Decrypt(string ciphertext, string key, Encoding encoding = null)
        {
            var result = string.Empty;
            if (string.IsNullOrEmpty(ciphertext))
                return result;
            key = string.IsNullOrEmpty(key) ? Key : key;
            var data = Convert.FromBase64String(ciphertext);
            return (encoding ?? Encoding.Default).GetString(data).Replace(key, "");
        }
        #endregion
    }
}