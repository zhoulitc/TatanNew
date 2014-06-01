namespace Tatan.Common.Configuration
{
    /// <summary>
    /// 配置文件对象
    /// </summary>
    public interface IConfig<T>
    {
        /// <summary>
        /// 获取或设置某个配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        T this[string key] { get; }
    }

    /// <summary>
    /// 配置文件对象
    /// </summary>
    public interface IConfig : IConfig<string>
    {
    }
}