namespace Tatan.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using Common.Collections;
    using Common.Extension.String.Convert;

    #region 抽象通用实体类
    /// <summary>
    /// 抽象通用实体类
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    [Serializable]
    public abstract class DataEntity : IDataEntity
    {
        //默认标识符值
        internal readonly static string DefaultId = string.Empty;

        #region 构造函数

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">唯一标识</param>
        /// <param name="creator">创建者</param>
        /// <param name="createdTime">创建时间</param>
        protected DataEntity(string id = null, string creator = null, string createdTime = null)
        {
            Id = id ?? string.Empty;
            Creator = creator ?? "System";
            CreatedTime = createdTime.AsValue(DateTime.Now);
        }
        #endregion

        /// <summary>
        /// 清理实体值
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// 获取实体的属性
        /// </summary>
        protected abstract PropertyCollection Properties { get; }

        #region IDataEntity
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public object this[string name]
        {
            get
            {
                return Properties[this, name];
            }
            set
            {
                var oldValue = Properties[this, name];
                if (oldValue != value)
                {
                    Properties[this, name] = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, name);
                }
            }
        }
        #endregion

        #region IDentifiable
        /// <summary>
        /// 一个自动生成的唯一标识符
        /// </summary>
        public string Id { get; private set; }
        #endregion

        #region IEnumerable
        /// <summary>
        /// 返回一个循环访问集合的枚举器。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<string> GetEnumerator() => Properties.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Properties.GetEnumerator();
        #endregion

        #region IReplicable
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDataEntity Clone(string id)
        {
            var entity = (DataEntity)MemberwiseClone();
            entity.Clear();
            entity.Id = id;
            entity.PropertyChanged = null;
            entity.Creator = Creator;
            entity.CreatedTime = DateTime.Now;
            return entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IDataEntity Copy(string id)
        {
            var entity = Clone(id);
            foreach (var property in Properties)
            {
                entity[property] = Properties[this, property];
            }
            return entity;
        }
        #endregion

        #region IPropertyChanged
        /// <summary>
        /// 属性改变时的行为
        /// </summary>
        public event Action<object, string> PropertyChanged;
        #endregion

        #region IPropertyChanged
        /// <summary>
        /// 创建者
        /// </summary>
        public string Creator { get; private set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; private set; }
        #endregion

        #region Object
        /// <summary>
        /// 是否与另一个对象完全相等
        /// </summary>
        /// <param name="obj">另一个对象</param>
        /// <returns>是否相等</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return GetHashCode() == obj.GetHashCode();
        }

        /// <summary>
        /// 获取对象的hash码，如果两个对象相等，则hash码一定相等
        /// </summary>
        /// <returns>hash码</returns>
        public override int GetHashCode() => Id.GetHashCode();

        /// <summary>
        /// 获取对象的字符串描述
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">当字符串超过最大长度时抛出</exception>
        /// <returns>对象的字符串描述</returns>
        public override string ToString()
        {
            var builder = new StringBuilder(Properties.Count * 20);
            builder.AppendFormat("\"Id\":\"{0}\",\"Creator\":\"{1}\",\"CreatedTime\":\"{2}\"", 
                Id, Creator, CreatedTime.ToString("yyyy-MM-dd hh:mm:ss"));
            foreach (var property in Properties)
            {
                if (property == "Id" || property == "Creator" || property == "CreatedTime") continue;
// ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                if (Properties.IsString(property))
                    builder.AppendFormat(",\"{0}\":\"{1}\"", property, Properties[this, property]);
                else
                    builder.AppendFormat(",\"{0}\":{1}", property, Properties[this, property]);
            }
            return string.Format("{{{0}}}", builder);
        }
        #endregion
    }
    #endregion
}