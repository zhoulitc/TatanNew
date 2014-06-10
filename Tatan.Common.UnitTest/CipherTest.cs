using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tatan.Common.UnitTest
{
    using Tatan.Common.Cryptography;

    [TestClass]
    public class CipherTest
    {
        [TestMethod]
        public void TestDESEncrypt()
        {
            var ic = CipherFactory.GetCipher("des");
            var s1 = ic.Encrypt("wahahasdsalkjflsakflkjfsaaslfkjsal");
            Assert.AreEqual(s1, "99F8135513C9A21D423D6B68C909B8ECD04E82BC5A2D51F4FD7B3A93B9057D1479317001DDAA7D9E");
            s1 = ic.Encrypt(null);
            Assert.AreEqual(s1, string.Empty);
        }


        [TestMethod]
        public void TestDESDecrypt()
        {
            var ic = CipherFactory.GetCipher("des");
            var s1 = ic.Decrypt("99F8135513C9A21D423D6B68C909B8ECD04E82BC5A2D51F4FD7B3A93B9057D1479317001DDAA7D9E");
            Assert.AreEqual(s1, "wahahasdsalkjflsakflkjfsaaslfkjsal");
            s1 = ic.Decrypt(null);
            Assert.AreEqual(s1, string.Empty);
        }

        [TestMethod]
        public void TestMD5Encrypt()
        {
            var ic = CipherFactory.GetCipher("md5");
            var s1 = ic.Encrypt("wahahasdsalkjflsakflkjfsaaslfkjsal");
            Assert.AreEqual(s1, "c86fdd4105b631118cfa7e4a06c21");
            s1 = ic.Encrypt("wahahasdsalkjflsakflkjfsaaslfkjsal", null, Encoding.Default);
            Assert.AreEqual(s1, "c86fdd4105b631118cfa7e4a06c21");
            s1 = ic.Encrypt(null, null, Encoding.Default);
            Assert.AreEqual(s1, string.Empty);
            s1 = ic.Encrypt(null);
            Assert.AreEqual(s1, string.Empty);
        }

        [TestMethod]
        public void TestMD5Decrypt()
        {
            var ic = CipherFactory.GetCipher("md5");
            try
            {
                var s1 = ic.Decrypt("c86fdd4105b631118cfa7e4a06c21");
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "此操作没有实现。");
            }

            try
            {
                var s1 = ic.Decrypt("c86fdd4105b631118cfa7e4a06c21", "");
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "此操作没有实现。");
            }
        }

        [TestMethod]
        public void TestSHA1Encrypt()
        {
            var ic = CipherFactory.GetCipher("sha1");
            var s1 = ic.Encrypt("wahahasdsalkjflsakflkjfsaaslfkjsal");
            Assert.AreEqual(s1, "dd44bfed6459fd4e712ce8bba6b7a62d5198d");
            s1 = ic.Encrypt(null);
            Assert.AreEqual(s1, string.Empty);
        }

        [TestMethod]
        public void TestSHA1Decrypt()
        {
            var ic = CipherFactory.GetCipher("sha1");
            try
            {
                var s1 = ic.Decrypt("c86fdd4105b631118cfa7e4a06c21");
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "此操作没有实现。");
            }

            try
            {
                var s1 = ic.Decrypt("c86fdd4105b631118cfa7e4a06c21", "");
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "此操作没有实现。");
            }
        }

        [TestMethod]
        public void TestAesEncrypt()
        {
            var ic = CipherFactory.GetCipher("aes");
            var s1 = ic.Encrypt("wahahasdsalkjflsakflkjfsaaslfkjsal");
            Assert.AreEqual(s1, "A7FFA2A5E4B8669D530072F95593C4115DD2C27B92DF8EFD93A432DAE4632A4446B0A8018347B639780FAC440C9C4B66");
            s1 = ic.Encrypt(null);
            Assert.AreEqual(s1, string.Empty);
        }

        [TestMethod]
        public void TestAesDecrypt()
        {
            var ic = CipherFactory.GetCipher("aes");
            var s1 = ic.Decrypt("A7FFA2A5E4B8669D530072F95593C4115DD2C27B92DF8EFD93A432DAE4632A4446B0A8018347B639780FAC440C9C4B66");
            Assert.AreEqual(s1, "wahahasdsalkjflsakflkjfsaaslfkjsal");
            s1 = ic.Decrypt(null);
            Assert.AreEqual(s1, string.Empty);
        }

        [TestMethod]
        public void TestBase64Encrypt()
        {
            var ic = CipherFactory.GetCipher("base64");
            var s1 = ic.Encrypt("wahahasdsalkjflsakflkjfsaaslfkjsal");
            Assert.AreEqual(s1, "d2FoYWhhc2RzYWxramZsc2FrZmxramZzYWFzbGZranNhbFoxbDJ0M2M0");
            s1 = ic.Encrypt(null);
            Assert.AreEqual(s1, string.Empty);
        }

        [TestMethod]
        public void TestBase64Decrypt()
        {
            var ic = CipherFactory.GetCipher("base64");
            var s1 = ic.Decrypt("d2FoYWhhc2RzYWxramZsc2FrZmxramZzYWFzbGZranNhbFoxbDJ0M2M0");
            Assert.AreEqual(s1, "wahahasdsalkjflsakflkjfsaaslfkjsal");
            s1 = ic.Decrypt(null);
            Assert.AreEqual(s1, string.Empty);
        }

        [TestMethod]
        public void TestNullEncrypt()
        {
            var ic = CipherFactory.GetCipher(null);
            var s1 = ic.Encrypt("c86fdd4105b631118cfa7e4a06c21");
            Assert.AreEqual(s1, "c86fdd4105b631118cfa7e4a06c21");
            s1 = ic.Encrypt("c86fdd4105b631118cfa7e4a06c21", "");
            Assert.AreEqual(s1, "c86fdd4105b631118cfa7e4a06c21");
            ic = CipherFactory.GetCipher("sdsa");
            s1 = ic.Decrypt("c86fdd4105b631118cfa7e4a06c21");
            Assert.AreEqual(s1, "c86fdd4105b631118cfa7e4a06c21");
            s1 = ic.Decrypt("c86fdd4105b631118cfa7e4a06c21", "");
            Assert.AreEqual(s1, "c86fdd4105b631118cfa7e4a06c21");
        }
    }
}
