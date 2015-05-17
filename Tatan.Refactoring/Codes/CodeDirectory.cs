namespace Tatan.Refactoring.Codes
{
    using Collections;

    /// <summary>
    /// 代码目录，一个目录包含多个文件和多个子目录
    /// </summary>
    public class CodeDirectory : CodeBase
    {
        /// <summary>
        /// 代码目录的路径
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// 代码目录里面的文件
        /// </summary>
        public CodeFileCollection Files { get; set; }

        /// <summary>
        /// 代码目录里面的子目录
        /// </summary>
        public CodeDirectoryCollection Directories { get; set; }
    }
}