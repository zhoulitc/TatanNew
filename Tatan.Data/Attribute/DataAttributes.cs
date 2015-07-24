namespace Tatan.Data.Attribute
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// 特性操作方法
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public static class DataAttributes
    {
        private static readonly Type _enumType = typeof (int);
        /// <summary>
        /// 获取表名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetTableName<T>()
        {
            var attribute = typeof(T).GetCustomAttribute<TableAttribute>();
            if (attribute == null)
                return string.Empty;
            return attribute.Name;
        }

        /// <summary>
        /// 获取主键名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        //public static string GetPrimaryKey<T>()
        //{
        //    var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        //    foreach (var property in properties)
        //    {
        //        var attribute = property.GetCustomAttribute<FieldAttribute>();
        //        if (attribute != null && attribute.IsPrimaryKey)
        //            return attribute.Name;
        //    }
        //    return string.Empty;
        //}

        /// <summary>
        /// 获取字段定义集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IList<string> GetFieldNames<T>()
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var fields = new List<string>(properties.Length);
            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<FieldAttribute>();
                if (attribute != null)
                    fields.Add(attribute.Name);
            }
            return fields;
        }

        /// <summary>
        /// 获取字段定义集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        //public static IDictionary<string, Tuple<Type, object>> GetFieldValues<T>(T entity, bool isReadOnly = false)
        //{
        //    var properties = typeof (T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        //    var fields = new Dictionary<string, Tuple<Type, object>>(properties.Length);
        //    foreach (var property in properties)
        //    {
        //        var attribute = property.GetCustomAttribute<FieldAttribute>();
        //        if (isReadOnly && (attribute.IsReadOnly || attribute.IsPrimaryKey)) continue;
        //        if (attribute != null)
        //        {
        //            if (attribute.IsEnum)
        //            {
        //                fields[attribute.Name] = Tuple.Create(_enumType, property.GetValue(entity));
        //            }
        //            else
        //            {
        //                fields[attribute.Name] = Tuple.Create(property.PropertyType, property.GetValue(entity));
        //            }
        //        }
        //    }
        //    return fields;
        //}
    }
}