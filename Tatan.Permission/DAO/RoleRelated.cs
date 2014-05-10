namespace Tatan.Data.DAO
{
    using System;
    using System.Collections.Generic;
    using Tatan.Data;

    #region RoleRelated的实体类，无法继承
    /// <summary>
    /// RoleRelated的实体类，无法继承。此实体保存了用户以及登录信息
    /// </summary>
    [Serializable]
    public sealed class RoleRelated : DataEntity
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <exception cref="System.ArgumentException">当存在相同的字段名时抛出</exception>
        public RoleRelated()
        {
            _identity = _Table.CreateUniqueValue();
            _map = new Dictionary<string, object>(_Table.Fields.Count);
            foreach (string field in _Table.Fields.Keys)
                _map.Add(field, _Table.Fields[field].DefaultValue);
        }
        #endregion

        #region 字段封装
        /// <summary>
        /// 角色ID
        /// </summary>
        public string RoleID
        {
            get { return this["roleID"] as string; }
            set { this["roleID"] = value; }
        }

        /// <summary>
        /// 关联对象ID
        /// </summary>
        public string RelatedID
        {
            get { return this["relatedID"] as string; }
            set { this["relatedID"] = value; }
        }

        /// <summary>
        /// 关联备注
        /// </summary>
        public string Remark
        {
            get { return this["remark"] as string; }
            set { this["remark"] = value; }
        }
        #endregion

        #region 重写方法
        /// <summary>
        /// 获取实体所属表
        /// </summary>
        protected override Table _Table
        {
            get { return _table; }
        }

        public override string GetIdentity()
        {
            return RoleID + RelatedID;
        }
        #endregion

        #region Table
        [NonSerialized]
        private static readonly Table _table = new RoleRelatedTable();
        private class RoleRelatedTable : Table
        {
            public RoleRelatedTable()
                : base("RoleRelated", "id")
            {
                Fields = new Dictionary<string, Field>(2);
                Fields.Add("roleID", new Field(size: 50));
                Fields.Add("relatedID", new Field(size: 50));
                Fields.Add("remark", new Field(size: 100));
                InitSQL();
            }
        }
        #endregion
    }
    #endregion
}