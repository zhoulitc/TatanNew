namespace Tatan.Workflow
{
    /// <summary>
    /// 流程轨迹
    /// </summary>
    public interface ITrack
    {
        /// <summary>
        /// 获取当前活动实例
        /// </summary>
        IActivityInstance Current { get; }
    }
}
