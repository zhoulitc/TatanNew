using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tatan.Common.IO;
using Tatan.Common.Logging;

namespace Tatan.Common.UnitTest
{
    using Common;

    [TestClass]
    public class LogTest
    {
        private readonly string _path = Runtime.Root + "log";

        [TestInitialize]
        public void Init()
        {
            if (System.IO.Directory.Exists(_path)) System.IO.Directory.Delete(_path, true);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (System.IO.Directory.Exists(_path)) System.IO.Directory.Delete(_path, true);
        }

        #region 
        public class TestLog : ILog
        {
            public TestLog(int a)
            {
            }

            public void Debug(string message, System.Exception inner = null)
            {
                throw new NotImplementedException();
            }

            public void Debug(Type logger, string message, System.Exception inner = null)
            {
                throw new NotImplementedException();
            }

            public void Info(string message, System.Exception inner = null)
            {
                throw new NotImplementedException();
            }

            public void Info(Type logger, string message, System.Exception inner = null)
            {
                throw new NotImplementedException();
            }

            public void Warn(string message, System.Exception inner = null)
            {
                throw new NotImplementedException();
            }

            public void Warn(Type logger, string message, System.Exception inner = null)
            {
                throw new NotImplementedException();
            }

            public void Error(string message, System.Exception inner = null)
            {
                throw new NotImplementedException();
            }

            public void Error(Type logger, string message, System.Exception inner = null)
            {
                throw new NotImplementedException();
            }

            public void Fatal(string message, System.Exception inner = null)
            {
                throw new NotImplementedException();
            }

            public void Fatal(Type logger, string message, System.Exception inner = null)
            {
                throw new NotImplementedException();
            }
        }

        public class TestLog2 : ILog
        {
            public TestLog2()
            {
            }

            public void Debug(string message, System.Exception inner = null)
            {
            }

            public void Debug(Type logger, string message, System.Exception inner = null)
            {
            }

            public void Info(string message, System.Exception inner = null)
            {
            }

            public void Info(Type logger, string message, System.Exception inner = null)
            {
            }

            public void Warn(string message, System.Exception inner = null)
            {
            }

            public void Warn(Type logger, string message, System.Exception inner = null)
            {
            }

            public void Error(string message, System.Exception inner = null)
            {
            }

            public void Error(Type logger, string message, System.Exception inner = null)
            {
            }

            public void Fatal(string message, System.Exception inner = null)
            {
            }

            public void Fatal(Type logger, string message, System.Exception inner = null)
            {
            }
        }
        #endregion

        [TestMethod]
        public void TestNullLog()
        {
            var log = Log.Get<TestLog>();
            log.Debug("yeye");
            log.Info("yeye");
            log.Warn("yeye");
            log.Error("yeye");
            log.Fatal(null);
            log.Debug(null, "yeye");
            log.Info(typeof(string), "yeye");
            log.Warn(null, "yeye");
            log.Error(typeof(LogTest), "yeye");
            log.Fatal(null, "");
            log.Debug("haha", new System.Exception("wahaha", new System.Exception("walala")));
            Assert.IsFalse(System.IO.Directory.Exists(_path));
        }

        [TestMethod]
        public void TestCustomLog()
        {
            var log = Log.Get<TestLog2>();
            log.Debug("yeye");
            log.Info("yeye");
            log.Warn("yeye");
            log.Error("yeye");
            log.Fatal(null);
            log.Debug(null, "yeye");
            log.Info(typeof(string), "yeye");
            log.Warn(null, "yeye");
            log.Error(typeof(LogTest), "yeye");
            log.Fatal(null, "");
            log.Debug("haha", new System.Exception("wahaha", new System.Exception("walala")));
            Assert.IsFalse(System.IO.Directory.Exists(_path));
        }

        [TestMethod]
        public void TestDebug()
        {
            
            Log.Default.Debug("yeye");
            Log.Default.Info("yeye");
            Log.Default.Warn(null, "yeye");
            Log.Default.Error(typeof(LogTest), "yeye");
            Log.Default.Fatal(null);
            Log.Default.Debug("haha", new System.Exception("wahaha", new System.Exception("walala")));
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

        [TestMethod]
        public void TestInfo()
        {
            Thread.CurrentThread.Name = "ss";
            Log.Level = LogLevel.Info;
            Log.Default.Debug("yeye");
            Log.Default.Info("yeye");
            Log.Default.Warn("yeye");
            Log.Default.Error("yeye");
            Log.Default.Fatal("yeye");
            Log.Default.Info("haha", new System.Exception("wahaha", new System.Exception("walala")));
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

        [TestMethod]
        public void TestWarn()
        {
            Log.Level = LogLevel.Warn;
            Log.Default.Debug("yeye");
            Log.Default.Info("yeye");
            Log.Default.Warn("yeye");
            Log.Default.Error("yeye");
            Log.Default.Fatal("yeye");
            Log.Default.Warn("haha", new System.Exception("wahaha", new System.Exception("walala")));
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

        [TestMethod]
        public void TestError()
        {
            Log.Level = LogLevel.Error;
            Log.Default.Debug("yeye");
            Log.Default.Info("yeye");
            Log.Default.Warn("yeye");
            Log.Default.Error("yeye");
            Log.Default.Fatal("yeye");
            Log.Default.Error("haha", new System.Exception("wahaha", new System.Exception("walala")));
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

        [TestMethod]
        public void TestFatal()
        {
            Log.Level = LogLevel.Fatal;
            Log.Default.Debug("yeye");
            Log.Default.Info("yeye");
            Log.Default.Warn("yeye");
            Log.Default.Error("yeye");
            Log.Default.Fatal("yeye");
            Log.Default.Fatal("haha", new System.Exception("wahaha", new System.Exception("walala")));
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
