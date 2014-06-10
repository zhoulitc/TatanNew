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
        protected override SymmetricAlgorithm CreateSymmetricCipher()
        {
            return new DESCryptoServiceProvider();
        }
        #endregion
    }
}