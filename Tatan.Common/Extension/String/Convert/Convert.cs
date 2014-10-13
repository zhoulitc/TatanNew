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
        public static bool AsBoolean(this string value, bool def = default(bool))
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
            if (!DateTime.TryParse(value, outs ret))
                ret = def;
            return ret;
        }
        #endregion

        #region 转换为枚举
        /// <summary>
        /// 转换为枚举，不会抛出异常。转换失败则返回def
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public static T AsEnum<T>(this string value, T def = default(T)) where T : struct
        {
            T ret;
            if (!Enum.TryParse(value, out ret))
                ret = def;
            return ret;
        }
        #endregion

        #region 转换为浮点数
        /// <summary>
        /// 转换为Decimal，不会抛出异常。转换失败则返回def
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns>Decimal</returns>
        public static decimal AsDecimal(this string value, decimal def = default(decimal))
        {
            decimal ret;
            if (!decimal.TryParse(value, out ret))
                ret = def;
            return ret;
        }

        /// <summary>
        /// 转换为Double，不会抛出异常。转换失败则返回def
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns>Double</returns>
        public static double AsDouble(this string value, double def = default(double))
        {
            double ret;
            if (!double.TryParse(value, out ret))
                ret = def;
            return ret;
        }

        /// <summary>
        /// 转换为Float，不会抛出异常。转换失败则返回def
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns>Float</returns>
        public static float AsFloat(this string value, float def = default(float))
        {
            float ret;
            if (!float.TryParse(value, out ret))
                ret = def;
            return ret;
        }
        #endregion

        #region 转换为有符号整数
        /// <summary>
        /// 转换为Byte，不会抛出异常。转换失败则返回def
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns>Byte</returns>
        public static sbyte AsInt8(this string value, sbyte def = default(sbyte))
        {
            sbyte ret;
            if (!sbyte.TryParse(value, out ret))
                ret = def;
            return ret;
        }

        /// <summary>
        /// 转换为Short，不会抛出异常。转换失败则返回def
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns>Short</returns>
        public static short AsInt16(this string value, short def = default(short))
        {
            short ret;
            if (!short.TryParse(value, out ret))
                ret = def;
            return ret;
        }

        /// <summary>
        /// 转换为Int，不会抛出异常。转换失败则返回def
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns>Int</returns>
        public static int AsInt32(this string value, int def = default(int))
        {
            int ret;
            if (!int.TryParse(value, out ret))
                ret = def;
            return ret;
        }

        /// <summary>
        /// 转换为Long，不会抛出异常。转换失败则返回def
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns>Long</returns>
        public static long AsInt64(this string value, long def = default(long))
        {
            long ret;
            if (!long.TryParse(value, out ret))
                ret = def;
            return ret;
        }
        #endregion

        #region 转换为无符号整数 
        /// <summary>
        /// 转换为Byte，不会抛出异常。转换失败则返回def
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns>Byte</returns>
        public static byte AsUInt8(this string value, byte def = default(byte))
        {
            byte ret;
            if (!byte.TryParse(value, out ret))
                ret = def;
            return ret;
        }

        /// <summary>
        /// 转换为UShort，不会抛出异常。转换失败则返回def
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns>UShort</returns>
        public static ushort AsUInt16(this string value, ushort def = default(ushort))
        {
            ushort ret;
            if (!ushort.TryParse(value, out ret))
                ret = def;
            return ret;
        }

        /// <summary>
        /// 转换为UInt，不会抛出异常。转换失败则返回def
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns>UInt</returns>
        public static uint AsUInt32(this string value, uint def = default(uint))
        {
            uint ret;
            if (!uint.TryParse(value, out ret))
                ret = def;
            return ret;
        }

        /// <summary>
        /// 转换为ULong，不会抛出异常。转换失败则返回def
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns>ULong</returns>
        public static ulong AsUInt64(this string value, ulong def = default(ulong))
        {
            ulong ret;
            if (!ulong.TryParse(value, out ret))
                ret = def;
            return ret;
        }
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
                Log.Current.Warn(typeof(Convert), ex.Message, ex);
                ret = new byte[0];
            }
            return ret;
        }
        #endregion
    }
    #endregion
}