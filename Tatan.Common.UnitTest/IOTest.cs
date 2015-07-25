using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tatan.Common.Extension.String.IO;

namespace Tatan.Common.UnitTest
{
    using IO;

    [TestClass]
    public class IOTest
    {
        string olddir = Runtime.Root + @"olddir\";
        string newdir = Runtime.Root + @"newdir\";
        private string file = Runtime.Root + "testfile.txt";

        [TestInitialize]
        public void Init()
        {
            if (Directory.Exists(olddir))
                Directory.Delete(olddir, true);
            if (Directory.Exists(newdir))
                Directory.Delete(newdir, true);

            Directory.CreateDirectory(olddir);
            var testdir = olddir + "testdir\\";
            Directory.CreateDirectory(testdir).CreateSubdirectory("testsubdir\\");
            File.Create(testdir + "testfile.txt").Close();

            File.Create(file).Close();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (Directory.Exists(olddir))
                Directory.Delete(olddir, true);
            if (Directory.Exists(newdir))
                Directory.Delete(newdir, true);

            File.Delete(Runtime.Root + "testfile.txt");
        }

        [TestMethod]
        public void DirectoryTestCopy()
        {
            olddir.CopyDirectory(newdir);
            Assert.AreEqual(Directory.Exists(newdir + "testdir\\"), true);

            try
            {
                IOExtension.CopyDirectory(null, newdir);
            }
            catch (System.Exception e)
            {
                Assert.IsTrue(e.Message.Contains("参数名"));
            }
        }

        [TestMethod]
        public void DirectoryTestCopyNull()
        {
            try
            {
                IOExtension.CopyDirectory(null, newdir);
            }
            catch (System.Exception e)
            {
                Assert.IsTrue(e.Message.Contains("参数名"));
            }
        }

        [TestMethod]
        public void FileTestAppendText()
        {
            file.AppendText(a => a.Write("a"));
            var ss = new byte[512];
            file.OpenRead(a => a.Read(ss, 0, ss.Length));
            Assert.AreEqual(ss[0], 97);

            try
            {
                file.AppendText(null);
            }
            catch (System.Exception e)
            {
                Assert.IsTrue(e.Message.Contains("参数名"));
            }

            try
            {
                file.AppendText(null);
            }
            catch (System.Exception e)
            {
                Assert.IsTrue(e.Message.Contains("参数名"));
            }
        }

        [TestMethod]
        public void FileTestCreate()
        {
            file.CreateFile(a => a.Write(Encoding.UTF8.GetBytes("a"), 0, 1));
            var ss = new byte[512];
            file.OpenRead(a => a.Read(ss, 0, ss.Length));
            Assert.AreEqual(ss[0], 97);

            try
            {
                file.AppendText(null);
            }
            catch (System.Exception e)
            {
                Assert.IsTrue(e.Message.Contains("参数名"));
            }
        }

        [TestMethod]
        public void FileTestOpenWrite()
        {
            file.OpenWrite(a => a.Write(Encoding.UTF8.GetBytes("a"), 0, 1));
            file.OpenRead(a => Assert.AreEqual(a.ReadToEnd(), "a"));

            try
            {
                file.OpenWrite(null);
            }
            catch (System.Exception e)
            {
                Assert.IsTrue(e.Message.Contains("参数名"));
            }
            try
            {
                file.AppendText(null);
            }
            catch (System.Exception e)
            {
                Assert.IsTrue(e.Message.Contains("参数名"));
            }
            try
            {
                file.OpenRead((Action<FileStream>)null);
            }
            catch (System.Exception e)
            {
                Assert.IsTrue(e.Message.Contains("参数名"));
            }
            try
            {
                file.OpenRead((Action<StreamReader>)null);
            }
            catch (System.Exception e)
            {
                Assert.IsTrue(e.Message.Contains("参数名"));
            }
        }

        [TestMethod]
        public void RuntimeTest()
        {
            Assert.AreEqual(Runtime.Root, AppDomain.CurrentDomain.BaseDirectory + "\\");
            Assert.AreEqual(Runtime.NewLine, "\r\n");
            Assert.AreEqual(Runtime.Separator, "\\");
        }
    }
}
