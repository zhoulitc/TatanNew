namespace Tatan.Common.UnitTest.Collections
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Common.Collections;

    [TestClass]
    public class PropertyCollectionTest
    {
        [TestMethod]
        public void TestCreate()
        {
            var p = new PropertyCollection(
                new { Name = "wahaha", Value = 1 }.GetType(),
                "Name", "Value");
            Assert.AreEqual(p.Contains("Name") && p.Contains("Value"), true);

            p = new PropertyCollection(
                new { Name = "wahaha", Value = 1 }.GetType());
            Assert.AreEqual(p.Contains("Name") && p.Contains("Value"), true);

            try
            {
                p = new PropertyCollection(null);
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "参数为空。\r\n参数名: type");
            }
        }

        [TestMethod]
        public void TestGet()
        {
            var o = new {Name = "wahaha", Value = 1};
            var p = new PropertyCollection(
                o.GetType(),
                "Name", "Value");
            Assert.AreEqual(p[o, "Name"], "wahaha");
            try
            {
                var v = p[null, "Name"];
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "参数为空。\r\n参数名: instance");
            }
            try
            {
                var v = p[o, ""];
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "参数为空。\r\n参数名: name");
            }
            try
            {
                var v = p[o, "sdas"];
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "键不存在。");
            }
        }

        [TestMethod]
        public void TestSet()
        {
            var o = new TestObject { Name = "wahaha", Value = 1 };
            var p = new PropertyCollection(
                o.GetType(),
                "Name", "Value");
            p[o, "Name"] = "fuck";
            Assert.AreEqual(p[o, "Name"], "fuck");
            try
            {
                p[null, "Name"] = "fuck";
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "参数为空。\r\n参数名: instance");
            }
            try
            {
                p[o, ""] = "fuck";
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "参数为空。\r\n参数名: name");
            }
            try
            {
                p[o, "sdas"] = "fuck";
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "键不存在。");
            }
        }

        [TestMethod]
        public void PropertyCollectionDispose()
        {
            var p = new PropertyCollection(
                new { Name = "wahaha", Value = 1 }.GetType(),
                "Name", "Value");
            p.Dispose();
            Assert.AreEqual(p.Count, 0);
        }

        [TestMethod]
        public void PropertyCollectionIsString()
        {
            var p = new PropertyCollection(
                new { Name = "wahaha", Value = 1 }.GetType(),
                "Name", "Value");
            Assert.AreEqual(p.IsString("Name"), true);
            Assert.AreEqual(p.IsString("Name1"), false);
            try
            {
                p.IsString(null);
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "参数为空。\r\n参数名: name");
            }
            try
            {
                p.IsString("");
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "参数为空。\r\n参数名: name");
            }
        }

        public class TestObject
        {
            public string Name { get; set; }
            public object Value { get; set; }
        }
    }
}