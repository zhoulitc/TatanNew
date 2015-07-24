using System;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tatan.Common.Extension.Enum;
using Tatan.Common.Extension.Stream.Convert;


namespace Tatan.Common.UnitTest
{
    using Extension.String.Convert;
    using Extension.String.Target;
    using Extension;

    [TestClass]
    public class StreamTest
    {
        private static void CommonConvertTest<T>(T value, T min, T max, T def) where T : struct
        {
            var t = typeof(T);
            System.IO.MemoryStream s;

            //测试正常值
            using (s = new System.IO.MemoryStream())
            {
                s.Write(value);
                s.Position = 0;
                Assert.AreEqual(s.Read<T>(), value);
            }

            //测试空值
            s = null;
            Assert.AreEqual(s.Read(def), def);

            //测试越界值
            using (s = new System.IO.MemoryStream())
            {
                s.Write(min);
                s.Position = 0;
                Assert.AreEqual(s.Read<T>(), min);
            }
            using (s = new System.IO.MemoryStream())
            {
                s.Write(max);
                s.Position = 0;
                Assert.AreEqual(s.Read<T>(), max);
            }

            //测试offet越界
            using (s = new System.IO.MemoryStream())
            {
                s.Write(value);
                Assert.AreEqual(s.Read(def), def);
            }
        }

        [TestMethod]
        public void TestConvert()
        {
            CommonConvertTest<short>(-1233, short.MinValue, short.MaxValue, -1);
            CommonConvertTest<ushort>(1233, ushort.MinValue, ushort.MaxValue, 1);
            CommonConvertTest(-12323123, int.MinValue, int.MaxValue, -1);
            CommonConvertTest<uint>(1233231, uint.MinValue, uint.MaxValue, 1);
            CommonConvertTest(-1212312421221321333, long.MinValue, long.MaxValue, -1);
            CommonConvertTest<ulong>(12123124212321321333, ulong.MinValue, ulong.MaxValue, 1);
            CommonConvertTest(1.79769e+208, double.MinValue, double.MaxValue, -1);
            CommonConvertTest(-3.40282e+028f, float.MinValue, float.MaxValue, -1);

            var value = "娃哈哈";
            System.IO.MemoryStream s;

            //测试正常值
            using (s = new System.IO.MemoryStream())
            {
                s.Write(value);
                s.Position = 0;
                Assert.AreEqual(s.Read(), value);
            }

            //测试空值
            s = null;
            Assert.AreEqual(s.Read(), string.Empty);

            //测试offet越界
            using (s = new System.IO.MemoryStream())
            {
                s.Write(value);
                Assert.AreEqual(s.Read(), string.Empty);
            }

            //测试正常值
            using (s = new System.IO.MemoryStream())
            {
                s.Write(false);
                s.Position = 0;
                Assert.AreEqual(s.Read<bool>(), false);
            }

            //测试空值
            s = null;
            Assert.AreEqual(s.Read<bool>(), default(bool));

            //测试offet越界
            using (s = new System.IO.MemoryStream())
            {
                s.Write(value);
                Assert.AreEqual(s.Read<bool>(), default(bool));
            }
        }
    }
}
