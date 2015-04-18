using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tatan.Common.Extension.String.IO;

namespace Tatan.Common.UnitTest
{
    using Tatan.Common.IO;
    using SystemFile = System.IO.File;
    using SystemDirectory = System.IO.Directory;

    [TestClass]
    public class IOTest
    {
        string olddir = Runtime.Root + @"olddir\";
        string newdir = Runtime.Root + @"newdir\";
        private string file = Runtime.Root + "testfile.txt";

        [TestInitialize]
        public void Init()
        {
            if (SystemDirectory.Exists(olddir))
                SystemDirectory.Delete(olddir, true);
            if (SystemDirectory.Exists(newdir))
                SystemDirectory.Delete(newdir, true);

            SystemDirectory.CreateDirectory(olddir);
            var testdir = olddir + "testdir\\";
            SystemDirectory.CreateDirectory(testdir).CreateSubdirectory("testsubdir\\");
            SystemFile.Create(testdir + "testfile.txt").Close();

            SystemFile.Create(file).Close();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (SystemDirectory.Exists(olddir))
                SystemDirectory.Delete(olddir, true);
            if (SystemDirectory.Exists(newdir))
                SystemDirectory.Delete(newdir, true);

            SystemFile.Delete(Runtime.Root + "testfile.txt");
        }

        [TestMethod]
        public void DirectoryTestCopy()
        {
            olddir.CopyDirectory(newdir);
            Assert.AreEqual(SystemDirectory.Exists(newdir + "testdir\\"), true);

            try
            {
                Io.CopyDirectory(null, newdir);
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "参数为空。\r\n参数名: source");
            }
        }

        [TestMethod]
        public void DirectoryTestCopyNull()
        {
            try
            {
                Io.CopyDirectory(null, newdir);
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "参数为空。\r\n参数名: source");
            }
        }

        [TestMethod]
        public void FileTestAppendText()
        {
            file.AppendText(a => a.Write("a"));
            byte[] ss = new byte[512];
            file.OpenRead(a => a.Read(ss, 0, ss.Length));
            Assert.AreEqual(ss[0], 97);

            try
            {
                file.AppendText(null);
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "参数为空。\r\n参数名: path");
            }

            try
            {
                file.AppendText(null);
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "参数为空。\r\n参数名: action");
            }
        }

        [TestMethod]
        public void FileTestCreate()
        {
            file.CreateFile(a => a.Write(Encoding.UTF8.GetBytes("a"), 0, 1));
            byte[] ss = new byte[512];
            file.OpenRead(a => a.Read(ss, 0, ss.Length));
            Assert.AreEqual(ss[0], 97);

            try
            {
                file.AppendText(null);
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "参数为空。\r\n参数名: path");
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
                Assert.AreEqual(e.Message, "参数为空。\r\n参数名: path");
            }
            try
            {
                file.AppendText(null);
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "参数为空。\r\n参数名: action");
            }
            try
            {
                file.OpenRead((Action<FileStream>)null);
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "参数为空。\r\n参数名: path");
            }
            try
            {
                file.OpenRead((Action<StreamReader>)null);
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "参数为空。\r\n参数名: action");
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
