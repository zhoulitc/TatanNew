namespace Tatan.Refactoring.Codes
{
    using Collections;

    /// <summary>
    /// 代码文件，一个文件包含多个类，但通常是一个
    /// </summary>
    public class CodeFile : CodeBase
    {
        /// <summary>
        /// 文件的路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// 文件中的类个数
        /// </summary>
        public CodeClassCollection Classes { get; set; }
    }
}