using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tatan.Common.Collections;


namespace Tatan.Data.UnitTest
{
    [TestClass]
    public class DataEntityTest
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
        public void TestCloneCopy()
        {
            var table = _source.Tables.Add(typeof(TestDataEntity));
            var entity = table.NewEntity<TestDataEntity>(1);
            Assert.AreEqual(entity.Id, 1);
            entity.PropertyChanged += (o, s) =>
            {
                string s1 = o.ToString();
                if (s == s1)
                    return;
                s1 = s;
            };
            entity["Name"] = "zhouli";
            entity.Age = 1;
            var entity1 = entity.Clone();
            entity1["Name"] = "";
            entity["Name"] = "zhouli1";
            Assert.AreEqual(entity1["Name"],string.Empty);
            Assert.AreEqual(entity1["Age"], 0);
            var entity2 = entity.Copy();
            Assert.AreEqual(entity2["Name"], "zhouli1");
            Assert.AreEqual(entity2["Age"], 1);
            entity2["Age"] = 2;
            Assert.AreEqual(entity["Age"], 1);
            Assert.AreEqual(entity1["Age"], 0);
        }

        [TestMethod]
        public void TestForeach()
        {
            var table = _source.Tables.Add(typeof(TestDataEntity));
            var entity = table.NewEntity<TestDataEntity>(1);
            entity["Name"] = "2";
            entity["Age"] = 2;
            foreach (var name in entity)
            {
                if (name == "Name") Assert.AreEqual(entity[name], "2");
                else if (name == "Age") Assert.AreEqual(entity[name], 2);
            }
            var entity1 = entity.Copy();
            Assert.AreEqual(entity1.Equals(entity), false);
            Assert.AreEqual(entity1.Equals(null), false);
        }

        public class TestDataEntity : DataEntity
        {
            private static readonly PropertyCollection _perproties;

            static TestDataEntity()
            {
                _perproties = new PropertyCollection(typeof(TestDataEntity),
                    "Name", "Age");
            }

            public TestDataEntity(int id = -1) : base(id)
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
    