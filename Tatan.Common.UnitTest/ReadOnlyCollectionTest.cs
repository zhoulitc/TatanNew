namespace Tatan.Common.UnitTest.Collections
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Common.Collections;
    using System.Reflection;
    using System.Collections.Generic;

    [TestClass]
    public class ReadOnlyCollectionTest
    {
        [TestMethod]
        public void TestCreate()
        {
            ReadOnlyCollection<int> p = new TestCollection(3);
            Assert.AreEqual(p.Count, 0);
            p = new TestCollection(0);
            Assert.AreEqual(p.Count, 0);
            p = new TestCollection(null);
            Assert.AreEqual(p.Count, 0);

            var d = new Dictionary<string, int>()
            {
                {"1", 1},
                {"2", 2},
            };
            p = new TestCollection(d);
            Assert.AreEqual(p.Count, 2);
        }

        [TestMethod]
        public void TestGetEnumerator()
        {
            ReadOnlyCollection<PropertyInfo> p = new PropertyCollection(
                new { Name = "wahaha", Value = 1 }.GetType(),
                "Name", "Value");
            var i = p.GetEnumerator();
            Assert.AreEqual(i.MoveNext() && i.Current == "Name", true);
        }

        [TestMethod]
        public void TestContains()
        {
            ReadOnlyCollection<PropertyInfo> p = new PropertyCollection(
                new {Name = "wahaha", Value = 1}.GetType(),
                "Name", "Value");
            Assert.AreEqual(p.Contains("Name"), true);
            Assert.AreEqual(p.Contains("Name1"), false);
            Assert.AreEqual(p.Contains(null), false);
            Assert.AreEqual(p.Contains(""), false);
        }

        [TestMethod]
        public void TestCount()
        {
            var p = new PropertyCollection(
                new { Name = "wahaha", Value = 1 }.GetType(),
                "Name", "Value");
            Assert.AreEqual(p.Count, 2);
        }

        public class TestCollection : ReadOnlyCollection<int>
        {
            public TestCollection(int c)
                : base(c)
            {
                
            }

            public TestCollection(IDictionary<string, int> c)
                : base(c)
            {

            }
        }
    }
}