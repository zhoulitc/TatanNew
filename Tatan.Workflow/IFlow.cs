using System;
using System.Collections.Generic;
using Tatan.Common;

namespace Tatan.Workflow
{
    /// <summary>
    /// 流程接口，决定了一个工作流程的全部
    /// </summary>
    public interface IFlow : INameable, INotifiable<IFlowInstance>
    {
        /// <summary>
        /// 获取或设置流程绑定的业务名称
        /// </summary>
        string BusinessTable { get; set; }

        /// <summary>
        /// 获取或设置流程绑定的业务架构
        /// </summary>
        IDictionary<string, Type> BusinessFields { get; set; }

        /// <summary>
        /// 获取或设置流程类型
        /// </summary>
        int Type { get; set; }

        /// <summary>
        /// 获取流程配置
        /// </summary>
        IFlowPersistence Configuration { get; }

        /// <summary>
        /// 处理当前流程之前的行为
        /// </summary>
        event Action<IFlowInstance> HandlerBefore;

        /// <summary>
        /// 处理当前流程之后的行为
        /// </summary>
        event Action<IFlowInstance> HandlerAfter;

        /// <summary>
        /// 新建一个属于当前流程的活动
        /// </summary>
        /// <param name="name">活动名</param>
        /// <param name="isEnd">是否为终结活动</param>
        /// <returns></returns>
        IActivity NewActivity(string name, bool isEnd = false);

        /// <summary>
        /// 新建一个流程实例，需要指定一个起始活动
        /// </summary>
        /// <param name="activity">活动名</param>
        /// <param name="creator">创建者</param>
        /// <returns></returns>
        IFlowInstance NewInstance(string activity, string creator);

        /// <summary>
        /// 获取一个流程实例
        /// </summary>
        /// <param name="flowId">流程标识</param>
        /// <returns></returns>
        IFlowInstance GetInstance(string flowId);
    }
}