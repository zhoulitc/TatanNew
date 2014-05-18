namespace Tatan.Permission.Collections
{
    using System.Collections.Generic;
    using Common;
    using Entities;

    /// <summary>
    /// 权限集合
    /// </summary>
    public sealed class PermissionRelationCollection : AbstractRelationCollection<Permission>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="fieldName"></param>
        /// <param name="tableName"></param>
        internal PermissionRelationCollection(IDentifiable identity, string fieldName, string tableName)
            : base(identity, fieldName, tableName, "PermissionId")
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
                return UserGroupContains(relation) || UserRoleContains(relation);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override Permission GetById(int id)
        {
            var permission = base.GetById(id);
            if (permission != null) 
                return permission;
            if (Identity is Role) //角色的权限，角色的权限只为它本身
                return null;
            if (Identity is Group) //组的权限，组的权限还包含组关联的角色权限
                return GetByGroupRoleId(id);
            if (Identity is User) //用户的权限，用户的权限还包含关联的角色权限以及
                return GetByUserGroup(id) ?? GetByUserRoleId(id);
            return null;
        }

        private static readonly Dictionary<string, string> _sqlsExtension;

        static PermissionRelationCollection()
        {
            _sqlsExtension = new Dictionary<string, string>(6)
            {
                {"UsersGroupsContains", "SELECT COUNT(1) FROM UserGroup AS t1 LEFT JOIN GroupPermission AS t2 ON t1.GroupId=t2.GroupId WHERE t1.UserId={0}UserId AND t2.PermissionId={1}PermissionId"},
                {"UsersRolesContains", "SELECT COUNT(1) FROM UsersRole AS t1 LEFT JOIN RolePermission AS t2 ON t1.RoleId=t2.RoleId WHERE t1.UserId={0}UserId AND t2.PermissionId={1}PermissionId"},
                {"GroupsRolesContains", "SELECT COUNT(1) FROM GroupRole AS t1 LEFT JOIN RolePermission AS t2 ON t1.RoleId=t2.RoleId WHERE t1.GroupId={0}GroupId AND t2.PermissionId={1}PermissionId"},
                
                {"UsersGroupsGetById", "SELECT * FROM User WHERE Id=(SELECT * FROM UserGroup AS t1 LEFT JOIN GroupPermission AS t2 ON t1.GroupId=t2.GroupId WHERE t1.UserId={0}UserId AND t2.PermissionId={1}PermissionId)"},
                {"UsersRolesGetById", "SELECT * FROM User WHERE Id=(SELECT * FROM UsersRole AS t1 LEFT JOIN RolePermission AS t2 ON t1.RoleId=t2.RoleId WHERE t1.UserId={0}UserId AND t2.PermissionId={1}PermissionId)"},
                {"GroupsRolesGetById", "SELECT * FROM Group WHERE Id=(SELECT * FROM GroupRole AS t1 LEFT JOIN RolePermission AS t2 ON t1.RoleId=t2.RoleId WHERE t1.GroupId={0}GroupId AND t2.PermissionId={1}PermissionId)"}
            };
        }

        private bool GroupRoleContains(Permission relation)
        {
            var sql = string.Format(_sqlsExtension["GroupsRolesContains"],
                Source.Provider.ParameterSymbol, Source.Provider.ParameterSymbol);
            return Source.UseSession(TableName, session => session.GetScalar<int>(sql, parameters =>
            {
                parameters[ThisName] = relation.Id;
                parameters[FieldName] = Identity.Id;
            }) == 1);
        }

        private bool UserRoleContains(Permission relation)
        {
            var sqlUserRole = string.Format(_sqlsExtension["UsersRolesContains"],
                Source.Provider.ParameterSymbol, Source.Provider.ParameterSymbol);
            return (Source.UseSession(TableName, session => session.GetScalar<int>(sqlUserRole, parameters =>
            {
                parameters[ThisName] = relation.Id;
                parameters[FieldName] = Identity.Id;
            }) == 1));
        }

        private bool UserGroupContains(Permission relation)
        {
            var sqlUserGroup = string.Format(_sqlsExtension["UsersGroupsContains"],
                Source.Provider.ParameterSymbol, Source.Provider.ParameterSymbol);
            return Source.UseSession(TableName, session => session.GetScalar<int>(sqlUserGroup, parameters =>
            {
                parameters[ThisName] = relation.Id;
                parameters[FieldName] = Identity.Id;
            }) == 1);
        }

        private Permission GetByUserRoleId(int id)
        {
            var sqlUserRole = string.Format(_sqlsExtension["UsersRolesGetById"],
                Source.Provider.ParameterSymbol, Source.Provider.ParameterSymbol);
            return Source.UseSession(TableName, session =>
            {
                var entities = session.GetEntities<Permission>(sqlUserRole, parameters =>
                {
                    parameters[ThisName] = id;
                    parameters[FieldName] = Identity.Id;
                });
                if (entities == null || entities.Count != 1)
                    return null;
                return entities[0];
            });
        }

        private Permission GetByUserGroup(int id)
        {
            var sqlUserGroup = string.Format(_sqlsExtension["UsersGroupsGetById"],
                Source.Provider.ParameterSymbol, Source.Provider.ParameterSymbol);
            return Source.UseSession(TableName, session =>
            {
                var entities = session.GetEntities<Permission>(sqlUserGroup, parameters =>
                {
                    parameters[ThisName] = id;
                    parameters[FieldName] = Identity.Id;
                });
                if (entities == null || entities.Count != 1)
                    return null;
                return entities[0];
            });
        }

        private Permission GetByGroupRoleId(int id)
        {
            var sql = string.Format(_sqlsExtension["GroupsRolesGetById"],
                Source.Provider.ParameterSymbol, Source.Provider.ParameterSymbol);
            return Source.UseSession(TableName, session =>
            {
                var entities = session.GetEntities<Permission>(sql, parameters =>
                {
                    parameters[ThisName] = id;
                    parameters[FieldName] = Identity.Id;
                });
                if (entities == null || entities.Count != 1)
                    return null;
                return entities[0];
            });
        }
    }
}