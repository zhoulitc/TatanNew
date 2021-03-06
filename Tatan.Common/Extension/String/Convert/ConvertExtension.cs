﻿namespace Tatan.Common.Extension.String.Convert
{
    using System;
    using System.Text;
    using System.Reflection;
    using Logging;
    using Reflect;
    using Serialization;

    internal delegate bool TryParse<T>(string value, out T def);

    #region 提供字符串的转换扩展方法

    /// <summary>
    /// 提供字符串的转换扩展方法
    /// <para>author:zhoulitcqq</para>
    /// <para>此方法组不会抛出异常</para>
    /// </summary>
    public static class ConvertExtension
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

        static ConvertExtension()
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
            typeof (ConvertExtension).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static)
                .CreateDelegate(out Extend<T>.Call);
        }

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
        public static T AsValue<T>(this string value, T def = default(T)) where T : struct
        {
            if (string.IsNullOrEmpty(value))
                return def;

            T ret;
            if (typeof(T).IsEnum)
            {
                if (!Enum.TryParse(value, out ret))
                {
                    ret = Extend<T>.Call != null ? Extend<T>.Call(value, def) : def;
                }
                return ret;
            }
            if (Traits<T>.Call == null || !Traits<T>.Call(value, out ret))
            {
                ret = Extend<T>.Call != null ? Extend<T>.Call(value, def) : def;
            }
            return ret;
        }

        #region 转换为对象

        /// <summary>
        /// 将字符串反序列化成指定对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T AsObject<T>(this string value) where T : class
        {
            if (string.IsNullOrEmpty(value)) return null;

            value = value.Trim();
            if (SameJson(value))
                return Serializers.Json.Deserialize<T>(value);
            else if (SameXml(value))
                return Serializers.Xml.Deserialize<T>(value);
            else
                return null;
        }

        private static bool SameJson(string value)
        {
            return (value.StartsWith("{") && value.EndsWith("}")) || (value.StartsWith("[") && value.EndsWith("]"));
        }

        private static bool SameXml(string value)
        {
            return (value.StartsWith("<?xml") && value.EndsWith(">"));
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