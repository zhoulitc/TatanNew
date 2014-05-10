namespace Tatan.Web
{
    using System;

    /// <summary>
    /// Session会话接口
    /// </summary>
    public interface ISession
    {
        /// <summary>
        /// 取消当前Session
        /// </summary>
        void Abandon();

        void Clear();

        int Count { get; }

        /// <summary>
        /// 获取Session唯一标识符
        /// </summary>
        string ID { get; }

        /// <summary>
        /// 判断Session是否为刚创建
        /// </summary>
        bool IsNew { get; }

        object this[string key] { set; }

        int Timeout { get; set; }
    }
}