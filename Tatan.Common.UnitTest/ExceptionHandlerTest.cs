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
            var s = Tatan.Common.Exception.Assert.GetText("IllegalSql");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(s, "非法的数据库命令。数据库命令只支持Select、Insert、Update、Delete、Truncate和存储过程调用。sql：{0}");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestArgumentNull()
        {
            Tatan.Common.Exception.Assert.ArgumentNotNull("s", null);
            Tatan.Common.Exception.Assert.ArgumentNotNull("s", "");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public void TestDatabaseError()
        {
            Tatan.Common.Exception.Assert.DatabaseError(new System.Exception("wahaha"), "select *");
        }

        [TestMethod]
        [ExpectedException(typeof(System.IO.DirectoryNotFoundException))]
        public void TestDirectoryNotFound()
        {
            Tatan.Common.Exception.Assert.DirectoryFound(@"C:\wahaha");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public void TestDuplicateRecords()
        {
            object s = "";
            Tatan.Common.Exception.Assert.DuplicateRecords(s, "");
        }

        [TestMethod]
        [ExpectedException(typeof(System.IO.FileNotFoundException))]
        public void TestFileNotFound()
        {
            Tatan.Common.Exception.Assert.FileFound(@"C:\wahaha.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public void TestIllegalMatch()
        {
            Tatan.Common.Exception.Assert.LegalMatch(new System.Text.RegularExpressions.Regex("^s"), "a");
        }

        [TestMethod]
        [ExpectedException(typeof(System.IndexOutOfRangeException))]
        public void TestIndexOutOfRange()
        {
            Tatan.Common.Exception.Assert.IndexInRange(-1);
            Tatan.Common.Exception.Assert.IndexInRange(1);

            Tatan.Common.Exception.Assert.IndexInRange(3, 2);
            Tatan.Common.Exception.Assert.IndexInRange(1, 2);

            Tatan.Common.Exception.Assert.IndexInRange(-1, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void TestKeyNotFound()
        {
            Tatan.Common.Exception.Assert.KeyFound<string>(null);
            Tatan.Common.Exception.Assert.KeyFound<string>("haha");

            var map = new Dictionary<string, string>()
                {
                    {"wahaha","1"}
                };
            Tatan.Common.Exception.Assert.KeyFound<string>(map, "wayaya");
            Tatan.Common.Exception.Assert.KeyFound<string>(map, "wahaha");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public void TestNotExistRecords()
        {
            Tatan.Common.Exception.Assert.NotExistRecords("","");
        }

        [TestMethod]
        [ExpectedException(typeof(System.NotSupportedException))]
        public void TestNotSupported()
        {
            Tatan.Common.Exception.Assert.NotSupported();
        }
    }
}
