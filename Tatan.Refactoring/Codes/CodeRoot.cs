namespace Tatan.Refactoring.Codes
{
    using Collections;

    /// <summary>
    /// 代码根
    /// </summary>
    public class CodeRoot : CodeBase
    {
        /// <summary>
        /// 根路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 根下面的模块集合
        /// </summary>
        public CodeModuleCollection Modules { get; set; }
    }
}