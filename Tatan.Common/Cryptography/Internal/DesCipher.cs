namespace Tatan.Common.Cryptography.Internal
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    internal sealed class DesCipher : SymmetricCipher
    {
        #region 单例
        private static readonly DesCipher _instance = new DesCipher();
        private DesCipher()
        {
        }
        public static DesCipher Instance
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
            byte[] bytes;
            var data = (encoding ?? Encoding.Default).GetBytes(expressly);
            using (SymmetricAlgorithm sa = new DESCryptoServiceProvider())
            {
                sa.Key = Encoding.ASCII.GetBytes(key);
                sa.IV = Encoding.ASCII.GetBytes(key);

                var ms = new MemoryStream();
                using (var cs = new CryptoStream(ms, sa.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    sa.Clear();
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    bytes = ms.ToArray();
                }
            }
            var sb = new StringBuilder();
            foreach (var b in bytes)
                sb.AppendFormat("{0:X2}", b);
            return sb.ToString();
        }

        public override string Decrypt(string ciphertext, string key, Encoding encoding = null)
        {
            var result = string.Empty;
            if (string.IsNullOrEmpty(ciphertext))
                return result;
            var len = ciphertext.Length / 2;
            var data = new byte[len];
            for (var i = 0; i < len; i++)
                data[i] = Convert.ToByte(Convert.ToInt32(ciphertext.Substring(i * 2, 2), 16));
            using (SymmetricAlgorithm sa = new DESCryptoServiceProvider())
            {
                sa.Key = Encoding.ASCII.GetBytes(key);
                sa.IV = Encoding.ASCII.GetBytes(key);

                var ms = new MemoryStream();
                using (var cs = new CryptoStream(ms, sa.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    sa.Clear();
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    result = (encoding ?? Encoding.Default).GetString(ms.ToArray());
                }
            }
            return result;
        }
        #endregion
    }
}