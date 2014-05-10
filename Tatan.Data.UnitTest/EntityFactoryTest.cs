using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tatan.Data;
using Tatan.Common.Serialization;
using Tatan.Common.Configuration;


namespace Tatan.Data.UnitTest
{
    using Tatan.Common.Cryptography;

    [TestClass]
    public class EntityFactoryTest
    {

        [TestMethod]
        public void TestAddTable()
        {
            //EntityFactory.AddTable("Wahaha", () => null);
            //IEntity e = EntityFactory.Create("Wahaha");
            //EntityFactory.RemoveTable("Wahaha");
            //Assert.AreEqual(e, null);
        }

        [TestMethod]
        public void TestCreate()
        {
            //IEntity e = EntityFactory.Create("TablesInfo");
            //Assert.AreNotEqual(e, null);
        }

        [TestMethod]
        public void TestCreateNull()
        {
            //IEntity e = EntityFactory.Create(null);
            
            //Assert.AreEqual(e, null);
        }
    }
}
    