namespace Tatan.Common.IO
{
    using System;
    using SystemPath = System.IO.Path;

    /// <summary>
    /// 通用Path操作
    /// </summary>
    public static class Path
    {
        #region 运行时根目录
        /// <summary>
        /// 运行时根目录
        /// </summary>
        /// <exception cref="System.AppDomainUnloadedException">应用程序域被卸载时</exception>
        /// <exception cref="System.Security.SecurityException">没有权限时</exception>
        /// <exception cref="System.IO.IOException">发生I/O错误时</exception>
        public static string GetRootDirectory()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            if (IsWebProject())
                path += "bin" + Separator;
            if (!path.EndsWith(Separator.ToString()))
                path += Separator;
            return path;
        }

        /// <summary>
        /// 获取平台分隔符
        /// </summary>
        public static char Separator
        {
            get { return SystemPath.DirectorySeparatorChar; }
        }

        private static bool IsWebProject()
        {
            return AppDomain.CurrentDomain.BaseDirectory == Environment.CurrentDirectory + Separator;
        }
        #endregion
    }
}