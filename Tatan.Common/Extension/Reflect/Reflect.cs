using Tatan.Common.Exception;

namespace Tatan.Common.Extension.Reflect
{
    using System;
    using System.Reflection;

    /// <summary>
    /// 反射相关扩展类
    /// </summary>
    public static class Reflect
    {
        /// <summary>
        /// 创建一个委托
        /// </summary>
        /// <param name="method"></param>
        /// <param name="result"></param>
        /// <typeparam name="T"></typeparam>
        public static void CreateDelegate<T>(this MethodInfo method, out T result)
        {
            result = (T) (object) Delegate.CreateDelegate(typeof (T), method);
        }

        /// <summary>
        /// 获得一个类型的单例对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T GetSingleInstance<T>(this Type type) where T : class
        {
            var property = type.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public);
            if (property != null)
            {
                return property.GetValue(null) as T;
            }
            var method = type.GetMethod("GetInstance", BindingFlags.Static | BindingFlags.Public);
            if (method != null)
            {
                return method.Invoke(null, null) as T;
            }
            return null;
        }

        /// <summary>
        /// 调用当前类型的非继承公开方法
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="method"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static object InvokeDeclaredOnly(this object obj, string method, params object[] arguments)
        {
            Assert.ArgumentNotNull("method", method);
            Assert.ArgumentNotNull("arguments", arguments);

            var type = obj.GetType();
            var methodInfo = type.GetMethod(method,
                BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            if (methodInfo == null)
            {
                var property = type.GetProperty(method,
                    BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
                if (property == null)
                    return null;
                return property.GetValue(obj);
            }
            return methodInfo.Invoke(obj, arguments);
        }
    }
}