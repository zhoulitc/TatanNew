using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tatan.Common.Exception;

namespace Tatan.Common.UnitTest
{

    [TestClass]
    public class ExceptionHandlerTest
    {
        [TestMethod]
        public void TestGetText()
        {
            var s = ExceptionHandler.GetText("IllegalSql");
            Assert.AreEqual(s, "非法的数据库命令。数据库命令只支持Select、Insert、Update、Delete、Truncate和存储过程调用。");
        }

        [TestMethod]
        public void TestArgument()
        {
            try
            {
                ExceptionHandler.Argument();
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "参数无效。");
            }
        }

        [TestMethod]
        public void TestArgumentNull()
        {
            try
            {
                ExceptionHandler.ArgumentNull("s", null);
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "参数为空。\r\n参数名: s");
            }
            ExceptionHandler.ArgumentNull("s", "s");

            try
            {
                ExceptionHandler.ArgumentNull("s", "");
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "参数为空。\r\n参数名: s");
            }
        }

        [TestMethod]
        public void TestDatabaseError()
        {
            try
            {
                ExceptionHandler.DatabaseError(new System.Exception("wahaha"));
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "执行数据库操作错误。");
                Assert.AreEqual(ex.InnerException.Message, "wahaha");
            }
        }

        [TestMethod]
        public void TestDirectoryNotFound()
        {
            try
            {
                ExceptionHandler.DirectoryNotFound(@"C:\wahaha");
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "目录不存在。");
            }
            ExceptionHandler.DirectoryNotFound(@"C:\Windows");
        }

        [TestMethod]
        public void TestDuplicateRecords()
        {
            try
            {
                ExceptionHandler.DuplicateRecords();
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "重复记录。");
            }
        }

        [TestMethod]
        public void TestFileNotFound()
        {
            try
            {
                ExceptionHandler.FileNotFound(@"C:\wahaha.txt");
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "文件不存在。");
            }
            ExceptionHandler.FileNotFound(@"C:\Windows\regedit.exe");
        }

        [TestMethod]
        public void TestIllegalMatch()
        {
            try
            {
                ExceptionHandler.IllegalMatch(new System.Text.RegularExpressions.Regex("^s"), "a");
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "非法匹配。");
            }
            ExceptionHandler.IllegalMatch(new System.Text.RegularExpressions.Regex("^s"), "s");
        }

        [TestMethod]
        public void TestIndexOutOfRange()
        {
            try
            {
                ExceptionHandler.IndexOutOfRange(-1);
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "索引超出范围。");
            }
            ExceptionHandler.IndexOutOfRange(1);

            try
            {
                ExceptionHandler.IndexOutOfRange(3, 2);
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "索引超出范围。");
            }
            ExceptionHandler.IndexOutOfRange(1,2);

            try
            {
                ExceptionHandler.IndexOutOfRange(-1, 2);
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "索引超出范围。");
            }
        }

        [TestMethod]
        public void TestKeyNotFound()
        {
            try
            {
                ExceptionHandler.KeyNotFound<string>(null);
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "键不存在。");
            }
            ExceptionHandler.KeyNotFound<string>("haha");

            var map = new Dictionary<string, string>()
                {
                    {"wahaha","1"}
                };
            try
            {
                ExceptionHandler.KeyNotFound<string>(map, "wayaya");
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "键不存在。");
            }
            ExceptionHandler.KeyNotFound<string>(map, "wahaha");
        }

        [TestMethod]
        public void TestNotExistRecords()
        {
            try
            {
                ExceptionHandler.NotExistRecords();
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "不存在此记录。");
            }
        }

        [TestMethod]
        public void TestNotSupported()
        {
            try
            {
                ExceptionHandler.NotSupported();
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "此操作没有实现。");
            }
        }
    }
}
