using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tatan.Common.Caching;

namespace Tatan.Common.UnitTest
{
    [TestClass]
    public class CacheTest
    {
        [TestMethod]
        public void TestCustom()
        {
            Cache.Set("1", 1, (k, v) =>
            {
                Assert.AreEqual(k, "1");
                Assert.AreEqual(v, 1);
            });
            Assert.AreEqual(Cache.Get<int>("1"), 1);
            Cache.Set("2", 1, new TimeSpan(0,0,1), (k, v) =>
            {
                Assert.AreEqual(k, "2");
                Assert.AreEqual(v, 1);
            });
            Assert.IsTrue(Cache.Contains("1"));
            Cache.Remove("1");
            Cache.Clear();
            Assert.IsFalse(Cache.Contains("1"));
        }


        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void TestInvokeExtension()
        {
            var s = Cache.InvokeExtension("EffectiveBytes");
            Assert.AreEqual(s, (long)-1);
        }

        [TestMethod]
        public void TestWeb()
        {
            Cache.Set("1", 1, (k, v) =>
            {
                Assert.AreEqual(k, "1");
                Assert.AreEqual(v, 1);
            });
            Assert.AreEqual(Cache.Get<int>("1"), 1);
            Cache.Set("2", 1, new TimeSpan(0, 0, 1), (k, v) =>
            {
                Assert.AreEqual(k, "2");
                Assert.AreEqual(v, 1);
            });
            Assert.IsTrue(Cache.Contains("1"));
            Cache.Remove("1");
            Cache.Clear();
            Assert.IsFalse(Cache.Contains("1"));
        }
    }
}
