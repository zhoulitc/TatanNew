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

            r = s.Replace("", "", t);
            Assert.AreEqual(r, "1111111111111111111sfsadasdsa222222222222 3333333333333wqeqwrqwewqsd");

            r = s.Replace(null);
            Assert.AreEqual(r, s);

            var s1 = "1111111111111111111<%key1%>222222222222 3333333333333<%key2%>sd";
            var r1 = s1.Replace("<%", "%>", t);
            Assert.AreEqual(r1, "1111111111111111111sfsadasdsa222222222222 3333333333333wqeqwrqwewqsd");

            try
            {
                r = s.Replace("A", "", t);
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "非法匹配。");
            }


            try
            {
                r = s.Replace("", "A", t);
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "非法匹配。");
            }
        }
    }
}
