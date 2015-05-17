namespace Tatan.Data.Attribute
{
    using System;
    using Common;

    /// <summary>
    /// 字段特性
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public class TableAttribute : Attribute, INameable
    {
        /// <summary>
        /// 表的名称
        /// </summary>
        public string Name { get; set; }
    }
}