namespace Tatan.Permission.Entities.Relation
{
    /// <summary>
    /// 用户权限表，关联表
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    internal class UserPermission
    {
        /// <summary>
        /// 关联用户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 关联权限Id
        /// </summary>
        public int PermissionId { get; set; }
    }
}