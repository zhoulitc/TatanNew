namespace Tatan.Data.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Common.IO;
    using Relation;
    using Tatan.Data.Attribute;


    /// <summary>
    /// Sqlite建表生成器
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public sealed class SqliteBuilder : TableBuilder
    {
        private readonly static Dictionary<string, string> _types = new Dictionary<string, string>(17)
            {
                {"int", "INTEGER"},
                {"long", "INTEGER"},
                {"sbyte", "INTEGER"},
                {"short", "INTEGER"},
                {"double", "DECIMAL({0},{1})"},
                {"float", "DECIMAL({0},{1})"},
                {"decimal", "DECIMAL({0},{1})"},
                {"string", "VARCHAR({0})"},
                {"bool", "BOOLEAN"},
                {"datetime", "DATETIME"}
            };

        private readonly IDataSource _source;

        #region 构造函数

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="source"></param>
        public SqliteBuilder(IDataSource source, Assembly assembly)
            : base(assembly)
        {
            _source = source;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="types"></param>
        /// <param name="source"></param>
        public SqliteBuilder(IDataSource source, params Type[] types)
            : base(types)
        {
            _source = source;
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
        /// <param name="field"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        protected override string GetFieldInfo(FieldAttribute field, PropertyInfo property)
        {
            var name = field == null ? property.Name : field.Name;
            var type = GetType(field, property);

            return string.Format("[{0}] {1} {2} {3}", 
                name, type, GetNotNull(field), GetDefaultValue(field, property.PropertyType));
        }

        private string GetType(FieldAttribute field, PropertyInfo property)
        {
            if (property.PropertyType.Name.ToLower() == "string")
                return string.Format(_types[property.PropertyType.Name.ToLower()], 
                    field != null ? field.Size : 4000);
            if (property.PropertyType.Name.ToLower() == "double")
                return string.Format(_types[property.PropertyType.Name.ToLower()], 
                    field != null ? field.Size : 4000, field != null ? field.Scale : 4000);
            return _types[property.PropertyType.Name.ToLower()];
        }
    }
}