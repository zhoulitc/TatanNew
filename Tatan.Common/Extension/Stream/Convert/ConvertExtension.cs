namespace Tatan.Common.Extension.Stream.Convert
{
    using System;
    using System.Text;
    using System.Reflection;
    using Reflect;
    using System.Threading.Tasks;

    #region 提供流的转换扩展方法

    /// <summary>
    /// 提供字符串的转换扩展方法
    /// <para>author:zhoulitcqq</para>
    /// <para>此方法组不会抛出异常</para>
    /// </summary>
    public static class ConvertExtension
    {
        private static class Traits<T> where T : struct
        {
            public static Func<byte[], int, T> Call;
        }

        private static class Bytes<T> where T : struct
        {
            public static Func<T, byte[]> Call;
        }

        private static void SetParse<T>(string methodName) where T : struct
        {
            var methods = typeof (BitConverter).GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (var method in methods)
            {
                if (method.Name == methodName && method.GetParameters().Length == 2)
                {
                    method.CreateDelegate(out Traits<T>.Call);
                    return;
                }
            }
        }

        private static void SetBytes<T>(string methodName) where T : struct
        {
            var methods = typeof(BitConverter).GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (var method in methods)
            {
                if (method.Name == methodName && method.GetParameters()[0].ParameterType == typeof(T))
                {
                    method.CreateDelegate(out Bytes<T>.Call);
                    return;
                }
            }
        }

        static ConvertExtension()
        {
            SetParse<int>("ToInt32");
            SetParse<uint>("ToUInt32");
            SetParse<short>("ToInt16");
            SetParse<ushort>("ToUInt16");
            SetParse<long>("ToInt64");
            SetParse<ulong>("ToUInt64");

            SetParse<float>("ToSingle");
            SetParse<double>("ToDouble");

            SetParse<bool>("ToBoolean");
            SetParse<char>("ToChar");

            SetBytes<int>("GetBytes");
            SetBytes<uint>("GetBytes");
            SetBytes<short>("GetBytes");
            SetBytes<ushort>("GetBytes");
            SetBytes<long>("GetBytes");
            SetBytes<ulong>("GetBytes");

            SetBytes<float>("GetBytes");
            SetBytes<double>("GetBytes");

            SetBytes<bool>("GetBytes");
            SetBytes<char>("GetBytes");
        }

        /// <summary>
        /// 读取流的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="def"></param>
        /// <exception cref="System.Exception">调用扩展转换时抛出的异常</exception>
        /// <returns></returns>
        public static T Read<T>(this System.IO.Stream value, T def = default(T)) where T : struct
        {
            if (value == null || !value.CanRead || value.Position >= value.Length)
                return def;

            var buffer = new byte[value.Length - value.Position];
            value.Read(buffer, (int)value.Position, buffer.Length);
            if (Traits<T>.Call == null)
            {
                return def;
            }
            return Traits<T>.Call(buffer, 0);
        }

        /// <summary>
        /// 读取流的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public async static Task<T> ReadAsync<T>(this System.IO.Stream value, T def = default(T)) where T : struct
        {
            if (value == null || !value.CanRead || value.Position >= value.Length)
                return def;

            var buffer = new byte[value.Length - value.Position];
            await value.ReadAsync(buffer, (int)value.Position, buffer.Length);
            if (Traits<T>.Call == null)
            {
                return def;
            }
            return Traits<T>.Call(buffer, 0);
        }

        /// <summary>
        /// 读取流的值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Read(this System.IO.Stream value, Encoding encoding = null)
        {
            if (value == null || !value.CanRead || value.Position >= value.Length)
                return string.Empty;

            var buffer = new byte[value.Length - value.Position];
            value.Read(buffer, (int)value.Position, buffer.Length);
            return (encoding ?? Encoding.UTF8).GetString(buffer);
        }

        /// <summary>
        /// 读取流的值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public async static Task<string> ReadAsync(this System.IO.Stream value, Encoding encoding = null)
        {
            if (value == null || !value.CanRead || value.Position >= value.Length)
                return string.Empty;

            var buffer = new byte[value.Length - value.Position];
            await value.ReadAsync(buffer, (int)value.Position, buffer.Length);
            return (encoding ?? Encoding.UTF8).GetString(buffer);
        }


        /// <summary>
        /// 往流中写入值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="s"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static void Write<T>(this System.IO.Stream s, T value, int offset = 0) where T : struct
        {
            if (s == null || !s.CanWrite || Bytes<T>.Call == null)
                return;

            var buffer = Bytes<T>.Call(value);
            s.Write(buffer, offset, buffer.Length);
        }

        /// <summary>
        /// 往流中写入值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="s"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public async static Task WriteAsync<T>(this System.IO.Stream s, T value, int offset = 0) where T : struct
        {
            if (s == null || !s.CanWrite || Bytes<T>.Call == null)
                return;

            var buffer = Bytes<T>.Call(value);
            await s.WriteAsync(buffer, offset, buffer.Length);
        }

        /// <summary>
        /// 往流中写入值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="s"></param>
        /// <param name="offset"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static void Write(this System.IO.Stream s, string value, int offset = 0, Encoding encoding = null)
        {
            if (s == null || !s.CanWrite || string.IsNullOrEmpty(value))
                return;
            
            var buffer = (encoding ?? Encoding.UTF8).GetBytes(value);
            s.Write(buffer, offset, buffer.Length);
        }

        /// <summary>
        /// 往流中写入值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="s"></param>
        /// <param name="offset"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public async static Task WriteAsync(this System.IO.Stream s, string value, int offset = 0, Encoding encoding = null)
        {
            if (s == null || !s.CanWrite || string.IsNullOrEmpty(value))
                return;

            var buffer = (encoding ?? Encoding.UTF8).GetBytes(value);
            await s.WriteAsync(buffer, offset, buffer.Length);
        }
    }

    #endregion
}