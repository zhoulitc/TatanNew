using Tatan.Common;
using Tatan.Data.Interface;

namespace Tatan.Workflow
{
    /// <summary>
    /// 活动实例接口
    /// </summary>
    public interface IActivityInstance : IDataCreated, IDentifiable
    {
        /// <summary>
        /// 获取当前活动接口
        /// </summary>
        IActivity Activity { get; }

        /// <summary>
        /// 获取流程Id
        /// </summary>
        string FlowId { get; }

        /// <summary>
        /// 获取活动备注
        /// </summary>
        string Remark { get; }

        /// <summary>
        /// 获取或设置活动实例状态
        /// </summary>
        ActivityInstanceState State { get; set; }
    }
}