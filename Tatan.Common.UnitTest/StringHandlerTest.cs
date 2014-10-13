using System;
using System.Text;
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
                Assert.AreEqual(e.Message, "非法匹配。");
            }


            try
            {
                r = s.Replace("", "A", t);
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "非法匹配。");
            }
        }

        [TestMethod]
        public void TestConvert()
        {
            var a1 = "false".AsBoolean();
            var a2 = "2321".AsBoolean(true);
            Assert.IsFalse(a1);
            Assert.IsTrue(a2);

            var q1 = byte.MaxValue.ToString().AsInt8();
            var q2 = "s12".AsInt8(2);
            Assert.AreEqual(q1, byte.MaxValue);
            Assert.AreEqual(q2, 2);

            var z1 = "32".AsBytes();
            var z2 = "s12".AsBytes(Encoding.ASCII);
            Assert.AreEqual(z1.Length, 2);
            Assert.AreEqual(z2.Length, 3);

            var w1 = "2014-05-10".AsDateTime();
            var w2 = "s12".AsDateTime(new DateTime(2014, 5, 10));
            Assert.AreEqual(w1, new DateTime(2014,5,10));
            Assert.AreEqual(w2, new DateTime(2014, 5, 10));

            var q11 = decimal.MaxValue.ToString().AsDecimal();
            var q22 = "s12".AsDecimal(2);
            Assert.AreEqual(q11, decimal.MaxValue);
            Assert.AreEqual(q22, 2);

            var q111 = "32.2".ToString().AsDouble();
            var q222 = "s12".AsDouble(2);
            Assert.AreEqual(q111, 32.2);
            Assert.AreEqual(q222, 2);

            var q1111 = "32.2".ToString().AsFloat();
            var q2222 = "s12".AsFloat(2);
            Assert.AreEqual(q1111, 32.2f);
            Assert.AreEqual(q2222, 2);

            var s1 = Guid.New().AsGuid();
            var s2 = Guid.New().AsGuid("x");
            var s3 = "".AsGuid();
            var s4 = "sdsadsa".AsGuid();
            Assert.AreEqual(s1.ToString().Length>0, true);
            Assert.AreEqual(s2.ToString(), "00000000-0000-0000-0000-000000000000");
            Assert.AreEqual(s3.ToString(), "00000000-0000-0000-0000-000000000000");
            Assert.AreEqual(s4.ToString(), "00000000-0000-0000-0000-000000000000");

            var q11111 = int.MaxValue.ToString().AsInt32();
            var q22222 = "s12".AsInt32(2);
            Assert.AreEqual(q11111, int.MaxValue);
            Assert.AreEqual(q22222, 2);

            var q111111 = long.MaxValue.ToString().AsInt64();
            var q222222 = "s12".AsInt64(2);
            Assert.AreEqual(q111111, long.MaxValue);
            Assert.AreEqual(q222222, 2);

            var q1111111 = sbyte.MaxValue.ToString().AsInt8();
            var q2222222 = "s12".AsInt8(2);
            Assert.AreEqual(q1111111, sbyte.MaxValue);
            Assert.AreEqual(q2222222, 2);

            var q11111111 = short.MaxValue.ToString().AsInt16();
            var q22222222 = "s12".AsInt16(2);
            Assert.AreEqual(q11111111, short.MaxValue);
            Assert.AreEqual(q22222222, 2);

            var q111111111 = uint.MaxValue.ToString().AsUInt32();
            var q222222222 = "s12".AsUInt32(2);
            Assert.AreEqual(q111111111, uint.MaxValue);
            Assert.AreEqual(q222222222, (uint)2);

            var q1111111111 = ulong.MaxValue.ToString().AsUInt64();
            var q2222222222 = "s12".AsUInt64(2);
            Assert.AreEqual(q1111111111, ulong.MaxValue);
            Assert.AreEqual(q2222222222, (ulong)2);

            var q11111111111 = ushort.MaxValue.ToString().AsUInt16();
            var q22222222222 = "s12".AsUInt16(2);
            Assert.AreEqual(q11111111111, ushort.MaxValue);
            Assert.AreEqual(q22222222222, (ushort)2);

            var x1 = "32".As<int>();
            //var x2 = "s12".As<Test>(new Test());
            //var x3 = "s12".As<Test2>(new Test2());
            string s = null;
            var x4 = s.As<int>();
            Assert.AreEqual(x1, 32);
            //Assert.IsNotNull(x2);
            //Assert.IsNotNull(x3);
            Assert.IsNotNull(x4);
            var sss = 1;
            var ssss = sss.As<double>();
        }

        [TestMethod]
        public void TestRegex()
        {
            Assert.IsFalse("fals1e".IsBool());
            Assert.IsTrue("false".IsBool());

            Assert.IsTrue("2014-5-10".IsDateTime());
            Assert.IsFalse("2014-51-10".IsDateTime());

            Assert.IsTrue("2014-5-10".IsDateTimeOffset());
            Assert.IsFalse("2014-51-10".IsDateTimeOffset());

            Assert.IsTrue("2014@qq.com".IsEmail());
            Assert.IsFalse("2014@@qq.com".IsEmail());

            Assert.IsTrue("2014".IsInt());
            Assert.IsFalse("2014@".IsInt());


            Assert.IsTrue("2014.3".IsNumber());
            Assert.IsFalse("2014@.3".IsNumber());

            Assert.IsTrue("13899999999".IsPhone());
            Assert.IsFalse("138999999992".IsPhone());

            Assert.IsTrue("13899999999".IsUInt());
            Assert.IsFalse("-138999999992".IsUInt());

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
        }

        [TestMethod]
        public void TestEnum()
        {
            Assert.AreEqual(T1.A.AsInt(), 0);
            Assert.AreEqual(T1.B.AsInt(), 1);
            Assert.AreEqual(T1.A.AsEnum<T2>(), T2.AA);
            try
            {
                var T = T1.B.AsEnum<T2>();
                Assert.AreEqual(T1.B.AsEnum<T2>(), T2.BB);
            }
            catch (System.Exception ex)
            {
                var s = ex.Message;
            }

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
