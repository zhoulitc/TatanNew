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
                {typeof (DefaultLog).FullName, new DefaultLog()}
            };
            _lock = new object();
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
                var constructor = type.GetConstructor(new Type[] {});
                    if (constructor == null)
                        return _logs[typeof (NullLog).FullName];


                    var log = constructor.Invoke(null) as ILog;
                    if (log == null)
                        return _logs[typeof(NullLog).FullName];

                lock (_lock)
                {
                    if (!_logs.ContainsKey(type.FullName))
                    {
                        _logs.Add(type.FullName, log);
                    }
                }
            }
            return _logs[type.FullName];
        }

        /// <summary>
        /// 获取默认的日志对象
        /// </summary>
        public static ILog Default
        {
            get
            {
                return Get<DefaultLog>();
            }
        }
    }
}