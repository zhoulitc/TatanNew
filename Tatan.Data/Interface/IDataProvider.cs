﻿// ReSharper disable once CheckNamespace
namespace Tatan.Data
{
    /// <summary>
    /// 数据提供者
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// 供应者名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// 参数符号
        /// </summary>
        string ParameterSymbol { get; }

        /// <summary>
        /// 字符串连接符号
        /// </summary>
        string StringSplicingSymbol { get; }

        /// <summary>
        /// 模糊匹配符号
        /// </summary>
        string FuzzyMatchingSymbol { get; }

        /// <summary>
        /// 调用存储过程命令
        /// </summary>
        string CallStoredProcedure { get; }

        /// <summary>
        /// 名称左符号
        /// </summary>
        string LeftSymbol { get; }

        /// <summary>
        /// 名称右符号
        /// </summary>
        string RightSymbol { get; }
    }
}