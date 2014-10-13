using System.Collections.Generic;
using System.Reflection;

namespace Tatan.Common.Extension
{
    using Logging;
    using System;

    #region 提供值类型的转换扩展方法
    /// <summary>
    /// 提供值类型的转换扩展方法
    /// <para>此方法组不会抛出异常</para>
    /// </summary>
    public static class Convert
    {
        private static readonly IDictionary<string, MethodInfo> _parses;
        private static readonly object _lock;

        static Convert()
        {
            _lock = new object();
            _parses = new Dictionary<string, MethodInfo>();
        }

        #region 转换为泛型值类型，必须指定def
        /// <summary>
        /// 转换为泛型值类型，必须指定def，不会抛出异常。转换失败则返回def
        /// <para>运用了反射，效率上比特定的转换要慢</para>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">默认值</param>
        /// <returns>泛型值类型</returns>
        public static T As<T>(this IConvertible value, T def = default(T)) where T : struct, IConvertible
        {
            if (value == null)
                return def;
            var ret = def;
            var type = typeof(T);
            if (!_parses.ContainsKey(type.FullName))
            {
                lock (_lock)
                {
                    if (!_parses.ContainsKey(type.FullName))
                    {
                        try
                        {
                            _parses.Add(type.FullName, type.GetMethod("Parse", new[] {typeof (string)}));
                        }
                        catch (Exception ex)
                        {
                            _parses.Add(type.FullName, null);
                            Log.Current.Warn(typeof(Convert), ex.Message, ex);
                        }
                    }
                }
            }
            var method = _parses[type.FullName];
            if (method == null)
                return ret;
            try
            {
                ret = (T)method.Invoke(null, new object[] { value.ToString() });
            }
            catch (Exception ex)
            {
                Log.Current.Warn(typeof(Convert), ex.Message, ex);
            }
            return ret;
        }
        #endregion
    }
    #endregion
}