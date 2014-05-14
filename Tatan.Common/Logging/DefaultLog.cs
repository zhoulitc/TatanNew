namespace Tatan.Common.Logging
{
    using IO;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using SystemFile = System.IO.File;
    using SystemPath = System.IO.Path;

    /// <summary>
    /// 默认日志类，所有文件都是一个不切分
    /// </summary>
    internal class DefaultLog : ILog
    {
        private readonly Type _type;
        private readonly IDictionary<LogLevel, string> _filenames;
        public DefaultLog()
        {
            _type = typeof(DefaultLog);
            var datetime = Date.Now("yyyy-mm-dd");
            _filenames = new Dictionary<LogLevel, string>(5)
            {
                {LogLevel.Debug, string.Format("{0}.debug.log", datetime)},
                {LogLevel.Info, string.Format("{0}.info.log", datetime)},
                {LogLevel.Warn, string.Format("{0}.warn.log", datetime)},
                {LogLevel.Error, string.Format("{0}.error.log", datetime)},
                {LogLevel.Fatal, string.Format("{0}.fatal.log", datetime)}
            };
        }

        public void Debug(string message, Exception inner = null)
        {
            Debug(_type, message, inner);
        }

        public void Debug(Type logger, string message, Exception inner = null)
        {
            Write(LogLevel.Debug, logger, message, inner);
        }

        public void Info(string message, Exception inner = null)
        {
            Info(_type, message, inner);
        }

        public void Info(Type logger, string message, Exception inner = null)
        {
            Write(LogLevel.Info, logger, message, inner);
        }

        public void Warn(string message, Exception inner = null)
        {
            Warn(_type, message, inner);
        }

        public void Warn(Type logger, string message, Exception inner = null)
        {
            Write(LogLevel.Warn, logger, message, inner);
        }

        public void Error(string message, Exception inner = null)
        {
            Error(_type, message, inner);
        }

        public void Error(Type logger, string message, Exception inner = null)
        {
            Write(LogLevel.Error, logger, message, inner);
        }

        public void Fatal(string message, Exception inner = null)
        {
            Fatal(_type, message, inner);
        }

        public void Fatal(Type logger, string message, Exception inner = null)
        {
            Write(LogLevel.Fatal, logger, message, inner);
        }

        private string GetPath(LogLevel level)
        {
            return string.Format("{0}log{1}{2}", Path.GetRootDirectory(), Path.Separator, _filenames[level]);
        }

        private void Write(LogLevel level, Type logger, string message, Exception inner)
        {
            if (Log.Level < level) return;
            var path = GetPath(level);
            var content = GetContent(logger, message, inner);
            if (!SystemFile.Exists(path)) SystemFile.Create(path).Close();
            File.AppendText(path, writer => writer.WriteAsync(content));
        }

        /// <summary>
        /// 格式：[时间] [线程] [Logger] message \r inner
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        /// <returns></returns>
        private string GetContent(Type logger, string message, Exception inner)
        {
            return string.Format("[{0}] [{1}] [{2}] {3}{4}{5}", 
                Date.Now("YYYY-MM-DD hh:mm:ss.fff"), 
                Thread.CurrentThread.Name, 
                (logger ?? _type).FullName,
                message ?? string.Empty,
                Environment.NewLine,
                inner == null ? string.Empty : inner.ToString());
        }
    }
}