using Tatan.Common.Exception;

namespace Tatan.Common.Extension.String.Target
{
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// 标签处理
    /// </summary>
    public static class Target
    {
        /// <summary>
        /// 替换
        /// </summary>
        /// <param name="source">源文本</param>
        /// <param name="targets">标签映射表</param>
        /// <returns>输出文本</returns>
        public static string Replace(this string source, IDictionary<string, string> targets)
        {
            return Replace(source, _regex, 2, 2, targets);
        }

        /// <summary>
        /// 替换
        /// </summary>
        /// <param name="leftMatch">左匹配</param>
        /// <param name="rightMatch">右匹配</param>
        /// <param name="source">源文本</param>
        /// <param name="targets">标签映射表</param>        
        /// <exception cref="System.ArgumentOutOfRangeException">参数超出范围时</exception>
        /// <exception cref="System.Text.RegularExpressions.RegexMatchTimeoutException">解析超时时</exception>
        /// <returns>输出文本</returns>
        public static string Replace(this string source, string leftMatch, string rightMatch, IDictionary<string, string> targets)
        {
            if (string.IsNullOrEmpty(leftMatch))
                leftMatch = "{%";
            ExceptionHandler.IllegalMatch(_regexLetterOrDigit, leftMatch);
            if (string.IsNullOrEmpty(rightMatch))
                rightMatch = "%}";
            ExceptionHandler.IllegalMatch(_regexLetterOrDigit, rightMatch);

            var pattern = string.Format("{0}([A-Za-z0-9_-])*{1}", leftMatch, rightMatch);
            return Replace(source, new Regex(pattern), leftMatch.Length, rightMatch.Length, targets);
        }

        private static readonly Regex _regex = new Regex("{%([A-Za-z0-9_-])*%}");
        private static readonly Regex _regexLetterOrDigit = new Regex("([^A-Za-z0-9])");
        private static string Replace(this string source, Regex regex, int leftLength, int rightLength, IDictionary<string, string> targets)
        {
            if (targets == null)
                return source;
            var begin = 0;
            var match = regex.Match(source, begin);
            var result = new StringBuilder();
            while (match.Success)
            {
                result.Append(source.Substring(begin, match.Index - begin));
                string target = source.Substring(match.Index + leftLength, match.Length - leftLength - rightLength);
                if (targets.ContainsKey(target))
                    result.Append(targets[target]);
                begin = match.Index + match.Length;
                match = regex.Match(source, begin);
            }
            if (begin < source.Length)
                result.Append(source.Substring(begin));
            return result.ToString();
        }
    }
}