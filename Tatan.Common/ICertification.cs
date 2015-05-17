namespace Tatan.Common
{
    /// <summary>
    /// 证书接口
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public interface ICertification
    {
        /// <summary>
        /// 认证的域名
        /// </summary>
        string Domain { get; set; }

        /// <summary>
        /// 认证用户名
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// 证书用户密码
        /// </summary>
        string Password { get; set; }
    }
}