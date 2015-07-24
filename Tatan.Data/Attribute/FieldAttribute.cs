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
        /// 字段长度/精度
        /// </summary>
        public uint Size { get; set; } = 0;

        /// <summary>
        /// 字段小数位数
        /// </summary>
        public uint Scale { get; set; } = 0;

        /// <summary>
        /// 字段是否属于枚举类型
        /// </summary>
        public bool IsEnum { get; set; } = false;

        /// <summary>
        /// 字段是否不为空
        /// </summary>
        public bool IsNotNull { get; set; } = false;

        /// <summary>
        /// 字段的默认值
        /// </summary>
        public object DefaultValue { get; set; } = null;

        /// <summary>
        /// 字段的说明
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}