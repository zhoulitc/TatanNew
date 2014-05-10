using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tatan.Data;
using Tatan.Common.Serialization;
using Tatan.Common.Configuration;
using System.Text;


namespace Tatan.Data.UnitTest
{
    [TestClass]
    public class EntityTest
    {
        [TestMethod]
        public void TestDataAccess()
        {
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
        public void TestDataAccessNull()
        {
            //TablesInfo a = null;
            //try
            //{
            //    DataAccess.Add(a);
            //}
            //catch (Exception ex)
            //{
            //    Assert.AreNotEqual(ex.Message, "实体为空。\r\n参数名: entity");
            //}
        }

        [TestMethod]
        public void TestEntityClone()
        {
            //TablesInfo a = new TablesInfo();
            //a.Name = "table1";
            //a.Title = "表1";
            //a.Remark = "测试表1";
            //IEntity ea = a.Clone();
            //Assert.AreEqual(ea["name"] == null && ea["title"] == null && ea["remark"] == null, true);
        }

        [TestMethod]
        public void TestEntityString()
        {
            //TablesInfo a = new TablesInfo();
            //a.Name = "table1";
            //a.Title = "表1";
            //a.Remark = "测试表1";
            //string s1 = a.ToString(Serializer.JSON);
            //Assert.AreEqual(s1, "{\"name\":\"table1\",\"title\":\"表1\",\"remark\":\"测试表1\"}");
            //IEntity ea = a.Clone();
            //Assert.AreEqual(ea.ToString(), "{\"name\":null,\"title\":null,\"remark\":null}");
        }

        [TestMethod]
        public void TestEntityForeach()
        {
            //TablesInfo a = new TablesInfo();
            //a.Name = "table1";
            //a.Title = "表1";
            //a.Remark = "测试表1";
            //foreach (string name in a)
            //{
            //    Assert.AreEqual(name, "name");
            //    break;
            //}
        }
    }
}
    