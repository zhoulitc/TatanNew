namespace Tatan.Permission.Entities.Relation
{
    /// <summary>
    /// 组权限表，关联表
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    internal class GroupPermission
    {
        /// <summary>
        /// 关联组Id
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// 关联权限Id
        /// </summary>
        public int PermissionId { get; set; }
    }
}