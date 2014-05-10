using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tatan.Common.UnitTest
{
    using Tatan.Common.Cryptography;

    [TestClass]
    public class CipherTest
    {
        [TestMethod]
        public void DESEncrypt()
        {
            var ic = CipherFactory.GetCipher("des");
            var s1 = ic.Encrypt("wahahasdsalkjflsakflkjfsaaslfkjsal");
            Assert.AreEqual(s1, "99F8135513C9A21D423D6B68C909B8ECD04E82BC5A2D51F4FD7B3A93B9057D1479317001DDAA7D9E");
        }

        [TestMethod]
        public void DESEncryptNull()
        {
            var ic = CipherFactory.GetCipher("des");
            var s1 = ic.Encrypt(null);
            Assert.AreEqual(s1, string.Empty);
        }

        [TestMethod]
        public void DESDecrypt()
        {
            var ic = CipherFactory.GetCipher("des");
            var s1 = ic.Decrypt("99F8135513C9A21D423D6B68C909B8ECD04E82BC5A2D51F4FD7B3A93B9057D1479317001DDAA7D9E");
            Assert.AreEqual(s1, "wahahasdsalkjflsakflkjfsaaslfkjsal");
        }

        [TestMethod]
        public void DESDecryptNull()
        {
            var ic = CipherFactory.GetCipher("des");
            var s1 = ic.Decrypt(null);
            Assert.AreEqual(s1, string.Empty);
        }

        [TestMethod]
        public void MD5Encrypt()
        {
            var ic = CipherFactory.GetCipher("md5");
            var s1 = ic.Encrypt("wahahasdsalkjflsakflkjfsaaslfkjsal");
            Assert.AreEqual(s1, "c86fdd4105b631118cfa7e4a06c21");
        }

        [TestMethod]
        public void MD5EncryptNull()
        {
            var ic = CipherFactory.GetCipher("md5");
            var s1 = ic.Encrypt(null);
            Assert.AreEqual(s1, string.Empty);
        }

        [TestMethod]
        public void MD5Decrypt()
        {
            var ic = CipherFactory.GetCipher("md5");
            try
            {
                var s1 = ic.Decrypt("c86fdd4105b631118cfa7e4a06c21");
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "非对称加密不支持解密方法。");
            }
        }

        [TestMethod]
        public void SHA1Encrypt()
        {
            var ic = CipherFactory.GetCipher("sha1");
            var s1 = ic.Encrypt("wahahasdsalkjflsakflkjfsaaslfkjsal");
            Assert.AreEqual(s1, "dd44bfed6459fd4e712ce8bba6b7a62d5198d");
        }

        [TestMethod]
        public void SHA1EncryptNull()
        {
            var ic = CipherFactory.GetCipher("sha1");
            var s1 = ic.Encrypt(null);
            Assert.AreEqual(s1, string.Empty);
        }

        [TestMethod]
        public void SHA1Decrypt()
        {
            var ic = CipherFactory.GetCipher("sha1");
            try
            {
                var s1 = ic.Decrypt("c86fdd4105b631118cfa7e4a06c21");
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "非对称加密不支持解密方法。");
            }
        }

        [TestMethod]
        public void NullEncrypt()
        {
            var ic = CipherFactory.GetCipher(null);
            var s1 = ic.Encrypt("c86fdd4105b631118cfa7e4a06c21");
            Assert.AreEqual(s1, "c86fdd4105b631118cfa7e4a06c21");
        }

        [TestMethod]
        public void NullDecrypt()
        {
            var ic = CipherFactory.GetCipher("sdsadsa");
            var s1 = ic.Decrypt("c86fdd4105b631118cfa7e4a06c21");
            Assert.AreEqual(s1, "c86fdd4105b631118cfa7e4a06c21");
        }
    }
}
