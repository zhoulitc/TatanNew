namespace Tatan.Permission.Collections
{
    using Common;
    using Entities;

    /// <summary>
    /// 组集合
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public sealed class GroupRelationCollection : AbstractRelationCollection<Group>
    {
        internal GroupRelationCollection(IDentifiable identity, string tableName, string thatName)
            : base(identity, tableName, thatName, nameof(Group) + nameof(Group.Id))
        {
        }
    }
}