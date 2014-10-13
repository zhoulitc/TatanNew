namespace Tatan.Data.Relation
{
    using System;
    using Common.Collections;

    #region Tables的实体类，无法继承
    /// <summary>
    /// Tables的实体类，无法继承。此实体保存了表的信息
    /// </summary>
    [Serializable]
    public partial class Tables : DataEntity
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public Tables()
            : base(string.Empty)
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">唯一标识符</param>
        public Tables(string id)
            : base(id)
        {
        }
        #endregion

        #region Static Properties
        [NonSerialized]
        private static readonly PropertyCollection _perproties;

        static Tables()
        {
            _perproties = new PropertyCollection(typeof(Tables),
                "Name", "Title", "Remark"
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
            Title = default(string);
            Remark = default(string);
        }
        #endregion

        #region Properties
        /// <summary>
        /// 表名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 表显示名
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 表备注
        /// </summary>
        public string Remark { get; set; }

        #endregion
    }
    #endregion
}
