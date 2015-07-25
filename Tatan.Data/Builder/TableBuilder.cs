namespace Tatan.Data.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Common.Exception;
    using Common.IO;
    using System.Reflection;
    using Common.Extension.Reflect;
    using Attribute;




    /// <summary>
    /// sql建表生成器
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public abstract class TableBuilder : Builder
    {
        /// <summary>
        /// 数据表
        /// </summary>
        protected readonly IEnumerable<Type> Types;
        private static readonly Type _interface = typeof(DataEntity);
        private static readonly Type _stringType = typeof(string);

        #region 构造函数

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly">程序集</param>
        public TableBuilder(Assembly assembly)
        {
            Assert.ArgumentNotNull(nameof(assembly), assembly);
            Types = assembly.GetTypes().Where(t => _interface.IsAssignableFrom(t) && t != _interface);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="types">类型集合</param>
        public TableBuilder(params Type[] types)
        {
            Assert.ArgumentNotNull(nameof(types), types);
            Types = types.Where(t => _interface.IsAssignableFrom(t) && t != _interface);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFolder"></param>
        public override void Execute(string inputFile, string outputFolder)
        {
            Assert.FileFound(inputFile);
            Assert.DirectoryFound(outputFolder);

            if (!outputFolder.EndsWith(Runtime.Separator))
                outputFolder += Runtime.Separator;

            foreach (var type in Types)
            {
                var columns = new StringBuilder();
                var properties = type.GetDeclaredOnlyProperties();
                foreach (var property in properties)
                {
                    var attribute = property.GetCustomAttribute<FieldAttribute>();
                    if (attribute == null) continue;
                    columns.AppendFormat("\r\n\t,{0}", GetFieldInfo(attribute, property));
                }

                var targets = new Dictionary<string, string>
                {
                    {"Table", type.Name},
                    {"Columns", columns.Length > 0 ? columns.Remove(0, 3).ToString() : string.Empty}
                };

                var outputFile = outputFolder + targets["Table"] + ".sql";
                WriteFile(inputFile, outputFile, targets);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        protected abstract string GetFieldInfo(FieldAttribute field, PropertyInfo property);
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual string GetNotNull(FieldAttribute field) 
            => (field != null && field.IsNotNull) ? "NOT NULL" : string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual string GetDefaultValue(FieldAttribute field, Type type)
        {
            if (field == null || field.DefaultValue == null)
                return string.Empty;
            if (type == _stringType)
                return string.Format("DEFAULT '{0}'", field.DefaultValue);
            return string.Format("DEFAULT {0}", field.DefaultValue);
        }

        ///// <summary>
        ///// Mysql Comment
        ///// </summary>
        ///// <param name="field"></param>
        ///// <returns></returns>
        //protected virtual string GetDescription(FieldAttribute field)
        //{
        //    if (string.IsNullOrEmpty(field.Description))
        //        return string.Empty;
        //    return string.Format("COMMENT '{0}'", field.Description);
        //}
    }
}