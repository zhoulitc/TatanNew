using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tatan.Common.UnitTest
{
    using Common;

    public enum TestState
    {
        StatInWord,
        StatOutWord
    }

    [TestClass]
    public class DfaTest
    {
        [TestMethod]
        public void TestRun()
        {
            var count = 0;
            var dfa = new DFA(2);
            dfa.AddState(TestState.StatInWord, c =>
            {
                if (Char.IsLetterOrDigit(c))
                    return TestState.StatInWord;
                count++;
                return TestState.StatOutWord;
            });
            dfa.AddState(TestState.StatOutWord, c =>
            {
                if (Char.IsLetterOrDigit(c))
                    return TestState.StatInWord;
                return TestState.StatOutWord;
            });
            dfa.EndHandler = s => {
                if (s.Equals(TestState.StatInWord))
                {
                    count++;
                }
            };
            dfa.Run("i am a man", TestState.StatOutWord);

            Assert.AreEqual(count, 4);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void Test1Run()
        {
            var count = 0;
            var dfa = new DFA();
            dfa.AddState(TestState.StatInWord, c =>
            {
                if (Char.IsLetterOrDigit(c))
                    return TestState.StatInWord;
                count++;
                return TestState.StatOutWord;
            });
            dfa.AddState(TestState.StatOutWord, c =>
            {
                if (Char.IsLetterOrDigit(c))
                    return TestState.StatInWord;
                return TestState.StatOutWord;
            });
            dfa.EndHandler = s =>
            {
                if (s.Equals(TestState.StatInWord))
                {
                    count++;
                }
            };
            dfa.Run("i am a man", TestState.StatOutWord);
            Assert.AreEqual(count, 4);

            dfa.Run(null, TestState.StatOutWord);
            dfa.Run("", null);
        }

        [TestMethod]
        public void TestClear()
        {
            var count = 0;
            var dfa = new DFA(2);
            dfa.AddState(TestState.StatInWord, c =>
            {
                if (Char.IsLetterOrDigit(c))
                    return TestState.StatInWord;
                count++;
                return TestState.StatOutWord;
            });
            dfa.AddState(TestState.StatOutWord, c =>
            {
                if (Char.IsLetterOrDigit(c))
                    return TestState.StatInWord;
                return TestState.StatOutWord;
            });
            dfa.EndHandler = s =>
            {
                if (s.Equals(TestState.StatInWord))
                {
                    count++;
                }
            };
            dfa.Clear();
            dfa.Run("i am a man", TestState.StatOutWord);
            Assert.AreEqual(count, 0);
        }
    }
}
