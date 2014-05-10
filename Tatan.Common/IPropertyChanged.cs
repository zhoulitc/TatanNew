namespace Tatan.Common
{
    using System;

    /// <summary>
    /// 属性变更通知接口
    /// </summary>
    public interface IPropertyChanged
    {
        /// <summary>
        /// 当属性发生改变时的通知行为
        /// </summary>
        event Action<object, string> PropertyChanged;
    }
}