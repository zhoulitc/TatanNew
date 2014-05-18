namespace Tatan.Common.Extension.String.Convert
{
    using Logging;
    using System;
    using System.Text;

    #region 提供字符串的转换扩展方法
    /// <summary>
    /// 提供字符串的转换扩展方法
    /// <para>此方法组不会抛出异常</para>
    /// </summary>
    public static class Convert
    {
        #region 转换为Boolean
        /// <summary>
        /// 转换为Boolean，不会抛出异常。转换失败则返回def
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns>Boolean</returns>
        public static bool AsBoolean(this string value, bool def = false)
        {
            bool ret;
            if (!bool.TryParse(value, out ret))
                ret = def;
            return ret;
        }
        #endregion

        #region 转换为DateTime
        /// <summary>
        /// 转换为DateTime，不会抛出异常。转换失败则返回def
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns>DateTime</returns>
        public static DateTime AsDateTime(this string value, DateTime def = default(DateTime))
        {
            DateTime ret;
            if (!DateTime.TryParse(value, out ret))
                ret = def;
            return ret;
        }
        #endregion

        #region 转换为浮点数

        #region 转换为Decimal
        /// <summary>
        /// 转换为Decimal，不会抛出异常。转换失败则返回def
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns>Decimal</returns>
        public static decimal AsDecimal(this string value, decimal def = 0)
        {
            decimal ret;
            if (!decimal.TryParse(value, out ret))
                ret = def;
            return ret;
        }
        #endregion

        #region 转换为Double
        /// <summary>
        /// 转换为Double，不会抛出异常。转换失败则返回def
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns>Double</returns>
        public static double AsDouble(this string value, double def = 0)
        {
            double ret;
            if (!double.TryParse(value, out ret))
                ret = def;
            return ret;
        }
        #endregion

        #region 转换为Float
        /// <summary>
        /// 转换为Float，不会抛出异常。转换失败则返回def
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns>Float</returns>
        public static float AsFloat(this string value, float def = 0)
        {
            float ret;
            if (!float.TryParse(value, out ret))
                ret = def;
            return ret;
        }
        #endregion

        #endregion

        #region 转换为有符号整数

        #region 转换为Byte
        /// <summary>
        /// 转换为Byte，不会抛出异常。转换失败则返回def
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns>Byte</returns>
        public static byte AsByte(this string value, byte def = 0)
        {
            byte ret;
            if (!byte.TryParse(value, out ret))
                ret = def;
            return ret;
        }
        #endregion

        #region 转换为Short
        /// <summary>
        /// 转换为Short，不会抛出异常。转换失败则返回def
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns>Short</returns>
        public static short AsShort(this string value, short def = 0)
        {
            short ret;
            if (!short.TryParse(value, out ret))
                ret = def;
            return ret;
        }
        #endregion

        #region 转换为Int
        /// <summary>
        /// 转换为Int，不会抛出异常。转换失败则返回def
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns>Int</returns>
        public static int AsInt(this string value, int def = 0)
        {
            int ret;
            if (!int.TryParse(value, out ret))
                ret = def;
            return ret;
        }
        #endregion

        #region 转换为Long
        /// <summary>
        /// 转换为Long，不会抛出异常。转换失败则返回def
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns>Long</returns>
        public static long AsLong(this string value, long def = 0)
        {
            long ret;
            if (!long.TryParse(value, out ret))
                ret = def;
            return ret;
        }
        #endregion

        #endregion

        #region 转换为无符号整数 

        #region 转换为SByte
        /// <summary>
        /// 转换为SByte，不会抛出异常。转换失败则返回def
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns>SByte</returns>
        public static sbyte AsSByte(this string value, sbyte def = 0)
        {
            sbyte ret;
            if (!sbyte.TryParse(value, out ret))
                ret = def;
            return ret;
        }
        #endregion

        #region 转换为UShort
        /// <summary>
        /// 转换为UShort，不会抛出异常。转换失败则返回def
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns>UShort</returns>
        public static ushort AsUShort(this string value, ushort def = 0)
        {
            ushort ret;
            if (!ushort.TryParse(value, out ret))
                ret = def;
            return ret;
        }
        #endregion

        #region 转换为UInt
        /// <summary>
        /// 转换为UInt，不会抛出异常。转换失败则返回def
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns>UInt</returns>
        public static uint AsUInt(this string value, uint def = 0)
        {
            uint ret;
            if (!uint.TryParse(value, out ret))
                ret = def;
            return ret;
        }
        #endregion

        #region 转换为ULong
        /// <summary>
        /// 转换为ULong，不会抛出异常。转换失败则返回def
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns>ULong</returns>
        public static ulong AsULong(this string value, ulong def = 0)
        {
            ulong ret;
            if (!ulong.TryParse(value, out ret))
                ret = def;
            return ret;
        }
        #endregion

        #endregion

        #region 转换为GUID
        /// <summary>
        /// 转换为Guid，不会抛出异常。转换失败则返回Guid.Empty
        /// </summary>
        /// <param name="value"></param>
        /// <param name="format">格式化方式，默认无</param>
        /// <returns>Guid</returns>
        public static Guid AsGuid(this string value, string format = null)
        {
            Guid ret;
            if (string.IsNullOrEmpty(value))
                return Guid.Empty;
            if (string.IsNullOrEmpty(format))
            {
                if (!Guid.TryParse(value, out ret))
                    ret = Guid.Empty;
            }
            else
            {
                if (!Guid.TryParseExact(value, format, out ret))
                    ret = Guid.Empty;
            }
            return ret;
        }
        #endregion

        #region 转换为Bytes
        /// <summary>
        /// 转换为Bytes，不会抛出异常。转换失败则返回空的Bytes
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding">指定转换编码</param>
        /// <returns>byte数组</returns>
        public static byte[] AsBytes(this string value, Encoding encoding = null)
        {
            byte[] ret;
            try
            {
                ret = (encoding ?? Encoding.Default).GetBytes(value);
            }
            catch (EncoderFallbackException ex)
            {
                Log.Default.Warn(typeof(Convert), ex.Message, ex);
                ret = new byte[0];
            }
            return ret;
        }
        #endregion

        #region 转换为泛型值类型，必须指定def
        /// <summary>
        /// 转换为泛型值类型，必须指定def，不会抛出异常。转换失败则返回def
        /// <para>运用了反射，效率上比特定的转换要慢</para>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns>泛型值类型</returns>
        public static T As<T>(this string value, T def = default(T)) where T : struct
        {
            var ret = def;
            var type = typeof(T);
            try
            {
                //可能存在参数不匹配导致调用失败的情况，或者没有获取到方法
                var mi = type.GetMethod("Parse", new[] {typeof (string)});
                if (null == mi)
                    return ret;

                //可能转换失败
                ret = (T) mi.Invoke(null, new object[] {value});
            }
            catch (Exception ex)
            {
                Log.Default.Warn(typeof(Convert), ex.Message, ex);
            }
            return ret;
        }
        #endregion
    }
    #endregion
}