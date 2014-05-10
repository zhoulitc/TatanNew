namespace Tatan.Permission.Entities
{
    using Collections;

    /// <summary>
    /// 角色，角色的权限为自身的权限
    /// </summary>
    public partial class Role
    {
        private GroupRelationCollection _groups;
        private UserRelationCollection _users;
        private PermissionRelationCollection _permissions;

        /// <summary>
        /// 角色包含的用户关联集合
        /// </summary>
        public UserRelationCollection Users
        {
            get { return _users ?? (_users = new UserRelationCollection(this, "RoleId", "UserRole")); }
        }

        /// <summary>
        /// 角色包含的组关联集合
        /// </summary>
        public GroupRelationCollection Groups
        {
            get { return _groups ?? (_groups = new GroupRelationCollection(this, "RoleId", "GroupRole")); }
        }

        /// <summary>
        /// 角色包含的权限关联集合
        /// </summary>
        public PermissionRelationCollection Permissions
        {
            get { return _permissions ?? (_permissions = new PermissionRelationCollection(this, "RoleId", "RolePermission")); }
        }
    }
}