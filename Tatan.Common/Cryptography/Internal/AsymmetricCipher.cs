namespace Tatan.Common.Cryptography.Internal
{
    using System.Text;
    using Exception;

    /// <summary>
    /// 非对称密码抽象类
    /// </summary>
    internal abstract class AsymmetricCipher : ICipher
    {
        protected static readonly string Key = string.Empty;

        public string Encrypt(string expressly, Encoding encoding = null)
        {
            return Encrypt(expressly, Key, encoding);
        }

        public abstract string Encrypt(string expressly, string key, Encoding encoding = null);

        public string Decrypt(string ciphertext, Encoding encoding = null)
        {
            return ciphertext;
        }

        public string Decrypt(string ciphertext, string key, Encoding encoding = null)
        {
            return ciphertext;
        }
    }
}