using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tatan.Common.Collections;

namespace Tatan.Common.UnitTest
{
    using Common;

    [TestClass]
    public class ListMapTest
    {
        [TestMethod]
        public void TestAdd()
        {
            var map = new ListMap<string, object>();
            map.Add("", null);
            Assert.AreEqual(map.Count, 0);
            try
            {
                map.Add(null, 1);
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "键不存在。");
            }
            map.Add("key", null);
            Assert.AreEqual(map.Count, 0);
            map.Add("key", 1);
            Assert.AreEqual(map.Count, 1);
        }

        [TestMethod]
        public void TestCotr()
        {
            var dict = new Dictionary<string, object>();
            dict.Add("1", 1);
            dict.Add("2", 2);
            dict.Add("3", 3);
            var map = new ListMap<string, object>(dict);
            Assert.AreEqual(map.Count, 3);
            
            try
            {
                map = new ListMap<string, object>(null);
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "参数为空。\r\n参数名: collection");
            }
        }

        [TestMethod]
        public void TestClear()
        {
            var dict = new Dictionary<string, object>();
            dict.Add("1", 1);
            dict.Add("2", 2);
            dict.Add("3", 3);
            var map = new ListMap<string, object>(dict);
            map.Clear();
            Assert.AreEqual(map.Count, 0);
        }

        [TestMethod]
        public void TestContains()
        {
            var map = new ListMap<string, object>();
            map.Add("1", 1);
            map.Add("2", 2);
            Assert.IsTrue(map.Contains("2"));
            Assert.IsFalse(map.Contains("3"));
            Assert.IsFalse(map.Contains(null));
        }

        [TestMethod]
        public void TestGet()
        {
            var map = new ListMap<string, object>();
            map.Add("1", 1);
            map.Add("2", 2);
            Assert.AreEqual(map["2"], 2);
            try
            {
                var v = map["3"];
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "键不存在。");
            }
            try
            {
                var v = map[null];
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "键不存在。");
            }
        }

        [TestMethod]
        public void TestSet()
        {
            var map = new ListMap<string, object>();
            map.Add("1", 1);
            map.Add("2", 2);
            map["2"] = 3;
            Assert.AreEqual(map["2"], 3);
            map["3"] = 3;
            Assert.AreEqual(map["3"], 3);
            try
            {
                map[null] = 3;
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "键不存在。");
            }
        }

        [TestMethod]
        public void TestToString()
        {
            var map = new ListMap<string, object>();
            map.Add("1", 1);
            map.Add("2", 2);
            map["2"] = 3;
            Assert.AreEqual(map["2"], 3);
            map["3"] = 3;
            Assert.AreEqual(map["3"], 3);
            Assert.AreEqual(map.ToString(), "{\"1\":1,\"2\":3,\"3\":3}");
        }

        [TestMethod]
        public void TestEqual()
        {
            var dict = new Dictionary<string, object>();
            dict.Add("1", 1);
            dict.Add("2", 2);
            dict.Add("3", 3);
            var map = new ListMap<string, object>(dict);
            var map1 = new ListMap<string, object>(dict);
            Assert.IsFalse(map.Equals(null));
            Assert.IsTrue(map.Equals(map1));
        }
    }
}
