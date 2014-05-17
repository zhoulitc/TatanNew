using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tatan.Common.Collections;
using Tatan.Common.IO;
using Tatan.Data.ObjectQuery;
using Tatan.Data.Relation;


namespace Tatan.Data.UnitTest
{
    [TestClass]
    public class DataConfigTest
    {

        [TestMethod]
        public void TestConnectionString()
        {
            var st = new Dictionary<string, object>
            {
                {
                    "Tables", new Dictionary<string, IList<Fields>>()
                    {
                        {
                            "Table1", new List<Fields>()
                            {
                                new Fields(1)
                                {
                                    Name = "Column1",
                                    Title = "列1",
                                    Type = "I"
                                },
                                new Fields(2)
                                {
                                    Name = "Column2",
                                    Title = "列2",
                                    Type = "L"
                                },
                                new Fields(3)
                                {
                                    Name = "Column3",
                                    Title = "列3",
                                    Type = "N"
                                },
                                new Fields(4)
                                {
                                    Name = "Column4",
                                    Title = "列4",
                                    Type = "S"
                                },
                                new Fields(5)
                                {
                                    Name = "Column5",
                                    Title = "列5",
                                    Type = "B"
                                },
                                new Fields(6)
                                {
                                    Name = "Column6",
                                    Title = "列6",
                                    Type = "D"
                                }
                            }
                        }
                    }
                }
            };
            var gre = new EntityGenerator(st);
            gre.Execute(Runtime.Root + @"Template\Entity.template", @"D:\");
            var oql = ObjectQuerier.
                Select().
                From("wahaha").
                Where(WhereExpression.Equal("name", "zhouli"));

            const string provider = "System.Data.SqlClient";
            const string defaultConnectionString = "Data Source = BL48VQ68YDRNQMN\\SQLEXPRESS; Initial Catalog = tempdb; User Id = admin; Password = 123456;";
            var source = DataSource.Connect(provider, defaultConnectionString);
            var table = source.Tables.Add(typeof(TestDataEntity));
            var entity = table.NewEntity<TestDataEntity>(1);
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
            var entity2 = entity.Copy();
            entity2["Age"] = 2;
            table.Delete<TestDataEntity>(exp => exp.Name == "zhouli" && ((int)exp["age"] > 18 || exp.Age < 15));
            table.Count<TestDataEntity>(exp => exp.Name == "zhouli" && ((int)exp["age"] > 18 || exp.Age < 15));
            //DataConfig config = ConfigFactory.GetXmlConfig<DataConfig>("DataConfig");
            //string connectionString = CipherFactory.GetCipher("des").Decrypt(config.ConnectionString);
            //Assert.AreEqual(connectionString, defaultConnectionString);
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
    