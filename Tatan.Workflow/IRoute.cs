using Tatan.Common;

namespace Tatan.Workflow
{
    /// <summary>
    /// 路由对象，决定活动的后继
    /// </summary>
    public interface IRoute : IDentifiable 
    {
        /// <summary>
        /// 从哪个活动
        /// </summary>
        IActivity From { get; }

        /// <summary>
        /// 到哪个活动
        /// </summary>
        IActivity To { get; }

        /// <summary>
        /// 触发，判断是否进行后续处理
        /// </summary>
        /// <param name="entry">触发的对象</param>
        /// <returns></returns>
        bool Trigger(IFlowInstance entry);
    }
}