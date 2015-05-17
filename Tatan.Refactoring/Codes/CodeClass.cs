namespace Tatan.Refactoring.Codes
{
    using Collections;

    /// <summary>
    /// 代码类，一个类包含多个属性
    /// </summary>
    public class CodeClass : CodeBase
    {
        /// <summary>
        /// 代码的可访问级别
        /// </summary>
        public CodeAccessibility Accessibility { get; set; } = CodeAccessibility.Public;

        /// <summary>
        /// 类占用的行数
        /// </summary>
        public int Lines { get; set; }

        /// <summary>
        /// 类继承父类和实现的接口，只向上一层
        /// </summary>
        public CodeClassCollection Extends { get; set; }

        /// <summary>
        /// 类中的成员字段
        /// </summary>
        public CodeVariableCollection Fields { get; set; }

        /// <summary>
        /// 类中的静态成员字段
        /// </summary>
        public CodeVariableCollection StaticFields { get; set; }

        /// <summary>
        /// 类中的成员函数、包括属性和方法
        /// </summary>
        public CodeFunctionCollection Functions { get; set; }

        /// <summary>
        /// 类中的静态成员函数、包括属性和方法
        /// </summary>
        public CodeFunctionCollection StaticFunctions { get; set; }
    }
}