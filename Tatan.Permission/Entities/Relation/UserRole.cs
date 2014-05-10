namespace Tatan.Permission.Entities.Relation
{
    /// <summary>
    /// 用户角色表，关联表
    /// </summary>
    internal class UserRole
    {
        /// <summary>
        /// 关联用户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 关联角色Id
        /// </summary>
        public int RoleId { get; set; }
    }
}