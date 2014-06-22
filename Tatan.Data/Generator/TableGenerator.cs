namespace Tatan.Data.Generator
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Common.Exception;
    using Common.Extension.String.Target;
    using Common.IO;
    using Relation;
    using File = System.IO.File;

    /// <summary>
    /// sql建表生成器
    /// </summary>
    public abstract class TableGenerator : IGenerator
    {
        protected readonly IEnumerable<Tables> Tables;
        protected readonly IDataSource Source;

        #region 构造函数

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tables"></param>
        /// <param name="source"></param>
        protected TableGenerator(IEnumerable<Tables> tables, IDataSource source)
        {
            Tables = tables ?? new List<Tables>();
            Source = source;
        }
        #endregion

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
        public void Execute(string inputFile, string outputFolder)
        {
            ExceptionHandler.FileNotFound(inputFile);
            ExceptionHandler.DirectoryNotFound(outputFolder);
            ExceptionHandler.ArgumentNull("DataSource", Source);

            if (!outputFolder.EndsWith(Runtime.Separator))
                outputFolder += Runtime.Separator;

            foreach (var table in Tables)
            {
                var columns = new StringBuilder();
                foreach (var column in table.GetFields(Source))
                {
                    columns.AppendFormat("\n\t,[{0}] {1} {2} {3}", 
                        column.Name, GetType(column), GetNotNull(column), GetDefaultValue(column));
                }

                var targets = new Dictionary<string, string>
                {
                    {"Table", table.Name},
                    {"Columns", columns.Length > 0 ? columns.Remove(0, 2).ToString() : string.Empty}
                };

                WriteCSharpCode(inputFile, outputFolder, targets);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        protected virtual string GetType(Fields column)
        {
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        protected virtual string GetNotNull(Fields column)
        {
            return column.IsNotNull ? "NOT NULL" : string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        protected virtual string GetDefaultValue(Fields column)
        {
            if (column.DefaultValue == null)
                return string.Empty;
            if (column.Type == "S")
                return string.Format("DEFAULT '{0}'", column.DefaultValue);
            return string.Format("DEFAULT {0}", column.DefaultValue);
        }

        private static void WriteCSharpCode(string inPath, string outPath, IDictionary<string, string> targets)
        {
            var fileName = outPath + targets["Table"] + ".sql";
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