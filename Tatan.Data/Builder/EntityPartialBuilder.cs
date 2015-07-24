namespace Tatan.Data.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Common.Exception;
    using Common.IO;
    using System.Reflection;
    using Tatan.Common.Extension.Reflect;


    /// <summary>
    /// 实体生成器
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public class EntityPartialBuilder : Builder
    {
        private readonly IEnumerable<Type> _types;
        private static readonly Type _interface = typeof(IDataEntity);

        #region 构造函数

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly">程序集</param>
        public EntityPartialBuilder(Assembly assembly)
        {
            Assert.ArgumentNotNull(nameof(assembly), assembly);
            _types = assembly.GetTypes().Where(t => _interface.IsAssignableFrom(t));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="types">类型集合</param>
        public EntityPartialBuilder(params Type[] types)
        {
            Assert.ArgumentNotNull(nameof(types), types);
            _types = types.Where(t => _interface.IsAssignableFrom(t));
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputFolder"></param>
        public override void Execute(string outputFolder) => Execute(Runtime.Root + @"Template\EntityPartial.template", outputFolder);

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
            
            foreach (var type in _types)
            {
                var clears = new StringBuilder();
                var properties = type.GetDeclaredOnlyProperties();
                foreach (var property in properties)
                {
                    if (property.CanWrite)
                        clears.AppendFormat("\n\t\t\t{0} = default({1});", property.Name, property.PropertyType.Name);
                }

                var targets = new Dictionary<string, string>
                {
                    {"ProjectName", type.Namespace},
                    {"Entity", type.Name},
                    {"Clear", clears.Length > 4 ? clears.Remove(0, 4).ToString() : string.Empty}
                };

                var outputFile = outputFolder + type.Name + ".cs";
                WriteFile(inputFile, outputFile, targets);
            }
        }
    }
}