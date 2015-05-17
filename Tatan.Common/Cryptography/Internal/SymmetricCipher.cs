namespace Tatan.Common.Cryptography.Internal
{
    using System;
    using System.IO;
    using System.Text;
    using System.Security.Cryptography;

    /// <summary>
    /// 对称密码抽象类
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    internal abstract class SymmetricCipher : ICipher
    {
        protected const string DefaultKey = "Z1l2t3c4";
        protected static readonly Encoding Encoding = Encoding.UTF8;

        protected virtual string GetKey(string key)
        {
            return key;
        }

        protected virtual string GetIv(string key)
        {
            return key;
        }

        protected virtual SymmetricAlgorithm CreateSymmetricCipher()
        {
            return null;
        }

        public virtual string Encrypt(string expressly, string key = null)
        {
            if (string.IsNullOrEmpty(expressly))
                return string.Empty;
            byte[] bytes;
            key = string.IsNullOrEmpty(key) ? DefaultKey : key;
            var data = Encoding.GetBytes(expressly);
            using (var sa = CreateSymmetricCipher())
            {
                sa.Key = Encoding.GetBytes(GetKey(key));
                sa.IV = Encoding.GetBytes(GetIv(key));

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

        public virtual string Decrypt(string ciphertext, string key = null)
        {
            var result = string.Empty;
            if (string.IsNullOrEmpty(ciphertext))
                return result;
            key = string.IsNullOrEmpty(key) ? DefaultKey : key;
            var len = ciphertext.Length/2;
            var data = new byte[len];
            for (var i = 0; i < len; i++)
                data[i] = Convert.ToByte(Convert.ToInt32(ciphertext.Substring(i*2, 2), 16));
            using (var sa = CreateSymmetricCipher())
            {
                sa.Key = Encoding.GetBytes(GetKey(key));
                sa.IV = Encoding.GetBytes(GetIv(key));

                var ms = new MemoryStream();
                using (var cs = new CryptoStream(ms, sa.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    sa.Clear();
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    result = Encoding.GetString(ms.ToArray());
                }
            }
            return result;
        }
    }
}