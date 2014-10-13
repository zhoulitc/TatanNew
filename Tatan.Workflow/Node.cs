using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tatan.Common;

namespace Tatan.Workflow
{
    /// <summary>
    /// 流程节点
    /// </summary>
    public class Node
    {
        /// <summary>
        /// 节点名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        public Node(string name)
        {
            Name = name;
        }

        public RuleCollection Rules { get; set; }

        public override string ToString()
        {
            return Name.ToString();
        }
    }

    /// <summary>
    /// 流程流转
    /// </summary>
    public class Route
    {
        /// <summary>
        /// 路由名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public Route(Node from, Node to)
        {
            From = from;
            To = to;
            Name = string.Format("{0}_{1}", From.Name, To.Name);
        }

        /// <summary>
        /// 从哪个节点
        /// </summary>
        public Node From { get; set; }

        /// <summary>
        /// 到哪个节点
        /// </summary>
        public Node To { get; set; }

        public override string ToString()
        {
            return Name.ToString();
        }
    }

    /// <summary>
    /// 规则
    /// </summary>
    public class Rule
    {
        /// <summary>
        /// 条件
        /// </summary>
        public Predicate<Node> Condition { get;set;}

        /// <summary>
        /// 成功的后继节点
        /// </summary>
        public Node Next { get;set;}
    }

    public class RuleCollection : IEnumerable<Rule>
    {
        public Rule this[string name]
        {
            get
            {
                return null;
            }
            set
            {
                
            }
        }

        public IEnumerator<Rule> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class RouteCollection
    {
        public Route this[string name]
        {
            get
            {
                return null;
            }
            set
            {
                
            }
        }
    }

    public class NodeCollection
    {
        public Node this[string name]
        {
            get
            {
                return null;
            }
            set
            {

            }
        }
    }

    /// <summary>
    /// 工作流的模式
    /// </summary>
    public enum Mode
    {
        /// <summary>
        /// 顺序模式
        /// </summary>
        Sequential,

        /// <summary>
        /// 状态机模式
        /// </summary>
        StateMachine
    }

    /// <summary>
    /// 节点状态
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// 驳回，流程返回上一节点
        /// </summary>
        Overrule,

        /// <summary>
        /// 否决，流程返回起始节点
        /// </summary>
        Veto,

        /// <summary>
        /// 通过，流程进入下一节点
        /// </summary>
        Pass,

        /// <summary>
        /// 结束，流程终止
        /// </summary>
        End,

        /// <summary>
        /// 撤销，流程如果进入了下一节点，则回退到当前节点
        /// </summary>
        Undo
    }

    public class Demo
    {
        public void Do()
        {
            //start=>node3=>node2=>node1
            Flow flow = new Flow();
            flow.Begin("start")
                .To("L3approver", (n => n.Equals(null)))
                .To("L2approver")
                .To("L1approver").End("Null");

            flow.Begin("L3approver").To("L1approver", n => n.Name == "1").End();
        }
    }

    public class FlowInstance
    {
        public string Begin { get; set; }
        public string Current { get; set; }
        public Status Status { get; set; }
    }

    public class Flow
    {
        public string Name { get; set; }
        public NodeCollection Nodes { get; set; }
        public RouteCollection Routes { get; set; }
        public Flow To(string nodeName, Predicate<Node> predicate)
        {
            return this;
        }

        public Flow To(string nodeName)
        {
            return this;
        }

        public Flow Begin(string nodeName)
        {
            return this;
        }

        public void End(string nodeName = null)
        {

        }

        /// <summary>
        /// 继续流程
        /// </summary>
        /// <param name="id">标识符</param>
        public string Continue(string id)
        {
            var instance = Revert(id);
            Node begin = Nodes[instance.Current];
            foreach (var rule in begin.Rules)
            {
                if (rule.Condition(begin))
                {
                    instance.Current = rule.Next.Name;
                    instance.Status = Status.Pass;
                    return Persistence(instance);
                }
            }
            //instance.Status = begin.Rules.Count == 0 ? Status.End : Status.Veto;
            return Persistence(instance);
        }

        /// <summary>
        /// 恢复流程现场
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private FlowInstance Revert(string id)
        {
            return null;
        }

        /// <summary>
        /// 运行一个流程实例
        /// </summary>
        /// <param name="beginName"></param>
        public string Run(string beginName)
        {
            Node begin = Nodes[beginName];
            var instance = new FlowInstance {Begin = beginName};
            foreach (var rule in begin.Rules)
            {
                if (rule.Condition(begin))
                {
                    instance.Current = rule.Next.Name;
                    instance.Status = Status.Pass;
                    return Persistence(instance);
                }
            }
            instance.Status = Status.Veto;
            return Persistence(instance);
        }

        private string Persistence(FlowInstance instance)
        {
            return string.Empty;
        }

        //private void Run(Rule r)
        //{
        //    Node next = r.Next;
        //    foreach (var rule in next.Rules)
        //    {
        //        if (rule.Condition(next))
        //        {
        //            Run(rule);
        //            if (rule.Status == Status.Pass)
        //                break;
        //        }
        //    }
        //}
    }
}
