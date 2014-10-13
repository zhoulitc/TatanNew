namespace Tatan.Common.IO
{
    using System;
    using System.IO;
    using SystemFile = System.IO.File;
    using Exception;

    /// <summary>
    /// 通用File操作，用于处理System.IO.File中返回值为IDispose借口对象的方法
    /// </summary>
    public static class File
    {
        #region AppendText
        /// <summary>
        /// 为指定文件追加数据，追加行为自定
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="action">追加行为</param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.NotSupportedException">文件格式无效时</exception>
        public static void AppendText(string path, Action<StreamWriter> action)
        {
            Assert.ArgumentNotNull("path", path);
            Assert.ArgumentNotNull("action", action);

            using (var sw = SystemFile.AppendText(path))
            {
                action(sw);
            }
        }
        #endregion

        #region Create
        /// <summary>
        /// 创建文件，之后行为自定
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="action">创建行为</param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.IO.IOException">发生I/O错误时</exception>
        /// <exception cref="System.NotSupportedException">文件格式无效时</exception>
        public static void Create(string path, Action<FileStream> action = null)
        {
            Assert.ArgumentNotNull("path", path);
            using (var fs = SystemFile.Create(path))
            {
                if (action != null)
                    action(fs);
            }
        }
        #endregion

        #region CreateText
        /// <summary>
        /// 创建文件，并写入内容
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="action">创建行为</param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.NotSupportedException">文件格式无效时</exception>
        public static void CreateText(string path, Action<StreamWriter> action)
        {
            Assert.ArgumentNotNull("path", path);
            Assert.ArgumentNotNull("action", action);

            using (var sw = SystemFile.CreateText(path))
            {
                action(sw);
            }
        }
        #endregion

        #region OpenRead
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="action">读取行为</param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.FileNotFoundException">文件没有找到时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.NotSupportedException">文件格式无效时</exception>
        public static void OpenRead(string path, Action<FileStream> action)
        {
            Assert.ArgumentNotNull("path", path);
            Assert.ArgumentNotNull("action", action);

            using (var fs = SystemFile.OpenRead(path))
            {
                action(fs);
            }
        }
        #endregion

        #region OpenText
        /// <summary>
        /// 读取文件,文本方式
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="action">读取行为</param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.FileNotFoundException">文件没有找到时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.NotSupportedException">文件格式无效时</exception>
        public static void OpenText(string path, Action<StreamReader> action)
        {
            Assert.ArgumentNotNull("path", path);
            Assert.ArgumentNotNull("action", action);

            using (var sr = SystemFile.OpenText(path))
            {
                action(sr);
            }
        }
        #endregion

        #region OpenWrite
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="action">写入行为</param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.FileNotFoundException">文件没有找到时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.NotSupportedException">文件格式无效时</exception>
        public static void OpenWrite(string path, Action<FileStream> action)
        {
            Assert.ArgumentNotNull("path", path);
            Assert.ArgumentNotNull("action", action);

            using (var fs = SystemFile.OpenWrite(path))
            {
                action(fs);
            }
        }
        #endregion
    }
}