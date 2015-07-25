namespace Tatan.Data.Builder
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Common.Extension.String.Template;

    /// <summary>
    /// 实体生成器
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public abstract class Builder : IBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputFolder"></param>
        public abstract void Execute(string outputFolder);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFolder"></param>
        public abstract void Execute(string inputFile, string outputFolder);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inPath"></param>
        /// <param name="outPath"></param>
        /// <param name="targets"></param>
        protected static void WriteFile(string inPath, string outPath, IDictionary<string, string> targets)
        {
            if (!File.Exists(inPath))
                return;
            if (!File.Exists(outPath))
                File.Create(outPath).Close();
            using (var sw = new StreamWriter(outPath, false, Encoding.UTF8))
            {
                using (var sr = new StreamReader(inPath, Encoding.UTF8))
                {
                    while (!sr.EndOfStream)
                    {
                        sw.WriteLine(sr.ReadLine().Replace(targets));
                    }
                }
            }
        }
    }
}