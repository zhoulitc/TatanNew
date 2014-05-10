namespace Tatan.Web
{
    using System;

    /// <summary>
    /// 服务器缓存接口
    /// </summary>
    public interface ICache
    {
        void Clear();

        int Count { get; }

        T Get<T>(string key);

        object this[string key] { set; }

        object this[string key, DateTime absoluteExpiration, TimeSpan slidingExpiration] { set; }
    }
}