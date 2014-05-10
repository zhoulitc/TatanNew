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
        private static readonly Dictionary<string, string> _sqlsExtension;

        static PermissionRelationCollection()
        {
            _sqlsExtension = new Dictionary<string, string>(6)
            {
                {"UsersGroupsConstains", "SELECT COUNT(1) FROM UserGroup AS t1 LEFT JOIN GroupPermission AS t2 ON t1.GroupId=t2.GroupId WHERE t1.UserId={0}UserId AND t2.PermissionId={1}PermissionId"},
                {"UsersRolesConstains", "SELECT COUNT(1) FROM UsersRole AS t1 LEFT JOIN RolePermission AS t2 ON t1.RoleId=t2.RoleId WHERE t1.UserId={0}UserId AND t2.PermissionId={1}PermissionId"},
                {"GroupsRolesConstains", "SELECT COUNT(1) FROM GroupRole AS t1 LEFT JOIN RolePermission AS t2 ON t1.RoleId=t2.RoleId WHERE t1.GroupId={0}GroupId AND t2.PermissionId={1}PermissionId"},
                
                {"UsersGroupsGetById", "SELECT * FROM User WHERE Id=(SELECT * FROM UserGroup AS t1 LEFT JOIN GroupPermission AS t2 ON t1.GroupId=t2.GroupId WHERE t1.UserId={0}UserId AND t2.PermissionId={1}PermissionId)"},
                {"UsersRolesGetById", "SELECT * FROM User WHERE Id=(SELECT * FROM UsersRole AS t1 LEFT JOIN RolePermission AS t2 ON t1.RoleId=t2.RoleId WHERE t1.UserId={0}UserId AND t2.PermissionId={1}PermissionId)"},
                {"GroupsRolesGetById", "SELECT * FROM Group WHERE Id=(SELECT * FROM GroupRole AS t1 LEFT JOIN RolePermission AS t2 ON t1.RoleId=t2.RoleId WHERE t1.GroupId={0}GroupId AND t2.PermissionId={1}PermissionId)"}
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="fieldName"></param>
        /// <param name="tableName"></param>
        internal PermissionRelationCollection(IDentityObject identity, string fieldName, string tableName)
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
            var group = Identity as Group;
            if (group != null) //组的权限，组的权限还包含组关联的角色权限
            {
                var sql = string.Format(_sqlsExtension["GroupsRolesConstains"], 
                    Source.Provider.ParameterSymbol, Source.Provider.ParameterSymbol);
                return Source.UseSession(TableName, session => session.GetScalar<int>(sql, parameters =>
                {
                    parameters[ThisName] = relation.Id;
                    parameters[FieldName] = Identity.Id;
                }) == 1);
            }
            var user = Identity as User;
            if (user != null) //用户的权限，用户的权限还包含关联的角色权限以及
            {
                var sqlUserGroup = string.Format(_sqlsExtension["UsersGroupsConstains"],
                    Source.Provider.ParameterSymbol, Source.Provider.ParameterSymbol);
                if (Source.UseSession(TableName, session => session.GetScalar<int>(sqlUserGroup, parameters =>
                {
                    parameters[ThisName] = relation.Id;
                    parameters[FieldName] = Identity.Id;
                }) == 1))
                    return true;
                var sqlUserRole = string.Format(_sqlsExtension["UsersRolesConstains"],
                    Source.Provider.ParameterSymbol, Source.Provider.ParameterSymbol);
                return (Source.UseSession(TableName, session => session.GetScalar<int>(sqlUserRole, parameters =>
                {
                    parameters[ThisName] = relation.Id;
                    parameters[FieldName] = Identity.Id;
                }) == 1));
            }
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
            if (permission != null) return permission;
            if (Identity is Role) //角色的权限，角色的权限只为它本身
                return null;
            var group = Identity as Group;
            if (group != null) //组的权限，组的权限还包含组关联的角色权限
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
            var user = Identity as User;
            if (user != null) //用户的权限，用户的权限还包含关联的角色权限以及
            {
                var sqlUserGroup = string.Format(_sqlsExtension["UsersGroupsGetById"],
                    Source.Provider.ParameterSymbol, Source.Provider.ParameterSymbol);
                permission = Source.UseSession(TableName, session =>
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
                if (permission != null) return permission;

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
            return null;
        }
    }
}