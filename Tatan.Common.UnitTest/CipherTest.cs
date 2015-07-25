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
            var s1 = "wahahasdsalkjflsakflkjfsaaslfkjsal".AsEncode(Coding.Des);
            Assert.AreEqual(s1, "99F8135513C9A21D423D6B68C909B8ECD04E82BC5A2D51F4FD7B3A93B9057D1479317001DDAA7D9E");
            string s2 = null;
            s1 = s2.AsEncode(Coding.Des);
            Assert.AreEqual(s1, string.Empty);
        }


        [TestMethod]
        public void TestDESDecrypt()
        {
            var s1 = "99F8135513C9A21D423D6B68C909B8ECD04E82BC5A2D51F4FD7B3A93B9057D1479317001DDAA7D9E".AsDecode(Coding.Des);
            Assert.AreEqual(s1, "wahahasdsalkjflsakflkjfsaaslfkjsal");
            string s2 = null;
            s1 = s2.AsDecode(Coding.Des);
            Assert.AreEqual(s1, string.Empty);
        }

        [TestMethod]
        public void TestMD5Encrypt()
        {
            var s1 = "wahahasdsalkjflsakflkjfsaaslfkjsal".AsEncode(Coding.Md5);
            Assert.AreEqual(s1, "c86fdd4105b631118cfa7e4a06c21");
            s1 = "wahahasdsalkjflsakflkjfsaaslfkjsal".AsEncode(Coding.Md5);
            Assert.AreEqual(s1, "c86fdd4105b631118cfa7e4a06c21");
            string s2 = null;
            s1 = s2.AsEncode(Coding.Md5, null);
            Assert.AreEqual(s1, string.Empty);
            s1 = s2.AsEncode(Coding.Md5);
            Assert.AreEqual(s1, string.Empty);
        }

        [TestMethod]
        public void TestMD5Decrypt()
        {
            Assert.AreEqual("c86fdd4105b631118cfa7e4a06c21".AsDecode(Coding.Md5), "c86fdd4105b631118cfa7e4a06c21");
            Assert.AreEqual("c86fdd4105b631118cfa7e4a06c21".AsDecode(Coding.Md5, ""), "c86fdd4105b631118cfa7e4a06c21");
        }

        [TestMethod]
        public void TestSHA1Encrypt()
        {
            var s1 = "wahahasdsalkjflsakflkjfsaaslfkjsal".AsEncode(Coding.Sha1);
            Assert.AreEqual(s1, "dd44bfed6459fd4e712ce8bba6b7a62d5198d");
            string s2 = null;
            s1 = s2.AsEncode(Coding.Sha1);
            Assert.AreEqual(s1, string.Empty);
        }

        [TestMethod]
        public void TestSHA1Decrypt()
        {
            Assert.AreEqual("c86fdd4105b631118cfa7e4a06c21".AsDecode(Coding.Sha1), "c86fdd4105b631118cfa7e4a06c21");
            Assert.AreEqual("c86fdd4105b631118cfa7e4a06c21".AsDecode(Coding.Sha1, ""), "c86fdd4105b631118cfa7e4a06c21");
        }

        [TestMethod]
        public void TestAesEncrypt()
        {
            var s1 = "wahahasdsalkjflsakflkjfsaaslfkjsal".AsEncode(Coding.Aes);
            Assert.AreEqual(s1, "A7FFA2A5E4B8669D530072F95593C4115DD2C27B92DF8EFD93A432DAE4632A4446B0A8018347B639780FAC440C9C4B66");
            string s2 = null;
            s1 = s2.AsEncode(Coding.Aes);
            Assert.AreEqual(s1, string.Empty);
        }

        [TestMethod]
        public void TestAesDecrypt()
        {
            var s1 = "A7FFA2A5E4B8669D530072F95593C4115DD2C27B92DF8EFD93A432DAE4632A4446B0A8018347B639780FAC440C9C4B66".AsDecode(Coding.Aes);
            Assert.AreEqual(s1, "wahahasdsalkjflsakflkjfsaaslfkjsal");
            string s2 = null;
            s1 = s2.AsDecode(Coding.Aes);
            Assert.AreEqual(s1, string.Empty);
        }

        [TestMethod]
        public void TestBase64Encrypt()
        {
            var s1 = ("wahahasdsalkjflsakflkjfsaaslfkjsal").AsEncode(Coding.Base64);
            Assert.AreEqual(s1, "d2FoYWhhc2RzYWxramZsc2FrZmxramZzYWFzbGZranNhbFoxbDJ0M2M0");
            string s2 = null;
            s1 = s2.AsEncode(Coding.Base64);
            Assert.AreEqual(s1, string.Empty);
        }

        [TestMethod]
        public void TestBase64Decrypt()
        {
            var s1 = ("d2FoYWhhc2RzYWxramZsc2FrZmxramZzYWFzbGZranNhbFoxbDJ0M2M0").AsDecode(Coding.Base64);
            Assert.AreEqual(s1, "wahahasdsalkjflsakflkjfsaaslfkjsal");
            string s2 = null;
            s1 = s2.AsDecode(Coding.Base64);
            Assert.AreEqual(s1, string.Empty);
        }

        [TestMethod]
        public void TestHtmlCodec()
        {
            var s1 = ("<wahaha>").AsEncode(Coding.Html);
            Assert.AreEqual(s1, "&lt;wahaha&gt;");
            string s2 = "&lt;wahaha&gt;";
            s1 = s2.AsDecode(Coding.Html);
            Assert.AreEqual(s1, "<wahaha>");
            var s3 = ("sdsadsa").AsEncode(Coding.Html);
            Assert.AreEqual(s3, "sdsadsa");
            s3 = ("sdsadsa").AsDecode(Coding.Html);
            Assert.AreEqual(s3, "sdsadsa");
            var s4 = string.Empty;
            s3 = s4.AsDecode(Coding.Html);
            Assert.AreEqual(s3, string.Empty);
            s3 = s4.AsEncode(Coding.Html);
            Assert.AreEqual(s3, string.Empty);
            s4 = "&lt;wahaha&gt;";
            s3 = s4.AsDecode(Coding.None);
            Assert.AreEqual(s3, "&lt;wahaha&gt;");
            s4 = "<wahaha>";
            s3 = s4.AsDecode(Coding.None);
            Assert.AreEqual(s3, "<wahaha>");
            s4 = "<wahaha>";
            s3 = s4.AsDecode();
            Assert.AreEqual(s3, "<wahaha>");
            s4 = "&lt;wahaha&gt;";
            s3 = s4.AsDecode();
            Assert.AreEqual(s3, "&lt;wahaha&gt;");
        }

        [TestMethod]
        public void TestUrlCodec()
        {
            var s1 = ("我擦").AsEncode(Coding.Url);
            Assert.AreEqual(s1, "%e6%88%91%e6%93%a6");
            string s2 = "%e6%88%91%e6%93%a6";
            s1 = s2.AsDecode(Coding.Url);
            Assert.AreEqual(s1, "我擦");
            var s3 = ("sdsadsa").AsEncode(Coding.Url);
            Assert.AreEqual(s3, "sdsadsa");
            s3 = ("sdsadsa").AsDecode(Coding.Url);
            Assert.AreEqual(s3, "sdsadsa");
            var s4 = string.Empty;
            s3 = s4.AsDecode(Coding.Url);
            Assert.AreEqual(s3, string.Empty);
            s3 = s4.AsEncode(Coding.Url);
            Assert.AreEqual(s3, string.Empty);
            s4 = "%e6%88%91%e6%93%a6";
            s3 = s4.AsDecode(Coding.None);
            Assert.AreEqual(s3, "%e6%88%91%e6%93%a6");
            s4 = "我擦";
            s3 = s4.AsDecode(Coding.None);
            Assert.AreEqual(s3, "我擦");
            s4 = "我擦";
            s3 = s4.AsDecode();
            Assert.AreEqual(s3, "我擦");
            s4 = "%e6%88%91%e6%93%a6";
            s3 = s4.AsDecode();
            Assert.AreEqual(s3, "%e6%88%91%e6%93%a6");
            s4 = "%e6%88%91%e6%93%a6";
            s3 = s4.AsDecode(Coding.Url, "utf-16");
            Assert.AreEqual(s3, "裦ꚓ");
        }

        [TestMethod]
        public void TestNullEncrypt()
        {
            var s1 = ("c86fdd4105b631118cfa7e4a06c21").AsEncode();
            Assert.AreEqual(s1, "c86fdd4105b631118cfa7e4a06c21");
            s1 = ("c86fdd4105b631118cfa7e4a06c21").AsDecode(Coding.None, "");
            Assert.AreEqual(s1, "c86fdd4105b631118cfa7e4a06c21");
            s1 = ("c86fdd4105b631118cfa7e4a06c21").AsEncode(Coding.None);
            Assert.AreEqual(s1, "c86fdd4105b631118cfa7e4a06c21");
            s1 = "c86fdd4105b631118cfa7e4a06c21".AsDecode(Coding.None, "");
            Assert.AreEqual(s1, "c86fdd4105b631118cfa7e4a06c21");
        }
    }
}
