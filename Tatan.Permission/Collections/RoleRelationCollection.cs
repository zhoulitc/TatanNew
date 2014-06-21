namespace Tatan.Permission.Collections
{
    using Common;
    using Entities;

    /// <summary>
    /// 角色集合
    /// </summary>
    public sealed class RoleRelationCollection : AbstractRelationCollection<Role>
    {
        internal RoleRelationCollection(IDentifiable identity, string tableName, string thatName)
            : base(identity, tableName, thatName, "RoleId")
        {
        }
    }
}