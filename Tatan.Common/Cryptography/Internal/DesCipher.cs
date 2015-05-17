namespace Tatan.Common.Cryptography.Internal
{
    using System.Security.Cryptography;

    /// <summary>
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    internal sealed class DesCipher : SymmetricCipher
    {
        #region 单例

        private static readonly DesCipher _instance = new DesCipher();

        private DesCipher() { }

        public static DesCipher Instance => _instance;

        #endregion

        #region ICipher

        protected override SymmetricAlgorithm CreateSymmetricCipher() => new DESCryptoServiceProvider();

        #endregion
    }
}