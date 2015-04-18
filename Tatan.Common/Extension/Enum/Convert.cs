namespace Tatan.Common.Extension.Enum
{
    using System;

    #region 提供枚举的转换扩展方法

    /// <summary>
    /// 提供枚举的转换扩展方法
    /// <para>此方法组不会抛出异常</para>
    /// </summary>
    public static class Convert
    {
        #region 将枚举类型转换为int

        /// <summary>
        /// 将枚举类型转换为int
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int AsInt(this Enum value)
        {
            return ((IConvertible) value).ToInt32(null);
        }

        /// <summary>
        /// 将枚举类型转换为另一种枚举类型，他们的int值必须相等
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T As<T>(this Enum value) where T : struct
        {
            return (T) Enum.ToObject(typeof (T), AsInt(value));
        }

        #endregion
    }

    #endregion
}