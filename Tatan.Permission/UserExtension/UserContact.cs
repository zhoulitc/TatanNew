namespace Tatan.Permission.UserExtension
{
    using System;
    using Common.Collections;
    using Data;

    /// <summary>
    /// 用户联系信息
    /// </summary>
    [Serializable]
    public partial class UserContact : DataEntity
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">唯一标识符</param>
        public UserContact(int id)
            : base(id)
        {
        }
        #endregion

        #region Static Properties
        [NonSerialized]
        private static readonly PropertyCollection _perproties;

        static UserContact()
        {
            _perproties = new PropertyCollection(typeof(UserContact),
                "Email", "Phone", "Address"
            );
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// 获取属性集合
        /// </summary>
        protected override PropertyCollection Properties
        {
            get { return _perproties; }
        }

        /// <summary>
        /// 清理属性
        /// </summary>
        public override void Clear()
        {
            Email = default(string);
            Phone = default(string);
            Address = default(string);
        }
        #endregion

        #region Properties

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        #endregion
    }
}