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
        [ExpectedException(typeof(ArgumentException))]
        public void TestArgument()
        {
            ExceptionHandler.Argument();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestArgumentNull()
        {
            ExceptionHandler.ArgumentNull("s", null);
            ExceptionHandler.ArgumentNull("s", "");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public void TestDatabaseError()
        {
            ExceptionHandler.DatabaseError(new System.Exception("wahaha"));
        }

        [TestMethod]
        [ExpectedException(typeof(System.IO.DirectoryNotFoundException))]
        public void TestDirectoryNotFound()
        {
            ExceptionHandler.DirectoryNotFound(@"C:\wahaha");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public void TestDuplicateRecords()
        {
            ExceptionHandler.DuplicateRecords();
        }

        [TestMethod]
        [ExpectedException(typeof(System.IO.FileNotFoundException))]
        public void TestFileNotFound()
        {
            ExceptionHandler.FileNotFound(@"C:\wahaha.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public void TestIllegalMatch()
        {
            ExceptionHandler.IllegalMatch(new System.Text.RegularExpressions.Regex("^s"), "a");
        }

        [TestMethod]
        [ExpectedException(typeof(System.IndexOutOfRangeException))]
        public void TestIndexOutOfRange()
        {
            ExceptionHandler.IndexOutOfRange(-1);
            ExceptionHandler.IndexOutOfRange(1);

            ExceptionHandler.IndexOutOfRange(3, 2);
            ExceptionHandler.IndexOutOfRange(1,2);

            ExceptionHandler.IndexOutOfRange(-1, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void TestKeyNotFound()
        {
            ExceptionHandler.KeyNotFound<string>(null);
            ExceptionHandler.KeyNotFound<string>("haha");

            var map = new Dictionary<string, string>()
                {
                    {"wahaha","1"}
                };
            ExceptionHandler.KeyNotFound<string>(map, "wayaya");
            ExceptionHandler.KeyNotFound<string>(map, "wahaha");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public void TestNotExistRecords()
        {
            ExceptionHandler.NotExistRecords();
        }

        [TestMethod]
        [ExpectedException(typeof(System.NotSupportedException))]
        public void TestNotSupported()
        {
            ExceptionHandler.NotSupported();
        }
    }
}
