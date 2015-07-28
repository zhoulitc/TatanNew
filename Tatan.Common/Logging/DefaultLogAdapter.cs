namespace Tatan.Common.Logging
{
    using IO;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using Extension.String.IO;

    /// <summary>
    /// 
    /// </summary>
    public class DefaultLogAdapter : LogAdapter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        public DefaultLogAdapter(Log.Level level = Log.Level.Debug)
            : base(new DefaultLog(level).WriteLog)
        {
        }

        /// <summary>
        /// 默认日志类，所有日志类型文件都是一个不切分
        /// </summary>
        private class DefaultLog
        {
            private readonly IDictionary<string, string> _fileNames;
            private readonly Log.Level _level;
            private readonly string _fileFormat;
            private readonly string _cententFormat;
            private readonly string _dirName;

            public DefaultLog(Log.Level level)
            {
                _level = level;
                _dirName = "logs";
                _fileFormat = "yyyyMMdd";
                _cententFormat = "yyyy-MM-dd hh:mm:ss.fff";
                _fileNames = new Dictionary<string, string>(5)
            {
                {"debug", "{0}.debug.log"},
                {"info", "{0}.info.log"},
                {"warn", "{0}.warn.log"},
                {"error", "{0}.error.log"},
                {"fatal", "{0}.fatal.log"}
            };
            }

            public void WriteLog(Log.Level level, string logger, string message, Exception ex)
            {
                var l = level.ToString().ToLower();
                if (!CanWirte(l)) return;
                var path = GetPath(l);
                var content = GetContent(logger, message, ex);
                if (!File.Exists(path)) File.Create(path).Close();
                path.AppendText(writer => writer.WriteAsync(content));
            }

            private bool CanWirte(string level)
            {
                switch (_level)
                {
                    case Log.Level.Debug:
                        return true;
                    case Log.Level.Info:
                        return level != "debug";
                    case Log.Level.Warn:
                        return level != "debug" && level != "info";
                    case Log.Level.Error:
                        return level == "error" || level == "fatal";
                    case Log.Level.Fatal:
                        return level == "fatal";
                }
                return true;
            }

            private string GetPath(string level)
            {
                var dir = string.Format("{0}{1}{2}", Runtime.Root, _dirName, Runtime.Separator);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                return dir + string.Format(_fileNames[level], Date.Now(_fileFormat));
            }

            /// <summary>
            /// format：[datetime] [thread] [Logger] message \r inner
            /// </summary>
            /// <param name="logger"></param>
            /// <param name="message"></param>
            /// <param name="inner"></param>
            /// <returns></returns>
            private string GetContent(string logger, string message, Exception inner)
            {
                return string.Format("[{0}] [{1}] [{2}] {3}{4}{5}{6}",
                    Date.Now(_cententFormat),
                    Thread.CurrentThread.Name ?? Thread.CurrentThread.ManagedThreadId.ToString(),
                    logger ?? string.Empty,
                    message ?? string.Empty,
                    Runtime.NewLine,
                    inner == null ? string.Empty : inner.ToString(),
                    Runtime.NewLine);
            }
        }
    }
}