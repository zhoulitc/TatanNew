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
    using Tatan.Common.Extension.Object;

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
            Configurations.Register<TestConfig>(_path);
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

        [XmlRoot]
        public class TestData
        {
            [XmlElement]
            public string Name { get; set; }
        }

        [TestMethod]
        public void TestLoadConfig()
        {
            Configurations.Register(Runtime.Root + "Configuration\\config.json");
            Assert.AreEqual(Configurations.Current["1"], "1");
            Assert.AreEqual(Configurations.Current["2", "22"], "22");
        }

        [TestMethod]
        public void TestGetConfig()
        {
            Configurations.Register(Runtime.Root + "Configuration\\config.xml");
            var config = Configurations.Get("config");
            Assert.AreEqual(config["1"], "1");
            Assert.AreEqual(config["2", "22"], "22");
            Assert.AreEqual(config["2", "232"], string.Empty);
            Assert.AreEqual(config["22", "232"], string.Empty);
            Assert.AreEqual(config["22"], string.Empty);
        }

        [TestMethod]
        public void TestGetNullConfig()
        {
            var config = Configurations.Get("config1");
            Assert.AreEqual(config["1"], string.Empty);
            Assert.AreEqual(config["2", "22"], string.Empty);
        }

        [TestMethod]
        public void TestGetXmlConfig()
        {
            var o = Configurations.Get<TestConfig>("test");
            Assert.AreEqual(o.Name, "wahaha");
        }

        [TestMethod]
        public void TestAppConfig()
        {
            var o = Configurations.App;
            Assert.AreEqual(o["key"], string.Empty);
            Assert.AreEqual(o["s", "1"], string.Empty);
            Assert.AreEqual(o["s"], "1");
        }

        [TestMethod]
        public void TestConnectionConfig()
        {
            var o = Configurations.Connection;
            Assert.AreEqual(o["key", "ConnectionString"], string.Empty);
            Assert.AreEqual(o["LocalSqlServer", "ProviderName"], "System.Data.SqlClient");
            Assert.AreEqual(o["LocalSqlServer", "sdas"], string.Empty);
            Assert.AreEqual(o["LocalSqlServer"], @"data source=.\SQLEXPRESS;Integrated Security=SSPI;AttachDBFilename=|DataDirectory|aspnetdb.mdf;User Instance=true");
            Assert.AreEqual(o["LocalSsdsadsaqlServer"], string.Empty);
        }

        [TestMethod]
        public void TestXmlSerializerExtension()
        {
            Component.ComponentManager.Register(new JsonSerializerAdapter(
                JsonConvert.SerializeObject,
                JsonConvert.DeserializeObject
                ));
            var o = Configurations.Get<TestConfig>("test");
            var s = o.ToJsonString();
            Assert.AreEqual(s, "{\"Name\":\"wahaha\"}");

            object oo = null;
            s = oo.ToXmlString();
            Assert.AreEqual(s, string.Empty);
        }

        [TestMethod]
        public void TestXmlSerializer()
        {
            Component.ComponentManager.Register(new XmlSerializerAdapter());

            var s = new TestData { Name = "wahaha" }.ToXmlString();
            Assert.AreEqual(s.Length>0, true);

            s = new TestConfig2 { Name = "wahaha" }.ToXmlString();
            Assert.AreEqual(s.Length > 0, true);

            var o = Configurations.Get<TestConfig>("test");
            s = o.ToXmlString();
            Assert.AreEqual(s, "<?xml version=\"1.0\"?>\r\n<TestConfig xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Name>wahaha</Name>\r\n</TestConfig>");


            object oo = null;
            s = oo.ToXmlString();
            Assert.AreEqual(s, string.Empty);
        }
    }
}
