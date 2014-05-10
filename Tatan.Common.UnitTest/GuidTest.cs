using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tatan.Common.UnitTest
{
    using Newtonsoft.Json;
    using Tatan.Common;
    using Tatan.Common.Serialization;

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
