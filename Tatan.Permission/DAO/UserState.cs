namespace Tatan.Data.DAO
{
    /// <summary>
    /// 用户状态信息
    /// </summary>
    public struct UserState
    {
        /// <summary>
        /// 是否登录
        /// </summary>
        public bool IsLogin;

        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserName;

        /// <summary>
        /// 登录序列，与口令有关
        /// </summary>
        public string LoginOrder;

        /// <summary>
        /// 登录状态，与session有关
        /// </summary>
        public string LoginState;
    }
}