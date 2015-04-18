using System;
using Tatan.Common;

namespace Tatan.Data.Attribute
{
    /// <summary>
    /// 字段特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public class TableAttribute : System.Attribute, INameable
    {
        /// <summary>
        /// 表的名称
        /// </summary>
        public string Name { get; set; }
    }
}