using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using Tatan.Common.Configuration;
using Tatan.Common.Extension.Enum;

namespace Tatan.Net.Wcf
{
    /// <summary>
    /// 公共服务接口
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)] 
    public class Services : IServices
    {
        private static readonly IDictionary<string, Tuple> _tuples;
        private static readonly object _lock;

        private class Tuple
        {
            public Func<IAction> CreateInstance { get; set; }

            public IAction Action { get; set; }

            public InstanceContextMode Mode { get; private set; }

            public MethodInfo[] Methods { get; private set; }

            public Tuple(Type type)
            {
                Mode = GetMode(type);
                SetMethod(type);
            }

            private static InstanceContextMode GetMode(MemberInfo type)
            {
                var behavior = type.GetCustomAttribute<ServiceBehaviorAttribute>();
                if (behavior != null)
                {
                    return behavior.InstanceContextMode;
                }
                return InstanceContextMode.Single;
            }

            private void SetMethod(IReflect type)
            {
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                Methods = new MethodInfo[4];
                foreach (var method in methods)
                {
                    var action = method.GetCustomAttribute<ActionAttribute>();
                    if (action == null) continue;

                    Methods[action.Method.AsInt()] = method;
                }
            }
        }

        static Services()
        {
            _lock = new object();
            _tuples = new Dictionary<string, Tuple>();
            LoadConfig();
        }

        /// <summary>
        /// 初始载入程序集
        /// </summary>
        /// <param name="createFunction"></param>
        public static void Register<T>(Func<T> createFunction = null) where T : IAction, new()
        {
            var type = typeof (T);
            var className = typeof(T).FullName;

            lock (_lock)
            {
                if (!_tuples.ContainsKey(className))
                {
                    var tuple = new Tuple(type)
                    {
                        CreateInstance = GetFunction(createFunction, type),
                    };
                    if (tuple.Mode == InstanceContextMode.Single)
                    {
                        tuple.Action = tuple.CreateInstance();
                    }

                    _tuples.Add(className, tuple);
                }
            }
        }

        private static void LoadConfig()
        {
        }

        private static Func<IAction> GetFunction<T>(Func<T> createFunction, Type type) where T : IAction, new()
        {
            if (createFunction != null) return () => createFunction();

            var constructor = type.GetConstructor(Type.EmptyTypes);
            if (constructor == null)
                return () => null;
            return () => constructor.Invoke(null) as IAction;
        }

        private static ServiceResponse Invoke(HttpMethod method, ServiceRequest request)
        {
            if (request == null)
                return ServiceResponse.Exception("request is null.");
            if (string.IsNullOrEmpty(request.Service))
                return ServiceResponse.Exception("service is null.");
            if (string.IsNullOrEmpty(request.Action))
                return ServiceResponse.Exception("action is null.");
            if (!_tuples.ContainsKey(request.Service))
                return ServiceResponse.Exception("service is not found.");

            var tuple = _tuples[request.Service];
            var methodInfo = tuple.Methods[(int)method];
            if (methodInfo == null || !request.Action.Equals(methodInfo.Name))
                return ServiceResponse.Exception("action is not found.");
           
            try
            {
                var instance = tuple.Action;
                if (tuple.Mode == InstanceContextMode.PerCall)
                {
                    instance = tuple.CreateInstance();
                }
                if (instance == null)
                    return ServiceResponse.Exception("instance is null.");

                var result = methodInfo.Invoke(instance, request.Arguments);
                var response = result as ServiceResponse;
                return response ?? ServiceResponse.Success(result);
            }
            catch (Exception ex)
            {
                return ServiceResponse.Exception(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServiceResponse Query(ServiceRequest request)
        {
            return Invoke(HttpMethod.Get, request);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServiceResponse Add(ServiceRequest request)
        {
            return Invoke(HttpMethod.Post, request);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServiceResponse Delete(ServiceRequest request)
        {
            return Invoke(HttpMethod.Delete, request);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServiceResponse Modify(ServiceRequest request)
        {
            return Invoke(HttpMethod.Put, request);
        }
    }
}