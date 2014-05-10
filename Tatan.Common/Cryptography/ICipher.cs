namespace Tatan.Common.Cryptography
{
    using System.Text;

    /// <summary>
    /// 密码加解密接口
    /// </summary>
    public interface ICipher
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="expressly">明文</param>
        /// <param name="encoding">字符编码</param>
        /// <exception cref="System.ArgumentNullException">当expressly为null时</exception>
        /// <exception cref="System.Text.EncoderFallbackException">字符编码出错，发生回退时</exception>
        /// <exception cref="System.Exception">其他异常发生时</exception>
        /// <returns>密文</returns>
        string Encrypt(string expressly, Encoding encoding = null);

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="expressly">明文</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">字符编码</param>
        /// <exception cref="System.ArgumentNullException">当expressly为null时</exception>
        /// <exception cref="System.Text.EncoderFallbackException">字符编码出错，发生回退时</exception>
        /// <exception cref="System.Exception">其他异常发生时</exception>
        /// <returns>密文</returns>
        string Encrypt(string expressly, string key, Encoding encoding = null);

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="ciphertext">密文</param>
        /// <param name="encoding">字符编码</param>
        /// <exception cref="System.ArgumentNullException">当ciphertext为null时</exception>
        /// <exception cref="System.Text.EncoderFallbackException">字符编码出错，发生回退时</exception>
        /// <exception cref="System.Exception">其他异常发生时</exception>
        /// <returns>明文</returns>
        string Decrypt(string ciphertext, Encoding encoding = null);

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="ciphertext">密文</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">字符编码</param>
        /// <exception cref="System.ArgumentNullException">当ciphertext为null时</exception>
        /// <exception cref="System.Text.EncoderFallbackException">字符编码出错，发生回退时</exception>
        /// <exception cref="System.Exception">其他异常发生时</exception>
        /// <returns>明文</returns>
        string Decrypt(string ciphertext, string key, Encoding encoding = null);
    }
}