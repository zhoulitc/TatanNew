namespace Tatan.Common.Extension.String.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Exception;
    using Common.IO;

    #region 提供枚举的转换扩展方法

    /// <summary>
    /// 提供枚举的转换扩展方法
    /// <para>author:zhoulitcqq</para>
    /// <para>此方法组不会抛出异常</para>
    /// </summary>
    public static class IOExtension
    {
        #region Path

        /// <summary>
        /// 改变文件的扩展名
        /// </summary>
        /// <param name="value"></param>
        /// <param name="extension"></param>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <returns></returns>
        public static string ChangeExtension(this string value, string extension)
            => Path.ChangeExtension(value, extension);

        /// <summary>
        /// 获取路径中的目录名
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <returns></returns>
        public static string GetDirectoryName(this string value) => Path.GetDirectoryName(value);

        /// <summary>
        /// 获取路径中的扩展名
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <returns></returns>
        public static string GetExtension(this string value) => Path.GetExtension(value);

        /// <summary>
        /// 获取路径中的文件名
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isExtend"></param>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <returns></returns>
        public static string GetFileName(this string value, bool isExtend = true)
            => isExtend ? Path.GetFileName(value) : Path.GetFileNameWithoutExtension(value);

        /// <summary>
        /// 获取相对路径的绝对路径
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.Security.SecurityException">没有权限时</exception>
        /// <exception cref="System.NotSupportedException">文件格式无效时</exception>
        /// <returns></returns>
        public static string GetFullPath(this string value) => Path.GetFullPath(value);

        /// <summary>
        /// 获取路径的根
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <returns></returns>
        public static string GetPathRoot(this string value) => Path.GetPathRoot(value);

        /// <summary>
        /// 判断是否含有扩展名
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <returns></returns>
        public static bool HasExtension(this string value) => Path.HasExtension(value);

        /// <summary>
        /// 判断是否含有根
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <returns></returns>
        public static bool IsPathRooted(this string value) => Path.IsPathRooted(value);

        #endregion

        #region Directory

        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        /// <param name="value">源路径</param>
        /// <param name="dest">目的路径</param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.FileNotFoundException">文件没有找到时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.IO.IOException">发生I/O错误时</exception>
        /// <exception cref="System.NotSupportedException">文件格式无效时</exception>
        public static void CopyDirectory(this string value, string dest)
        {
            Assert.ArgumentNotNull(nameof(value), value);
            if (dest[dest.Length - 1].ToString() != Runtime.Separator)
            {
                dest += Runtime.Separator;
            }
            if (!Directory.Exists(dest))
            {
                Directory.CreateDirectory(dest);
            }
            var sourcePaths = Directory.GetFileSystemEntries(value);
            foreach (var sourcePath in sourcePaths)
            {
                string destinationPath = dest + Path.GetFileName(sourcePath);
                if (Directory.Exists(sourcePath))
                {
                    CopyDirectory(sourcePath, destinationPath);
                }
                else
                {
                    File.Copy(sourcePath, destinationPath, true);
                }
            }
        }

        /// <summary>
        /// 创建一个目录
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.IO.IOException">发生I/O错误时</exception>
        /// <exception cref="System.NotSupportedException">文件格式无效时</exception>
        /// <returns></returns>
        public static DirectoryInfo CreateDirectory(this string value) => Directory.CreateDirectory(value);

        /// <summary>
        /// 删除一个目录，并指示是否删除所有子目录和子文件
        /// </summary>
        /// <param name="value"></param>
        /// <param name="recursive"></param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.IO.IOException">发生I/O错误时</exception>
        /// <returns></returns>
        public static void DeleteDirectory(this string value, bool recursive = false)
            => Directory.Delete(value, recursive);

        /// <summary>
        /// 获得路径下的所有目录
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pattern"></param>
        /// <param name="deep"></param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.FileNotFoundException">文件没有找到时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.IO.IOException">发生I/O错误时</exception>
        /// <exception cref="System.NotSupportedException">文件格式无效时</exception>
        /// <exception cref="System.Security.SecurityException">没有权限时</exception>
        /// <returns></returns>
        public static IEnumerable<string> GetDirectories(this string value, string pattern = null, bool deep = true)
        {
            if (string.IsNullOrEmpty(pattern))
                return Directory.EnumerateDirectories(value);
            return Directory.EnumerateDirectories(value, pattern,
                deep ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// 获得路径下的所有文件
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pattern"></param>
        /// <param name="deep"></param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.IO.IOException">发生I/O错误时</exception>
        /// <exception cref="System.NotSupportedException">文件格式无效时</exception>
        /// <exception cref="System.Security.SecurityException">没有权限时</exception>
        /// <returns></returns>
        public static IEnumerable<string> GetFiles(this string value, string pattern = null, bool deep = true)
        {
            if (string.IsNullOrEmpty(pattern))
                return Directory.EnumerateFiles(value);
            return Directory.EnumerateFiles(value, pattern,
                deep ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// 判断目录是否存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ExistsDirectory(this string value) => Directory.Exists(value);

        /// <summary>
        /// 判断目录是否存在
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.IO.IOException">发生I/O错误时</exception>
        /// <returns></returns>
        public static DirectoryInfo GetParentDirectory(this string value) => Directory.GetParent(value);

        /// <summary>
        /// 移动目录
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dest"></param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.IO.IOException">发生I/O错误时</exception>
        /// <returns></returns>
        public static void MoveDirectory(this string value, string dest) => Directory.Move(value, dest);

        #endregion

        #region File

        /// <summary>
        /// 追加UTF8文本
        /// </summary>
        /// <param name="value"></param>
        /// <param name="action"></param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.IO.IOException">发生I/O错误时</exception>
        /// <exception cref="System.NotSupportedException">文件格式无效时</exception>
        /// <returns></returns>
        public static void AppendText(this string value, Action<StreamWriter> action)
        {
            using (var sw = File.AppendText(value))
            {
                if (action != null)
                    action(sw);
            }
        }

        /// <summary>
        /// 拷贝文件
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dest"></param>
        /// <param name="overwrite"></param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.FileNotFoundException">文件没有找到时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.IO.IOException">发生I/O错误时</exception>
        /// <exception cref="System.NotSupportedException">文件格式无效时</exception>
        /// <exception cref="System.Security.SecurityException">没有权限时</exception>
        /// <returns></returns>
        public static void CopyFile(this string value, string dest, bool overwrite)
            => File.Copy(value, dest, overwrite);

        /// <summary>
        /// 创建一个文件
        /// </summary>
        /// <param name="value"></param>
        /// <param name="action"></param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.IO.IOException">发生I/O错误时</exception>
        /// <exception cref="System.NotSupportedException">文件格式无效时</exception>
        /// <returns></returns>
        public static void CreateFile(this string value, Action<FileStream> action = null)
        {
            using (var fs = File.Create(value))
            {
                if (action != null)
                    action(fs);
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.IO.IOException">发生I/O错误时</exception>
        /// <exception cref="System.NotSupportedException">文件格式无效时</exception>
        /// <exception cref="System.Security.SecurityException">没有权限时</exception>
        /// <returns></returns>
        public static void DeleteFile(this string value) => File.Delete(value);

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ExistsFile(this string value) => File.Exists(value);

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dest"></param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.IO.IOException">发生I/O错误时</exception>
        /// <exception cref="System.NotSupportedException">文件格式无效时</exception>
        /// <returns></returns>
        public static void MoveFile(this string value, string dest) => File.Move(value, dest);

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.IO.IOException">发生I/O错误时</exception>
        /// <exception cref="System.NotSupportedException">文件格式无效时</exception>
        /// <exception cref="System.Security.SecurityException">没有权限时</exception>
        /// <returns></returns>
        public static byte[] ReadFile(this string value) => File.ReadAllBytes(value);

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.IO.IOException">发生I/O错误时</exception>
        /// <exception cref="System.NotSupportedException">文件格式无效时</exception>
        /// <exception cref="System.Security.SecurityException">没有权限时</exception>
        /// <returns></returns>
        public static string ReadFile(this string value, Encoding encoding)
        {
            if (encoding == null)
                return File.ReadAllText(value);
            return File.ReadAllText(value, encoding);
        }

        /// <summary>
        /// 只读方式打开文件
        /// </summary>
        /// <param name="value"></param>
        /// <param name="action"></param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.IO.IOException">发生I/O错误时</exception>
        /// <exception cref="System.NotSupportedException">文件格式无效时</exception>
        /// <returns></returns>
        public static void OpenRead(this string value, Action<FileStream> action)
        {
            using (var fs = File.OpenRead(value))
            {
                if (action != null)
                    action(fs);
            }
        }

        /// <summary>
        /// 只读方式打开UTF8文件
        /// </summary>
        /// <param name="value"></param>
        /// <param name="action"></param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.IO.IOException">发生I/O错误时</exception>
        /// <exception cref="System.NotSupportedException">文件格式无效时</exception>
        /// <returns></returns>
        public static void OpenRead(this string value, Action<StreamReader> action)
        {
            using (var sr = File.OpenText(value))
            {
                if (action != null)
                    action(sr);
            }
        }

        /// <summary>
        /// 读写方式打开文件
        /// </summary>
        /// <param name="value"></param>
        /// <param name="action"></param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.IO.IOException">发生I/O错误时</exception>
        /// <exception cref="System.NotSupportedException">文件格式无效时</exception>
        /// <returns></returns>
        public static void OpenWrite(this string value, Action<FileStream> action)
        {
            using (var fs = File.OpenWrite(value))
            {
                if (action != null)
                    action(fs);
            }
        }

        /// <summary>
        /// 将内容写入文件
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bytes"></param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.IO.IOException">发生I/O错误时</exception>
        /// <exception cref="System.NotSupportedException">文件格式无效时</exception>
        /// <exception cref="System.Security.SecurityException">没有权限时</exception>
        /// <returns></returns>
        public static void WriteFile(this string value, byte[] bytes) => File.WriteAllBytes(value, bytes);

        /// <summary>
        /// 将内容写入文件
        /// </summary>
        /// <param name="value"></param>
        /// <param name="content"></param>
        /// <param name="encoding"></param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.IO.IOException">发生I/O错误时</exception>
        /// <exception cref="System.NotSupportedException">文件格式无效时</exception>
        /// <exception cref="System.Security.SecurityException">没有权限时</exception>
        /// <returns></returns>
        public static void WriteFile(this string value, string content, Encoding encoding = null)
        {
            if (encoding == null)
                File.WriteAllText(value, content);
            else
                File.WriteAllText(value, content, encoding);
        }

        #endregion
    }

    #endregion
}