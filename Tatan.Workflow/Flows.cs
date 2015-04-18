using System;
using System.Collections.Generic;
using Tatan.Common.Exception;
using Tatan.Workflow.Internal;

namespace Tatan.Workflow
{
    /// <summary>
    /// 流程对象
    /// </summary>
    public static class Flows
    {
        private static readonly IDictionary<string, IFlow> _flows;

        /// <summary>
        /// </summary>
        static Flows()
        {
            _flows = new Dictionary<string, IFlow>();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static IFlow GetFlow(string name)
        {
            Assert.ArgumentNotNull("name", name);

            if (!_flows.ContainsKey(name))
            {
                _flows.Add(name, new Flow {Name = name});
            }
            return _flows[name];
        }
    }
}