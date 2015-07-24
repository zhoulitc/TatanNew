namespace Tatan.Permission.Entities
{
    using System;
    using Common;
    using Common.Collections;
    using Data;

    /// <summary>
    /// 权限
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public partial class Permission : DataEntity, INameable
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">唯一标识符</param>
        /// <param name="creator"></param>
        /// <param name="createTime">创建时间</param>
        public Permission(string id = null, string creator = null, string createTime = null)
            : base(id, creator, createTime)
        {
        }
        #endregion

        #region Static Properties
        [NonSerialized]
        private static readonly PropertyCollection _perproties;

        static Permission()
        {
            _perproties = new PropertyCollection(typeof(Permission));
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// 获取属性集合
        /// </summary>
        protected override PropertyCollection Properties => _perproties;

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
        /// 权限名
        /// </summary>
        public string Name { get; set; }

        #endregion
    }
}