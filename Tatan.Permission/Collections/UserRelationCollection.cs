namespace Tatan.Permission.Collections
{
    using Common;
    using Entities;

    /// <summary>
    /// 用户集合
    /// </summary>
    public sealed class UserRelationCollection : AbstractRelationCollection<User>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="fieldName"></param>
        /// <param name="tableName"></param>
        internal UserRelationCollection(IDentityObject identity, string fieldName, string tableName)
            : base(identity, fieldName, tableName, "UserId")
        {
        }
    }
}