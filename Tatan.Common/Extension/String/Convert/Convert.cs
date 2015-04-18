using System.Reflection;
using Tatan.Common.Extension.Reflect;

namespace Tatan.Common.Extension.String.Convert
{
    using Logging;
    using System;
    using System.Text;

    internal delegate bool TryParse<T>(string value, out T def);

    #region 提供字符串的转换扩展方法
    /// <summary>
    /// 提供字符串的转换扩展方法
    /// <para>此方法组不会抛出异常</para>
    /// </summary>
    public static class Convert
    {
        private static class Traits<T> where T : struct
        {
            public static TryParse<T> Call;
        }

        private static class Extend<T>
        {
            public static Func<string, T, T> Call;
        }

        private static void SetParse<T>() where T : struct
        {
            var methods = typeof (T).GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (var method in methods)
            {
                if (method.Name == "TryParse" && method.GetParameters().Length == 2)
                {
                    method.CreateDelegate(out Traits<T>.Call);
                    return;
                }
            }
        }

        static Convert()
        {
            SetParse<int>();
            SetParse<uint>();
            SetParse<byte>();
            SetParse<sbyte>();
            SetParse<short>();
            SetParse<ushort>();
            SetParse<long>();
            SetParse<ulong>();

            SetParse<float>();
            SetParse<double>();
            SetParse<decimal>();

            SetParse<bool>();
            SetParse<char>();
            SetParse<DateTime>();
            SetParse<Guid>();

            SetExtend<bool>("AsBooleanExtend");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="T"></typeparam>
        public static void SetExtend<T>(string name)
        {
            typeof (Convert).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static)
                .CreateDelegate(out Extend<T>.Call);
        }

        // ReSharper disable once UnusedMember.Local
        private static bool AsBooleanExtend(string s, bool def)
        {
            return s.Trim() == "1" || def;
        }

        /// <summary>
        /// 转换为特定的值类型，不会抛出异常，转换失败则返回def
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="def"></param>
        /// <exception cref="System.Exception">调用扩展转换时抛出的异常</exception>
        /// <returns></returns>
        public static T As<T>(this string value, T def = default(T)) where T : struct
        {
            if (string.IsNullOrEmpty(value))
                return def;

            T ret;
            if (Traits<T>.Call == null || !Traits<T>.Call(value, out ret))
            {
                ret = Extend<T>.Call != null ? Extend<T>.Call(value, def) : def;
            }
            return ret;
        }

        /// <summary>
        /// 转换为枚举，不会抛出异常。转换失败则返回def
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public static T AsEnum<T>(this string value, T def = default(T)) where T : struct
        {
            if (string.IsNullOrEmpty(value))
                return def;

            T ret;
            if (!Enum.TryParse(value, out ret))
            {
                ret = Extend<T>.Call != null ? Extend<T>.Call(value, def) : def;
            }
            return ret;
        }

        #region 转换为Bytes
        /// <summary>
        /// 转换为Bytes，不会抛出异常。转换失败则返回空的Bytes
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding">指定转换编码</param>
        /// <returns>byte数组</returns>
        public static byte[] AsBytes(this string value, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(value))
                return new byte[0];

            byte[] ret;
            try
            {
                ret = (encoding ?? Encoding.Default).GetBytes(value);
            }
            catch (EncoderFallbackException ex)
            {
                Log.Warn(ex.Message, ex);
                ret = new byte[0];
            }
            return ret;
        }
        #endregion
    }
    #endregion
}