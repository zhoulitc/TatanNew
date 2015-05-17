using System;
using Tatan.Common;
using Tatan.Data;

namespace Tatan.Workflow
{
    /// <summary>
    /// 流程实例接口
    /// </summary>
    public interface IFlowInstance : IDataCreated, IDataModified, IDentifiable
    {
        /// <summary>
        /// 获取业务Id
        /// </summary>
        string BusinessId { get; }

        /// <summary>
        /// 获取当前流程接口
        /// </summary>
        IFlow Flow { get; }

        /// <summary>
        /// 获取当前轨迹接口
        /// </summary>
        ITrack Track { get; }

        /// <summary>
        /// 获取流程实例类型
        /// </summary>
        int Type { get; }

        /// <summary>
        /// 获取流程结束时间
        /// </summary>
        DateTime EndDate { get; }

        /// <summary>
        /// 获取或设置流程实例状态
        /// </summary>
        FlowInstanceState State { get; set; }

        /// <summary>
        /// 获取或设置字段值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object this[string key] { get; set; }

        /// <summary>
        /// 处理当前流程
        /// </summary>
        void Handle(string remark = null);
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ActivityInstanceState
    {
        /// <summary>
        /// 表示当前活动实例处于等待状态，接受状态的改变
        /// </summary>
        Wait,

        /// <summary>
        /// 表示当前活动实例处理完毕，不再可写
        /// </summary>
        Finish
    }

    /// <summary>
    /// 
    /// </summary>
    public enum FlowInstanceState
    {
        /// <summary>
        /// 表示当前流程已被处理完毕
        /// </summary>
        Finish,

        /// <summary>
        /// 表示当前流程处于运行状态
        /// </summary>
        Running
    }

    /// <summary>
    /// 任务状态
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// 驳回，流程返回上一节点
        /// </summary>
        Reject,

        /// <summary>
        /// 否决，流程返回起始节点
        /// </summary>
        Veto,

        /// <summary>
        /// 通过，流程进入下一节点
        /// </summary>
        Pass,

        /// <summary>
        /// 结束，流程终止
        /// </summary>
        End,

        /// <summary>
        /// 撤销，流程如果进入了下一节点，则回退到当前节点
        /// </summary>
        Undo
    }
}
