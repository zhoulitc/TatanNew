using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tatan.Data;
using Tatan.Common.Serialization;
using Tatan.Common.Configuration;
using System.Text;
using Tatan.Data.Relation;


namespace Tatan.Data.UnitTest
{
    [TestClass]
    public class DataDocumentTest
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
            _source.UseSession("Fields", session => session.ExecuteAsync("TRUNCATE TABLE Fields"));
            _source.UseSession("Tables", session => session.ExecuteAsync("TRUNCATE TABLE Tables"));
        }

        [TestMethod]
        public void TestDataIndex()
        {
            var doc = _source.UseSession("sdsa1", session =>
                session.GetData("SELECT * FROM Fields"));
            if (doc.Count == 0)
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
                doc = _source.UseSession("sdsa1", session => session.GetData("SELECT * FROM Fields"));
            }
            Assert.AreEqual(doc.ToString().Length > 0, true);
            Assert.AreEqual(doc[0]["Name"], "col1");
            Assert.AreEqual(doc[0].Count, 10);
            Assert.IsTrue(doc[0].Equals(doc[0]));
            Assert.IsFalse(doc[0].Equals(null));
            Assert.AreEqual(doc[0].ToString().Length > 0, true);
            foreach (var re in doc)
            {
                foreach (var name in re)
                {
                    if (name == "Name") Assert.AreEqual(re[name], "col1");
                }
                Assert.AreEqual(re[8], (long)1);
            }
        }

        [TestMethod]
        public void TestDataEqual()
        {
            var doc = _source.UseSession("sdsa1", session =>
                session.GetData("SELECT * FROM Fields"));
            Assert.IsTrue(doc.Equals(doc));
            Assert.IsFalse(doc.Equals(null));
        }
    }
}
    