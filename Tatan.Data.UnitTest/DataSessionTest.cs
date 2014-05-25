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
    public class DataSessionTest
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
        public void TestDataAccessTransaction()
        {
            var ss = _source.UseSession("sdsa", session =>
            {
                var trans = session.BeginTransaction();
                var result = session.Execute("UPDATE Fields SET Name='Column1' WHERE Id=38");
                if (result == 0)
                {
                    trans.Rollback();
                    return 0;
                }
                var s = session.GetScalar<long>(("SELECT Size FROM Fields WHERE Id=38"));
                trans.Commit();
                return s;
            });
            Assert.AreEqual(ss, 0);
            // IDataDocument doc = new DataDocument();
            // doc.NewProperty("key1", DataType.Date);
            // doc.NewProperty("key2", DataType.Binary);
            // doc.NewProperty("key3", DataType.Record);
            // IDataRecord rec = doc.NewRecord();
            // IDataRecord rec1 = new DataRecord("name", "age");
            // rec1["name"] = "周立";
            // rec1["age"] = 26;

            // rec["key1"] = DateTime.Now;
            // rec["key2"] = Encoding.UTF8.GetBytes("wahaha");
            // rec["key3"] = DataValue.ValueOf(rec1);
            // //string sssss = (string)rec["key1"];
            // doc.Add(rec);

            // IDataDocument doc1 = doc.Clone();
            // IDataDocument doc2 = doc.Copy();
            // foreach (var r in doc2)
            // {
            //     r["key2"] = Encoding.UTF8.GetBytes("ceshi");
            // }
            // rec1["name"] = "何丹";
            // rec1["age"] = 22;
            // rec["key1"] = DateTime.Now.AddDays(3);

            //// SQLEntity sql = SQLManager.Get("select_test");
            // TablesInfo a = new TablesInfo();
            // a.Name = "table1";
            // a.Title = "表1";
            // a.Remark = "测试表1";
            // try
            // {
            //     DataAccess.Add(a);
            // }
            // catch (Exception ex)
            // {
            //     Assert.AreNotEqual(ex.Message, "执行数据库操作错误。");
            // }
        }

        [TestMethod]
        public void TestDataAccess()
        {
            Assert.AreEqual(_source.UseSession("sdsa1", session => 
                session.Execute("UPDATE Fields SET Name='Column1' WHERE Id=38")), 0);
            Assert.AreEqual(_source.UseSession("sdsa1", session => 
                session.GetData("SELECT * FROM Fields")).Count>=0, true);
            Assert.AreEqual(_source.UseSession("sdsa1", session => 
                session.GetScalar<long>("SELECT Size FROM Fields WHERE Id=1")), 0);
            Assert.AreEqual(_source.UseSession("sdsa1", session => 
                session.GetEntities<Fields>("SELECT * FROM Fields")).Count>=0, true);
        }

        [TestMethod]
        public void TestDataAccessAsync()
        {
            var ss = GetResult().Result;
            Assert.AreEqual(ss, 0);

            var s1 = GetResult1().Result;
            Assert.AreEqual(s1.Count>=0, true);

            var s2 = GetResult2().Result;
            Assert.AreEqual(s2, 0);

            var s3 = GetResult3().Result;
            Assert.AreEqual(s3.Count>=0, true);
        }

        private async Task<int> GetResult()
        {
            var ss = await _source.UseSession("sdsa1", session => session.ExecuteAsync("UPDATE Fields SET Name='Column1' WHERE Id=38"));
            Assert.AreEqual(ss, 0);
            return ss;
        }

        private async Task<IDataDocument> GetResult1()
        {
            var ss = await _source.UseSession("sdsa1", session => session.GetDataAsync("SELECT * FROM Fields"));
            Assert.AreNotEqual(ss, null);
            return ss;
        }

        private async Task<long> GetResult2()
        {
            var ss = await _source.UseSession("sdsa1", session => session.GetScalarAsync<long>("SELECT Size FROM Fields WHERE Id=41"));
            return ss;
        }

        private async Task<IDataEntities<Fields>> GetResult3()
        {
            var ss = await _source.UseSession("sdsa1", session => session.GetEntitiesAsync<Fields>("SELECT * FROM Fields"));
            return ss;
        }
    }
}
    