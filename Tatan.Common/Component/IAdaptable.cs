namespace Tatan.Common.Component
{
    using System;

    /// <summary>
    /// 可适配接口
    /// <para>适用于静态适配对象</para>
    /// </summary>
    public interface IAdaptable
    {
    }

    /// <summary>
    /// 适配器接口
    /// <para>适用于实例适配对象，并提供释放方法</para>
    /// </summary>
    public interface IAdapter : IDisposable
    {
    }
}