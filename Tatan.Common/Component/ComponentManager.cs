namespace Tatan.Common.Component
{
    using System;
    using System.Collections.Generic;
    using Exception;

    /// <summary>
    /// 组件管理器
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public static class ComponentManager
    {
        private static readonly IList<IDisposable> _disposes = new List<IDisposable>();

        /// <summary>
        /// 注册一个可适配接口对象
        /// </summary>
        /// <param name="adaptable"></param>
        public static void Register(IAdaptable adaptable) => Assert.ArgumentNotNull(nameof(adaptable), adaptable);

        /// <summary>
        /// 注册一个适配器接口对象
        /// </summary>
        /// <param name="adapter"></param>
        public static void Register(IAdapter adapter)
        {
            Assert.ArgumentNotNull(nameof(adapter), adapter);

            _disposes.Add(adapter);
        }

        /// <summary>
        /// 销毁所有适配器接口对象
        /// </summary>
        public static void Dispose()
        {
            foreach (var dispose in _disposes)
            {
                dispose.Dispose();
            }
            _disposes.Clear();
        }
    }
}