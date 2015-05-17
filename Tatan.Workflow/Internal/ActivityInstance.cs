using System;
using Tatan.Common.Exception;
using Tatan.Data.Attribute;
using Guid = Tatan.Common.Guid;

namespace Tatan.Workflow.Internal
{
    [Table(Name = "ActivityInstance")]
    internal class ActivityInstance : IActivityInstance
    {
        public ActivityInstance(Activity activity, IFlowInstance instance, string remark)
        {
            Assert.ArgumentNotNull("activity", activity);
            Assert.ArgumentNotNull("instance", instance);
            Activity = activity;

            State = ActivityInstanceState.Wait;
            Id = Guid.New();
            FlowId = instance.Id;
            Creator = instance.Creator;
            CreatedTime = DateTime.Now;
            Remark = remark ?? string.Empty;
        }

        public IActivity Activity { get; private set; }

        [Field(Name = "Remark", IsReadOnly = true)]
        public string Remark { get; private set; }

        [Field(Name = "Id", IsPrimaryKey = true)]
        public string Id { get; private set; }

        [Field(Name = "FlowId", IsReadOnly = true)]
        public string FlowId { get; private set; }

        [Field(Name = "State", IsEnum = true)]
        public ActivityInstanceState State { get; set; }

        [Field(Name = "Creator", IsReadOnly = true)]
        public string Creator { get; private set; }

        [Field(Name = "CreatedTime", IsReadOnly = true)]
        public DateTime CreatedTime { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}({1})", Activity.Name, State.ToString());
        }
    }
}