using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tatan.Common.Caching;

namespace Tatan.Common.UnitTest
{
    using Common;

    [TestClass]
    public class CachesTest
    {
        [TestMethod]
        public void TestCustom()
        {
            Caches.CustomCache.Set("1", 1, (k, v) =>
            {
                Assert.AreEqual(k, "1");
                Assert.AreEqual(v, 1);
            });
            Assert.AreEqual(Caches.CustomCache.Get<int>("1"), 1);
            Caches.CustomCache.Set("2", 1, new TimeSpan(0,0,1), (k, v) =>
            {
                Assert.AreEqual(k, "2");
                Assert.AreEqual(v, 1);
            });
            Assert.IsTrue(Caches.CustomCache.Contains("1"));
            Caches.CustomCache.Remove("1");
            Caches.CustomCache.Clear();
            Assert.IsFalse(Caches.CustomCache.Contains("1"));
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void TestDispose()
        {
            Caches.CustomDispose();
            Caches.CustomCache.Remove("1");
        }

        [TestMethod]
        public void TestWeb()
        {
            Caches.WebCache.Set("1", 1, (k, v) =>
            {
                Assert.AreEqual(k, "1");
                Assert.AreEqual(v, 1);
            });
            Assert.AreEqual(Caches.WebCache.Get<int>("1"), 1);
            Caches.WebCache.Set("2", 1, new TimeSpan(0, 0, 1), (k, v) =>
            {
                Assert.AreEqual(k, "2");
                Assert.AreEqual(v, 1);
            });
            Assert.IsTrue(Caches.WebCache.Contains("1"));
            Caches.WebCache.Remove("1");
            Caches.WebCache.Clear();
            Assert.IsFalse(Caches.WebCache.Contains("1"));
        }
    }
}
