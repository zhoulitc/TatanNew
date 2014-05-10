namespace Tatan.Web
{
    using System;

    /// <summary>
    /// Cookies接口
    /// </summary>
    public interface ICookies
    {
        void Clear();

        int Count { get; }

        string this[string key] { get; set; }

        void SetExpires(string key, double expires);
    }
}