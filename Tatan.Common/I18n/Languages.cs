namespace Tatan.Common.I18n
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Xml;
    using IO;
    using Xml;

    #region 多语言国际化处理
    /// <summary>
    /// 多语言国际化处理
    /// </summary>
    public class Languages
    {
        private const string _exception = "ANALYSING XML DOCUMENT ERROR.";
        private const string _notFound = "NOT FOUND THE TEXT.";
        private const string _buildError = "CONSTRUCTOR ERROR.";

        #region 公开行为
        /// <summary>
        /// 构造一类XML的多语言方案
        /// </summary>
        /// <param name="directory">语言xml文件格式</param>
        /// <exception cref="System.ArgumentNullException">参数为空时</exception>
        public Languages(string directory)
        {
            if (string.IsNullOrEmpty(directory))
                throw new ArgumentNullException("directory", _buildError);
            if (directory[directory.Length - 1].ToString() != Runtime.Separator)
                directory += Runtime.Separator;
            _format = directory + "{0}.xml";
            _informations = new Dictionary<string, IDictionary<string, string>>();
            LoadInformation(CultureInfo.CurrentUICulture.Name.ToLower());
        }

        /// <summary>
        /// 重新载入某个区域的语言信息
        /// </summary>
        /// <param name="culture">区域</param>
        /// <returns></returns>
        public bool Reload(string culture)
        {
            if (string.IsNullOrEmpty(culture))
                culture = CultureInfo.CurrentUICulture.Name;
            lock (_syncRoot)
            {
                return LoadInformation(culture.ToLower());
            }
        }

        /// <summary>
        /// 获取异常文本
        /// </summary>
        /// <param name="key">唯一键</param>
        /// <param name="culture">区域</param>
        /// <returns></returns>
        public string GetText(string key, string culture = null)
        {
            if (string.IsNullOrEmpty(key))
                return _notFound;
            if (string.IsNullOrEmpty(culture))
                culture = CultureInfo.CurrentUICulture.Name;
            culture = culture.ToLower();
            if (!_informations.ContainsKey(culture))
            {
                lock (_syncRoot)
                {
                    if (!LoadInformation(culture))
                        return _exception;
                }
            }
            if (!_informations[culture].ContainsKey(key))
                return _notFound;
            return _informations[culture][key];
        }
        #endregion

        #region 非公开数据和行为
        private readonly IDictionary<string, IDictionary<string, string>> _informations;
        private readonly string _format;
        private static readonly object _syncRoot = new object();

        private bool LoadInformation(string culture)
        {
            var root = XmlParser.GetRoot(string.Format(_format, culture));
            if (root != null)
            {
                if (_informations.ContainsKey(culture))
                {
                    _informations[culture].Clear();
                    _informations.Remove(culture);
                }
                _informations.Add(culture, new Dictionary<string, string>(root.ChildNodes.Count));
                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node is XmlComment) continue;
                    _informations[culture].Add(node.Name, node.InnerText);
                }
            }
            return root != null;
        } 
        #endregion
    }
    #endregion
}