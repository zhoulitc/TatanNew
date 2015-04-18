using Newtonsoft.Json;
using Tatan.Common.Extension.String.IO;
using Tatan.Common.Serialization;

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
        private const string _xml = "<?xml version='1.0' encoding='utf-8'?><TestConfig><Name>wahaha</Name></TestConfig>";

        [TestInitialize]
        public void Init()
        {
            _path.CreateFile();
            _path.AppendText(w =>
                w.WriteLine(_xml));
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

        public class TestData
        {
            public string Name { get; set; }
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void TestGetXmlConfig()
        {
            var o = Configurations.Get<TestConfig>("test");
            Assert.AreEqual(o.Name, "wahaha");

            var o2 = Configurations.Get<TestConfig2>("test");
        }

        [TestMethod]
        public void TestAppConfig()
        {
            var o = Configurations.App;
            Assert.AreEqual(o["key"], string.Empty);
            Assert.AreEqual(o["s"], "1");
        }

        [TestMethod]
        public void TestConnectionConfig()
        {
            var o = Configurations.Connection;
            Assert.AreEqual(o["key", "ConnectionString"], string.Empty);
            Assert.AreEqual(o["LocalSqlServer", "ProviderName"], "System.Data.SqlClient");
        }

        [TestMethod]
        public void TestXmlSerializerExtension()
        {
            var xml = Serializers.CreateXmlSerializer(
                JsonConvert.SerializeObject,
                JsonConvert.DeserializeObject
                );
            var o = Configurations.Get<TestConfig>("test");
            var s = xml.Serialize(o);
            Assert.AreEqual(s, "{\"Name\":\"wahaha\"}");

            s = xml.Serialize(null);
            Assert.AreEqual(s, string.Empty);
        }

        [TestMethod]
        public void TestXmlSerializer()
        {
            var xml = Serializers.Xml;
            var s = xml.Serialize(new TestData { Name = "wahaha" });
            Assert.AreEqual(s.Length>0, true);

            s = xml.Serialize(new TestConfig2 { Name = "wahaha" });
            Assert.AreEqual(s.Length > 0, true);

            var o = Configurations.Get<TestConfig>("test");
            s = xml.Serialize(o);
            Assert.AreEqual(s, "<?xml version=\"1.0\"?>\r\n<TestConfig xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Name>wahaha</Name>\r\n</TestConfig>");

            

            s = xml.Serialize(null);
            Assert.AreEqual(s, string.Empty);
        }
    }
}
