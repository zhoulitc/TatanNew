namespace Tatan.Permission.Entities
{
    using System;
    using Collections;

    /// <summary>
    /// 用户，用户的权限为自身权限和关联角色、关联组权限的集合
    /// </summary>
    public partial class User
    {
        [NonSerialized]
        private GroupRelationCollection _groups;

        [NonSerialized]
        private RoleRelationCollection _roles;

        [NonSerialized]
        private PermissionRelationCollection _permissions;

        /// <summary>
        /// 用户包含的组关联集合
        /// </summary>
        public GroupRelationCollection Groups
        {
            get { return _groups ?? (_groups = new GroupRelationCollection(this, "UserGroup", "UserId")); }
        }

        /// <summary>
        /// 用户包含的角色关联集合
        /// </summary>
        public RoleRelationCollection Roles
        {
            get { return _roles ?? (_roles = new RoleRelationCollection(this, "UserRole", "UserId")); }
        }

        /// <summary>
        /// 用户包含的权限关联集合
        /// </summary>
        public PermissionRelationCollection Permissions
        {
            get { return _permissions ?? (_permissions = new PermissionRelationCollection(this, "UserPermission", "UserId")); }
        }
    }
}