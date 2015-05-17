namespace Tatan.Data.Attribute
{
    using System;
    using Common;

    /// <summary>
    /// 字段特性
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited=false)]
    public class FieldAttribute : Attribute, INameable
    {
        /// <summary>
        /// 字段的名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 字段是否为主键
        /// </summary>
        public bool IsPrimaryKey { get; set; } = false;

        /// <summary>
        /// 字段是否为只读
        /// </summary>
        public bool IsReadOnly { get; set; } = false;

        /// <summary>
        /// 字段是否属于枚举类型
        /// </summary>
        public bool IsEnum { get; set; } = false;

        /// <summary>
        /// 字段的默认值
        /// </summary>
        public object DefaultValue { get; set; } = null;
    }
}