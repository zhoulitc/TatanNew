namespace Tatan.Common.Cryptography
{
    /// <summary>
    /// 密码加解密接口
    /// </summary>
    internal interface ICipher
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="expressly">明文</param>
        /// <param name="key">密钥</param>
        /// <exception cref="System.ArgumentNullException">当expressly为null时</exception>
        /// <exception cref="System.Exception">其他异常发生时</exception>
        /// <returns>密文</returns>
        string Encrypt(string expressly, string key = null);

        /// <summary>
        /// 解密，非对称则直接返回密文
        /// </summary>
        /// <param name="ciphertext">密文</param>
        /// <param name="key">密钥</param>
        /// <exception cref="System.ArgumentNullException">当ciphertext为null时</exception>
        /// <exception cref="System.Exception">其他异常发生时</exception>
        /// <returns>明文</returns>
        string Decrypt(string ciphertext, string key = null);
    }
}