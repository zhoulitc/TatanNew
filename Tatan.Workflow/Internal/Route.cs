using System;
using Tatan.Common.Exception;
using Guid = Tatan.Common.Guid;

namespace Tatan.Workflow.Internal
{
    internal class Route : IRoute
    {
        public Route(Activity from, Activity to, Predicate<IFlowInstance> expression = null)
        {
            Assert.ArgumentNotNull("from", from);
            Assert.ArgumentNotNull("to", to);

            From = from;
            To = to;
            Id = Guid.New();
            Expression = expression;
        }

        public Predicate<IFlowInstance> Expression { get; set; }

        public string Id { get; private set; }

        public IActivity From { get; private set; }

        public IActivity To { get; private set; }

        public bool Trigger(IFlowInstance entry)
        {
            if (entry == null || Expression == null)
                return true;
            return Expression(entry);
        }

        public override string ToString() => string.Format("{0}->{1}", From.Name, To.Name);
    }
}