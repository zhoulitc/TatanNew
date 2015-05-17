namespace Tatan.Common.Logging
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// 日志类
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public static class Log
    {
        private static event Action<Level, string, string, Exception> Writing;
        private static readonly string _fullName = typeof (Log).FullName;

        /// <summary>
        /// 注册日志行为
        /// </summary>
        /// <param name="writing"></param>
        /// <returns></returns>
        internal static void Register(Action<Level, string, string, Exception> writing)
        {
            if (writing == null) return;
            Writing += writing;
        }

        /// <summary>
        /// 销毁日志行为
        /// </summary>
        /// <param name="writing"></param>
        /// <returns></returns>
        internal static void Undo(Action<Level, string, string, Exception> writing)
        {
            if (writing == null) return;
            Writing -= writing;
        }

        /// <summary>
        /// 日志级别
        /// </summary>
        public enum Level
        {
            /// <summary>
            /// 调试
            /// </summary>
            Debug = 4,

            /// <summary>
            /// 信息
            /// </summary>
            Info = 3,

            /// <summary>
            /// 警告
            /// </summary>
            Warn = 2,

            /// <summary>
            /// 错误
            /// </summary>
            Error = 1,

            /// <summary>
            /// 致命错误
            /// </summary>
            Fatal = 0
        }

        #region Debug

        /// <summary>
        /// 提供Debug级别的日志
        /// <para>此级别日志只能写入文件</para>
        /// <para>文件名格式为????.debug.log</para>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Debug(string message, Exception ex = null)
            => Write(Level.Debug, CurrentLogger, message, ex);

        /// <summary>
        /// 提供Debug级别的日志
        /// <para>此级别日志只能写入文件</para>
        /// <para>文件名格式为????.debug.log</para>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Debug<T>(string message, Exception ex = null) where T : class
            => Write(Level.Debug, typeof(T).FullName, message, ex);

        #endregion

        #region Info

        /// <summary>
        /// 提供Info级别的日志
        /// <para>此级别日志只能写入文件</para>
        /// <para>文件名格式为????.info.log</para>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Info(string message, Exception ex = null)
            => Write(Level.Info, CurrentLogger, message, ex);

        /// <summary>
        /// 提供Info级别的日志
        /// <para>此级别日志只能写入文件</para>
        /// <para>文件名格式为????.info.log</para>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Info<T>(string message, Exception ex = null) where T : class
            =>  Write(Level.Info, typeof (T).FullName, message, ex);

        #endregion

        #region Warn

        /// <summary>
        /// 提供Warn级别的日志
        /// <para>此级别日志既可以写入文件也可以写入数据库</para>
        /// <para>写入表必须带有表名、字段Logger、Message、Exception</para>
        /// <para>文件名格式为????.warn.log</para>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Warn(string message, Exception ex = null)
            => Write(Level.Warn, CurrentLogger, message, ex);

        /// <summary>
        /// 提供Warn级别的日志
        /// <para>此级别日志既可以写入文件也可以写入数据库</para>
        /// <para>写入表必须带有表名、字段Logger、Message、Exception</para>
        /// <para>文件名格式为????.warn.log</para>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Warn<T>(string message, Exception ex = null) where T : class
            => Write(Level.Warn, typeof(T).FullName, message, ex);

        #endregion

        #region Error

        /// <summary>
        /// 提供Error级别的日志
        /// <para>此级别日志既可以写入文件也可以写入数据库</para>
        /// <para>写入表必须带有表名、字段Logger、Message、Exception</para>
        /// <para>文件名格式为????.error.log</para>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Error(string message, Exception ex = null)
            => Write(Level.Error, CurrentLogger, message, ex);

        /// <summary>
        /// 提供Error级别的日志
        /// <para>此级别日志既可以写入文件也可以写入数据库</para>
        /// <para>写入表必须带有表名、字段Logger、Message、Exception</para>
        /// <para>文件名格式为????.error.log</para>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Error<T>(string message, Exception ex = null) where T : class
            => Write(Level.Error, typeof(T).FullName, message, ex);

        #endregion

        #region Fatal

        /// <summary>
        /// 提供Fatal级别的日志
        /// <para>此级别日志既可以写入文件也可以写入数据库</para>
        /// <para>写入表必须带有表名、字段Logger、Message、Exception</para>
        /// <para>文件名格式为????.fatal.log</para>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Fatal(string message, Exception ex = null)
            => Write(Level.Fatal, CurrentLogger, message, ex);

        /// <summary>
        /// 提供Fatal级别的日志
        /// <para>此级别日志既可以写入文件也可以写入数据库</para>
        /// <para>写入表必须带有表名、字段Logger、Message、Exception</para>
        /// <para>文件名格式为????.fatal.log</para>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Fatal<T>(string message, Exception ex = null) where T : class
            => Write(Level.Fatal, typeof(T).FullName, message, ex);

        #endregion

        private static string CurrentLogger
        {
            get
            {
                var trace = new StackTrace(false);
                var index = 1;
                string className;
                do
                {
                    var method = trace.GetFrame(index).GetMethod();
                    className = method.ReflectedType != null
                        ? method.ReflectedType.FullName
                        : _fullName;
                    index++;
                } while (className == _fullName);
                return className;
            }
        }

        private static void Write(Level level, string logger, string message, Exception ex)
        {
            if (Writing == null) return;
            Writing(level, logger, message, ex);
        }
    }
}