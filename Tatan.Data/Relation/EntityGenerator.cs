namespace Tatan.Data.Relation
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Common.Extension.String.Target;
    using Common.Exception;
    using CommonRuntime = Common.IO.Runtime;

    /// <summary>
    /// 实体生成器
    /// </summary>
    public class EntityGenerator : IGenerator
    {
        private readonly IDictionary<string, object> _data;
        private readonly IDictionary<string, string> _types = new Dictionary<string, string>(6)
            {
                {"I", "int"},
                {"L", "long"},
                {"N", "double"},
                {"S", "string"},
                {"B", "bool"},
                {"D", "DateTime"}
            };

        #region 构造函数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public EntityGenerator(Dictionary<string, object> data)
        {
            _data = data ?? new Dictionary<string, object>();
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputFolder"></param>
        public void Execute(string outputFolder)
        {
            Execute(CommonRuntime.Root + @"Template\Entity.template", outputFolder);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFolder"></param>
        public void Execute(string inputFile, string outputFolder)
        {
            ExceptionHandler.FileNotFound(inputFile);
            ExceptionHandler.DirectoryNotFound(outputFolder);

            var tables = GetProperty<IDictionary<string, IList<Fields>>>("Tables");
            if (tables == null || tables.Count <= 0)
                return;
            if (!outputFolder.EndsWith(Path.DirectorySeparatorChar.ToString()))
                outputFolder += Path.DirectorySeparatorChar;

            var names = new StringBuilder();
            var fields = new StringBuilder();
            var clears = new StringBuilder();
            foreach (var name in tables.Keys)
            {
                fields.Clear();
                clears.Clear();
                foreach (var column in tables[name])
                {
                    names.AppendFormat("\"{0}\",", column.Name);
                    fields.AppendFormat("\n\t\t/// <summary>\n\t\t/// {0}\n\t\t/// </summary>", column.Title);
                    fields.AppendFormat("\n\t\tpublic {0} {1} {{ get; set; }}\n", _types[column.Type], column.Name);
                    clears.AppendFormat("\n\t\t\t{0} = default({1});", column.Name, _types[column.Type]);
                }

                var targets = new Dictionary<string, string>
                {
                    {"ProjectName", (GetProperty<string>("ProjectName") ?? "Tatan.Entities")},
                    {"Entity", name},
                    {"Names", names.Remove(names.Length - 1, 1).ToString()},
                    {"Fields", fields.ToString()},
                    {"Clear", clears.Remove(0, 4).ToString()}
                };

                WriteCSharpCode(inputFile, outputFolder, targets);
            }
        }

        private T GetProperty<T>(string key) where T : class
        {
            if (!_data.ContainsKey(key)) 
                return default(T);
            return _data[key] as T;
        }

        private static void WriteCSharpCode(string inPath, string outPath, IDictionary<string, string> targets)
        {
            var fileName = outPath + targets["Entity"] + ".cs";
            if (!File.Exists(fileName))
                File.Create(fileName).Close();
            using (var sw = new StreamWriter(fileName, false, Encoding.UTF8))
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