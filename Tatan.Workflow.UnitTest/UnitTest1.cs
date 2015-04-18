using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tatan.Data;

namespace Tatan.Workflow.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var dataSource = DataSource.Connect("default");

            //初始化工作流引擎
            var flow = Flows.GetFlow("test");
            flow.Configuration.SetDatabase("");
            flow.BusinessTable = "test";
            flow.BusinessFields = new Dictionary<string, Type> {{"routeId", typeof (string)}};

            var apply = flow.NewActivity("Apply");
            var director = flow.NewActivity("Director");
            var l3 = flow.NewActivity("L3Approver");
            var l2 = flow.NewActivity("L2Approver");
            var boss = flow.NewActivity("Boss");
            var end = flow.NewActivity("End", true);

            apply
                .SetNext(director, entry => entry["routeId"].ToString() == "tod")
                .SetNext(apply, entry => entry["routeId"].ToString() == "toself1");
            director
                .SetNext(l3, entry => entry["routeId"].ToString() == "tol3")
                .SetNext(director, entry => entry["routeId"].ToString() == "toself2")
                .SetNext(apply, entry => entry["routeId"].ToString() == "tobegin1");
            l3
                .SetNext(l2, entry => entry["routeId"].ToString() == "tol2")
                .SetNext(l3, entry => entry["routeId"].ToString() == "toself3")
                .SetNext(apply, entry => entry["routeId"].ToString() == "tobegin2");
            l2
                .SetNext(boss, entry => entry["routeId"].ToString() == "toboss")
                .SetNext(l3, entry => entry["routeId"].ToString() == "toself4")
                .SetNext(apply, entry => entry["routeId"].ToString() == "tobegin3");
            boss
                .SetNext(boss, entry => entry["routeId"].ToString() == "toself5")
                .SetNext(apply, entry => entry["routeId"].ToString() == "tobegin4")
                .SetEnd(end, entry => entry["routeId"].ToString() == "toend");

            //run
            var instance = flow.NewInstance("Apply", "zhouli");
            instance["routeId"] = "tod";
            instance.Handle();

            var id = instance.Id;

            instance = flow.GetInstance(id);
            instance["routeId"] = "toself2";
            instance.Handle();

            instance = flow.GetInstance(id);
            instance["routeId"] = "tol3";
            instance.Handle();

            instance = flow.GetInstance(id);
            instance["routeId"] = "tol2";
            instance.Handle();

            instance = flow.GetInstance(id);
            instance["routeId"] = "toboss";
            instance.Handle();

            instance = flow.GetInstance(id);
            instance["routeId"] = "toend";
            instance.Handle();
        }
    }
}
