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
        private readonly string _path = Runtime.Root + "logs";

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
            Log.Current.Debug("yeye");
            Log.Current.Info("yeye");
            Log.Current.Warn(null, "yeye");
            Log.Current.Error(typeof(LogTest), "yeye");
            Log.Current.Fatal(null);
            Log.Current.Debug("haha", new System.Exception("wahaha", new System.Exception("walala")));
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
            Log.Level = LogLevel.Info;
            Log.Current.Debug("yeye");
            Log.Current.Info("yeye");
            Log.Current.Warn("yeye");
            Log.Current.Error("yeye");
            Log.Current.Fatal("yeye");
            Log.Current.Info("haha", new System.Exception("wahaha", new System.Exception("walala")));
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
            Log.Level = LogLevel.Warn;
            Log.Current.Debug("yeye");
            Log.Current.Info("yeye");
            Log.Current.Warn("yeye");
            Log.Current.Error("yeye");
            Log.Current.Fatal("yeye");
            Log.Current.Warn("haha", new System.Exception("wahaha", new System.Exception("walala")));
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
            Log.Level = LogLevel.Error;
            Log.Current.Debug("yeye");
            Log.Current.Info("yeye");
            Log.Current.Warn("yeye");
            Log.Current.Error("yeye");
            Log.Current.Fatal("yeye");
            Log.Current.Error("haha", new System.Exception("wahaha", new System.Exception("walala")));
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
            Log.Level = LogLevel.Fatal;
            Log.Current.Debug("yeye");
            Log.Current.Info("yeye");
            Log.Current.Warn("yeye");
            Log.Current.Error("yeye");
            Log.Current.Fatal("yeye");
            Log.Current.Fatal("haha", new System.Exception("wahaha", new System.Exception("walala")));
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
