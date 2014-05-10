using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tatan.Common.UnitTest
{
    using Newtonsoft.Json;
    using Tatan.Common;
    using Tatan.Common.Serialization;

    public enum TestState
    {
        STAT_IN_WORD,
        STAT_OUT_WORD
    }

    [TestClass]
    public class DFATest
    {
        [TestMethod]
        public void DFARunTest()
        {
            var count = 0;
            var dfa = new DFA(2);
            dfa.AddState(TestState.STAT_IN_WORD, (c) =>
            {
                if (Char.IsLetterOrDigit(c))
                    return TestState.STAT_IN_WORD;
                else
                {
                    count++;
                    return TestState.STAT_OUT_WORD;
                }
            });
            dfa.AddState(TestState.STAT_OUT_WORD, (c) =>
            {
                if (Char.IsLetterOrDigit(c))
                    return TestState.STAT_IN_WORD;
                else
                    return TestState.STAT_OUT_WORD;
            });
            dfa.EndHandler = (s) => {
                if (s.Equals(TestState.STAT_IN_WORD))
                {
                    count++;
                }
            };
            dfa.Run("i am a man", TestState.STAT_OUT_WORD);

            Assert.AreEqual(count, 4);
        }

        [TestMethod]
        public void DFARunTest1()
        {
            var count = 0;
            var dfa = new DFA();
            dfa.AddState(TestState.STAT_IN_WORD, (c) =>
            {
                if (Char.IsLetterOrDigit(c))
                    return TestState.STAT_IN_WORD;
                else
                {
                    count++;
                    return TestState.STAT_OUT_WORD;
                }
            });
            dfa.AddState(TestState.STAT_OUT_WORD, (c) =>
            {
                if (Char.IsLetterOrDigit(c))
                    return TestState.STAT_IN_WORD;
                else
                    return TestState.STAT_OUT_WORD;
            });
            dfa.EndHandler = (s) =>
            {
                if (s.Equals(TestState.STAT_IN_WORD))
                {
                    count++;
                }
            };
            dfa.Run("i am a man", TestState.STAT_OUT_WORD);

            Assert.AreEqual(count, 4);
        }

        [TestMethod]
        public void DFARunArgumentNull1Test()
        {
            var count = 0;
            var dfa = new DFA(2);
            dfa.AddState(TestState.STAT_IN_WORD, (c) =>
            {
                if (Char.IsLetterOrDigit(c))
                    return TestState.STAT_IN_WORD;
                else
                {
                    count++;
                    return TestState.STAT_OUT_WORD;
                }
            });
            dfa.AddState(TestState.STAT_OUT_WORD, (c) =>
            {
                if (Char.IsLetterOrDigit(c))
                    return TestState.STAT_IN_WORD;
                else
                    return TestState.STAT_OUT_WORD;
            });
            dfa.EndHandler = (s) =>
            {
                if (s.Equals(TestState.STAT_IN_WORD))
                {
                    count++;
                }
            };
            try
            {
                dfa.Run(null, TestState.STAT_OUT_WORD);
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "参数为空。\r\n参数名: tokens");
            }
        }

        [TestMethod]
        public void DFARunArgumentNull2Test()
        {
            var count = 0;
            var dfa = new DFA(2);
            dfa.AddState(TestState.STAT_IN_WORD, (c) =>
            {
                if (Char.IsLetterOrDigit(c))
                    return TestState.STAT_IN_WORD;
                else
                {
                    count++;
                    return TestState.STAT_OUT_WORD;
                }
            });
            dfa.AddState(TestState.STAT_OUT_WORD, (c) =>
            {
                if (Char.IsLetterOrDigit(c))
                    return TestState.STAT_IN_WORD;
                else
                    return TestState.STAT_OUT_WORD;
            });
            dfa.EndHandler = (s) =>
            {
                if (s.Equals(TestState.STAT_IN_WORD))
                {
                    count++;
                }
            };
            try
            {
                dfa.Run("", null);
            }
            catch (System.Exception e)
            {
                Assert.AreEqual(e.Message, "参数为空。\r\n参数名: beginState");
            }
        }

        [TestMethod]
        public void DFAClearTest()
        {
            var count = 0;
            var dfa = new DFA(2);
            dfa.AddState(TestState.STAT_IN_WORD, (c) =>
            {
                if (Char.IsLetterOrDigit(c))
                    return TestState.STAT_IN_WORD;
                else
                {
                    count++;
                    return TestState.STAT_OUT_WORD;
                }
            });
            dfa.AddState(TestState.STAT_OUT_WORD, (c) =>
            {
                if (Char.IsLetterOrDigit(c))
                    return TestState.STAT_IN_WORD;
                else
                    return TestState.STAT_OUT_WORD;
            });
            dfa.EndHandler = (s) =>
            {
                if (s.Equals(TestState.STAT_IN_WORD))
                {
                    count++;
                }
            };
            dfa.Clear();
            dfa.Run("i am a man", TestState.STAT_OUT_WORD);
            Assert.AreEqual(count, 0);
        }
    }
}
