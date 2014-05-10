namespace Tatan.Common
{
    /// <summary>
    /// 操作全局唯一标识符 (GUID)
    /// </summary>
    public static class Guid
    {
        /// <summary>
        /// 获取一个新的GUID
        /// </summary>
        /// <param name="format">格式化方式
        /// <para>格式可以为n、d、b、p、x</para>
        /// <para>n：默认，格式为xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx</para>
        /// <para>d：添加-，格式为xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx</para>
        /// <para>b：外围大括号，格式为{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}</para>
        /// <para>p：外围小括号，格式为(xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)</para>
        /// <para>x：不常用</para>
        /// </param>
        /// <exception cref="System.FormatException">非法格式化时</exception>
        /// <returns>字符串</returns>
        public static string New(string format = null)
        {
            if (string.IsNullOrEmpty(format))
                return System.Guid.NewGuid().ToString("n");
            return System.Guid.NewGuid().ToString(format);
        }
    }
}