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
                var s = session.ExecuteScalar<long>(("SELECT Size FROM Fields WHERE Id=38"));
                trans.Commit();
                return s;
            });
            Assert.AreEqual(ss, 0);
        }

        [TestMethod]
        public void TestDataAccess()
        {
            Assert.AreEqual(_source.UseSession("sdsa1", session => 
                session.Execute("UPDATE Fields SET Name='Column1' WHERE Id=38")), 0);
            Assert.AreEqual(_source.UseSession("sdsa1", session => 
                session.ExecuteScalar<long>("SELECT Size FROM Fields WHERE Id=1")), 0);
            Assert.AreEqual(_source.UseSession("sdsa1", session => 
                session.GetEntities<Fields>("SELECT * FROM Fields")).Count>=0, true);
        }

        [TestMethod]
        public void TestDataAccessAsync()
        {
            var ss = GetResult().Result;
            Assert.AreEqual(ss, 0);

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

        private async Task<long> GetResult2()
        {
            var ss = await _source.UseSession("sdsa1", session => session.ExecuteScalarAsync<long>("SELECT Size FROM Fields WHERE Id=41"));
            return ss;
        }

        private async Task<IDataEntities<Fields>> GetResult3()
        {
            var ss = await _source.UseSession("sdsa1", session => session.GetEntitiesAsync<Fields>("SELECT * FROM Fields"));
            return ss;
        }
    }
}
    