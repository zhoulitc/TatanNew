namespace Tatan.Common.IO
{
    using System;
    using System.IO;

    /// <summary>
    /// 通用Path操作
    /// </summary>
    public static class Runtime
    {
        #region 运行时根目录
        /// <summary>
        /// 运行时根目录
        /// </summary>
        /// <exception cref="System.AppDomainUnloadedException">应用程序域被卸载时</exception>
        /// <exception cref="System.Security.SecurityException">没有权限时</exception>
        /// <exception cref="System.IO.IOException">发生I/O错误时</exception>
        public static string Root
        {
            get
            {
                var path = AppDomain.CurrentDomain.BaseDirectory;
                if (IsWebProject())
                    path += "bin" + Separator;
                if (!path.EndsWith(Separator))
                    path += Separator;
                return path;
            }
        }

        /// <summary>
        /// 获取平台分隔符
        /// </summary>
        public static string Separator
        {
            get { return Path.DirectorySeparatorChar.ToString(); }
        }

        /// <summary>
        /// 获取平台换行符
        /// </summary>
        public static string NewLine
        {
            get { return Environment.NewLine; }
        }

        private static bool IsWebProject()
        {
            return AppDomain.CurrentDomain.BaseDirectory == Environment.CurrentDirectory + Separator;
        }
        #endregion
    }
}