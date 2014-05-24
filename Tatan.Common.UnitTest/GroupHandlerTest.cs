using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
namespace Tatan.Common.UnitTest
{
    [TestClass]
    public class GroupHandlerTest
    {
        [TestMethod]
        public void TestGetSubGroupsById()
        {
            //指定一个正确guid
            var guidSuccess = System.Guid.Parse("");
            var groupsSuccess = GroupHandler.GetSubGroupsById(guidSuccess);

            //做出期望正确的断言
            Assert.IsNotNull(groupsSuccess);
            Assert.IsTrue(groupsSuccess.Length > 0);

            //指定一个错误的guid
            var guidFault = System.Guid.Parse("");
            var groupsFault = GroupHandler.GetSubGroupsById(guidSuccess);

            //做出期望错误的的断言
            Assert.IsNull(groupsFault);
            Assert.IsTrue(groupsFault.Length == 0);
        }

        [TestMethod]
        public void Test()
        {
            using (Microsoft.QualityTools.Testing.Fakes.ShimsContext.Create())
            {
                System.Web.Fakes.ShimHttpRequest request = new ShimHttpRequest();
                var ss = request.Instance.Params["guid"];
            }
        }
    }
}
