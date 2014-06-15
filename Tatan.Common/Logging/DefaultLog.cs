namespace Tatan.Common.Logging
{
    using IO;
    using System;
    using System.Threading;
    using SystemFile = System.IO.File;
    using SystemDirectory = System.IO.Directory;
    using SystemPath = System.IO.Path;
    using Collections;

    /// <summary>
    /// 默认日志类，所有文件都是一个不切分
    /// </summary>
    internal class DefaultLog : ILog
    {
        private readonly Type _type;
        private readonly ListMap<LogLevel, string> _filenames;
        private readonly string _fileFormat;
        private readonly string _cententFormat;
        public DefaultLog()
        {
            _type = typeof(DefaultLog);
            _fileFormat = "yyyyMMdd";
            _cententFormat = "yyyy-MM-dd hh:mm:ss.fff";
            _filenames = new ListMap<LogLevel, string>(5)
            {
                {LogLevel.Debug, "{0}.debug.log"},
                {LogLevel.Info, "{0}.info.log"},
                {LogLevel.Warn, "{0}.warn.log"},
                {LogLevel.Error, "{0}.error.log"},
                {LogLevel.Fatal, "{0}.fatal.log"}
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
            var dir = string.Format("{0}log{1}", Runtime.Root, Runtime.Separator);
            if (!SystemDirectory.Exists(dir)) SystemDirectory.CreateDirectory(dir);
            return dir + string.Format(_filenames[level], Date.Now(_fileFormat));
        }

        private void Write(LogLevel level, Type logger, string message, Exception inner)
        {
            if (Log.Level < level) return;
            var path = GetPath(level);
            var content = GetContent(logger, message, inner);
            if (!SystemFile.Exists(path)) SystemFile.Create(path).Close();
            File.AppendText(path, writer => writer.Write(content));
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
            return string.Format("[{0}] [{1}] [{2}] {3}{4}{5}{6}",
                Date.Now(_cententFormat),
                Thread.CurrentThread.Name ?? Thread.CurrentThread.ManagedThreadId.ToString(), 
                (logger ?? _type).FullName,
                message ?? string.Empty,
                Runtime.NewLine,
                inner == null ? string.Empty : inner.ToString(),
                Runtime.NewLine);
        }
    }
}