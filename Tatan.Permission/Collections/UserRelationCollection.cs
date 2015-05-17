namespace Tatan.Permission.Collections
{
    using Common;
    using Entities;

    /// <summary>
    /// 用户集合
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public sealed class UserRelationCollection : AbstractRelationCollection<User>
    {
        internal UserRelationCollection(IDentifiable identity, string tableName, string thatName)
            : base(identity, tableName, thatName, nameof(User) + nameof(User.Id))
        {
        }
    }
}