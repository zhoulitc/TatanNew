using System;
using System.Collections.Generic;
using System.Linq;
using Tatan.Common.Exception;

namespace Tatan.Workflow.Internal
{
    internal class Track : ITrack
    {
        private readonly IList<IActivityInstance> _activities;
        private readonly IFlowInstance _flowInstance;

        public Track(IActivity begin, IFlowInstance instance, string remark = null)
        {
            Assert.ArgumentNotNull("instance", instance);
            Assert.ArgumentNotNull("begin", begin);

            _flowInstance = instance;
            _activities = new List<IActivityInstance>();
            Add(begin, remark);
        }

        public IActivityInstance Current
        {
            get
            {
                if (_activities.Count <= 0)
                    throw new Exception("activities is empty.");
                return _activities[_activities.Count - 1];
            }
        }

        internal void Add(IActivity activity, string remark = null)
        {
            Assert.ArgumentNotNull("activity", activity);

            if (_activities.Count > 0)
            {
                Current.State = ActivityInstanceState.Finish;
            }
            _activities.Add(activity.NewInstance(_flowInstance, remark));
        }

        public override string ToString()
        {
            string name = _activities.Aggregate(string.Empty,
                (current, activity) => current + (activity.ToString() + "->"));
            return name.Substring(0, name.Length - 2);
        }
    }
}