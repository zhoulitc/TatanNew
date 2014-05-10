namespace Tatan.Common.Cryptography.Internal
{
    using System.Text;

    /// <summary>
    /// 对称密码抽象类
    /// </summary>
    internal abstract class SymmetricCipher : ICipher
    {
        private const string _key = "Z1l2t3c4";

        public string Encrypt(string expressly, Encoding encoding = null)
        {
            return Encrypt(expressly, _key, encoding);
        }

        public abstract string Encrypt(string expressly, string key, Encoding encoding = null);

        public string Decrypt(string ciphertext, Encoding encoding = null)
        {
            return Decrypt(ciphertext, _key, encoding);
        }

        public abstract string Decrypt(string ciphertext, string key, Encoding encoding = null);
    }
}