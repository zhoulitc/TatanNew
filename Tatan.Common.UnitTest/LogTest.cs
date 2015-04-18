using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tatan.Common.Component;
using Tatan.Common.IO;
using Tatan.Common.Logging;

namespace Tatan.Common.UnitTest
{
    [TestClass]
    public class LogTest
    {
        private readonly string _path = Runtime.Root + "Log";

        [TestInitialize]
        public void Init()
        {
            ComponentManager.Dispose();
            if (System.IO.Directory.Exists(_path)) System.IO.Directory.Delete(_path, true);
        }

        [TestCleanup]
        public void Cleanup()
        {
            ComponentManager.Dispose();
            if (System.IO.Directory.Exists(_path)) System.IO.Directory.Delete(_path, true);
        }

        #region 
        public class TestLog
        {
            public TestLog(int a)
            {
            }

            public void WriteLog(Log.Level level, string logger, string message, System.Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public class TestLog2
        {
            public void WriteLog(Log.Level level, string logger, string message, System.Exception ex)
            {
            }
        }
        #endregion

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void TestNullLog()
        {
            ComponentManager.Register(new LogAdapter(new TestLog(0).WriteLog));
            Log.Debug("yeye");
            Log.Info("yeye");
            Log.Warn("yeye");
            Log.Error("yeye");
            Log.Fatal(null);
            Log.Debug("yeye");
            Log.Info<string>("yeye");
            Log.Warn("yeye");
            Log.Error<LogTest>("yeye");
            Log.Fatal("");
            Log.Debug("haha", new System.Exception("wahaha", new System.Exception("walala")));
            Assert.IsFalse(System.IO.Directory.Exists(_path));
        }

        [TestMethod]
        public void TestCustomLog()
        {
            ComponentManager.Register(new LogAdapter(new TestLog2().WriteLog));
            Log.Debug("yeye");
            Log.Info("yeye");
            Log.Warn("yeye");
            Log.Error("yeye");
            Log.Fatal(null);
            Log.Debug("yeye");
            Log.Info<string>("yeye");
            Log.Warn("yeye");
            Log.Error<LogTest>("yeye");
            Log.Fatal("");
            Log.Debug("haha", new System.Exception("wahaha", new System.Exception("walala")));
            Assert.IsFalse(System.IO.Directory.Exists(_path));
        }

        [TestMethod]
        public void TestDebug()
        {
            ComponentManager.Register(new DefaultLogAdapter());
            Log.Debug("yeye");
            Log.Info("yeye");
            Log.Warn("yeye");
            Log.Error<LogTest>("yeye");
            Log.Fatal(null);
            Log.Debug("haha", new System.Exception("wahaha", new System.Exception("walala")));
            if (System.IO.Directory.Exists(_path))
            {
                var files = System.IO.Directory.GetFiles(_path, "*.debug.log");
                Assert.IsTrue(files.Length > 0);
                files = System.IO.Directory.GetFiles(_path, "*.info.log");
                Assert.IsTrue(files.Length > 0);
                files = System.IO.Directory.GetFiles(_path, "*.warn.log");
                Assert.IsTrue(files.Length > 0);
                files = System.IO.Directory.GetFiles(_path, "*.error.log");
                Assert.IsTrue(files.Length > 0);
                files = System.IO.Directory.GetFiles(_path, "*.fatal.log");
                Assert.IsTrue(files.Length > 0);
            }
        }

        [TestMethod]
        public void TestInfo()
        {
            Thread.CurrentThread.Name = "ss";
            ComponentManager.Register(new DefaultLogAdapter(Log.Level.Info));
            Log.Debug("yeye");
            Log.Info("yeye");
            Log.Warn("yeye");
            Log.Error("yeye");
            Log.Fatal("yeye");
            Log.Info("haha", new System.Exception("wahaha", new System.Exception("walala")));
            if (System.IO.Directory.Exists(_path))
            {
                var files = System.IO.Directory.GetFiles(_path, "*.debug.log");
                Assert.IsTrue(files.Length == 0);
                files = System.IO.Directory.GetFiles(_path, "*.info.log");
                Assert.IsTrue(files.Length > 0);
                files = System.IO.Directory.GetFiles(_path, "*.warn.log");
                Assert.IsTrue(files.Length > 0);
                files = System.IO.Directory.GetFiles(_path, "*.error.log");
                Assert.IsTrue(files.Length > 0);
                files = System.IO.Directory.GetFiles(_path, "*.fatal.log");
                Assert.IsTrue(files.Length > 0);
            }
        }

        [TestMethod]
        public void TestWarn()
        {
            ComponentManager.Register(new DefaultLogAdapter(Log.Level.Warn));
            Log.Debug("yeye");
            Log.Info("yeye");
            Log.Warn("yeye");
            Log.Error("yeye");
            Log.Fatal("yeye");
            Log.Warn("haha", new System.Exception("wahaha", new System.Exception("walala")));
            if (System.IO.Directory.Exists(_path))
            {
                var files = System.IO.Directory.GetFiles(_path, "*.debug.log");
                Assert.IsTrue(files.Length == 0);
                files = System.IO.Directory.GetFiles(_path, "*.info.log");
                Assert.IsTrue(files.Length == 0);
                files = System.IO.Directory.GetFiles(_path, "*.warn.log");
                Assert.IsTrue(files.Length > 0);
                files = System.IO.Directory.GetFiles(_path, "*.error.log");
                Assert.IsTrue(files.Length > 0);
                files = System.IO.Directory.GetFiles(_path, "*.fatal.log");
                Assert.IsTrue(files.Length > 0);
            }
        }

        [TestMethod]
        public void TestError()
        {
            ComponentManager.Register(new DefaultLogAdapter(Log.Level.Error));
            Log.Debug("yeye");
            Log.Info("yeye");
            Log.Warn("yeye");
            Log.Error("yeye");
            Log.Fatal("yeye");
            Log.Error("haha", new System.Exception("wahaha", new System.Exception("walala")));
            if (System.IO.Directory.Exists(_path))
            {
                var files = System.IO.Directory.GetFiles(_path, "*.debug.log");
                Assert.IsTrue(files.Length == 0);
                files = System.IO.Directory.GetFiles(_path, "*.info.log");
                Assert.IsTrue(files.Length == 0);
                files = System.IO.Directory.GetFiles(_path, "*.warn.log");
                Assert.IsTrue(files.Length == 0);
                files = System.IO.Directory.GetFiles(_path, "*.error.log");
                Assert.IsTrue(files.Length > 0);
                files = System.IO.Directory.GetFiles(_path, "*.fatal.log");
                Assert.IsTrue(files.Length > 0);
            }
        }

        [TestMethod]
        public void TestFatal()
        {
            ComponentManager.Register(new DefaultLogAdapter(Log.Level.Fatal));
            Log.Debug("yeye");
            Log.Info("yeye");
            Log.Warn("yeye");
            Log.Error("yeye");
            Log.Fatal("yeye");
            Log.Fatal("haha", new System.Exception("wahaha", new System.Exception("walala")));
            if (System.IO.Directory.Exists(_path))
            {
                var files = System.IO.Directory.GetFiles(_path, "*.debug.log");
                Assert.IsTrue(files.Length == 0);
                files = System.IO.Directory.GetFiles(_path, "*.info.log");
                Assert.IsTrue(files.Length == 0);
                files = System.IO.Directory.GetFiles(_path, "*.warn.log");
                Assert.IsTrue(files.Length == 0);
                files = System.IO.Directory.GetFiles(_path, "*.error.log");
                Assert.IsTrue(files.Length == 0);
                files = System.IO.Directory.GetFiles(_path, "*.fatal.log");
                Assert.IsTrue(files.Length > 0);
            }
        }
    }
}
