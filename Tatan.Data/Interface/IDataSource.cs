// ReSharper disable once CheckNamespace
namespace Tatan.Data
{
    using System;

    /// <summary>
    /// 数据源
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public interface IDataSource : IDisposable
    {
        /// <summary>
        /// 数据提供者
        /// </summary>
        IDataProvider Provider { get; }

        /// <summary>
        /// 获取表集合
        /// </summary>
        DataTableCollection Tables { get; }

        /// <summary>
        /// 使用库的session
        /// </summary>
        /// <param name="function"></param>
        /// <exception cref="System.ArgumentNullException">参数为空时抛出</exception>
        /// <returns>想要的结果</returns>
        T UseSession<T>(Func<IDataSession, T> function);

        /// <summary>
        /// 使用库的session
        /// </summary>
        /// <param name="identity">session标识符</param>
        /// <param name="function"></param>
        /// <exception cref="System.ArgumentNullException">参数为空时抛出</exception>
        /// <returns>想要的结果</returns>
        T UseSession<T>(string identity, Func<IDataSession, T> function);
    }
}