using System;
using System.Collections.Generic;
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
            var s1 = ExpressionParser.Parse<TestDataEntity>(e => e.Name == null && e.Age != 10, "@");
            Assert.AreEqual(s1.Condition, "(Name IS NULL) AND (Age!=@param1)");

            s1 = ExpressionParser.Parse<TestDataEntity>(e => e.Name != null && e.Age > 10 && e.Age <= 50, "@");
            Assert.AreEqual(s1.Condition, "((Name IS NOT NULL) AND (Age>@param1)) AND (Age<=@param2)");

            s1 = ExpressionParser.Parse<TestDataEntity>(e => e.IsFuck == true && e.Age < 10 && e.Age >= 1, "@");
            Assert.AreEqual(s1.Condition, "((IsFuck=@param1) AND (Age<@param2)) AND (Age>=@param3)");

            s1 = ExpressionParser.Parse<TestDataEntity>(e => e.IsFuck == false && e.DbValue == DBNull.Value, "@");
            Assert.AreEqual(s1.Condition, "(IsFuck=@param1) AND (DbValue IS NULL)");
            Assert.IsNotNull(s1.Parameters);
        }

        [TestMethod]
        public void Test2()
        {
            IDictionary<string, object> sets = new Dictionary<string, object>
            {
                {"Name", null},
                {"Age", 10}
            };
            var s1 = ExpressionParser.Parse(sets, "@");
            Assert.AreEqual(s1.Condition, "Name=@Name,Age=@Age");

            s1 = ExpressionParser.Parse(new{Name="",Age=10}, "@");
            Assert.AreEqual(s1.Condition, "Name=@Name,Age=@Age");
        }
    }

    public class TestDataEntity
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public bool IsFuck { get; set; }

        public DBNull DbValue { get; set; }
    }
}
