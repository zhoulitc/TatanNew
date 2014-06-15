namespace Tatan.Common.Cryptography.Internal
{
    using System.Security.Cryptography;

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
        protected override HashAlgorithm CreateAsymmetricCipher()
        {
            return new SHA1CryptoServiceProvider();
        }
        #endregion
    }
}