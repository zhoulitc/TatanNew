using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tatan.Common.Compiler;

namespace Tatan.Common.UnitTest
{
    using Common;
    using Tatan.Common.Extension.Reflect;

    [TestClass]
    public class ReflectTest
    {
        [TestMethod]
        public void NewTest()
        {
            var aa = new AA
            {
                Name = "wahaha",
                Age = 20,
                Sex = SexType.Nan
            };
            var aa1 = aa.CloneTo<AA, AA1>();
            Assert.AreEqual(aa1.Name, aa.Name);
            Assert.AreEqual(aa1.Age, aa.Age);
            Assert.AreEqual(aa1.Sex, aa.Sex);

            var aa2 = aa.CloneTo<AA, AA2>();
            Assert.AreEqual(aa2.Name, aa.Name);
            Assert.AreEqual(aa2.Age, aa.Age);
            Assert.AreEqual(aa2.Sex, aa.Sex);

            aa = aa2.CloneTo<AA2, AA>();
            Assert.AreEqual(aa2.Name, aa.Name);
            Assert.AreEqual(aa2.Age, aa.Age);
            Assert.AreEqual(aa2.Sex, aa.Sex);

            var aa3 = aa.CloneTo<AA, AA3>();
            Assert.AreEqual(aa.Name, aa3.Name);
            Assert.AreEqual(aa.Age, aa3.Age);
            Assert.AreEqual(aa.Sex, aa3.Sex);

            aa = aa3.CloneTo<AA3, AA>();
            Assert.AreEqual(aa.Name, aa3.Name);
            Assert.AreEqual(aa.Age, aa3.Age);
            Assert.AreEqual(aa.Sex, aa3.Sex);

            var aa4 = aa.CloneTo<AA, AA4>();
            Assert.AreEqual(aa.Name, aa4.Name);
            Assert.AreEqual(aa.Age.ToString(), aa4.Age);
            Assert.AreEqual(aa.Sex, aa4.Sex);

            aa = aa4.CloneTo<AA4, AA>();
            Assert.AreEqual(aa.Name, aa4.Name);
            Assert.AreEqual(aa.Age.ToString(), aa4.Age);
            Assert.AreEqual(aa.Sex, aa4.Sex);

            var aa5 = aa.CloneTo<AA, AA5>();
            Assert.AreEqual(aa.Name, aa5.BigName);
            Assert.AreEqual(aa.Age, aa5.Age);
            Assert.AreEqual(aa.Sex, aa5.Sex);
        }

        [TestMethod]
        public void NewTest1()
        {
            var aa = new AA
            {
                Name = "wahaha",
                Age = 20,
                Sex = SexType.Nan,
                Sub = new SubAA
                {
                    A = 1,
                    B = "s"
                }
            };

            var aa00 = aa.CloneTo<AA>();
            Assert.AreEqual(aa.Name, aa00.Name);
            Assert.AreEqual(aa.Age, aa00.Age);
            Assert.AreEqual(aa.Sex, aa00.Sex);

            var aa55 = aa.CloneTo<AA5>();
            Assert.AreEqual(aa.Name, aa55.BigName);
            Assert.AreEqual(aa.Age, aa55.Age);
            Assert.AreEqual(aa.Sex, aa55.Sex);


            var aa1 = aa.CloneTo<AA1>();
            Assert.AreEqual(aa1.Name, aa.Name);
            Assert.AreEqual(aa1.Age, aa.Age);
            Assert.AreEqual(aa1.Sex, aa.Sex);

            var aa2 = aa.CloneTo<AA2>();
            Assert.AreEqual(aa2.Name, aa.Name);
            Assert.AreEqual(aa2.Age, aa.Age);
            Assert.AreEqual(aa2.Sex, aa.Sex);

            aa = aa2.CloneTo<AA>();
            Assert.AreEqual(aa2.Name, aa.Name);
            Assert.AreEqual(aa2.Age, aa.Age);
            Assert.AreEqual(aa2.Sex, aa.Sex);

            var aa3 = aa.CloneTo<AA3>();
            Assert.AreEqual(aa.Name, aa3.Name);
            Assert.AreEqual(aa.Age, aa3.Age);
            Assert.AreEqual(aa.Sex, aa3.Sex);

            aa = aa3.CloneTo<AA>();
            Assert.AreEqual(aa.Name, aa3.Name);
            Assert.AreEqual(aa.Age, aa3.Age);
            Assert.AreEqual(aa.Sex, aa3.Sex);

            var aa4 = aa.CloneTo<AA4>();
            Assert.AreEqual(aa.Name, aa4.Name);
            Assert.AreEqual(aa.Age.ToString(), aa4.Age);
            Assert.AreEqual(aa.Sex, aa4.Sex);

            aa = aa4.CloneTo<AA>();
            Assert.AreEqual(aa.Name, aa4.Name);
            Assert.AreEqual(aa.Age.ToString(), aa4.Age);
            Assert.AreEqual(aa.Sex, aa4.Sex);

            var aa5 = aa.CloneTo<AA5>();
            Assert.AreEqual(aa.Name, aa5.BigName);
            Assert.AreEqual(aa.Age, aa5.Age);
            Assert.AreEqual(aa.Sex, aa5.Sex);
        }
    }

    public class AA
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public SexType Sex { get; set; }

        public SubAA Sub { get; set; }
    }

    public class AAA
    {
        public object Name { get; set; }

        public int Age { get; set; }

        public SexType Sex { get; set; }
    }

    public partial class AA1
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public SexType Sex { get; set; }

        public string Other { get; set; }
    }

    public partial class AA2
    {
        public object Name { get; set; }

        public int Age { get; set; }

        public SexType Sex { get; set; }

        public string Other { get; set; }
    }

    public partial class AA3
    {
        public string Name { get; set; }

        public object Age { get; set; }

        public SexType Sex { get; set; }

        public string Other { get; set; }
    }

    public partial class AA4
    {
        public string Name { get; set; }

        public string Age { get; set; }

        public SexType Sex { get; set; }

        public string Other { get; set; }
    }

    public partial class AA5
    {
        [Mapping("Name")]
        public string BigName { get; set; }

        public byte Age { get; set; }

        public SexType Sex { get; set; }

        public SubAA Sub { get; set; }

        public string Other { get; set; }
    }

    public class SubAA
    {
        public int A { get; set; }

        public string B { get; set; }
    }

    public enum SexType
    {
        Nv,

        Nan
    }
}
