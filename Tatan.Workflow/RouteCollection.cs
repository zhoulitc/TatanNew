using System.Collections.Generic;
using System.Text;
using Tatan.Common.Exception;

namespace Tatan.Workflow
{
    /// <summary>
    /// 路由集合
    /// </summary>
    public class RouteCollection
    {
        private readonly SortedDictionary<int, IRoute> _routes;

        internal RouteCollection()
        {
            _routes = new SortedDictionary<int, IRoute>();
        }

        /// <summary>
        /// 获取当前路由
        /// </summary>
        /// <param name="index"></param>
        public IRoute this[int index]
        {
            get
            {
                Assert.IndexInRange(index, _routes.Count);
                return _routes[index];
            }
        }

        internal void Set(IRoute route, int? index)
        {
            Assert.ArgumentNotNull("route", route);

            if (index.HasValue)
            {
                Assert.IndexInRange(index.Value, _routes.Count);
                _routes[index.Value] = route;
            }
            else
                _routes.Add(_routes.Count, route);
        }

        internal IActivity GetNext(IFlowInstance entry)
        {
            foreach (var route in _routes.Values)
            {
                if (route.Trigger(entry))
                {
                    return route.To;
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (_routes.Count <= 0)
                return string.Empty;
            var builder = new StringBuilder();
            foreach (var route in _routes)
            {
                builder.AppendFormat(",{0}:{1}", route.Key, route.Value);
            }
            return builder.Remove(0, 1).ToString();
        }
    }
}