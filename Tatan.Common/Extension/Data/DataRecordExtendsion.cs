namespace Tatan.Common.Extension.Data
{
    using System.Data;
    using Exception;
    using String.Convert;

    #region 提供DataRecord的扩展方法

    /// <summary>
    /// 提供枚举的转换扩展方法
    /// <para>author:zhoulitcqq</para>
    /// <para>此方法组不会抛出异常</para>
    /// </summary>
    public static class DataRecordExtendsion
    {
        #region 获取DataRecord的值

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetValue(this IDataRecord record, string name)
        {
            Assert.ArgumentNotNull(nameof(name), record);
            Assert.ArgumentNotNull(nameof(name), name);
            var index = record.GetOrdinal(name);
            if (index < 0) return string.Empty;
            return (record[index] ?? string.Empty).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="record"></param>
        /// <param name="name"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static T GetValue<T>(this IDataRecord record, string name, T def = default(T)) where T : struct
        {
            var obj = GetValue(record, name);
            return string.IsNullOrEmpty(obj) ? def : obj.ToString().As(def);
        }

        #endregion
    }

    #endregion
}