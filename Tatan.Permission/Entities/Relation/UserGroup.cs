namespace Tatan.Permission.Entities.Relation
{
    /// <summary>
    /// 用户组表，关联表
    /// </summary>
    internal class UserGroup
    {
        /// <summary>
        /// 关联用户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 关联组Id
        /// </summary>
        public int GroupId { get; set; }
    }
}