using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tatan.Common.UnitTest
{
    using Common;

    [TestClass]
    public class DateTest
    {
        [TestMethod]
        public void DateNowTest()
        {
            Assert.AreEqual(Date.Now(), DateTime.Now.ToString("yyyyMMddhhmmss"));
        }

        [TestMethod]
        public void DateToStringTest()
        {
            Assert.AreEqual(Date.ToString(DateTime.Now), DateTime.Now.ToString("yyyyMMddhhmmss"));
        }

        [TestMethod]
        public void DateParseTest1()
        {
            var dt = Date.Parse("20140301113030");
            Assert.AreEqual(dt.Value, new DateTime(2014, 3, 1, 11, 30, 30));
        }

        [TestMethod]
        public void DateParseTest2()
        {
            var dt = Date.Parse("2014-03-01 11:30:30");
            Assert.IsNull(dt);
        }
    }
}
