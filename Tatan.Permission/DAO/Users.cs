namespace Tatan.Data.DAO
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Tatan.Data;

    #region Users的实体类，无法继承
    /// <summary>
    /// Users的实体类，无法继承。此实体保存了用户以及登录信息
    /// </summary>
    [Serializable]
    public sealed class Users : DataEntity
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <exception cref="System.ArgumentException">当存在相同的字段名时抛出</exception>
        public Users()
        {
            _identity = _Table.CreateUniqueValue();
            _map = new Dictionary<string, object>(_Table.Fields.Count);
            foreach (string field in _Table.Fields.Keys)
                _map.Add(field, _Table.Fields[field].DefaultValue);
        }
        #endregion

        #region 字段封装
        /// <summary>
        /// 用户组编号
        /// </summary>
        public int? GroupID
        {
            get { return this["groupID"] as int?; }
            set { this["groupID"] = value; }
        }

        /// <summary>
        /// 用户登录名
        /// </summary>
        public string Username
        {
            get { return this["username"] as string; }
            set { this["username"] = value; }
        }

        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string Email
        {
            get { return this["email"] as string; }
            set { this["email"] = value; }
        }

        /// <summary>
        /// 用户手机号
        /// </summary>
        public string Phone
        {
            get { return this["phone"] as string; }
            set { this["phone"] = value; }
        }

        /// <summary>
        /// 用户密码，保存密文
        /// </summary>
        public string Password
        {
            get { return this["password"] as string; }
            set { this["password"] = value; }
        }

        /// <summary>
        /// 注册时间
        /// </summary>
        public string RegisterTime
        {
            get { return this["registerTime"] as string; }
            set { this["registerTime"] = value; }
        }

        /// <summary>
        /// 登陆次数
        /// </summary>
        public int? LoginCount
        {
            get { return this["loginCount"] as int?; }
            set { this["loginCount"] = value; }
        }

        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        public string LastLoginTime
        {
            get { return this["lastLoginTime"] as string; }
            set { this["lastLoginTime"] = value; }
        }

        /// <summary>
        /// 最后一次登出时间
        /// </summary>
        public string LastLogoutTime
        {
            get { return this["lastLogoutTime"] as string; }
            set { this["lastLogoutTime"] = value; }
        }
                
        /// <summary>
        /// 最后一次登录IP
        /// </summary>
        public string LastLoginIP
        {
            get { return this["lastLoginIP"] as string; }
            set { this["lastLoginIP"] = value; }
        }

        /// <summary>
        /// 最后一次锁定时间
        /// </summary>
        public string LastLockedTime
        {
            get { return this["lastLockedTime"] as string; }
            set { this["lastLockedTime"] = value; }
        }

        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool? IsLocked
        {
            get { return this["isLocked"] as bool?; }
            set { this["isLocked"] = value; }
        }

        /// <summary>
        /// 用户备注
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
        private static readonly Table _table = new UsersTable();
        private class UsersTable : Table
        {
            public UsersTable()
                : base("Users", "id")
            {
                Fields = new Dictionary<string, Field>(12);
                Fields.Add("groupID", new Field(DbType.Int32, size: 4));
                Fields.Add("username", new Field(size: 20)); //可以通过用户名+密码登录
                Fields.Add("email", new Field(size: 50)); //可以通过邮箱+密码登录
                Fields.Add("phone", new Field(size: 20)); //可以通过手机号+密码登录
                Fields.Add("password", new Field(size: 50));
                Fields.Add("registerTime", new Field(size: 14));
                Fields.Add("loginCount", new Field(DbType.Int32, size: 8));
                Fields.Add("lastLoginTime", new Field(size: 14));
                Fields.Add("lastLogoutTime", new Field(size: 14));
                Fields.Add("lastLoginIP", new Field(size: 17));
                Fields.Add("lastLockedTime", new Field(size: 14));
                Fields.Add("isLocked", new Field(DbType.Int32, size: 1));
                Fields.Add("remark", new Field(size: 100));
                InitSQL();
            }
        }
        #endregion
    }
    #endregion
}