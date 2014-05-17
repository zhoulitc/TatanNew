namespace Tatan.Common.Cryptography.Internal
{
    using System.Text;
    using Exception;

    /// <summary>
    /// 非对称密码抽象类
    /// </summary>
    internal abstract class AsymmetricCipher : ICipher
    {
        private static readonly string _key = string.Empty;

        public string Encrypt(string expressly, Encoding encoding = null)
        {
            return Encrypt(expressly, _key, encoding);
        }

        public abstract string Encrypt(string expressly, string key, Encoding encoding = null);

        public string Decrypt(string ciphertext, Encoding encoding = null)
        {
            return ExceptionHandler.NotSupported<string>();
        }

        public string Decrypt(string ciphertext, string key, Encoding encoding = null)
        {
            return ExceptionHandler.NotSupported<string>();
        }
    }
}