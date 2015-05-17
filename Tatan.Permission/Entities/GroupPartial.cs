namespace Tatan.Permission.Entities
{
    using System;
    using Collections;

    /// <summary>
    /// 组，组的权限为自身权限和关联角色权限的集合
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public partial class Group
    {
        [NonSerialized]
        private UserRelationCollection _users;

        [NonSerialized]
        private RoleRelationCollection _roles;

        [NonSerialized]
        private PermissionRelationCollection _permissions;

        /// <summary>
        /// 组包含的用户关联集合
        /// </summary>
        public UserRelationCollection Users
        {
            get { return _users ?? (_users = new UserRelationCollection(this, "UserGroup", "GroupId")); }
        }

        /// <summary>
        /// 组包含的角色关联集合
        /// </summary>
        public RoleRelationCollection Roles
        {
            get { return _roles ?? (_roles = new RoleRelationCollection(this, "GroupRole", "GroupId")); }
        }

        /// <summary>
        /// 组包含的权限集合
        /// </summary>
        public PermissionRelationCollection Permissions
        {
            get { return _permissions ?? (_permissions = new PermissionRelationCollection(this, "GroupPermission", "GroupId")); }
        }
    }
}