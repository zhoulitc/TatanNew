namespace Tatan.Common.Logging
{
    using System;

    /// <summary>
    /// 日志接口
    /// </summary>
    public interface ILog
    {
        #region Debug
        /// <summary>
        /// 提供Debug级别的日志
        /// <para>此级别日志只能写入文件</para>
        /// <para>文件名格式为????.debug.log</para>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        void Debug(string message, Exception inner = null);
        /// <summary>
        /// 提供Debug级别的日志
        /// <para>此级别日志只能写入文件</para>
        /// <para>文件名格式为????.debug.log</para>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        void Debug(Type logger, string message, Exception inner = null);
        #endregion

        #region Info
        /// <summary>
        /// 提供Info级别的日志
        /// <para>此级别日志只能写入文件</para>
        /// <para>文件名格式为????.info.log</para>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        void Info(string message, Exception inner = null);
        /// <summary>
        /// 提供Info级别的日志
        /// <para>此级别日志只能写入文件</para>
        /// <para>文件名格式为????.info.log</para>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        void Info(Type logger, string message, Exception inner = null);
        #endregion

        #region Warn
        /// <summary>
        /// 提供Warn级别的日志
        /// <para>此级别日志既可以写入文件也可以写入数据库</para>
        /// <para>写入表必须带有表名、字段Logger、Message、Exception</para>
        /// <para>文件名格式为????.warn.log</para>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        void Warn(string message, Exception inner = null);
        /// <summary>
        /// 提供Warn级别的日志
        /// <para>此级别日志既可以写入文件也可以写入数据库</para>
        /// <para>写入表必须带有表名、字段Logger、Message、Exception</para>
        /// <para>文件名格式为????.warn.log</para>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        void Warn(Type logger, string message, Exception inner = null);
        #endregion

        #region Error
        /// <summary>
        /// 提供Error级别的日志
        /// <para>此级别日志既可以写入文件也可以写入数据库</para>
        /// <para>写入表必须带有表名、字段Logger、Message、Exception</para>
        /// <para>文件名格式为????.error.log</para>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        void Error(string message, Exception inner = null);
        /// <summary>
        /// 提供Error级别的日志
        /// <para>此级别日志既可以写入文件也可以写入数据库</para>
        /// <para>写入表必须带有表名、字段Logger、Message、Exception</para>
        /// <para>文件名格式为????.error.log</para>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        void Error(Type logger, string message, Exception inner = null);
        #endregion

        #region Fatal
        /// <summary>
        /// 提供Fatal级别的日志
        /// <para>此级别日志既可以写入文件也可以写入数据库</para>
        /// <para>写入表必须带有表名、字段Logger、Message、Exception</para>
        /// <para>文件名格式为????.fatal.log</para>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        void Fatal(string message, Exception inner = null);
        /// <summary>
        /// 提供Fatal级别的日志
        /// <para>此级别日志既可以写入文件也可以写入数据库</para>
        /// <para>写入表必须带有表名、字段Logger、Message、Exception</para>
        /// <para>文件名格式为????.fatal.log</para>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        void Fatal(Type logger, string message, Exception inner = null);
        #endregion
    }
}