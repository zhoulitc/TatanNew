using System;
using System.Collections.Generic;
using Tatan.Common.Exception;
using Tatan.Data.Attribute;
using Guid = Tatan.Common.Guid;

namespace Tatan.Workflow.Internal
{
    [Table(Name = "FlowInstance")]
    internal class FlowInstance : IFlowInstance
    {
        private readonly Flow _flow;
        private readonly IDictionary<string, object> _properties;
        private readonly Track _track;

        public FlowInstance(Flow flow, Activity begin, string creator, string remark = null)
        {
            Assert.ArgumentNotNull("flow", flow);
            Assert.ArgumentNotNull("begin", begin);

            _properties = new Dictionary<string, object>();
            _flow = flow;
            Id = Guid.New();
            BusinessId = Guid.New(); //TODO 业务编号生成器
            Type = _flow.Type;
            State = FlowInstanceState.Running;
            Creator = string.IsNullOrEmpty(creator) ? "System" : creator;
            CreatedTime = DateTime.Now;
            Version = 0;
            if (flow.BusinessFields != null)
            {
                foreach (string key in flow.BusinessFields.Keys)
                {
                    _properties.Add(key, null);
                }
            }
            _track = new Track(begin, this, remark);
        }

        public IFlow Flow => _flow;

        public ITrack Track => _track;

        public object this[string key]
        {
            get
            {
                Assert.ArgumentNotNull("key", key);
                Assert.KeyFound(_properties, key);
                return _properties[key];
            }
            set
            {
                Assert.ArgumentNotNull("key", key);
                Assert.KeyFound(_properties, key);
                Assert.ArgumentNotNull("value", value);
                if (Flow.BusinessFields != null)
                {
                    Type type = value.GetType();
                    if (!type.IsAssignableFrom(Flow.BusinessFields[key]))
                        Assert.TypeError(type, Flow.BusinessFields[key]);
                }
                _properties[key] = value;
            }
        }

        

        public void Handle(string remark = null)
        {
            _flow.OnHandlerBefore(this);

            if (State == FlowInstanceState.Finish)
            {
                throw new Exception("the flow state is finish.");
            }
            if (Track.Current.State == ActivityInstanceState.Finish)
            {
                throw new Exception("current activity state is finish.");
            }

            IActivity activity = Track.Current.Activity.Routes.GetNext(this);
            if (activity == null)
            {
                throw new Exception("not found route.");
            }

            _track.Add(activity, remark);
            if (activity.IsEnd)
            {
                EndDate = DateTime.Now;
                Track.Current.State = ActivityInstanceState.Finish;
                State = FlowInstanceState.Finish;
            }

            //执行配置处理
            _flow.HandleExtension(this);

            //执行通知处理
            _flow.OnNotify(this);

            _flow.OnHandlerAfter(this);
        }

        [Field(Name = "Id")]
        public string Id { get; private set; }

        [Field(Name = "BusinessId")]
        public string BusinessId { get; private set; }

        [Field(Name = "EndDate")]
        public DateTime EndDate { get; private set; }

        [Field(Name = "State", IsEnum = true)]
        public FlowInstanceState State { get; set; }

        [Field(Name = "Type")]
        public int Type { get; private set; }

        [Field(Name = "Creator")]
        public string Creator { get; private set; }

        [Field(Name = "CreatedTime")]
        public DateTime CreatedTime { get; private set; }

        [Field(Name = "Modifier")]
        public string Modifier { get; set; }

        [Field(Name = "ModifiedTime")]
        public DateTime ModifiedTime { get; set; }

        [Field(Name = "Version")]
        public uint Version { get; set; }

        public override string ToString() => _track.ToString();
    }
}