namespace Tatan.Common.Cryptography.Internal
{
    using System.Security.Cryptography;
    using System.Text;

    internal sealed class Sha1Cipher : AsymmetricCipher
    {
        #region 单例
        private static readonly Sha1Cipher _instance = new Sha1Cipher();

        private Sha1Cipher()
        {
        }
        public static Sha1Cipher Instance
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
            byte[] data;
            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                data = sha1.ComputeHash((encoding ?? Encoding.Default).GetBytes(key + expressly));
                sha1.Clear();
            }
            var sb = new StringBuilder();
            foreach (var b in data)
                sb.Append(b.ToString("x"));
            return sb.ToString();
        }
        #endregion
    }
}