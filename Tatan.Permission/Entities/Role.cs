namespace Tatan.Permission.Entities
{
    using System;
    using Common;
    using Common.Collections;
    using Data;

    /// <summary>
    /// 角色
    /// </summary>
    [Serializable]
    public partial class Role : DataEntity, IDentifiable, INameable
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public Role()
            : base(-1)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">唯一标识符</param>
        public Role(long id)
            : base(id)
        {
        }
        #endregion

        #region Static Properties
        [NonSerialized]
        private static readonly PropertyCollection _perproties;

        static Role()
        {
            _perproties = new PropertyCollection(typeof(Role),
                "Id", "Name", "ParentId"
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
            Name = default(string);
        }
        #endregion

        #region Properties
        /// <summary>
        /// 角色组名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父角色Id
        /// </summary>
        public int ParentId { get; set; }

        #endregion
    }
}