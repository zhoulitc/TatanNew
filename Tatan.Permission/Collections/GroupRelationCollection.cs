namespace Tatan.Permission.Collections
{
    using Common;
    using Entities;

    /// <summary>
    /// 组集合
    /// </summary>
    public sealed class GroupRelationCollection : AbstractRelationCollection<Group>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="fieldName"></param>
        /// <param name="tableName"></param>
        internal GroupRelationCollection(IDentityObject identity, string fieldName, string tableName)
            : base(identity, fieldName, tableName, "GroupId")
        {
        }
    }
}