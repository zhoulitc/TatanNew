namespace Tatan.Common.Cryptography.Internal
{
    using System.Security.Cryptography;
    using System.Text;

    internal sealed class Md5Cipher : AsymmetricCipher
    {
        #region 单例
        private static readonly Md5Cipher _instance = new Md5Cipher();

        private Md5Cipher()
        {        }
        public static Md5Cipher Instance
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
            key = string.IsNullOrEmpty(key) ? Key : key;
            using (var md5 = new MD5CryptoServiceProvider())
            {
                data = md5.ComputeHash((encoding ?? Encoding.Default).GetBytes(key + expressly));
                md5.Clear();
            }
            var sb = new StringBuilder();
            foreach (var b in data)
                sb.Append(b.ToString("x"));
            return sb.ToString();
        }
        #endregion
    }
}