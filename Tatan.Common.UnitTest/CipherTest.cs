using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tatan.Common.Extension.String.Codec;

namespace Tatan.Common.UnitTest
{
    using Tatan.Common.Cryptography;

    [TestClass]
    public class CipherTest
    {
        [TestMethod]
        public void TestDESEncrypt()
        {
            var s1 = "wahahasdsalkjflsakflkjfsaaslfkjsal".AsEncode("des");
            Assert.AreEqual(s1, "99F8135513C9A21D423D6B68C909B8ECD04E82BC5A2D51F4FD7B3A93B9057D1479317001DDAA7D9E");
            string s2 = null;
            s1 = s2.AsEncode("des");
            Assert.AreEqual(s1, string.Empty);
        }


        [TestMethod]
        public void TestDESDecrypt()
        {
            var s1 = "99F8135513C9A21D423D6B68C909B8ECD04E82BC5A2D51F4FD7B3A93B9057D1479317001DDAA7D9E".AsDecode("des");
            Assert.AreEqual(s1, "wahahasdsalkjflsakflkjfsaaslfkjsal");
            string s2 = null;
            s1 = s2.AsDecode("des");
            Assert.AreEqual(s1, string.Empty);
        }

        [TestMethod]
        public void TestMD5Encrypt()
        {
            var s1 = "wahahasdsalkjflsakflkjfsaaslfkjsal".AsEncode("md5");
            Assert.AreEqual(s1, "c86fdd4105b631118cfa7e4a06c21");
            s1 = "wahahasdsalkjflsakflkjfsaaslfkjsal".AsEncode("md5");
            Assert.AreEqual(s1, "c86fdd4105b631118cfa7e4a06c21");
            string s2 = null;
            s1 = s2.AsEncode("md5", null);
            Assert.AreEqual(s1, string.Empty);
            s1 = s2.AsEncode("md5");
            Assert.AreEqual(s1, string.Empty);
        }

        [TestMethod]
        public void TestMD5Decrypt()
        {
            Assert.AreEqual("c86fdd4105b631118cfa7e4a06c21".AsDecode("md5"), "c86fdd4105b631118cfa7e4a06c21");
            Assert.AreEqual("c86fdd4105b631118cfa7e4a06c21".AsDecode("md5", ""), "c86fdd4105b631118cfa7e4a06c21");
        }

        [TestMethod]
        public void TestSHA1Encrypt()
        {
            var s1 = "wahahasdsalkjflsakflkjfsaaslfkjsal".AsEncode("sha1");
            Assert.AreEqual(s1, "dd44bfed6459fd4e712ce8bba6b7a62d5198d");
            string s2 = null;
            s1 = s2.AsEncode("sha1");
            Assert.AreEqual(s1, string.Empty);
        }

        [TestMethod]
        public void TestSHA1Decrypt()
        {
            Assert.AreEqual("c86fdd4105b631118cfa7e4a06c21".AsDecode("sha1"), "c86fdd4105b631118cfa7e4a06c21");
            Assert.AreEqual("c86fdd4105b631118cfa7e4a06c21".AsDecode("sha1", ""), "c86fdd4105b631118cfa7e4a06c21");
        }

        [TestMethod]
        public void TestAesEncrypt()
        {
            var s1 = "wahahasdsalkjflsakflkjfsaaslfkjsal".AsEncode("aes");
            Assert.AreEqual(s1, "A7FFA2A5E4B8669D530072F95593C4115DD2C27B92DF8EFD93A432DAE4632A4446B0A8018347B639780FAC440C9C4B66");
            string s2 = null;
            s1 = s2.AsEncode("aes");
            Assert.AreEqual(s1, string.Empty);
        }

        [TestMethod]
        public void TestAesDecrypt()
        {
            var s1 = "A7FFA2A5E4B8669D530072F95593C4115DD2C27B92DF8EFD93A432DAE4632A4446B0A8018347B639780FAC440C9C4B66".AsDecode("aes");
            Assert.AreEqual(s1, "wahahasdsalkjflsakflkjfsaaslfkjsal");
            string s2 = null;
            s1 = s2.AsDecode("aes");
            Assert.AreEqual(s1, string.Empty);
        }

        [TestMethod]
        public void TestBase64Encrypt()
        {
            var s1 = ("wahahasdsalkjflsakflkjfsaaslfkjsal").AsEncode("base64");
            Assert.AreEqual(s1, "d2FoYWhhc2RzYWxramZsc2FrZmxramZzYWFzbGZranNhbFoxbDJ0M2M0");
            string s2 = null;
            s1 = s2.AsEncode("base64");
            Assert.AreEqual(s1, string.Empty);
        }

        [TestMethod]
        public void TestBase64Decrypt()
        {
            var s1 = ("d2FoYWhhc2RzYWxramZsc2FrZmxramZzYWFzbGZranNhbFoxbDJ0M2M0").AsDecode("base64");
            Assert.AreEqual(s1, "wahahasdsalkjflsakflkjfsaaslfkjsal");
            string s2 = null;
            s1 = s2.AsDecode("base64");
            Assert.AreEqual(s1, string.Empty);
        }

        [TestMethod]
        public void TestNullEncrypt()
        {
            var s1 = ("c86fdd4105b631118cfa7e4a06c21").AsEncode();
            Assert.AreEqual(s1, "c86fdd4105b631118cfa7e4a06c21");
            s1 = ("c86fdd4105b631118cfa7e4a06c21").AsDecode(null, "");
            Assert.AreEqual(s1, "c86fdd4105b631118cfa7e4a06c21");
            s1 = ("c86fdd4105b631118cfa7e4a06c21").AsEncode("sdsa");
            Assert.AreEqual(s1, "c86fdd4105b631118cfa7e4a06c21");
            s1 = "c86fdd4105b631118cfa7e4a06c21".AsDecode(null, "");
            Assert.AreEqual(s1, "c86fdd4105b631118cfa7e4a06c21");
        }
    }
}
