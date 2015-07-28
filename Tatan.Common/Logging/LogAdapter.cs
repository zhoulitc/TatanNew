namespace Tatan.Common.Logging
{
    using System;
    using Component;

    /// <summary>
    /// 日志可适配接口
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public class LogAdapter : IAdapter
    {
        private readonly Action<Log.Level, string, string, Exception> _action;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public LogAdapter(Action<Log.Level, string, string, Exception> action)
        {
            _action = action;
            Log.Register(_action);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose() => Log.Undo(_action);
    }
}