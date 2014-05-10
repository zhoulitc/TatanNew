using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tatan.Common.UnitTest
{

    [TestClass]
    public class ExpressionParserTest
    {
        [TestMethod]
        public void Test()
        {
            Action<TestDataEntity> lmabd = (entity)=>
            {
                if (entity != null)
                    entity.Name = "zhouli";
            };
            //string s1 = ExpressionParser.Parse<TestDataEntity>(lmabd);
        }
    }

    public class TestDataEntity
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
