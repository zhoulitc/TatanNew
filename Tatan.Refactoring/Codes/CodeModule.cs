namespace Tatan.Refactoring.Codes
{
    using Collections;

    /// <summary>
    /// 代码根
    /// </summary>
    public class CodeModule : CodeBase
    {
        /// <summary>
        /// 模块路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 模块下面的目录集合
        /// </summary>
        public CodeDirectoryCollection Directories { get; set; }

        /// <summary>
        /// 模块下面的文件集合
        /// </summary>
        public CodeFileCollection Files { get; set; }
    }
}