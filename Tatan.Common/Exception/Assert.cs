using System.IO;

namespace Tatan.Common.Exception
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using I18n;
    using IO;
    using Directory = Directory;
    using File = File;
    using Collections;

    #region 断言处理类
    /// <summary>
    /// 断言处理类，不满足条件则抛出异常
    /// </summary>
    public static class Assert
    {
        private static readonly Languages _exception;

        static Assert()
        {
            var format = string.Format("{0}Exception{1}", Runtime.Root, Runtime.Separator);
            _exception = new Languages(format);
        }

        /// <summary>
        /// 获取异常文本
        /// </summary>
        /// <param name="key">唯一键</param>
        /// <param name="culture">区域</param>
        /// <returns></returns>
        public static string GetText(string key, string culture = null)
        {
            return _exception.GetText(key, culture);
        }

        #region Exception
        /// <summary>
        /// 参数不为空
        /// </summary>
        /// <param name="argName"></param>
        /// <param name="arg"></param>
        public static void ArgumentNotNull(string argName, string arg)
        {
            if (string.IsNullOrEmpty(arg))
                throw new ArgumentNullException(argName, _exception.GetText("ArgumentNull"));
        }

        /// <summary>
        /// 参数不为空
        /// </summary>
        /// <param name="argName"></param>
        /// <param name="arg"></param>
        public static void ArgumentNotNull<T>(string argName, T arg)
        {
// ReSharper disable once CompareNonConstrainedGenericWithNull
            if (arg == null)
                throw new ArgumentNullException(argName, _exception.GetText("ArgumentNull"));
        }

        /// <summary>
        /// 键存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public static void KeyFound<T>(T key) where T : class
        {
            if (key == null)
                throw new KeyNotFoundException(_exception.GetText("KeyNotFound"));
        }

        /// <summary>
        /// 键存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="map"></param>
        /// <param name="key"></param>
        public static void KeyFound<T>(IDictionary<string, T> map, string key)
        {
            if (!map.ContainsKey(key))
                throw new KeyNotFoundException(_exception.GetText("KeyNotFound"));
        }

        /// <summary>
        /// 键存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="map"></param>
        /// <param name="key"></param>
        public static void KeyFound<T>(ListMap<string, T> map, string key)
        {
            if (!map.Contains(key))
                throw new KeyNotFoundException(_exception.GetText("KeyNotFound"));
        }

        /// <summary>
        /// 索引不越界
        /// </summary>
        /// <param name="index"></param>
        public static void IndexInOfRange(int index)
        {
            if (index < 0)
                throw new IndexOutOfRangeException(_exception.GetText("IndexOutOfRange"));
        }

        /// <summary>
        /// 索引不越界
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        public static void IndexInOfRange(int index, int count)
        {
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException(_exception.GetText("IndexOutOfRange"));
        }

        /// <summary>
        /// 不支持
        /// </summary>
        /// <returns></returns>
        public static void NotSupported()
        {
            throw new NotSupportedException(_exception.GetText("NotSupported"));
        }

        /// <summary>
        /// 不支持
        /// </summary>
        /// <returns></returns>
        public static T NotSupported<T>()
        {
            throw new NotSupportedException(_exception.GetText("NotSupported"));
        }

        /// <summary>
        /// 对象没被销毁
        /// </summary>
        /// <param name="isDisposed"></param>
        /// <exception cref="ObjectDisposedException"></exception>
        public static void ObjectNotDisposed(bool isDisposed)
        {
            if (isDisposed)
                throw new ObjectDisposedException(_exception.GetText("ObjectDisposed"));
        }

        /// <summary>
        /// 目录存在
        /// </summary>
        /// <param name="path"></param>
        public static void DirectoryFound(string path)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException(_exception.GetText("DirectoryNotFound"));
        }

        /// <summary>
        /// 文件不存在
        /// </summary>
        /// <param name="path"></param>
        public static void FileFound(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(_exception.GetText("FileNotFound"), path);
        }

        /// <summary>
        /// 合法匹配
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="input"></param>
        public static void LegalMatch(Regex regex, string input)
        {
            if (!regex.IsMatch(input))
                throw new Exception(_exception.GetText("IllegalMatch"));
        }

        /// <summary>
        /// 合法的数据库命令。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="call"></param>
        /// <exception cref="Exception"></exception>
        public static void LegalSql(string sql, string call)
        {
            if (!sql.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase) &&
                !sql.StartsWith("UPDATE", StringComparison.OrdinalIgnoreCase) &&
                !sql.StartsWith("INSERT", StringComparison.OrdinalIgnoreCase) &&
                !sql.StartsWith("DELETE", StringComparison.OrdinalIgnoreCase) &&
                !sql.StartsWith("TRUNCATE", StringComparison.OrdinalIgnoreCase) &&
                !sql.StartsWith(call, StringComparison.OrdinalIgnoreCase))
                throw new Exception(_exception.GetText("IllegalSql"));
        }

        /// <summary>
        /// 数据库错误
        /// </summary>
        /// <param name="ex"></param>
        public static void DatabaseError(Exception ex)
        {
            throw new Exception(_exception.GetText("DatabaseError"), ex);
        }

        /// <summary>
        /// 重复记录
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static void DuplicateRecords()
        {
            throw new Exception(_exception.GetText("DuplicateRecords"));
        }

        /// <summary>
        /// 不存在此记录
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static void NotExistRecords()
        {
            throw new Exception(_exception.GetText("NotExistRecords"));
        }

        /// <summary>
        /// 允许有更多元素
        /// </summary>
        /// <param name="count"></param>
        /// <param name="max"></param>
        /// <exception cref="Exception"></exception>
        public static void IsAllow(int count, int max)
        {
            if (count > max)
                throw new Exception(_exception.GetText("IsNotAllow"));
        }

        /// <summary>
        /// 类型错误
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static void TypeError()
        {
            throw new Exception(_exception.GetText("TypeError"));
        }
        #endregion
    }
    #endregion
}