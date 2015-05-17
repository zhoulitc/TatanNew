namespace Tatan.Common.Cryptography.Internal
{
    using System;

    /// <summary>
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    internal sealed class Base64Cipher : SymmetricCipher
    {
        #region 单例

        private static readonly Base64Cipher _instance = new Base64Cipher();

        private Base64Cipher() { }

        public static Base64Cipher Instance => _instance;

        #endregion

        #region ICipher

        public override string Encrypt(string expressly, string key = null)
        {
            if (string.IsNullOrEmpty(expressly))
                return string.Empty;
            key = string.IsNullOrEmpty(key) ? DefaultKey : key;
            var data = Encoding.GetBytes(expressly + key);
            return Convert.ToBase64String(data);
        }

        public override string Decrypt(string ciphertext, string key = null)
        {
            var result = string.Empty;
            if (string.IsNullOrEmpty(ciphertext))
                return result;
            key = string.IsNullOrEmpty(key) ? DefaultKey : key;
            var data = Convert.FromBase64String(ciphertext);
            return Encoding.GetString(data).Replace(key, "");
        }

        #endregion
    }
}