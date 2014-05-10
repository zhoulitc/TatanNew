namespace Tatan.Permission.Entities.Relation
{
    /// <summary>
    /// 组角色表，关联表
    /// </summary>
    internal class GroupRole
    {
        /// <summary>
        /// 关联组Id
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// 关联角色Id
        /// </summary>
        public int RoleId { get; set; }
    }
}