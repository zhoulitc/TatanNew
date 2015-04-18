namespace Tatan.Common.Configuration
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 集成信息接口
    /// </summary>
    public interface IIntegration
    {
        /// <summary>
        /// 集成名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 集成链接
        /// </summary>
        Uri Uri { get; }

        /// <summary>
        /// 集成认证
        /// </summary>
        ICertification Certification { get; }

        /// <summary>
        /// 集成扩展属性
        /// </summary>
        IDictionary<string, string> Properties { get; }
    }
}