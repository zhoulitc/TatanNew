namespace Tatan.Common.Cryptography.Internal
{
    using System.Security.Cryptography;

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
        protected override HashAlgorithm CreateAsymmetricCipher()
        {
            return new MD5CryptoServiceProvider();
        }
        #endregion
    }
}