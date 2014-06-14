using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tatan.Data;
using Tatan.Common.Serialization;
using Tatan.Common.Configuration;
using Tatan.Data.Relation;


namespace Tatan.Data.UnitTest
{
    using Tatan.Common.Cryptography;
    using Tatan.Common.Collections;

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
            _source.UseSession("Fields1", session => session.Execute("DELETE FROM Fields"));
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

            var result = _source.Tables["Fields"].Query<Fields>(query => query.
               Where(tde => tde.Name == "col1").
               OrderBy("Name", DataSort.Descending));

            Assert.AreEqual(result.TotalCount, 1);
            Assert.AreEqual(result.Entities.Count, 1);
            Assert.AreEqual(result.Entities[0].Name, "col1");
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

            Assert.AreEqual(_source.Tables["Tables"].Update(new Tables(1) {Name = "table2"}), false);

            Assert.AreEqual(_source.Tables["Tables"].Count<Tables>(table => table.Id == 0), 0);

            Assert.AreEqual(_source.Tables["Tables"].Update<Tables>(new{Title="wahaha1"}, table=>table.Name=="table1"), 1);

            Assert.AreEqual(_source.Tables["Tables"].Update<Tables>(
                new Dictionary<string, object>{{"Title","walala"}}, 
                table => table.Name == "table1"), 1);

            Assert.AreEqual(_source.Tables["Tables"].Delete(new Tables(1)), false);

            Assert.AreEqual(_source.Tables["Fields"].Delete<Fields>(field=>field.Id==1), 0);
        }

        [TestMethod]
        public void TestTableNew()
        {
            var table = _source.Tables.Add(typeof(TestDataEntity));
            var entity = table.NewEntity<TestDataEntity>(1);
            Assert.IsNotNull(entity);
        }

        public class TestDataEntity : DataEntity
        {
            private static readonly PropertyCollection _perproties;

            static TestDataEntity()
            {
                _perproties = new PropertyCollection(typeof(TestDataEntity),
                    "Name", "Age");
            }

            public TestDataEntity() : base(-1)
            {
            }

            public TestDataEntity(int id)
                : base(id)
            {
            }

            public string Name { get; set; }

            public int Age { get; set; }

            public override void Clear()
            {
                Name = default(string);
                Age = default(int);
            }

            protected override PropertyCollection Properties
            {
                get { return _perproties; }
            }
        }
    }
}
    