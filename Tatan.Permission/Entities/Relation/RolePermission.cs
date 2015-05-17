namespace Tatan.Permission.Entities.Relation
{
    /// <summary>
    /// 角色权限表，关联表
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    internal class RolePermission
    {
        /// <summary>
        /// 关联角色Id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 关联权限Id
        /// </summary>
        public int PermissionId { get; set; }
    }
}