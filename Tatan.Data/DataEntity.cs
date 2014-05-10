﻿// ReSharper disable once CheckNamespace
namespace Tatan.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using Common.Collections;

    #region 抽象通用实体类
    /// <summary>
    /// 抽象通用实体类
    /// </summary>
    [Serializable]
    public abstract class DataEntity : IDataEntity
    {
        //默认标识符值
        internal const int DefaultId = -1;

        #region 构造函数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        protected DataEntity(int id)
        {
            Id = id;
        }
        /// <summary>
        /// 析构
        /// </summary>
        ~DataEntity()
        {
            Properties.Dispose();
            Id = -1;
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
                Properties[this, name] = value;
                if (PropertyChanged != null && oldValue != value)
                    PropertyChanged(this, name);
            }
        }
        #endregion

        #region IDentifiable
        /// <summary>
        /// 一个自动生成的唯一标识符
        /// </summary>
        public int Id { get; set; }
        #endregion

        #region IEnumerable
        public IEnumerator<string> GetEnumerator()
        {
            return Properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Properties.GetEnumerator();
        }
        #endregion

        #region IReplicable
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDataEntity Clone()
        {
            var entity = (DataEntity)MemberwiseClone();
            entity.Clear();
            entity.Id = DefaultId;
            return entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IDataEntity Copy()
        {
            var entity = Clone();
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

        #region Object
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            var builder = new StringBuilder(Properties.Count);
            builder.AppendFormat("\"id\":{0}", Id);
            foreach (var property in Properties)
            {
// ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                if (Properties.IsString(property))
                    builder.AppendFormat(",\"{0}\":\"{1}\"", property, Properties[this, property]);
                else
                    builder.AppendFormat(",\"{0}\":{1}", property, Properties[this, property]);
            }
            return "{" + builder + "}";
        }
        #endregion
    }
    #endregion
}