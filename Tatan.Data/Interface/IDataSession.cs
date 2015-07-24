﻿// ReSharper disable once CheckNamespace
namespace Tatan.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Common;

    /// <summary>
    /// 数据会话接口
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public interface IDataSession : IDentifiable<string>
    {
        #region 数据处理
        /// <summary>
        /// 获取一个数据实体集
        /// <para>泛型必须继承至实体接口</para>
        /// <para>泛型必须带无参数的构造函数</para>
        /// </summary>
        /// <param name="request">请求串，向数据源请求数据，例如SQL、存储过程等</param>
        /// <param name="action">参数设置行为</param>
        /// <returns>泛型列表</returns>
        IReadOnlyList<T> GetEntities<T>(string request, Action<IDataParameters> action = null) 
            where T : class, IDataEntity;

        /// <summary>
        /// 获取一个数据实体集（异步版本）
        /// <para>泛型必须继承至实体接口</para>
        /// <para>泛型必须带无参数的构造函数</para>
        /// </summary>
        /// <param name="request">请求串，向数据源请求数据，例如SQL、存储过程等</param>
        /// <param name="action">参数设置行为</param>
        /// <returns>泛型列表</returns>
        Task<IReadOnlyList<T>> GetEntitiesAsync<T>(string request, Action<IDataParameters> action = null) 
            where T : class, IDataEntity;

        /// <summary>
        /// 执行请求获取一个数据标量
        /// </summary>
        /// <param name="request">请求串，向数据源请求数据，例如SQL、存储过程等</param>
        /// <param name="action">参数设置行为</param>
        /// <returns>唯一标量</returns>
        T ExecuteScalar<T>(string request, Action<IDataParameters> action = null);

        /// <summary>
        /// 执行请求获取一个数据标量（异步版本）
        /// </summary>
        /// <param name="request">请求串，向数据源请求数据，例如SQL、存储过程等</param>
        /// <param name="action">参数设置行为</param>
        /// <returns>唯一标量</returns>
        Task<T> ExecuteScalarAsync<T>(string request, Action<IDataParameters> action = null);

        /// <summary>
        /// 执行请求并处理返回的数据集
        /// </summary>
        /// <param name="request">请求串，向数据源请求数据，例如SQL、存储过程等</param>
        /// <param name="function">操作返回的结果集，并返回结果集加工后的数据</param>
        /// <returns>泛型列表</returns>
        T ExecuteReader<T>(string request, Func<IDataReader, T> function);

        /// <summary>
        /// 执行请求并处理返回的数据集
        /// </summary>
        /// <param name="request">请求串，向数据源请求数据，例如SQL、存储过程等</param>
        /// <param name="function">操作返回的结果集，并返回结果集加工后的数据</param>
        /// <returns>泛型列表</returns>
        Task<T> ExecuteReaderAsync<T>(string request, Func<IDataReader, Task<T>> function);

        /// <summary>
        /// 执行请求并处理返回的数据集
        /// </summary>
        /// <param name="request">请求串，向数据源请求数据，例如SQL、存储过程等</param>
        /// <param name="action">参数设置行为</param>
        /// <param name="function">操作返回的结果集，并返回结果集加工后的数据</param>
        /// <returns>泛型列表</returns>
        T ExecuteReader<T>(string request, Action<IDataParameters> action, Func<IDataReader, T> function);

        /// <summary>
        /// 执行请求并处理返回的数据集
        /// </summary>
        /// <param name="request">请求串，向数据源请求数据，例如SQL、存储过程等</param>
        /// <param name="action">参数设置行为</param>
        /// <param name="function">操作返回的结果集，并返回结果集加工后的数据</param>
        /// <returns>泛型列表</returns>
        Task<T> ExecuteReaderAsync<T>(string request, Action<IDataParameters> action, Func<IDataReader, Task<T>> function);

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command">命令，可以是SQL语句，也可以是存储过程名</param>
        /// <param name="action"></param>
        /// <returns>返回操作行数</returns>
        int Execute(string command, Action<IDataParameters> action = null);

        /// <summary>
        /// 执行命令（异步版本）
        /// </summary>
        /// <param name="command">命令，可以是SQL语句，也可以是存储过程名</param>
        /// <param name="action"></param>
        /// <returns>返回操作行数</returns>
        Task<int> ExecuteAsync(string command, Action<IDataParameters> action = null);
        #endregion

        #region 事务处理
        /// <summary>
        /// 开始事务处理，NOSQL待定
        /// </summary>
        /// <param name="lockLevel">事务锁定级别，默认与当前数据库的默认值一致</param>
        /// <returns>事务处理接口</returns>
        IDbTransaction BeginTransaction(IsolationLevel lockLevel = IsolationLevel.Unspecified);
        #endregion

        /// <summary>
        /// 设置命令的超时时间
        /// </summary>
        int Timeout { set; }
    }
}