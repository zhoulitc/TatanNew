namespace Tatan.Data.Relation
{
    /// <summary>
    /// 生成器接口
    /// </summary>
    public interface IGenerator
    {
        /// <summary>
        /// 执行生成操作，使用默认模板
        /// </summary>
        /// <param name="outputFolder">输出路径，通常是输出目录</param>
        /// <exception cref="System.Exception">执行可能抛出异常</exception>
        void Execute(string outputFolder);

        /// <summary>
        /// 执行生成操作
        /// </summary>
        /// <param name="inputFile">输入路径，通常是模板文件</param>
        /// <param name="outputFolder">输出路径，通常是输出目录</param>
        /// <exception cref="System.Exception">执行可能抛出异常</exception>
        void Execute(string inputFile, string outputFolder);
    }
}