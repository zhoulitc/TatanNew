namespace Tatan.Web.User
{
    /// <summary>
    /// 用户信息
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 是否登录
        /// </summary>
        public bool IsLogin { get; set; } = false;

        /// <summary>
        /// 用户Id
        /// </summary>
        public long Id { get; set; }

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
            return string.Format("{5}={0}&{6}={1}&{7}={2}&{8}={3}&{9}={4}",
                Id.ToString(), IsLogin, Name, Token, State,
                nameof(Id), nameof(IsLogin), nameof(Name), nameof(Token), nameof(State));
        }
    }
}