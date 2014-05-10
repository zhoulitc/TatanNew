namespace Tatan.Data.DAO
{
    using System;
    using Tatan.Data.DB;
    using Tatan.Common;
    using Tatan.Common.Extension.String.Regex;
    using Tatan.Common.Cryptography;
    using Tatan.Common.Internationalization;

    public static class RoleHandler
    {
        private static readonly string _deleteRoleSQL =
            "DELETE FROM Role WHERE id = @:id";

        private static readonly string _deleteRoleRelatesSQL =
            "DELETE FROM RoleRelated WHERE RoleID = @:roleID";

        private static readonly string _deleteRoleRelatedSQL =
            "DELETE FROM RoleRelated WHERE id = @:id";

        private static readonly string _deleteRolePermissionesSQL =
            "DELETE FROM RolePermission WHERE RoleID = @:roleID";

        private static readonly string _deleteRolePermissionSQL =
            "DELETE FROM RolePermission WHERE id = @:id";

        /// <summary>
        /// 创建一个权限
        /// </summary>
        /// <param name="name"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public static bool CreatePermission(string name, string remark = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", ExceptionHandler.GetMessage("ArgumentNull"));
            Permission p = new Permission();
            p.Name = name;
            p.Remark = remark ?? string.Empty;
            return DataAccess.Add(p);
        }

        /// <summary>
        /// 创建一个角色
        /// </summary>
        /// <param name="name"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public static bool CreateRole(string name, string remark = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", ExceptionHandler.GetMessage("ArgumentNull"));
            Role r = new Role();
            r.Name = name;
            r.Remark = remark ?? string.Empty;
            return DataAccess.Add(r);
        }

        /// <summary>
        /// 授予某个角色指定的关联对象
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="relatedId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public static bool GrantRoleRelated(string roleId, string relatedId, string remark = null)
        {
            if (string.IsNullOrEmpty(roleId))
                throw new ArgumentNullException("roleId", ExceptionHandler.GetMessage("ArgumentNull"));
            if (string.IsNullOrEmpty(relatedId))
                throw new ArgumentNullException("relatedId", ExceptionHandler.GetMessage("ArgumentNull"));
            RoleRelated r = new RoleRelated();
            r.RoleID = roleId;
            r.RelatedID = relatedId;
            r.Remark = remark ?? string.Empty;
            return DataAccess.Add(r);
        }

        /// <summary>
        /// 授予某个角色指定的权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permissionId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public static bool GrantRolePermission(string roleId, string permissionId, string remark = null)
        {
            if (string.IsNullOrEmpty(roleId))
                throw new ArgumentNullException("roleId", ExceptionHandler.GetMessage("ArgumentNull"));
            if (string.IsNullOrEmpty(permissionId))
                throw new ArgumentNullException("permissionId", ExceptionHandler.GetMessage("ArgumentNull"));
            RolePermission r = new RolePermission();
            r.RoleID = roleId;
            r.PermissionID = permissionId;
            r.Remark = remark ?? string.Empty;
            return DataAccess.Add(r);
        }

        /// <summary>
        /// 删除某个角色，以及和角色关联的对象和权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteRole(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id", ExceptionHandler.GetMessage("ArgumentNull"));

            return DataSessionFactory.UseSession<bool>("Role", (session) =>
            {
                var tran = session.BeginTransaction();
                try
                {
                    if (session.Execute(_deleteRoleSQL, p => { p["id"] = id; }) == 1)
                    {
                        session.Execute(_deleteRoleRelatesSQL, p => { p["roleID"] = id; });
                        session.Execute(_deleteRolePermissionesSQL, p => { p["roleID"] = id; });
                        tran.Commit();
                        return true;
                    }
                    else
                    {
                        tran.Rollback();
                        return false;
                    }
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    return false;
                }
            });
        }

        /// <summary>
        /// 删除某个角色与关联对象之间的关系
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteRoleRelated(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id", ExceptionHandler.GetMessage("ArgumentNull"));

            return DataSessionFactory.UseSession<bool>("RoleRelated", (session) =>
            {
                return session.Execute(_deleteRoleRelatedSQL, p => 
                {
                    p["id"] = id;
                }) == 1;
            });
        }

        /// <summary>
        /// 删除某个角色与权限之间的关系
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteRolePermission(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id", ExceptionHandler.GetMessage("ArgumentNull"));

            return DataSessionFactory.UseSession<bool>("RolePermission", (session) =>
            {
                return session.Execute(_deleteRolePermissionSQL, p => 
                {
                    p["id"] = id;
                }) == 1;
            });
        }
    }
}