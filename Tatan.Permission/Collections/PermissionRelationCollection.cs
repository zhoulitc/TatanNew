using Tatan.Common.Collections;

namespace Tatan.Permission.Collections
{
    using Common;
    using Entities;

    /// <summary>
    /// 权限集合
    /// </summary>
    public sealed class PermissionRelationCollection : AbstractRelationCollection<Permission>
    {
        internal PermissionRelationCollection(IDentifiable identity, string tableName, string thatName)
            : base(identity, tableName, thatName, "PermissionId")
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relation"></param>
        /// <returns></returns>
        public override bool Contains(Permission relation)
        {
            if (base.Contains(relation))
                return true;
            if (Identity is Role) //角色的权限，角色的权限只为它本身
                return false;
            if (Identity is Group) //组的权限，组的权限还包含组关联的角色权限
                return GroupRoleContains(relation);
            if (Identity is User) //用户的权限，用户的权限还包含关联的角色权限以及
                return UserGroupRoleContains(relation);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override Permission GetById(long id)
        {
            var permission = base.GetById(id);
            if (permission != null) 
                return permission;
            if (Identity is Role) //角色的权限，角色的权限只为它本身
                return null;
            if (Identity is Group) //组的权限，组的权限还包含组关联的角色权限
                return GetByGroupRoleId(id);
            if (Identity is User) //用户的权限，用户的权限还包含关联的角色权限以及
                return GetByUserGroupRoleId(id);
            return null;
        }

        private static readonly ListMap<string, string> _sqlsExtension;

        static PermissionRelationCollection()
        {
            _sqlsExtension = new ListMap<string, string>(4)
            {
                {"UsersGroupsRolesContains", "SELECT COUNT(1) FROM UserGroup AS t1 INNER JOIN GroupPermission AS t2 ON t1.GroupId=t2.GroupId INNER JOIN UserRole AS t3 ON t1.UserId=t3.UserId INNER JOIN RolePermission AS t4 ON t3.RoleId=t4.RoleId WHERE t1.UserId={0}UserId AND (t2.PermissionId={0}PermissionId OR t4.PermissionId={0}PermissionId)"},
                {"GroupsRolesContains", "SELECT COUNT(1) FROM GroupRole AS t1 LEFT JOIN RolePermission AS t2 ON t1.RoleId=t2.RoleId WHERE t1.GroupId={0}GroupId AND t2.PermissionId={0}PermissionId"},
                
                {"UsersGroupsRolesGetById", "SELECT * FROM Permission WHERE Id=(SELECT t2.PermissionId FROM UserRole AS t1 LEFT JOIN RolePermission AS t2 ON t1.RoleId=t2.RoleId WHERE t1.UserId={0}UserId AND t2.PermissionId={0}PermissionId) UNION SELECT * FROM Permission WHERE Id=(SELECT t2.PermissionId FROM UserGroup AS t1 LEFT JOIN GroupPermission AS t2 ON t1.GroupId=t2.GroupId WHERE t1.UserId={0}UserId AND t2.PermissionId={0}PermissionId)"},
                {"GroupsRolesGetById", "SELECT * FROM Permission WHERE Id=(SELECT t2.PermissionId FROM GroupRole AS t1 LEFT JOIN RolePermission AS t2 ON t1.RoleId=t2.RoleId WHERE t1.GroupId={0}GroupId AND t2.PermissionId={0}PermissionId)"}
            };
        }

        private bool GroupRoleContains(Permission relation)
        {
            var sql = string.Format(_sqlsExtension["GroupsRolesContains"], Source.Provider.ParameterSymbol);
            return Source.UseSession(TableName, session => session.ExecuteScalar<long>(sql, parameters =>
            {
                parameters[ThisName] = relation.Id;
                parameters[ThatName] = Identity.Id;
            }) > 0);
        }

        private bool UserGroupRoleContains(Permission relation)
        {
            var sql = string.Format(_sqlsExtension["UsersGroupsRolesContains"], Source.Provider.ParameterSymbol);
            return (Source.UseSession(TableName, session => session.ExecuteScalar<long>(sql, parameters =>
            {
                parameters[ThisName] = relation.Id;
                parameters[ThatName] = Identity.Id;
            }) > 0));
        }

        private Permission GetByUserGroupRoleId(long id)
        {
            var sql = string.Format(_sqlsExtension["UsersGroupsRolesGetById"], Source.Provider.ParameterSymbol);
            return Source.UseSession(TableName, session =>
            {
                var entities = session.GetEntities<Permission>(sql, parameters =>
                {
                    parameters[ThisName] = id;
                    parameters[ThatName] = Identity.Id;
                });
                if (entities == null || entities.Count <= 0)
                    return null;
                return entities[0];
            });
        }

        private Permission GetByGroupRoleId(long id)
        {
            var sql = string.Format(_sqlsExtension["GroupsRolesGetById"], Source.Provider.ParameterSymbol);
            return Source.UseSession(TableName, session =>
            {
                var entities = session.GetEntities<Permission>(sql, parameters =>
                {
                    parameters[ThisName] = id;
                    parameters[ThatName] = Identity.Id;
                });
                if (entities == null || entities.Count <= 0)
                    return null;
                return entities[0];
            });
        }
    }
}