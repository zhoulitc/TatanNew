namespace Tatan.Permission.Entities
{
    using System;
    using Collections;
    using Relation;

    /// <summary>
    /// 用户，用户的权限为自身权限和关联角色、关联组权限的集合
    /// <para>author:zhoulitcqq</para>
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
        /// 判断用户是否有权限
        /// </summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        public bool IsAuthentication(string permissionId) => Permissions.Contains(new Permission(permissionId));

        /// <summary>
        /// 用户包含的组关联集合
        /// </summary>
        public GroupRelationCollection Groups
            => _groups ?? (_groups = new GroupRelationCollection(this, nameof(UserGroup), nameof(UserGroup.UserId)));

        /// <summary>
        /// 用户包含的角色关联集合
        /// </summary>
        public RoleRelationCollection Roles
            => _roles ?? (_roles = new RoleRelationCollection(this, nameof(UserRole), nameof(UserRole.UserId)));

        /// <summary>
        /// 用户包含的权限关联集合
        /// </summary>
        public PermissionRelationCollection Permissions
            => _permissions ?? (_permissions = new PermissionRelationCollection(this, nameof(UserPermission), nameof(UserPermission.UserId)));
    }
}