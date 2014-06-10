namespace Tatan.Common.Cryptography.Internal
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    internal sealed class AesCipher : SymmetricCipher
    {
        #region 单例
        private static readonly AesCipher _instance = new AesCipher();
        private AesCipher()
        {
        }
        public static AesCipher Instance
        {
            get
            {
                return _instance;
            }
        }

        private const string _aseKey = "what are you talking ? nothing .";

        #endregion

        #region ICipher
        public override string Encrypt(string expressly, string key, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(expressly))
                return string.Empty;
            byte[] bytes;
            key = string.IsNullOrEmpty(key) ? Key : key;
            if (key.Length < 32)
            {
                key += _aseKey.Substring(key.Length);
            }
            else if (key.Length > 32)
            {
                key = key.Substring(0, 32);
            }
            var data = (encoding ?? Encoding.Default).GetBytes(expressly);
            using (SymmetricAlgorithm sa = new AesCryptoServiceProvider())
            {
                sa.Key = Encoding.ASCII.GetBytes(key);
                sa.IV = Encoding.ASCII.GetBytes(key.Substring(16));

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
            key = string.IsNullOrEmpty(key) ? Key : key;
            if (key.Length < 32)
            {
                key += _aseKey.Substring(key.Length);
            }
            else if (key.Length > 32)
            {
                key = key.Substring(0, 32);
            }
            var len = ciphertext.Length / 2;
            var data = new byte[len];
            for (var i = 0; i < len; i++)
                data[i] = Convert.ToByte(Convert.ToInt32(ciphertext.Substring(i * 2, 2), 16));
            using (SymmetricAlgorithm sa = new AesCryptoServiceProvider())
            {
                sa.Key = Encoding.ASCII.GetBytes(key);
                sa.IV = Encoding.ASCII.GetBytes(key.Substring(16));

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