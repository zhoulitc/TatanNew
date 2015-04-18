using System;
using Tatan.Common;

namespace Tatan.Workflow
{
    /// <summary>
    /// 活动接口，表示工作流中的一个节点
    /// </summary>
    public interface IActivity : INameable
    {
        /// <summary>
        /// 获取活动的所属流程
        /// </summary>
        IFlow Flow { get; }

        /// <summary>
        /// 获取当前活动的可到达的所有路径
        /// </summary>
        RouteCollection Routes { get; set; }

        /// <summary>
        /// 判断活动是否为流程终结活动
        /// </summary>
        /// <returns></returns>
        bool IsEnd { get; }

        /// <summary>
        /// 设置下一个活动
        /// </summary>
        /// <param name="activityName">将指定活动设为后继活动</param>
        /// <param name="expression">当表达式满足时，则返回其后继活动</param>
        /// <param name="index">表达式执行的优先级</param>
        IActivity SetNext(string activityName, Predicate<IFlowInstance> expression = null, int? index = null);

        /// <summary>
        /// 设置下一个活动
        /// </summary>
        /// <param name="activity">将指定活动设为后继活动</param>
        /// <param name="expression">当表达式满足时，则返回其后继活动</param>
        /// <param name="index">表达式执行的优先级</param>
        IActivity SetNext(IActivity activity, Predicate<IFlowInstance> expression = null, int? index = null);

        /// <summary>
        /// 设置结束活动
        /// </summary>
        /// <param name="activityName">将指定活动设为终结活动</param>
        /// <param name="expression">当表达式满足时，则返回其后继活动</param>
        /// <param name="index">表达式执行的优先级</param>
        void SetEnd(string activityName, Predicate<IFlowInstance> expression = null, int? index = null);

        /// <summary>
        /// 设置结束活动
        /// </summary>
        /// <param name="activity">将指定活动设为终结活动</param>
        /// <param name="expression">当表达式满足时，则返回其后继活动</param>
        /// <param name="index">表达式执行的优先级</param>
        void SetEnd(IActivity activity, Predicate<IFlowInstance> expression = null, int? index = null);

        /// <summary>
        /// 创建一个活动实例
        /// </summary>
        /// <param name="instance">活动实例所属的流程实例</param>
        /// <param name="remark">活动备注</param>
        /// <returns></returns>
        IActivityInstance NewInstance(IFlowInstance instance, string remark = null);
    }
}