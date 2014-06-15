namespace Tatan.Data.Relation
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Common.Collections;
    using Common.Extension.String.Target;
    using Common.Exception;
    using CommonRuntime = Common.IO.Runtime;

    /// <summary>
    /// 实体生成器
    /// </summary>
    public class EntityGenerator : IGenerator
    {
        private readonly IEnumerable<Tables> _tables;
        private readonly IDataSource _source;
        private readonly string _projectName;
        private readonly static ListMap<string, string> _types = new ListMap<string, string>(6)
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
        /// <param name="tables"></param>
        /// <param name="projectName"></param>
        /// <param name="source"></param>
        public EntityGenerator(IEnumerable<Tables> tables, string projectName = null, IDataSource source = null)
        {
            _tables = tables ?? new List<Tables>();
            _projectName = projectName ?? "Tatan.Entities";
            _source = source;
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

            if (_source == null)
                return;
            if (!outputFolder.EndsWith(CommonRuntime.Separator))
                outputFolder += CommonRuntime.Separator;

            foreach (var table in _tables)
            {
                var names = new StringBuilder();
                var fields = new StringBuilder();
                var clears = new StringBuilder();
                foreach (var column in table.GetFields(_source))
                {
                    names.AppendFormat("\"{0}\",", column.Name);
                    fields.AppendFormat("\n\t\t/// <summary>\n\t\t/// {0}\n\t\t/// </summary>", column.Title);
                    fields.AppendFormat("\n\t\tpublic {0} {1} {{ get; set; }}\n", _types[column.Type], column.Name);
                    clears.AppendFormat("\n\t\t\t{0} = default({1});", column.Name, _types[column.Type]);
                }

                var targets = new Dictionary<string, string>
                {
                    {"ProjectName", _projectName},
                    {"Entity", table.Name},
                    {"Names", names.Length > 0 ? names.Remove(names.Length - 1, 1).ToString() : string.Empty},
                    {"Fields", fields.ToString()},
                    {"Clear", clears.Length > 4 ? clears.Remove(0, 4).ToString() : string.Empty}
                };

                WriteCSharpCode(inputFile, outputFolder, targets);
            }
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