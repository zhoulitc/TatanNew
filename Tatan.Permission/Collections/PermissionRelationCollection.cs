using System.Collections.Generic;

namespace Tatan.Permission.Collections
{
    using Common;
    using Entities;
    using Entities.Relation;

    /// <summary>
    /// 权限集合
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public sealed class PermissionRelationCollection : AbstractRelationCollection<Permission>
    {
        internal PermissionRelationCollection(IDentifiable identity, string tableName, string thatName)
            : base(identity, tableName, thatName, nameof(Permission) + nameof(Permission.Id))
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
        public override Permission GetById(string id)
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

        private static readonly Dictionary<string, string> _sqlsExtension;

        static PermissionRelationCollection()
        {
            _sqlsExtension = new Dictionary<string, string>(4)
            {
                /*
                SELECT COUNT(1) 
                FROM {1}UserGroup{2} AS t1 
                INNER JOIN {1}GroupPermission{2} AS t2 
                ON t1.{1}GroupId{2}=t2.{1}GroupId{2} 
                INNER JOIN {1}UserRole{2} AS t3 
                ON t1.{1}UserId{2}=t3.{1}UserId{2} 
                INNER JOIN {1}RolePermission{2} AS t4 
                ON t3.{1}RoleId{2}=t4.{1}RoleId{2} 
                WHERE t1.{1}UserId{2}={0}UserId 
                AND (t2.{1}PermissionId{2}={0}PermissionId OR t4.{1}PermissionId{2}={0}PermissionId)
                */
                {nameof(UserGroupRoleContains), string.Format(@"
SELECT COUNT(1) 
FROM {{1}}{0}{{2}} AS t1 
INNER JOIN {{1}}{1}{{2}} AS t2 
ON t1.{{1}}{2}{{2}}=t2.{{1}}{3}{{2}} 
INNER JOIN {{1}}{4}{{2}} AS t3 
ON t1.{{1}}{5}{{2}}=t3.{{1}}{6}{{2}} 
INNER JOIN {{1}}{7}{{2}} AS t4 
ON t3.{{1}}{8}{{2}}=t4.{{1}}{9}{{2}} 
WHERE t1.{{1}}{5}{{2}}={{0}}{5} 
AND (t2.{{1}}{10}{{2}}={{0}}{10} OR t4.{{1}}{11}{{2}}={{0}}{11})", 
nameof(UserGroup), nameof(GroupPermission), nameof(UserGroup.GroupId), nameof(GroupPermission.GroupId),
nameof(UserRole), nameof(UserGroup.UserId), nameof(UserRole.UserId),  nameof(RolePermission),
nameof(UserRole.RoleId), nameof(RolePermission.RoleId), nameof(GroupPermission.PermissionId), nameof(RolePermission.PermissionId)
)},
                /*
                SELECT COUNT(1) 
                FROM {1}GroupRole{2} AS t1 
                LEFT JOIN {1}RolePermission{2} AS t2 
                ON t1.{1}RoleId{2}=t2.{1}RoleId{2}  
                WHERE t1.{1}GroupId{2}={0}GroupId AND t2.{1}PermissionId{2}={0}PermissionId
                */
                {nameof(GroupRoleContains), string.Format(@"
SELECT COUNT(1) 
FROM {{1}}{0}{{2}} AS t1 
LEFT JOIN {{1}}{1}{{2}} AS t2 
ON t1.{{1}}{2}{{2}}=t2.{{1}}{3}{{2}}  
WHERE t1.{{1}}{4}{{2}}={{0}}{4} AND t2.{{1}}{5}{{2}}={{0}}{5}",
nameof(GroupRole), nameof(RolePermission), nameof(GroupRole.RoleId), nameof(RolePermission.RoleId),
nameof(GroupRole.GroupId), nameof(RolePermission.PermissionId)
)},

                /*
                SELECT *
                FROM {1}Permission{2} 
                WHERE {1}Id{2}=(
                    SELECT t2.{1}PermissionId{2} 
                    FROM {1}UserRole{2} AS t1 
                    LEFT JOIN {1}RolePermission{2} AS t2 
                    ON t1.{1}RoleId{2}=t2.{1}RoleId{2} 
                    WHERE t1.{1}UserId{2}={0}UserId AND t2.{1}PermissionId{2}={0}PermissionId) 
                UNION 
                SELECT *
                FROM {1}Permission{2} 
                WHERE {1}Id{2}=(
                    SELECT t2.{1}PermissionId{2} 
                    FROM {1}UserGroup{2} AS t1 
                    LEFT JOIN {1}GroupPermission{2} AS t2 
                    ON t1.{1}GroupId{2}=t2.{1}GroupId{2} 
                    WHERE t1.{1}UserId{2}={0}UserId AND t2.{1}PermissionId{2}={0}PermissionId)
                */
                {nameof(GetByUserGroupRoleId), string.Format(@"
SELECT *
FROM {{1}}{0}{{2}} 
WHERE {{1}}{1}{{2}} IN(
    SELECT t2.{{1}}{7}{{2}} 
    FROM {{1}}{2}{{2}} AS t1 
    LEFT JOIN {{1}}{3}{{2}} AS t2 
    ON t1.{{1}}{4}{{2}}=t2.{{1}}{5}{{2}} 
    WHERE t1.{{1}}{6}{{2}}={{0}}{6} AND t2.{{1}}{7}{{2}}={{0}}{7}) 
UNION 
SELECT *
FROM {{1}}{0}{{2}} 
WHERE {{1}}{1}{{2}} IN(
    SELECT t2.{{1}}{13}{{2}} 
    FROM {{1}}{8}{{2}} AS t1 
    LEFT JOIN {{1}}{9}{{2}} AS t2 
    ON t1.{{1}}{10}{{2}}=t2.{{1}}{11}{{2}} 
    WHERE t1.{{1}}{12}{{2}}={{0}}{12} AND t2.{{1}}{13}{{2}}={{0}}{13})",
nameof(Permission), nameof(Permission.Id),nameof(UserRole), nameof(RolePermission),
nameof(UserRole.RoleId), nameof(RolePermission.RoleId), nameof(UserRole.UserId), nameof(RolePermission.PermissionId),
nameof(UserGroup), nameof(GroupPermission), nameof(UserGroup.GroupId), nameof(GroupPermission.GroupId), 
nameof(UserGroup.UserId), nameof(GroupPermission.PermissionId)
)},
                /*
                SELECT * 
                FROM {1}Permission{2} 
                WHERE {1}Id{2}=(
                    SELECT t2.{1}PermissionId{2} 
                    FROM {1}GroupRole{2} AS t1 
                    LEFT JOIN {1}RolePermission{2} AS t2 
                    ON t1.{1}RoleId{2}=t2.{1}RoleId{2} 
                    WHERE t1.{1}GroupId{2}={0}GroupId AND t2.{1}PermissionId{2}={0}PermissionId)
                */
                {nameof(GetByGroupRoleId), string.Format(@"
SELECT * 
FROM {{1}}{0}{{2}} 
WHERE {{1}}{1}{{2}} IN(
    SELECT t2.{{1}}{7}{{2}} 
    FROM {{1}}{2}{{2}} AS t1 
    LEFT JOIN {{1}}{3}{{2}} AS t2 
    ON t1.{{1}}{4}{{2}}=t2.{{1}}{5}{{2}} 
    WHERE t1.{{1}}{6}{{2}}={{0}}{6} AND t2.{{1}}{7}{{2}}={{0}}{7})",
nameof(Permission), nameof(Permission.Id),nameof(GroupRole), nameof(RolePermission),
nameof(GroupRole.RoleId), nameof(RolePermission.RoleId), nameof(GroupRole.GroupId), nameof(RolePermission.PermissionId)
)}
            };
        }

        private bool GroupRoleContains(Permission relation)
        {
            var sql = string.Format(_sqlsExtension[nameof(GroupRoleContains)],
                Source.Provider.ParameterSymbol, Source.Provider.LeftSymbol, Source.Provider.RightSymbol);
            return Source.UseSession(TableName, session => session.ExecuteScalar<long>(sql, parameters =>
            {
                parameters[ThisName] = relation.Id;
                parameters[ThatName] = Identity.Id;
            }) > 0);
        }

        private bool UserGroupRoleContains(Permission relation)
        {
            var sql = string.Format(_sqlsExtension[nameof(UserGroupRoleContains)],
                Source.Provider.ParameterSymbol, Source.Provider.LeftSymbol, Source.Provider.RightSymbol);
            return (Source.UseSession(TableName, session => session.ExecuteScalar<long>(sql, parameters =>
            {
                parameters[ThisName] = relation.Id;
                parameters[ThatName] = Identity.Id;
            }) > 0));
        }

        private Permission GetByUserGroupRoleId(string id)
        {
            var sql = string.Format(_sqlsExtension[nameof(GetByUserGroupRoleId)],
                Source.Provider.ParameterSymbol, Source.Provider.LeftSymbol, Source.Provider.RightSymbol);
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

        private Permission GetByGroupRoleId(string id)
        {
            var sql = string.Format(_sqlsExtension[nameof(GetByGroupRoleId)],
                Source.Provider.ParameterSymbol, Source.Provider.LeftSymbol, Source.Provider.RightSymbol);
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