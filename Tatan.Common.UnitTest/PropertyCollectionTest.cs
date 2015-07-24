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
                new { Name = "wahaha", Value = 1 }.GetType());
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
                Assert.IsTrue(ex.Message.Contains("参数名"));
            }
        }

        [TestMethod]
        public void TestGet()
        {
            var o = new {Name = "wahaha", Value = 1};
            var p = new PropertyCollection(
                o.GetType());
            Assert.AreEqual(p[o, "Name"], "wahaha");
            try
            {
                var v = p[null, "Name"];
            }
            catch (System.Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("参数名"));
            }
            try
            {
                var v = p[o, ""];
            }
            catch (System.Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("参数名"));
            }
            try
            {
                var v = p[o, "sdas"];
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message.Length > 0, true);
            }
        }

        [TestMethod]
        public void TestSet()
        {
            var o = new TestObject { Name = "wahaha", Value = 1 };
            var p = new PropertyCollection(
                o.GetType());
            p[o, "Name"] = "fuck";
            Assert.AreEqual(p[o, "Name"], "fuck");
            try
            {
                p[null, "Name"] = "fuck";
            }
            catch (System.Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("参数名"));
            }
            try
            {
                p[o, ""] = "fuck";
            }
            catch (System.Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("参数名"));
            }
            try
            {
                p[o, "sdas"] = "fuck";
            }
            catch (System.Exception ex)
            {
                Assert.IsTrue(ex.Message.Length > 0);
            }
        }

        [TestMethod]
        public void PropertyCollectionDispose()
        {
            var p = new PropertyCollection(
                new { Name = "wahaha", Value = 1 }.GetType());
            p.Dispose();
            Assert.AreEqual(p.Count, 0);
        }

        [TestMethod]
        public void PropertyCollectionIsString()
        {
            var p = new PropertyCollection(
                new { Name = "wahaha", Value = 1 }.GetType());
            Assert.AreEqual(p.IsString("Name"), true);
            Assert.AreEqual(p.IsString("Name1"), false);
            try
            {
                p.IsString(null);
            }
            catch (System.Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("参数名"));
            }
            try
            {
                p.IsString("");
            }
            catch (System.Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("参数名"));
            }
        }

        public class TestObject
        {
            public string Name { get; set; }
            public object Value { get; set; }
        }
    }
}