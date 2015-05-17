namespace Tatan.Common.Cryptography.Internal
{
    /// <summary>
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    internal sealed class NullCipher : ICipher
    {
        #region 单例

        private static readonly NullCipher _instance = new NullCipher();

        private NullCipher()
        {
        }

        public static NullCipher Instance
        {
            get { return _instance; }
        }

        #endregion

        #region ICipher

        public string Encrypt(string expressly, string key = null)
        {
            return expressly;
        }

        public string Decrypt(string ciphertext, string key = null)
        {
            return ciphertext;
        }

        #endregion
    }
}