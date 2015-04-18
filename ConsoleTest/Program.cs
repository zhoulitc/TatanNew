using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tatan.Common.Serialization;

namespace ConsoleTest
{
    class Program
    {
        public class Test5
        {
            public void Do()
            {
            }
        }

        static void Main(string[] args)
        {
            dynamic test5 = new Test5();
            test5.Do();
        }

        public static void CallByValue(int ii, object[] oo)
        {
            ii += 1;
            oo = new object[1];
            oo[0] = "1";
        }

        public class Singletion {
            private Singletion() { }
            private static Singletion _instance;
            private static readonly object _lock = new object();
            public Singletion Instance {
                get {
                    if (_instance == null) {
                        lock (_lock) {
                            if (_instance == null) {
                                _instance = new Singletion();
                            }
                        }
                    }
                    return _instance;
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class CheckRangeAttribute : Attribute
    {
        public int Min { get; set; }

        public int Max { get; set; }
    }

    public class Test3
    {
    }

    public class Test4 : Test3
    {       
    }
}
