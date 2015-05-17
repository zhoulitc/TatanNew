﻿namespace Tatan.Data.Builder
{
    using System.Collections.Generic;
    using Common.IO;
    using Relation;

    /// <summary>
    /// Sqlite建表生成器
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public sealed class SqliteBuilder : TableBuilder
    {
        private readonly static Dictionary<string, string> _types = new Dictionary<string, string>(6)
            {
                {"I", "INTEGER"},
                {"L", "INTEGER"},
                {"N", "DECIMAL({0},{1})"},
                {"S", "VARCHAR({0})"},
                {"B", "BOOLEAN"},
                {"D", "DATETIME"}
            };

        #region 构造函数

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tables"></param>
        /// <param name="source"></param>
        public SqliteBuilder(IEnumerable<Tables> tables, IDataSource source)
            : base(tables, source)
        {
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputFolder"></param>
        public override void Execute(string outputFolder)
            => Execute(Runtime.Root + @"Template\Sqlite.template", outputFolder);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        protected override string GetType(Fields column)
        {
            if (column.Type == "S")
                return string.Format(_types[column.Type], column.Size);
            if (column.Type == "N")
                return string.Format(_types[column.Type], column.Size, column.Scale);
            return _types[column.Type];
        }
    }
}