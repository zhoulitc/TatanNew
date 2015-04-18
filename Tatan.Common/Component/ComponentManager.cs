namespace Tatan.Common.Component
{
    using System;
    using Exception;

    /// <summary>
    /// 组件管理器
    /// </summary>
    public static class ComponentManager
    {
        /// <summary>
        /// 注册一个可适配接口对象
        /// </summary>
        /// <param name="adaptable"></param>
        public static void Register(IAdaptable adaptable)
        {
            Assert.ArgumentNotNull("adaptable", adaptable);
        }

        /// <summary>
        /// 注册一个适配器接口对象
        /// </summary>
        /// <param name="adapter"></param>
        public static void Register(IAdapter adapter)
        {
            Assert.ArgumentNotNull("adapter", adapter);

            Disposing += adapter.Dispose;
        }

        /// <summary>
        /// 销毁所有适配器接口对象
        /// </summary>
        public static void Dispose()
        {
            if (Disposing != null)
                Disposing();
        }

        private static event Action Disposing;
    }
}