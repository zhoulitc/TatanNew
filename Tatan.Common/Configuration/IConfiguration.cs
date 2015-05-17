namespace Tatan.Common.Configuration
{
    /// <summary>
    /// 配置文件对象
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// 获取某个配置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string this[string name] { get; }

        /// <summary>
        /// 获取某个配置
        /// </summary>
        /// <param name="section"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        string this[string section, string name] { get; }
    }
}