namespace Tatan.Permission.Collections
{
    using Common;
    using Entities;

    /// <summary>
    /// 组集合
    /// </summary>
    public sealed class GroupRelationCollection : AbstractRelationCollection<Group>
    {
        internal GroupRelationCollection(IDentifiable identity, string tableName, string thatName)
            : base(identity, tableName, thatName, "GroupId")
        {
        }
    }
}