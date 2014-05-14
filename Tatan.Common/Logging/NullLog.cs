namespace Tatan.Common.Logging
{
    using System;

    internal class NullLog : ILog
    {
        public void Debug(string message, Exception inner = null)
        {
        }

        public void Debug(Type logger, string message, Exception inner = null)
        {
        }

        public void Info(string message, Exception inner = null)
        {
        }

        public void Info(Type logger, string message, Exception inner = null)
        {
        }

        public void Warn(string message, Exception inner = null)
        {
        }

        public void Warn(Type logger, string message, Exception inner = null)
        {
        }

        public void Error(string message, Exception inner = null)
        {
        }

        public void Error(Type logger, string message, Exception inner = null)
        {
        }

        public void Fatal(string message, Exception inner = null)
        {
        }

        public void Fatal(Type logger, string message, Exception inner = null)
        {
        }
    }
}