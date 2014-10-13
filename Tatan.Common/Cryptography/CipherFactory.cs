namespace Tatan.Common.Cryptography
{
    using Internal;

    /// <summary>
    /// 密码学工厂
    /// </summary>
    internal static class CipherFactory
    {
        /// <summary>
        /// 获取加密解密算法类
        /// </summary>
        /// <param name="type">算法名称</param>
        /// <returns></returns>
        public static ICipher GetCipher(string type)
        {
            ICipher cipher;
            if (string.IsNullOrEmpty(type))
            {
                cipher = NullCipher.Instance;
            }
            else
            {
                switch (type.ToLower())
                {
                    case "md5":
                        cipher = Md5Cipher.Instance;
                        break;
                    case "sha1":
                        cipher = Sha1Cipher.Instance;
                        break;
                    case "des":
                        cipher = DesCipher.Instance;
                        break;
                    case "aes":
                        cipher = AesCipher.Instance;
                        break;
                    case "base64":
                        cipher = Base64Cipher.Instance;
                        break;
                    default:
                        cipher = NullCipher.Instance;
                        break;
                }
            }
            return cipher;
        }
    }
}