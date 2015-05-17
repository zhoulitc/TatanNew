namespace Tatan.Refactoring.Codes
{
    using Collections;

    /// <summary>
    /// 代码函数，一个函数包含多个分支
    /// </summary>
    public class CodeFunction : CodeBase
    {
        /// <summary>
        /// 函数的访问级别
        /// </summary>
        public CodeAccessibility Accessibility { get; set; } = CodeAccessibility.Public;

        /// <summary>
        /// 函数占用的行数
        /// </summary>
        public int Lines { get; set; }

        /// <summary>
        /// 函数的Block深度
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// 函数的圈复杂度
        /// </summary>
        public int Complex { get; set; }

        /// <summary>
        /// 函数的参数
        /// </summary>
        public CodeVariableCollection Parameters { get; set; }

        /// <summary>
        /// 函数的If分支
        /// </summary>
        public CodeIfElseCollection IfElses { get; set; }

        /// <summary>
        /// 函数的Switch分支
        /// </summary>
        public CodeSwitchCollection Switches { get; set; }
    }
}