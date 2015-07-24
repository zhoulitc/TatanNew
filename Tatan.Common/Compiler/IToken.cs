namespace Tatan.Common.Compiler
{
    /// <summary>
    /// Token接口
    /// </summary>
    public interface IToken
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public enum TokenStatus
    {
        /// <summary>
        /// 表示当前分析语言的关键字
        /// </summary>
        Keyword,

        /// <summary>
        /// 表示一个数字常量
        /// </summary>
        Number,

        /// <summary>
        /// 表示一个运算符
        /// </summary>
        Operator,
    }

    public class Token : IToken
    {
        public TokenStatus Status { get; set; }

        public string Text { get; set; }

        public object Value { get; set; }
    }
}