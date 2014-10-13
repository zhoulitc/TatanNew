using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tatan.Data;
using Tatan.Common.Serialization;
using Tatan.Common.Configuration;
using Tatan.Data.Relation;


namespace Tatan.Data.UnitTest
{
    using Tatan.Common.Cryptography;

    [TestClass]
    public class DataSourceTest
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
        public void TestConnect()
        {
            string p = "System.Data.SQLite";
            string c = @"Db\test.db3";
            var source = DataSource.Connect(p, c);
            Assert.IsNotNull(source);

            source = DataSource.Connect("sqlite");
            Assert.IsNotNull(source);
        }

        [TestMethod]
        public void TestDispose()
        {
            _source.Dispose();
            Assert.AreEqual(_source.Tables.Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestUseSession()
        {
            var doc = _source.UseSession(session => session.ExecuteReader("select * from fields", (reader) => reader.FieldCount));
            Assert.AreEqual(doc>=0,true);

            _source.UseSession(session => session.ExecuteReader(
                "select * from fields",
                (reader) => reader));
        }
    }
}
    