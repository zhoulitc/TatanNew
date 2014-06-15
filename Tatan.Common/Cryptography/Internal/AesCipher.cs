namespace Tatan.Common.Cryptography.Internal
{
    using System.Security.Cryptography;

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
        protected override string GetKey(string key)
        {
            if (key.Length < 32)
            {
                key += _aseKey.Substring(key.Length);
            }
            else if (key.Length > 32)
            {
                key = key.Substring(0, 32);
            }
            return key;
        }

        protected override string GetIv(string key)
        {
            return GetKey(key).Substring(16);
        }

        protected override SymmetricAlgorithm CreateSymmetricCipher()
        {
            return new AesCryptoServiceProvider();
        }
        #endregion
    }
}