namespace Tatan.Common.IO
{
    using SystemFile = System.IO.File;
    using SystemDirectory = System.IO.Directory;
    using SystemPath = System.IO.Path;
    using Exception;

    /// <summary>
    /// 通用File操作，用于处理System.IO.File中返回值为IDispose借口对象的方法
    /// </summary>
    public static class Directory
    {
        #region 拷贝文件夹
        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        /// <param name="source">源路径</param>
        /// <param name="destination">目的路径</param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.FileNotFoundException">文件没有找到时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.IO.IOException">发生I/O错误时</exception>
        /// <exception cref="System.NotSupportedException">文件格式无效时</exception>
        public static void Copy(string source, string destination)
        {
            ExceptionHandler.ArgumentNull("source", source);
            ExceptionHandler.ArgumentNull("destination", destination);

            if (destination[destination.Length - 1].ToString() != Runtime.Separator)
            {
                destination += Runtime.Separator;
            }
            if (!SystemDirectory.Exists(destination))
            {
                SystemDirectory.CreateDirectory(destination);
            }
            var sourcePaths = SystemDirectory.GetFileSystemEntries(source);
            foreach (var sourcePath in sourcePaths)
            {
                string destinationPath = destination + SystemPath.GetFileName(sourcePath);
                if (SystemDirectory.Exists(sourcePath))
                {
                    Copy(sourcePath, destinationPath);
                }
                else
                {
                    SystemFile.Copy(sourcePath, destinationPath, true);
                }
            }
        }
        #endregion
    }
}