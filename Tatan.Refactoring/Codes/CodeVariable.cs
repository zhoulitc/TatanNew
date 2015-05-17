using System;

namespace Tatan.Refactoring.Codes
{
    /// <summary>
    /// 代码变量，一个文件包含多个类，但通常是一个
    /// </summary>
    public class CodeVariable : CodeBase
    {
        /// <summary>
        /// 变量的访问级别
        /// </summary>
        public CodeAccessibility Accessibility { get; set; } = CodeAccessibility.Public;

        /// <summary>
        /// 变量类型
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// 变量位置
        /// </summary>
        public int Index { get; set; }
    }
}