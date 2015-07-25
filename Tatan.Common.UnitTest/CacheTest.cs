using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tatan.Common.Component;
using Tatan.Common.Net;

namespace Tatan.Common.UnitTest
{
    [TestClass]
    public class CacheTest
    {
        [TestCleanup]
        public void Cleanup()
        {
            ComponentManager.Dispose();
        }

        [TestMethod]
        public void TestCustom()
        {
            ComponentManager.Register((IAdapter) new CustomCacheAdapter());
            Http.Cache.Set("1", 1, (k, v) =>
            {
                Assert.AreEqual(k.Substring(k.Length - 1), "1");
                Assert.AreEqual(v, 1);
            });
            Assert.AreEqual(Http.Cache.Get<int>("1"), 1);
            Http.Cache.Set("2", 1, new TimeSpan(0,0,0,0,1), (k, v) =>
            {
                Assert.AreEqual(k.Substring(k.Length - 1), "2");
                Assert.AreEqual(v, 1);
            });
            Assert.IsTrue(Http.Cache.Contains("1"));
            Http.Cache.Remove("1");
            Http.Cache.Clear();
            Assert.IsFalse(Http.Cache.Contains("1"));
            Assert.AreEqual(Http.Cache.Count, 0);
        }

        [TestMethod]
        public void TestWeb()
        {
            ComponentManager.Register(new CacheAdapter());
            Http.Cache.Set("1", 1, (k, v) =>
            {
                Assert.AreEqual(k.Substring(k.Length - 1), "1");
                Assert.AreEqual(v, 1);
            });
            Http.Cache.Set("11", "2", 1);
            //Http.Cache.Remove("11");
            Assert.AreEqual(Http.Cache.Get<int>("1"), 1);
            Assert.AreEqual(Http.Cache.Get<int>("11", "2"), 1);
            Http.Cache.Set("3", 3);
            Http.Cache.Set("2", 1, new TimeSpan(0, 0, 1), (k, v) =>
            {
                Assert.AreEqual(k.Substring(k.Length - 1), "2");
                Assert.AreEqual(v, 1);
            });
            Assert.IsTrue(Http.Cache.Contains("1"));
            Assert.IsTrue(Http.Cache.Contains("11", "2"));
            Http.Cache.Remove("1");
            Http.Cache.Remove("11");
            Http.Cache.Clear();
            Assert.IsFalse(Http.Cache.Contains("1"));
            Assert.AreEqual(Http.Cache.Count, 0);
        }
    }
}
