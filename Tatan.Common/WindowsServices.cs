using System;
using System.ServiceProcess;
using Tatan.Common.Exception;

namespace Tatan.Common
{
    /// <summary>
    /// 操作Windows服务
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public static class WindowsServices
    {
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="name">服务名称</param>
        /// <returns></returns>
        public static void StartService(string name)
        {
            Assert.ArgumentNotNull(nameof(name), name);
            using (var controller = new ServiceController(name))
            {
                if (controller.Status == ServiceControllerStatus.Stopped || 
                    controller.Status == ServiceControllerStatus.StopPending)
                {
                    controller.Start();
                    controller.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 10));
                }
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="name">服务名称</param>
        /// <returns></returns>
        public static void StopService(string name)
        {
            Assert.ArgumentNotNull(nameof(name), name);
            using (var controller = new ServiceController(name))
            {
                if (controller.Status == ServiceControllerStatus.Running ||
                    controller.Status == ServiceControllerStatus.StartPending)
                {
                    controller.Stop();
                    controller.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 10));
                }
            }
        }
    }
}