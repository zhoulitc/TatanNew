namespace Tatan.Data.DAO
{
    using System;
    using System.Collections.Generic;
    using Tatan.Data;

    #region Role的实体类，无法继承
    /// <summary>
    /// Role的实体类，无法继承。此实体保存了用户以及登录信息
    /// </summary>
    [Serializable]
    public sealed class Role : DataEntity
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <exception cref="System.ArgumentException">当存在相同的字段名时抛出</exception>
        public Role()
        {
            _identity = _Table.CreateUniqueValue();
            _map = new Dictionary<string, object>(_Table.Fields.Count);
            foreach (string field in _Table.Fields.Keys)
                _map.Add(field, _Table.Fields[field].DefaultValue);
        }
        #endregion

        #region 字段封装
        /// <summary>
        /// 角色名
        /// </summary>
        public string Name
        {
            get { return this["name"] as string; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// 角色备注
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
        #endregion

        #region Table
        [NonSerialized]
        private static readonly Table _table = new RoleTable();
        private class RoleTable : Table
        {
            public RoleTable()
                : base("Role", "id")
            {
                Fields = new Dictionary<string, Field>(2);
                Fields.Add("name", new Field(size: 20)); 
                Fields.Add("remark", new Field(size: 100));
                InitSQL();
            }
        }
        #endregion
    }
    #endregion
}