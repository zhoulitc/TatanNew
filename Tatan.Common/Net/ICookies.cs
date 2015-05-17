namespace Tatan.Common.Net
{
    /// <summary>
    /// Cookies接口
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public interface ICookies : IClearable, ICountable
    {
        /// <summary>
        /// 获取或设置Cookies
        /// </summary>
        /// <param name="key"></param>
        string this[string key] { get; set; }

        /// <summary>
        /// 设置Cookies过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expires"></param>
        void SetExpires(string key, double expires);
    }
}