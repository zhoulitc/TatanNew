using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tatan.Common.UnitTest
{
    using Tatan.Common.Extension.String.Target;

    [TestClass]
    public class StringHandlerTest
    {
        [TestMethod]
        public void TestReplace()
        {
            var s = "1111111111111111111{%key1%}222222222222 3333333333333{%key2%}sd";
            System.Collections.Generic.IDictionary<string, string> t = new System.Collections.Generic.Dictionary<string, string>();
            t.Add("key1", "sfsadasdsa");
            t.Add("key2", "wqeqwrqwewq");
            var r = s.Replace(t);
            Assert.AreEqual(r, "1111111111111111111sfsadasdsa222222222222 3333333333333wqeqwrqwewqsd");
        }

        [TestMethod]
        public void TestReplace1()
        {
            var s = "1111111111111111111<%key1%>222222222222 3333333333333<%key2%>sd";
            System.Collections.Generic.IDictionary<string, string> t = new System.Collections.Generic.Dictionary<string, string>();
            t.Add("key1", "sfsadasdsa");
            t.Add("key2", "wqeqwrqwewq");
            var r = s.Replace("<%", "%>", t);
            Assert.AreEqual(r, "1111111111111111111sfsadasdsa222222222222 3333333333333wqeqwrqwewqsd");
        }

        [TestMethod]
        public void TestReplaceMatchNull()
        {
            var s = "1111111111111111111{%key1%}222222222222 3333333333333{%key2%}sd";
            System.Collections.Generic.IDictionary<string, string> t = new System.Collections.Generic.Dictionary<string, string>();
            t.Add("key1", "sfsadasdsa");
            t.Add("key2", "wqeqwrqwewq");
            var r = s.Replace("", "", t);
            Assert.AreEqual(r, "1111111111111111111sfsadasdsa222222222222 3333333333333wqeqwrqwewqsd");
        }

        [TestMethod]
        public void TestReplaceMatchThrow1()
        {
            var s = "1111111111111111111{%key1%}222222222222 3333333333333{%key2%}sd";
            System.Collections.Generic.IDictionary<string, string> t = new System.Collections.Generic.Dictionary<string, string>();
            t.Add("key1", "sfsadasdsa");
            t.Add("key2", "wqeqwrqwewq");
            try
            {
                var r = s.Replace("A", "", t);
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "匹配标志中含有数字或字母。");
            }
        }

        [TestMethod]
        public void TestReplaceMatchThrow2()
        {
            var s = "1111111111111111111{%key1%}222222222222 3333333333333{%key2%}sd";
            System.Collections.Generic.IDictionary<string, string> t = new System.Collections.Generic.Dictionary<string, string>();
            t.Add("key1", "sfsadasdsa");
            t.Add("key2", "wqeqwrqwewq");
            try
            {
                var r = s.Replace("", "A", t);
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "匹配标志中含有数字或字母。");
            }
        }

        [TestMethod]
        public void TestReplaceNull()
        {
            var s = "1111111111111111111{%key1%}222222222222 3333333333333{%key2%}sd";
            var r = s.Replace(null);
            Assert.AreEqual(r, s);
        }
    }
}
