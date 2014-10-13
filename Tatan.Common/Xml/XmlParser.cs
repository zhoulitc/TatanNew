namespace Tatan.Common.Xml
{
    using System.Collections.Generic;
    using System.Xml;
    using Logging;

    /// <summary>
    /// Xml解析类
    /// </summary>
    public static class XmlParser
    {
        /// <summary>
        /// 解析xml文件
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <exception cref="System.Xml.XmlException">加载或者分析错误时</exception>
        /// <exception cref="System.ArgumentException">参数非法时</exception>
        /// <exception cref="System.ArgumentNullException">参数为空时</exception>
        /// <exception cref="System.IO.PathTooLongException">路径超长时</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">路径无效时</exception>
        /// <exception cref="System.IO.IOException">发生 I/O 错误时</exception>
        /// <exception cref="System.UnauthorizedAccessException">访问失败时</exception>
        /// <exception cref="System.IO.FileNotFoundException">文件未找到时</exception>
        /// <exception cref="System.NotSupportedException">文件不支持时</exception>
        /// <exception cref="System.Security.SecurityException">没有权限时</exception>
        /// <returns>Xml文档</returns>
        public static XmlElement GetRoot(string filename)
        {
            var xml = new XmlDocument();
            try
            {
                xml.Load(filename);
                return xml.DocumentElement;
            }
            catch (System.Exception ex)
            {
                Log.Current.Warn(typeof(XmlParser), ex.Message, ex);
                return null;
            }
        }

        public static IDictionary<string, string> ToDictionary(string filename)
        {
            var root = GetRoot(filename);
            if (root == null)
                return null;

            var result = new Dictionary<string, string>(root.ChildNodes.Count);
            foreach (XmlNode node in root.ChildNodes)
            {
                if (node is XmlComment) continue;
                result.Add(node.Name, node.InnerText);
            }
            return result;
        }
    }
}