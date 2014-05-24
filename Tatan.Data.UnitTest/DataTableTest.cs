using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tatan.Data;
using Tatan.Common.Serialization;
using Tatan.Common.Configuration;
using Tatan.Data.Relation;


namespace Tatan.Data.UnitTest
{
    using Tatan.Common.Cryptography;

    [TestClass]
    public class DataTableTest
    {
        private IDataSource _source;

        [TestInitialize]
        public void Init()
        {
            string p = "System.Data.SQLite";
            string c = @"Data Source=Db\test.db3;Version=3;";
            _source = DataSource.Connect(p, c);
            _source.Tables.Add(typeof(Tatan.Data.Relation.Fields));
            _source.Tables.Add(typeof(Tatan.Data.Relation.Tables));
            _source.UseSession("Fields", session => session.Execute("DELETE FROM Fields"));
            _source.UseSession("Tables", session => session.Execute("DELETE FROM Tables"));
        }

        [TestMethod]
        public void TestTablesContains()
        {
            Assert.IsTrue(_source.Tables.Contains("Fields"));
        }

        [TestMethod]
        public void TestTableAdd()
        {
            var f = new Fields
            {
                Name = "col1",
                Title = "字段1",
                DefaultValue = "''",
                OrderId = 0,
                Size = 10,
                Type = "S",
                TableId = 1
            };
            _source.Tables["Fields"].Insert(f);
            f.Clear();

            Assert.AreEqual(_source.Tables["Fields"].Count(),1);

            var t = new Tables
            {
                Name = "table1",
                Title = "表1",
                Remark = "这是一个表"
            };
            _source.Tables["Tables"].Insert(t);
            t.Clear();
            Assert.AreEqual(_source.Tables["Tables"].Count(), 1);

            t = new Tables(1);
            var fields = t.GetFields(_source);
            Assert.AreEqual(fields.Count, 1);
            foreach (var field in fields)
            {
                Assert.IsNotNull(field.Name);
            }
        }
    }
}
    