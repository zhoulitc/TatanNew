namespace Tatan.Common.Extension.String.Regular
{
    using System;
    using System.Text.RegularExpressions;
    using Convert;

    #region 提供字符串的正则表达式匹配扩展方法

    /// <summary>
    /// 提供字符串的正则表达式匹配扩展方法
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public static class RegularExtension
    {
        private static readonly Regex _isInt;
        private static readonly Regex _isNumber;
        private static readonly Regex _isBool;
        private static readonly Regex _isDateTime;
        private static readonly Regex _isEmail;
        private static readonly Regex _isPhone;
        private static readonly string[] _verifyCodes;
        private static readonly int[] _wi;
        private static readonly string _addresses;

        static RegularExtension()
        {
            _isInt = new Regex(@"^-?[1-9]\d*$");
            _isNumber = new Regex(@"^-?([1-9]\d*.\d*|0.\d*[1-9]\d*|0?.0+|0)$");
            _isBool = new Regex(@"^(1|0|true|false)$", RegexOptions.IgnoreCase);
            _isDateTime =
                new Regex(
                    @"^((\d{2}(([02468][048])|([13579][26]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|([1-2][0-9])))))|(\d{2}(([02468][1235679])|([13579][01345789]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|(1[0-9])|(2[0-8]))))))(\s(((0?[0-9])|([1-2][0-3]))\:([0-5]?[0-9])((\s)|(\:([0-5]?[0-9])))))?(.\d{7})?$");
            _isEmail = new Regex(@"^\w+([-+.]\w+)*\@\w+([-.]\w+)*.\w+([-.]\w+)*$");
            _isPhone = new Regex(@"^(\+86\-)?1[3458]\d{1}(\-)?\d{4}(\-)?\d{4}$");
            _addresses = "11|22|35|44|53|12|23|36|45|54|13|31|37|46|61|14|32|41|50|62|15|33|42|51|63|21|34|43|52|64|65|71|81|82|91";
            _verifyCodes = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            _wi = new[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
        }

        #region IsMatch

        /// <summary>
        /// 判断是否匹配
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pattern">匹配模式</param>
        /// <param name="option">匹配选项</param>
        /// <param name="start">起始位置</param>
        /// <exception cref="System.ArgumentException">传入模式不合法时</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">参数超出范围时</exception>
        /// <exception cref="System.Text.RegularExpressions.RegexMatchTimeoutException">解析超时时</exception>
        /// <returns>Boolean</returns>
        public static bool IsMatch(this string value, string pattern, RegexOptions option = RegexOptions.None,
            int start = 0)
        {
            if (string.IsNullOrEmpty(pattern) ||
                start < 0 || start >= value.Length)
                return false;
            return new Regex(pattern, option).IsMatch(value, start);
        }

        #endregion

        #region Match

        /// <summary>
        /// 获取匹配子串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pattern">匹配模式</param>
        /// <param name="option">匹配选项</param>
        /// <param name="start">起始位置</param>
        /// <param name="length">搜索长度</param>
        /// <exception cref="System.ArgumentException">传入模式不合法时</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">参数超出范围时</exception>
        /// <exception cref="System.Text.RegularExpressions.RegexMatchTimeoutException">解析超时时</exception>
        /// <returns>匹配的字符串</returns>
        public static string Match(this string value, string pattern, RegexOptions option = RegexOptions.None,
            int start = 0, int length = 0)
        {
            if (string.IsNullOrEmpty(pattern) ||
                start < 0 || start >= value.Length ||
                length < 0 || length >= value.Length)
                return string.Empty;
            var r = new Regex(pattern, option);
            var m = length > start ? r.Match(value, start, length) : r.Match(value, start);
            if (!m.Success)
                return string.Empty;
            return m.Value;
        }

        #endregion

        #region Matches

        /// <summary>
        /// 获取匹配子串集合
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pattern">匹配模式</param>
        /// <param name="option">匹配选项</param>
        /// <param name="start">起始位置</param>
        /// <exception cref="System.ArgumentException">传入模式不合法时</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">参数超出范围时</exception>
        /// <exception cref="System.Text.RegularExpressions.RegexMatchTimeoutException">解析超时时</exception>
        /// <returns>匹配的字符串集合</returns>
        public static string[] Matches(this string value, string pattern, RegexOptions option = RegexOptions.None,
            int start = 0)
        {
            if (string.IsNullOrEmpty(pattern) ||
                start < 0 || start >= value.Length)
                return new string[0];
            var r = new Regex(pattern, option);
            var mc = r.Matches(value, start);
            var ret = new string[mc.Count];
            for (int i = 0, ii = mc.Count; i < ii; i++)
                if (mc[i].Success)
                    ret[i] = mc[i].Value;
            return ret;
        }

        #endregion

        /// <summary>
        /// 使用正则表达式匹配来替换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="regex"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static string Replace(this string value, Regex regex, string newValue)
        {
            if (regex == null)
                return value;
            var matches = regex.Matches(value);
            var result = value;
            foreach (Match match in matches)
            {
                result = result.Replace(match.Value, newValue);
            }
            return result;
        }

        #region Utility

        /// <summary>
        /// 字符串是否为整数
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="System.Text.RegularExpressions.RegexMatchTimeoutException">解析超时时</exception>
        /// <returns></returns>
        public static bool IsInteger(this string value) => _isInt.IsMatch(value);

        /// <summary>
        /// 字符串是否为数字
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="System.Text.RegularExpressions.RegexMatchTimeoutException">解析超时时</exception>
        /// <returns></returns>
        public static bool IsNumber(this string value) => _isNumber.IsMatch(value);

        /// <summary>
        /// 字符串是否为布尔
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="System.Text.RegularExpressions.RegexMatchTimeoutException">解析超时时</exception>
        /// <returns></returns>
        public static bool IsBoolean(this string value) => _isBool.IsMatch(value);

        /// <summary>
        /// 字符串是否为时间
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="System.Text.RegularExpressions.RegexMatchTimeoutException">解析超时时</exception>
        /// <returns></returns>
        public static bool IsDateTime(this string value) => _isDateTime.IsMatch(value);

        /// <summary>
        /// 字符串是否为邮箱
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="System.Text.RegularExpressions.RegexMatchTimeoutException">解析超时时</exception>
        /// <returns></returns>
        public static bool IsEmail(this string value) => _isEmail.IsMatch(value);

        /// <summary>
        /// 字符串是否为电话
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="System.Text.RegularExpressions.RegexMatchTimeoutException">解析超时时</exception>
        /// <returns></returns>
        public static bool IsPhone(this string value)
        {
            return _isPhone.IsMatch(value);
        }

        /// <summary>
        /// 字符串是否为18位身份证
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="System.Text.RegularExpressions.RegexMatchTimeoutException">解析超时时</exception>
        /// <returns></returns>
        public static bool IsIdCard18(this string value)
        {
            if (value.Length != 18)
            {
                return false; //长度验证 
            }

            var prefix = value.Remove(17);
            var suffix = value.Substring(17).ToLower();
            if (!prefix.IsInteger())
            {
                return false; //前17位身份验证
            }
            if (!suffix.IsInteger() && suffix != "x")
            {
                return false; //最后一位身份验证
            }

            if (_addresses.IndexOf(value.Remove(2)) == -1)
            {
                return false;//省份验证             
            }

            var birth = value.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            if (birth.AsValue(DateTime.MinValue) == DateTime.MinValue)
            {
                return false;//生日验证            
            }

            var sum = 0;
            for (var i = 0; i < prefix.Length; i++)
            {
                sum += _wi[i] * value[i].ToString().AsValue<int>();
            }
            if (_verifyCodes[sum % 11] != suffix)
            {
                return false;//校验码验证            
            }
            return true;
        }

        /// <summary>
        /// 字符串是否为15位身份证
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="System.Text.RegularExpressions.RegexMatchTimeoutException">解析超时时</exception>
        /// <returns></returns>
        public static bool IsIdCard15(this string value)
        {
            if (value.Length != 15)
            {
                return false; //长度验证 
            }

            if (!value.IsInteger())
            {
                return false; //数字验证
            }

            if (_addresses.IndexOf(value.Remove(2)) == -1)
            {
                return false; //省份验证              
            }

            var birth = value.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            if (birth.AsValue(DateTime.MinValue) == DateTime.MinValue)
            {
                return false; //生日验证            
            }
            return true;
        }

        #endregion
    }

    #endregion
}

/*
    匹配中文字符的正则表达式： [u4e00-u9fa5]   
　　评注：匹配中文还真是个头疼的事，有了这个表达式就好办了 
　　匹配双字节字符(包括汉字在内)：[^x00-xff] 
　　评注：可以用来计算字符串的长度（一个双字节字符长度计2，ASCII字符计1） 
　　匹配空白行的正则表达式：ns*r 
　　评注：可以用来删除空白行 
　　匹配HTML标记的正则表达式：<(S*?)[^>]*>.*?|<.*? /> 
　　评注：网上流传的版本太糟糕，上面这个也仅仅能匹配部分，对于复杂的嵌套标记依旧无能为力 
　　匹配首尾空白字符的正则表达式：^s*|s*$ 
　　评注：可以用来删除行首行尾的空白字符(包括空格、制表符、换页符等等)，非常有用的表达式 
　　匹配Email地址的正则表达式：w+([-+.]w+)*@w+([-.]w+)*.w+([-.]w+)* 
　　评注：表单验证时很实用 
　　匹配网址URL的正则表达式：[a-zA-z]+://[^s]* 
　　评注：网上流传的版本功能很有限，上面这个基本可以满足需求 
　　匹配帐号是否合法(字母开头，允许5-16字节，允许字母数字下划线)：^[a-zA-Z][a-zA-Z0-9_]{4,15}$ 
　　评注：表单验证时很实用 
　　匹配国内电话号码：d{3}-d{8}|d{4}-d{7} 
　　评注：匹配形式如 0511-4405222 或 021-87888822 
　　匹配腾讯QQ号：[1-9][0-9]{4,} 
　　评注：腾讯QQ号从10000开始 
　　匹配中国邮政编码：[1-9]d{5}(?!d) 
　　评注：中国邮政编码为6位数字 
　　匹配身份证：d{15}|d{18} 
　　评注：中国的身份证为15位或18位 
　　匹配ip地址：d+.d+.d+.d+ 
　　评注：提取ip地址时有用 
　　匹配特定数字： 
　　^[1-9]d*$　 　 //匹配正整数 
　　^-[1-9]d*$ 　 //匹配负整数 
　　^-?[1-9]d*$　　 //匹配整数 
　　^[1-9]d*|0$　 //匹配非负整数（正整数 + 0） 
　　^-[1-9]d*|0$　　 //匹配非正整数（负整数 + 0） 
　　^[1-9]d*.d*|0.d*[1-9]d*$　　 //匹配正浮点数 
　　^-([1-9]d*.d*|0.d*[1-9]d*)$　 //匹配负浮点数 
　　^-?([1-9]d*.d*|0.d*[1-9]d*|0?.0+|0)$　 //匹配浮点数 
　　^[1-9]d*.d*|0.d*[1-9]d*|0?.0+|0$　　 //匹配非负浮点数（正浮点数 + 0） 
　　^(-([1-9]d*.d*|0.d*[1-9]d*))|0?.0+|0$　　//匹配非正浮点数（负浮点数 + 0） 
　　评注：处理大量数据时有用，具体应用时注意修正 
　　匹配特定字符串： 
　　^[A-Za-z]+$　　//匹配由26个英文字母组成的字符串 
　　^[A-Z]+$　　//匹配由26个英文字母的大写组成的字符串 
　　^[a-z]+$　　//匹配由26个英文字母的小写组成的字符串 
　　^[A-Za-z0-9]+$　　//匹配由数字和26个英文字母组成的字符串 
　　^w+$　　//匹配由数字、26个英文字母或者下划线组成的字符串 
　　在使用RegularExpressionValidator验证控件时的验证功能及其验证表达式介绍如下: 
　　只能输入数字：“^[0-9]*$” 
　　只能输入n位的数字：“^d{n}$” 
　　只能输入至少n位数字：“^d{n,}$” 
　　只能输入m-n位的数字：“^d{m,n}$” 
　　只能输入零和非零开头的数字：“^(0|[1-9][0-9]*)$” 
　　只能输入有两位小数的正实数：“^[0-9]+(.[0-9]{2})?$” 
　　只能输入有1-3位小数的正实数：“^[0-9]+(.[0-9]{1,3})?$” 
　　只能输入非零的正整数：“^+?[1-9][0-9]*$” 
　　只能输入非零的负整数：“^-[1-9][0-9]*$” 
　　只能输入长度为3的字符：“^.{3}$” 
　　只能输入由26个英文字母组成的字符串：“^[A-Za-z]+$” 
　　只能输入由26个大写英文字母组成的字符串：“^[A-Z]+$” 
　　只能输入由26个小写英文字母组成的字符串：“^[a-z]+$” 
　　只能输入由数字和26个英文字母组成的字符串：“^[A-Za-z0-9]+$” 
　　只能输入由数字、26个英文字母或者下划线组成的字符串：“^w+$” 
　　验证用户密码:“^[a-zA-Z]w{5,17}$”正确格式为：以字母开头，长度在6-18之间， 
　　只能包含字符、数字和下划线。 
　　验证是否含有^%&'',;=?$"等字符：“[^%&'',;=?$x22]+” 
　　只能输入汉字：“^[u4e00-u9fa5],{0,}$” 
　　验证Email地址：“^w+[-+.]w+)*@w+([-.]w+)*.w+([-.]w+)*$” 
　　验证InternetURL：“^http://([w-]+.)+[w-]+(/[w-./?%&=]*)?$” 
　　验证电话号码：“^((d{3,4})|d{3,4}-)?d{7,8}$” 
　　正确格式为：“XXXX-XXXXXXX”，“XXXX-XXXXXXXX”，“XXX-XXXXXXX”， 
　　“XXX-XXXXXXXX”，“XXXXXXX”，“XXXXXXXX”。 
　　验证身份证号（15位或18位数字）：“^d{15}|d{}18$” 
　　验证一年的12个月：“^(0?[1-9]|1[0-2])$”正确格式为：“01”-“09”和“1”“12” 
　　验证一个月的31天：“^((0?[1-9])|((1|2)[0-9])|30|31)$” 
　　正确格式为：“01”“09”和“1”“31”。 
　　匹配中文字符的正则表达式： [u4e00-u9fa5] 
　　匹配双字节字符(包括汉字在内)：[^x00-xff] 
　　匹配空行的正则表达式：n[s| ]*r 
　　匹配HTML标记的正则表达式：/<(.*)>.*|<(.*) />/ 
　　匹配首尾空格的正则表达式：(^s*)|(s*$) 
　　匹配Email地址的正则表达式：w+([-+.]w+)*@w+([-.]w+)*.w+([-.]w+)* 
　　匹配网址URL的正则表达式：http://([w-]+.)+[w-]+(/[w- ./?%&=]*)? 
    匹配手机号码：
*/