using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tatan.Common.UnitTest
{
    using Tatan.Common.IO;
    using SystemFile = System.IO.File;
    using SystemDirectory = System.IO.Directory;

    [TestClass]
    public class IOTest
    {
        string olddir = System.Environment.CurrentDirectory + "\\olddir\\";
        string newdir = System.Environment.CurrentDirectory + "\\newdir\\"; 

        [TestMethod]
        public void DirectoryTestCopy()
        {
            Directory.Copy(olddir, newdir);
            Assert.AreEqual(SystemDirectory.Exists(newdir + "testdir\\"), true);
        }

        [TestMethod]
        public void DirectoryTestCopyNull()
        {
            try
            {
                Directory.Copy(null, newdir);
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "参数为空。\r\n参数名: source");
            }
        }

        [TestInitialize]
        public void CreateSource()
        {
            if (SystemDirectory.Exists(olddir))
                SystemDirectory.Delete(olddir, true);
            if (SystemDirectory.Exists(newdir))
                SystemDirectory.Delete(newdir, true);

            SystemDirectory.CreateDirectory(olddir);
            var testdir = olddir + "testdir\\";
            SystemDirectory.CreateDirectory(testdir).CreateSubdirectory("testsubdir\\");
            SystemFile.Create(testdir + "testfile.txt").Close();
        }

        [TestCleanup]
        public void Clear()
        {
            if (SystemDirectory.Exists(olddir))
                SystemDirectory.Delete(olddir, true);
            if (SystemDirectory.Exists(newdir))
                SystemDirectory.Delete(newdir, true);
        }
    }
}
