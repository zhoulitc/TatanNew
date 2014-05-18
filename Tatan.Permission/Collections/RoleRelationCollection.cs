namespace Tatan.Permission.Collections
{
    using Common;
    using Entities;

    /// <summary>
    /// 角色集合
    /// </summary>
    public sealed class RoleRelationCollection : AbstractRelationCollection<Role>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="fieldName"></param>
        /// <param name="tableName"></param>
        internal RoleRelationCollection(IDentifiable identity, string fieldName, string tableName)
            : base(identity, fieldName, tableName, "RoleId")
        {
        }
    }
}