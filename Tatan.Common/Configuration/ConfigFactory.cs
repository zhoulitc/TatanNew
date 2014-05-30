using System;
namespace Tatan.Common.Configuration
{
    using System.Text;
    using Serialization;
    using Exception;
    using IO;
    using SystemFile = System.IO.File;

    /// <summary>
    /// 配置文件工厂
    /// </summary>
    public static class ConfigFactory
    {
        /// <summary>
        /// 获取配置文件
        /// <para>请使用System.Xml.Serialization空间中的特性标明</para>
        /// </summary>
        /// <typeparam name="T">具体的配置文件类型</typeparam>
        /// <param name="fileName">XML配置文件名</param>
        /// <exception cref="System.ArgumentException">文件路径包含非法字符时</exception>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        /// <exception cref="System.IO.PathTooLongException">文件路径或者文件名超长时</exception>
        /// <exception cref="System.IO.FileNotFoundException">文件没有找到时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">目录没有找到时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.IO.IOException">发生I/O错误时</exception>
        /// <exception cref="System.NotSupportedException">文件格式无效时</exception>
        /// <exception cref="System.Security.SecurityException">没有权限时</exception>
        /// <exception cref="System.Text.DecoderExceptionFallback">回退时</exception>
        /// <returns>配置文件对象</returns>
        public static T GetXmlConfig<T>(string fileName)
        {
            ExceptionHandler.ArgumentNull("fileName", fileName);
            var path = String.Format("{0}{1}.xml", Runtime.Root, fileName);
            ExceptionHandler.FileNotFound(path);
            var content = SystemFile.ReadAllText(path, Encoding.UTF8);
            return Serializer.Xml.Deserialize<T>(content);
        }
    }
}