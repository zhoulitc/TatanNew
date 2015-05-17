using System;

namespace Tatan.Common
{
    /// <summary>
    /// 可通知接口
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public interface INotifiable<out T>
    {
        /// <summary>
        /// 发送通知
        /// </summary>
        event Action<T> Notify;
    }

    /// <summary>
    /// 可通知接口
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public interface INotifiable
    {
        /// <summary>
        /// 发送通知
        /// </summary>
        event Action Notify;
    }
}