namespace Tatan.Web.User
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 是否登录
        /// </summary>
        public bool IsLogin { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户Guid
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 登录序列，与口令有关
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 登录状态，与session有关
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("IsLogin={0}&Name={1}&Token={2}&State={3}",
                IsLogin, Name, Token, State);
        }
    }
}