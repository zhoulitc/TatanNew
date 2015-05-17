using System;
using System.Collections.Generic;
using Tatan.Common.Exception;

namespace Tatan.Workflow.Internal
{
    internal class Activity : IActivity
    {
        private readonly Flow _flow;
        private readonly HashSet<string> _names;

        public Activity(Flow flow)
        {
            Assert.ArgumentNotNull("flow", flow);

            _flow = flow;
            _names = new HashSet<string>();
            Routes = new RouteCollection();
        }

        public IFlow Flow => _flow;

        public string Name { get; set; }

        public RouteCollection Routes { get; set; }

        public bool IsEnd { get; set; }

        public IActivity SetNext(string activityName, Predicate<IFlowInstance> expression = null, int? index = null)
        {
            Assert.ArgumentNotNull("activityName", activityName);

            if (_names.Contains(activityName))
            {
                throw new Exception("route exists.");
            }
            _names.Add(activityName);

            if (!_flow.Activities.ContainsKey(activityName))
            {
                _flow.Activities.Add(activityName, new Activity(_flow));
            }
            Activity activity = _flow.Activities[activityName];

            Routes.Set(new Route(this, activity, expression), index);
            return this;
        }

        public IActivity SetNext(IActivity activity, Predicate<IFlowInstance> expression = null, int? index = null)
        {
            Assert.ArgumentNotNull("activity", activity);
            if (activity.Flow.Name != Flow.Name)
            {
                throw new Exception("flow is not only one");
            }

            return SetNext(activity.Name, expression, index);
        }

        public void SetEnd(string end, Predicate<IFlowInstance> expression = null, int? index = null)
        {
            Assert.ArgumentNotNull("end", end);
            Activity activity = _flow.Activities[end];
            if (!activity.IsEnd)
            {
                throw new Exception();
            }
            Routes.Set(new Route(this, activity, expression), index);
        }

        public void SetEnd(IActivity end, Predicate<IFlowInstance> expression = null, int? index = null)
        {
            Assert.ArgumentNotNull("end", end);
            if (end.Flow.Name != Flow.Name)
            {
                throw new Exception("flow is not only one");
            }

            SetEnd(end.Name, expression, index);
        }

        public IActivityInstance NewInstance(IFlowInstance instance, string remark = null)
            => new ActivityInstance(this, instance, remark);

        public override string ToString() => Name;
    }
}