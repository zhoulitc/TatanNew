namespace Tatan.Data.Builder
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Common.Exception;
    using Common.Extension.String.Template;
    using Common.IO;
    using Relation;

    /// <summary>
    /// 实体生成器
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public class EntityBuilder : Builder
    {
        private readonly IEnumerable<Tables> _tables;
        private readonly IDataSource _source;
        private readonly string _projectName;
        private readonly static Dictionary<string, string> _types = new Dictionary<string, string>(6)
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
        public EntityBuilder(IEnumerable<Tables> tables, IDataSource source, string projectName = null)
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
        public override void Execute(string outputFolder) => Execute(Runtime.Root + @"Template\Entity.template", outputFolder);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFolder"></param>
        public override void Execute(string inputFile, string outputFolder)
        {
            Assert.FileFound(inputFile);
            Assert.DirectoryFound(outputFolder);
            Assert.ArgumentNotNull(nameof(_source), _source);

            if (!outputFolder.EndsWith(Runtime.Separator))
                outputFolder += Runtime.Separator;

            foreach (var table in _tables)
            {
                var fields = new StringBuilder();
                var clears = new StringBuilder();
                foreach (var column in table.GetFields(_source))
                {
                    fields.AppendFormat("\n\t\t/// <summary>\n\t\t/// {0}\n\t\t/// </summary>", column.Title);
                    fields.AppendFormat("\n\t\tpublic {0} {1} {{ get; set; }}\n", _types[column.Type], column.Name);
                    clears.AppendFormat("\n\t\t\t{0} = default({1});", column.Name, _types[column.Type]);
                }

                var targets = new Dictionary<string, string>
                {
                    {"ProjectName", _projectName},
                    {"Entity", table.Name},
                    {"Fields", fields.ToString()},
                    {"Clear", clears.Length > 4 ? clears.Remove(0, 4).ToString() : string.Empty}
                };

                var outputFile = outputFolder + table.Name + ".cs";
                WriteFile(inputFile, outputFile, targets);
            }
        }
    }
}