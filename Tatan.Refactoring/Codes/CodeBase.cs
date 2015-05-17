namespace Tatan.Refactoring.Codes
{
    using Common;

    /// <summary>
    /// 代码基类
    /// </summary>
    public class CodeBase : INameable
    {
        /// <summary>
        /// 获取或设置代码对象的名称
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}