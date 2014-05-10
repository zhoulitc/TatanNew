namespace Tatan.Common.Cryptography.Internal
{
    using System.Text;

    internal sealed class NullCipher : ICipher
    {
        #region 单例
        private static readonly NullCipher _instance = new NullCipher();
        private NullCipher() { }
        public static NullCipher Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        #region ICipher
        public string Encrypt(string expressly, Encoding encoding = null)
        {
            return expressly;
        }

        public string Encrypt(string expressly, string key, Encoding encoding = null)
        {
            return expressly;
        }

        public string Decrypt(string ciphertext, Encoding encoding = null)
        {
            return ciphertext;
        }

        public string Decrypt(string ciphertext, string key, Encoding encoding = null)
        {
            return ciphertext;
        }
        #endregion
    }
}