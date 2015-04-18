using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tatan.Workflow;
using Task = Tatan.Workflow.Flow;

namespace Tatan.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //初始化工作流引擎
            var task = new Flow();
            var apply = task.NewActivity("Apply");
            var director = task.NewActivity("Director");
            director["Handler"] = "zhouli4";

            var l3 = task.NewActivity("L3Approver");
            l3["Handlers"] = new[] { "zhouli3", "wangwu" };

            var l2 = task.NewActivity("L2Approver");
            l2["Handler"] = "zhouli2";

            var boss = task.NewActivity("Boss");
            boss["Handler"] = "zhouli1";

            task.NewEnd("End");

            apply.SetNext("tod", director).SetNext("toself1", apply);
            director.SetNext("tol3", l3).SetNext("toself2", director).SetNext("tobegin1", apply);
            l3.SetNext("tol2", l2).SetNext("toself3", l3).SetNext("tobegin2", apply);
            l2.SetNext("toboss", boss).SetNext("toself4", l3).SetNext("tobegin3", apply);
            boss.SetNext("toself4", boss).SetNext("tobegin4", apply).SetEnd("toend", "End");

            var instance = task.NewInstance("Apply");
            instance.ToNext("tod");

            var id = instance.Id;

            instance = task.GetInstance(id);
            instance.ToNext("toself2");

            instance = task.GetInstance(id);
            instance.ToNext("tol3");

            instance = task.GetInstance(id);
            instance.ToNext("tol2");

            instance = task.GetInstance(id);
            instance.ToNext("toboss");

            instance = task.GetInstance(id);
            instance.ToNext("toend");
        }
    }
}
