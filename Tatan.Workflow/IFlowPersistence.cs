using System;
using Tatan.Data;

namespace Tatan.Workflow
{
    /// <summary>
    /// 流程持久化接口，决定流程的持久化行为
    /// </summary>
    public interface IFlowPersistence
    {
        /// <summary>
        /// 配置流程可保存到数据库中
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="connectionString"></param>
        void SetDatabase(string providerName, string connectionString);

        /// <summary>
        /// 配置流程可保存到数据库中
        /// </summary>
        /// <param name="configName"></param>
        void SetDatabase(string configName);

        /// <summary>
        /// 处理流程
        /// </summary>
        event Action<IDataSession, IFlowInstance> HandlerFlow;

        /// <summary>
        /// 处理活动
        /// </summary>
        event Action<IDataSession, IActivityInstance> HandlerActivity;

        /// <summary>
        /// 处理业务数据
        /// </summary>
        event Action<IDataSession, IFlowInstance> HandlerBusiness;
    }
}