namespace Tatan.Common.Logging
{
    using System.Collections.Generic;
    using System;

    /// <summary>
    /// 日志类
    /// </summary>
    public static class Log
    {
        private static readonly IDictionary<string, ILog> _logs;
        private static readonly object _lock;

        static Log()
        {
            _logs = new Dictionary<string, ILog>
            {
                {typeof (NullLog).FullName, new NullLog()},
                {typeof (DefaultLog).FullName, new DefaultLog("logs")}
            };
            _lock = new object();
            Level = LogLevel.Debug; //默认级别
            Current = _logs[typeof(DefaultLog).FullName];
        }

        /// <summary>
        /// 获取或设置当前的日志级别
        /// </summary>
        public static LogLevel Level { get; set; }

        /// <summary>
        /// 获取指定日志类型的对象
        /// </summary>
        /// <typeparam name="T">日志类型</typeparam>
        /// <returns></returns>
        public static ILog Get<T>() where T : ILog
        {
            var type = typeof (T);
            if (!_logs.ContainsKey(type.FullName))
            {
                var constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor == null)
                    return _logs[typeof (NullLog).FullName];

                var log = constructor.Invoke(null) as ILog;
                if (log != null)
                {
                    lock (_lock)
                    {
                        if (!_logs.ContainsKey(type.FullName))
                        {
                            Current = log;
                            _logs.Add(type.FullName, Current);
                        }
                    }
                }
            }
            return _logs[type.FullName];
        }

        /// <summary>
        /// 获取最近调用的日志对象
        /// </summary>
        public static ILog Current { get; private set; }
    }
}