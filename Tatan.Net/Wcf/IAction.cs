using System;

namespace Tatan.Net.Wcf
{
    /// <summary>
    /// 服务行为接口，所有的服务都必须继承此接口
    /// </summary>
    public interface IAction
    {
    }

    /// <summary>
    /// 服务行为特性，标注在IAction接口下，哪些公开的方法可以被服务所调用
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]            
    public class ActionAttribute : Attribute
    {
        /// <summary>
        /// HTTP请求
        /// </summary>
        public HttpMethod Method { get; set; }
    }

    /// <summary>
    /// HTTP请求
    /// </summary>
    public enum HttpMethod
    {
        /// <summary>
        /// 表示一个删除动作
        /// </summary>
        Delete = 0,

        /// <summary>
        /// 表示一个获取动作
        /// </summary>
        Get = 1,

        /// <summary>
        /// 表示一个提交动作
        /// </summary>
        Post = 2,

        /// <summary>
        /// 表示一个修改动作
        /// </summary>
        Put = 3
    }
}