namespace Tatan.Common
{
    using System;
    using System.Globalization;

    /// <summary>
    /// 时间操作
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public static class Date
    {
        static Date()
        {
            DisplayFormat = "yyyy-MM-dd HH:mm:ss";
        }

        private const string _format = "yyyyMMddhhmmss";

        /// <summary>
        /// 获取或设置时间的显示格式
        /// </summary>
        public static string DisplayFormat { get; set; }

        /// <summary>
        /// 获取当前时间串
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">当前区域时间超出范围时</exception>
        public static string Now(string format = null) => ToString(DateTime.Now, format);

        /// <summary>
        /// 获取指定时间串(String)
        /// </summary>
        /// <param name="dt">时间</param>
        /// <param name="format">格式</param>
        /// <exception cref="ArgumentOutOfRangeException">当前区域时间超出范围时</exception>
        /// <returns>时间串</returns>
        public static string ToString(DateTime dt, string format = null)
            => dt.ToString(format ?? _format);

        /// <summary>
        /// 获取指定时间(DateTime)
        /// </summary>
        /// <param name="time">时间串</param>
        /// <param name="format">格式</param>
        /// <returns>时间</returns>
        public static DateTime? Parse(string time, string format = null)
        {
            DateTime result;
            if (!DateTime.TryParseExact(time, format ?? _format, null, DateTimeStyles.None, out result))
                return null;
            return result;
        }
    }
}