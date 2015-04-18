using System;
using Tatan.Common;

namespace Tatan.Data.Attribute
{
    /// <summary>
    /// 字段特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited=false)]
    public class FieldAttribute : System.Attribute, INameable
    {
        /// <summary>
        /// 
        /// </summary>
        public FieldAttribute()
        {
            IsPrimaryKey = false;
            DefaultValue = null;
        }

        /// <summary>
        /// 字段的名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 字段是否为主键
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// 字段是否为只读
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// 字段是否属于枚举类型
        /// </summary>
        public bool IsEnum { get; set; }

        /// <summary>
        /// 字段的默认值
        /// </summary>
        public object DefaultValue { get; set; }
    }
}