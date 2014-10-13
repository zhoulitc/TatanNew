namespace Tatan.Net
{
    using Common;

    /// <summary>
    /// Session会话接口
    /// </summary>
    public interface ISession : IDentifiable<string>, IClearable, ICountable
    {
        /// <summary>
        /// 取消当前Session
        /// </summary>
        void Abandon();

        /// <summary>
        /// 判断Session是否为刚创建
        /// </summary>
        bool IsNew { get; }

        /// <summary>
        /// 获取Session
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// 获取或设置Session
        /// </summary>
        /// <param name="key"></param>
        object this[string key] { set; }

        /// <summary>
        /// 获取或设置超时时间
        /// </summary>
        int Timeout { get; set; }
    }
}