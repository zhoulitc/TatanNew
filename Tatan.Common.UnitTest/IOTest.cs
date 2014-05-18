using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            Directory.Copy(olddir, newdir);
            Assert.AreEqual(SystemDirectory.Exists(newdir + "testdir\\"), true);

            try
            {
                Directory.Copy(null, newdir);
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
                Directory.Copy(null, newdir);
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "参数为空。\r\n参数名: source");
            }
        }

        [TestMethod]
        public void FileTestAppendText()
        {
            File.AppendText(file, a => a.Write("a"));
            byte[] ss = new byte[512];
            File.OpenRead(file, a => a.Read(ss, 0, ss.Length));
            Assert.AreEqual(ss[0], 97);

            try
            {
                File.AppendText(null, null);
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "参数为空。\r\n参数名: path");
            }

            try
            {
                File.AppendText(file, null);
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "参数为空。\r\n参数名: action");
            }
        }

        [TestMethod]
        public void FileTestCreate()
        {
            File.Create(file, a => a.Write(Encoding.UTF8.GetBytes("a"), 0, 1));
            byte[] ss = new byte[512];
            File.OpenRead(file, a => a.Read(ss, 0, ss.Length));
            Assert.AreEqual(ss[0], 97);

            try
            {
                File.AppendText(null, null);
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "参数为空。\r\n参数名: path");
            }
        }

        [TestMethod]
        public void FileTestCreateText()
        {
            File.CreateText(file, a => a.Write("a"));
            byte[] ss = new byte[512];
            File.OpenRead(file, a => a.Read(ss, 0, ss.Length));
            Assert.AreEqual(ss[0], 97);

            try
            {
                File.CreateText(null, null);
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "参数为空。\r\n参数名: path");
            }
        }

        [TestMethod]
        public void FileTestOpenWrite()
        {
            File.OpenWrite(file, a => a.Write(Encoding.UTF8.GetBytes("a"), 0, 1));
            File.OpenText(file, a => Assert.AreEqual(a.ReadToEnd(), "a"));

            try
            {
                File.OpenWrite(null, null);
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "参数为空。\r\n参数名: path");
            }
            try
            {
                File.AppendText(file, null);
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "参数为空。\r\n参数名: action");
            }
            try
            {
                File.OpenText(null, null);
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "参数为空。\r\n参数名: path");
            }
            try
            {
                File.OpenText(file, null);
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "参数为空。\r\n参数名: action");
            }
            try
            {
                File.OpenRead(null, null);
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "参数为空。\r\n参数名: path");
            }
            try
            {
                File.OpenRead(file, null);
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
