using System;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tatan.Common.Extension.Enum;
using Tatan.Common.Extension.String.Regular;


namespace Tatan.Common.UnitTest
{
    using Extension.String.Convert;
    using Extension.String.Target;
    using Extension;

    [TestClass]
    public class StringHandlerTest
    {
        [TestMethod]
        public void TestReplace()
        {
            var s = "1111111111111111111{%key1%}222222222222 3333333333333{%key2%}sd";
            System.Collections.Generic.IDictionary<string, string> t = new System.Collections.Generic.Dictionary<string, string>();
            t.Add("key1", "sfsadasdsa");
            t.Add("key2", "wqeqwrqwewq");
            var r = s.Replace(t);
            Assert.AreEqual(r, "1111111111111111111sfsadasdsa222222222222 3333333333333wqeqwrqwewqsd");

            r = s.Replace("", "", t);
            Assert.AreEqual(r, "1111111111111111111sfsadasdsa222222222222 3333333333333wqeqwrqwewqsd");

            r = s.Replace(null);
            Assert.AreEqual(r, s);

            var s1 = "1111111111111111111<%key1%>222222222222 3333333333333<%key2%>sd";
            var r1 = s1.Replace("<%", "%>", t);
            Assert.AreEqual(r1, "1111111111111111111sfsadasdsa222222222222 3333333333333wqeqwrqwewqsd");

            try
            {
                r = s.Replace("A", "", t);
            }
            catch (System.Exception e)
            {
                Assert.IsTrue(e.Message.Contains("非法匹配。"));
            }


            try
            {
                r = s.Replace("", "A", t);
            }
            catch (System.Exception e)
            {
                Assert.IsTrue(e.Message.Contains("非法匹配。"));
            }
        }

        private static void CommonConvertTest<T>(T value, T min, T max, string lagel, T def) where T : struct
        {
            //测试正常值
            var s = value.ToString().As(def);
            Assert.AreEqual(s, value);

            //测试空值
            string n = null;
            Assert.AreEqual(n.As(def), def);

            //测试越界值
            if (min.ToString() == "0")
            {
                var lr = "-1".As(def);
                Assert.AreEqual(lr, def);
            }
            else
            {
                var lr = (min.ToString() + "0").As(def);
                Assert.AreEqual(lr, def);
            }
            var ur = (max.ToString() + "0").As(def);
            Assert.AreEqual(ur, def);

            //测试非法值
            var lg = lagel.As(def);
            Assert.AreEqual(lg, def);

            //测试前后空格值
            var tm = ("            " + value.ToString() + "            ").As(def);
            Assert.AreEqual(tm, value);
        }

        [TestMethod]
        public void TestConvert()
        {
            CommonConvertTest<byte>(123, byte.MinValue, byte.MaxValue, "12d", 1);
            CommonConvertTest<sbyte>(-123, sbyte.MinValue, sbyte.MaxValue, "12d", -1);
            CommonConvertTest<short>(-1233, short.MinValue, short.MaxValue, "12d", -1);
            CommonConvertTest<ushort>(1233, ushort.MinValue, ushort.MaxValue, "12d", 1);
            CommonConvertTest(-12323123, int.MinValue, int.MaxValue, "12d", -1);
            CommonConvertTest<uint>(1233231, uint.MinValue, uint.MaxValue, "12d", 1);
            CommonConvertTest(-1212312421221321333, long.MinValue, long.MaxValue, "12d", -1);
            CommonConvertTest<ulong>(12123124212321321333, ulong.MinValue, ulong.MaxValue, "12d", 1);
            CommonConvertTest(7922816251464337593543950335m, decimal.MinValue, decimal.MaxValue, "12d", -1);
            CommonConvertTest(1.79769e+208, double.MinValue, double.MaxValue, "12d", -1);
            CommonConvertTest(-3.40282e+028f, float.MinValue, float.MaxValue, "12d", -1);

            var a1 = "false".As<bool>();
            var aa1 = "1".As<bool>();
            var a2 = "2321".As(true);
            Assert.IsFalse(a1);
            Assert.IsTrue(aa1);
            Assert.IsTrue(a2);

            var z1 = "32".AsBytes();
            var z2 = "s12".AsBytes(Encoding.ASCII);
            Assert.AreEqual(z1.Length, 2);
            Assert.AreEqual(z2.Length, 3);
            string dsa = null;
            z1 = dsa.AsBytes();
            Assert.AreEqual(z1.Length, 0);

            var w1 = "2014-05-10".As<DateTime>();
            var w2 = "s12".As(new DateTime(2014, 5, 10));
            Assert.AreEqual(w1, new DateTime(2014,5,10));
            Assert.AreEqual(w2, new DateTime(2014, 5, 10));

            var s1 = Guid.New().As<System.Guid>();
            var s2 = Guid.New().As<System.Guid>();
            var s3 = "".As<System.Guid>();
            var s4 = "sdsadsa".As<System.Guid>();
            Assert.AreEqual(s1.ToString().Length>0, true);
            Assert.AreEqual(s2.ToString().Length > 0, true);
            Assert.AreEqual(s3.ToString(), "00000000-0000-0000-0000-000000000000");
            Assert.AreEqual(s4.ToString(), "00000000-0000-0000-0000-000000000000");
        }

        [TestMethod]
        public void TestRegex()
        {
            Assert.IsFalse("fals1e".IsBoolean());
            Assert.IsTrue("false".IsBoolean());

            Assert.IsTrue("2014-5-10".IsDateTime());
            Assert.IsFalse("2014-51-10".IsDateTime());

            Assert.IsTrue("2014@qq.com".IsEmail());
            Assert.IsFalse("2014@@qq.com".IsEmail());

            Assert.IsTrue("2014".IsInteger());
            Assert.IsFalse("2014@".IsInteger());


            Assert.IsTrue("2014.3".IsNumber());
            Assert.IsFalse("2014@.3".IsNumber());

            Assert.IsTrue("13899999999".IsPhone());
            Assert.IsFalse("138999999992".IsPhone());

            Assert.IsTrue("431021198805280038".IsIdCard18());

            Assert.IsTrue("13899999999".IsMatch(@"^\d"));
            Assert.IsFalse("-138999999992".IsMatch(@"^\d"));
            Assert.IsFalse("-138999999992".IsMatch(null));
            Assert.IsFalse("-138999999992".IsMatch(@"^\d", System.Text.RegularExpressions.RegexOptions.IgnoreCase, -1));
            Assert.IsFalse("-138999999992".IsMatch(@"^\d", System.Text.RegularExpressions.RegexOptions.IgnoreCase, 100));

            var s = "138999999992".Match(@"^\d{1}");
            Assert.AreEqual(s, "1");
            s = "138999999992".Match(null);
            Assert.AreEqual(s, string.Empty);
            s = "-138999999992".Match(@"^\d{1}", start:0, length: 1);
            Assert.AreEqual(s, string.Empty);
            s = "138999999992".Match(null, start:-1);
            Assert.AreEqual(s, string.Empty);
            s = "138999999992".Match(null, start: 100);
            Assert.AreEqual(s, string.Empty);

            var ss = "138999999992".Matches(@"\d{1}");
            Assert.AreEqual(ss.Length, 12);
            ss = "138999999992".Matches(null);
            Assert.AreEqual(ss.Length, 0);
            ss = "138999999992".Matches(null, start: -1);
            Assert.AreEqual(ss.Length, 0);
            ss = "138999999992".Matches(null, start: 1000);
            Assert.AreEqual(ss.Length, 0);

            var ss1 = "true1".Replace(new Regex("true"), "false");
            Assert.AreEqual(ss1, "false1");
            ss1 = "true1".Replace((Regex)null, "false");
            Assert.AreEqual(ss1, "true1");
        }

        [TestMethod]
        public void TestEnum()
        {
            Assert.AreEqual(T1.A.AsInt(), 0);
            Assert.AreEqual(T1.B.AsInt(), 1);
            Assert.AreEqual(T1.A.As<T2>(), T2.AA);
            try
            {
                var T = T1.B.As<T2>();
                Assert.AreEqual(T1.B.As<T2>(), T2.BB);
            }
            catch (System.Exception ex)
            {
                var s = ex.Message;
            }

            Assert.AreEqual("A".AsEnum<T1>(), T1.A);
            Assert.AreEqual("0".AsEnum<T2>(), T2.AA);
            string ss = null;
            Assert.AreEqual(ss.AsEnum<T1>(T1.A), T1.A);
        }

        public struct Test
        {
            
        }

        public struct Test2
        {
            public static Test2 Parse(string s)
            {
                return new Test2();
            }
        }

        public enum T1
        {
            A,
            B
        }

        public enum T2
        {
            AA=0,
            BB=2
        }
    }
}
