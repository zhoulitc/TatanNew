namespace Tatan.Common.UnitTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using IO;
    using Configuration;
    using System.Xml.Serialization;
    using SystemFile = System.IO.File;

    [TestClass]
    public class ConfigFactoryTest
    {
        [TestInitialize]
        public void Init()
        {
            File.CreateText(_path, w =>
                w.WriteLine("<?xml version='1.0' encoding='utf-8'?><TestConfig><Name>wahaha</Name></TestConfig>"));
        }

        [TestCleanup]
        public void Cleanup()
        {
            SystemFile.Delete(_path);
        }

        static readonly string _path = Runtime.Root + "test.xml";

        [XmlRoot]
        public class TestConfig
        {
            [XmlElement]
            public string Name { get; set; }
        }

        public class TestConfig2
        {
            public string Name { get; set; }
        }

        [TestMethod]
        public void TestGetXmlConfig()
        {
            var o = ConfigFactory.GetXmlConfig<TestConfig>("test");
            Assert.AreEqual(o.Name, "wahaha");


            try
            {
                var o2 = ConfigFactory.GetXmlConfig<TestConfig2>("test");
            }
            catch
            {
                Assert.IsTrue(true);
            }
        }
    }
}
