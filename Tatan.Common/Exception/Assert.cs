namespace Tatan.Common.Exception
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.IO;
    using System.Text.RegularExpressions;
    using I18n;
    using IO;

    #region 断言处理类

    /// <summary>
    /// 断言处理类，不满足条件则抛出异常
    /// <para>author:zhoulitcqq</para>
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
                throw new ArgumentNullException(argName);
        }

        /// <summary>
        /// 参数不为空
        /// </summary>
        /// <param name="argName"></param>
        /// <param name="arg"></param>
        public static void ArgumentNotNull<T>(string argName, T arg) where T : class
        {
            if (arg == null)
                throw new ArgumentNullException(argName);
        }

        /// <summary>
        /// 键存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public static void KeyFound<T>(T key) where T : class
        {
            if (key == null)
                throw new KeyNotFoundException();
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
                throw new KeyNotFoundException();
        }

        /// <summary>
        /// 索引不越界
        /// </summary>
        /// <param name="index"></param>
        public static void IndexInRange(int index)
        {
            if (index < 0)
                throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// 索引不越界
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        public static void IndexInRange(int index, int count)
        {
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// 不支持
        /// </summary>
        /// <returns></returns>
        public static void NotSupported()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 对象没被销毁
        /// </summary>
        /// <param name="isDisposed"></param>
        /// <param name="name"></param>
        /// <exception cref="ObjectDisposedException"></exception>
        public static void ObjectNotDisposed(bool isDisposed, string name)
        {
            if (isDisposed)
                throw new ObjectDisposedException(name);
        }

        /// <summary>
        /// 对象没被销毁
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <exception cref="ObjectDisposedException"></exception>
        public static void ObjectNotDisposed(object obj, string name)
        {
            if (obj == null)
                throw new ObjectDisposedException(name);
        }

        /// <summary>
        /// 目录存在
        /// </summary>
        /// <param name="path"></param>
        public static void DirectoryFound(string path)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException();
        }

        /// <summary>
        /// 文件存在
        /// </summary>
        /// <param name="path"></param>
        public static void FileFound(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException();
        }

        /// <summary>
        /// 合法匹配
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="input"></param>
        public static void LegalMatch(Regex regex, string input)
        {
            if (!regex.IsMatch(input))
                throw new Exception(string.Format(_exception.GetText("IllegalMatch"), regex.ToString(), input));
        }

        /// <summary>
        /// 合法的数据库命令。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="call"></param>
        /// <exception cref="Exception"></exception>
        public static void LegalSql(string sql, string call)
        {
            sql = sql.ToUpper();
            if (!sql.StartsWith("SELECT") &&
                !sql.StartsWith("UPDATE") &&
                !sql.StartsWith("INSERT") &&
                !sql.StartsWith("DELETE") &&
                !sql.StartsWith("TRUNCATE") &&
                !sql.StartsWith(call.ToUpper()))
                throw new Exception(string.Format(_exception.GetText("IllegalSql"), sql));
        }

        /// <summary>
        /// 数据库错误
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="text"></param>
        public static void DatabaseError(Exception ex, string text)
        {
            var code = string.Empty;
            var exception = ex as DbException;
            if (exception != null)
            {
                code = exception.ErrorCode.ToString();
            }
            throw new Exception(string.Format(_exception.GetText("DatabaseError"), code, text), ex);
        }

        /// <summary>
        /// 重复记录
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static void DuplicateRecords<T>(T id, string name)
        {
            throw new Exception(string.Format(_exception.GetText("DuplicateRecords"), id.ToString(), name));
        }

        /// <summary>
        /// 不存在此记录
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static void NotExistRecords<T>(T id, string name)
        {
            throw new Exception(string.Format(_exception.GetText("NotExistRecords"), id.ToString(), name));
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
                throw new Exception(string.Format(_exception.GetText("IsNotAllow"), max));
        }

        /// <summary>
        /// 类型错误
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static void TypeError(Type no, Type yes)
        {
            throw new Exception(string.Format(_exception.GetText("TypeError"), yes.FullName, no.FullName));
        }

        /// <summary>
        /// 类型必须为接口
        /// </summary>
        /// <param name="t"></param>
        public static void TypeIsInterface(Type t)
        {
            if (!t.IsInterface)
                throw new Exception(string.Format(_exception.GetText("TypeIsInterface"), t.FullName));
        }

        /// <summary>
        /// 适配器必须存在
        /// </summary>
        /// <param name="action"></param>
        public static void AdapterExists<T>(object action)
        {
            if (action == null)
                throw new Exception(string.Format(_exception.GetText("AdapterExists"), typeof (T).FullName));
        }

        #endregion
    }

    #endregion
}