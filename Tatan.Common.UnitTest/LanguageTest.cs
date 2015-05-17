using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tatan.Common.Extension.String.IO;
using Tatan.Common.I18n;
using Tatan.Common.IO;

namespace Tatan.Common.UnitTest
{
    using Common;
    using SystemFile = System.IO.File;

    [TestClass]
    public class LanguageTest
    {
        [TestInitialize]
        public void Init()
        {
            _pathC.CreateFile();
            _pathC.AppendText(w =>
                w.WriteLine("<?xml version='1.0' encoding='utf-8'?><China><Name>名称</Name></China>"));
            _pathE.CreateFile();
            _pathE.AppendText(w =>
                w.WriteLine("<?xml version='1.0' encoding='utf-8'?><English><Name>Name</Name></English>"));
        }

        [TestCleanup]
        public void Cleanup()
        {
            SystemFile.Delete(_pathC);
            SystemFile.Delete(_pathE);
        }

        static readonly string _pathC = Runtime.Root + @"zh-cn.xml";
        static readonly string _pathE = Runtime.Root + @"en-us.xml";

        [TestMethod]
        public void TestCreate()
        {
            Languages l = new Languages(Runtime.Root);
            l = new Languages(Runtime.Root.Substring(0, Runtime.Root.Length - 1));
            try
            {
                l = new Languages(null);
            }
            catch (System.Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("参数名"));
            }
            Assert.IsTrue(l.Reload("en-us"));
            Assert.IsTrue(l.Reload(null));
        }

        [TestMethod]
        public void TestGetText()
        {
            Languages l = new Languages(Runtime.Root);
            Assert.AreEqual(l.GetText("Name"), "名称");
            Assert.AreEqual(l.GetText("Name", "en-us"), "Name");
            Assert.AreEqual(l.GetText("Name", "en-us1"), "ANALYSING XML DOCUMENT ERROR.");
            Assert.AreEqual(l.GetText(null), "NOT FOUND THE TEXT.");
            Assert.AreEqual(l.GetText("Name1"), "NOT FOUND THE TEXT.");
        }
    }
}
