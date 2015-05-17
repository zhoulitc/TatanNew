namespace Tatan.Common.Cryptography.Internal
{
    using System.Text;
    using System.Security.Cryptography;

    /// <summary>
    /// 非对称密码抽象类
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    internal abstract class AsymmetricCipher : ICipher
    {
        protected static readonly string DefaultKey = string.Empty;
        protected static readonly Encoding Encoding = Encoding.UTF8;

        protected virtual HashAlgorithm CreateAsymmetricCipher() => null;

        public virtual string Encrypt(string expressly, string key = null)
        {
            if (string.IsNullOrEmpty(expressly))
                return string.Empty;
 
            byte[] data;
            key = string.IsNullOrEmpty(key) ? DefaultKey : key;
            using (var cipher = CreateAsymmetricCipher())
            {
                data = cipher.ComputeHash(Encoding.GetBytes(key + expressly));
                cipher.Clear();
            }
            var sb = new StringBuilder();
            foreach (var b in data)
                sb.Append(b.ToString("x"));
            return sb.ToString();
        }

        public string Decrypt(string ciphertext, string key = null) => ciphertext;
    }
}