using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tatan.Common.Compiler;

namespace Tatan.Common.UnitTest
{
    using Common;

    [TestClass]
    public class GuidTest
    {
        [TestMethod]
        public void NewTest()
        {
            string s = Guid.New();
            Assert.IsNotNull(s);
        }

        [TestMethod]
        public void NewFormatTest()
        {
            string s = Guid.New("d");
            Assert.AreEqual(s.Contains("-"), true);
        }
    }
}
