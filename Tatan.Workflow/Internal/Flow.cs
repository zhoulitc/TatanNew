using System;
using System.Collections.Generic;
using Tatan.Common.Exception;

namespace Tatan.Workflow.Internal
{
    internal class Flow : IFlow
    {
        private readonly FlowPersistence _persistence;
        private readonly IDictionary<string, IFlowInstance> _instances;

        public Flow()
        {
            Activities = new Dictionary<string, Activity>();
            _instances = new Dictionary<string, IFlowInstance>();
            _persistence = new FlowPersistence(this);
        }

        public string BusinessKey { get; set; }
        internal IDictionary<string, Activity> Activities { get; private set; }
        public string BusinessTable { get; set; }
        public IDictionary<string, Type> BusinessFields { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }

        public IFlowPersistence Configuration
        {
            get { return _persistence; }
        }


        public event Action<IFlowInstance> HandlerBefore;

        public event Action<IFlowInstance> Notify;

        public event Action<IFlowInstance> HandlerAfter;

        internal void OnHandlerBefore(IFlowInstance instance)
        {
            if (HandlerBefore != null)
                HandlerBefore(instance);
        }

        internal void OnNotify(IFlowInstance instance)
        {
            if (Notify != null)
                Notify(instance);
        }

        internal void OnHandlerAfter(IFlowInstance instance)
        {
            if (HandlerAfter != null)
                HandlerAfter(instance);
        }

        public IActivity NewActivity(string name, bool isEnd = false)
        {
            Assert.ArgumentNotNull("name", name);

            var activity = new Activity(this) {Name = name, IsEnd = isEnd};
            Activities[activity.Name] = activity;
            return activity;
        }

        public IFlowInstance NewInstance(string activity, string creator)
        {
            Assert.ArgumentNotNull("activity", activity);
            Assert.KeyFound(Activities, activity);

            Activity begin = Activities[activity];
            var instance = new FlowInstance(this, begin, creator);
            _instances[instance.Id] = instance;
            return instance;
        }

        public IFlowInstance GetInstance(string flowId)
        {
            Assert.ArgumentNotNull("flowId", flowId);
            Assert.KeyFound(_instances, flowId);
            return _instances[flowId];
        }

        internal void HandleExtension(IFlowInstance instance)
        {
            if (_persistence.IsPersistence)
            {
                _persistence.ToDataBase(instance);
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}