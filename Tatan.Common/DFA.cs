namespace Tatan.Common
{
    using System;
    using System.Collections.Generic;
    using Exception;

    /// <summary>
    /// 有穷状态机，默认字符型
    /// </summary>
// ReSharper disable once InconsistentNaming
    public class DFA : DFA<char> 
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="capacity">状态个数</param>
        public DFA(int capacity = 0)
            : base(capacity)
        {
        }
    }

    /// <summary>
    /// 有穷状态机
    /// </summary>
    /// <typeparam name="T">token类型</typeparam>
// ReSharper disable once InconsistentNaming
    public class DFA<T>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="capacity">状态个数</param>
        public DFA(int capacity = 0)
        {
            StateFunctions = capacity == 0 ? 
                new Dictionary<Enum, Func<T, Enum>>() : 
                new Dictionary<Enum, Func<T, Enum>>(capacity);
        }

        /// <summary>
        /// 添加一个状态到状态字典
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="function">处理状态的行为</param>
        /// <exception cref="System.ArgumentException">传入相同的状态时</exception>
        public void AddState(Enum state, Func<T, Enum> function)
        {
            StateFunctions.Add(state, function);
        }

        /// <summary>
        /// 清空所有状态
        /// </summary>
        public void Clear()
        {
            StateFunctions.Clear();
            EndHandler = null;
        }

        /// <summary>
        /// 状态机的状态字典
        /// </summary>
        protected IDictionary<Enum, Func<T, Enum>> StateFunctions { get; set; }

        /// <summary>
        /// 状态机运行时的状态
        /// </summary>
        protected Enum State { get; set; }    

        /// <summary>
        /// 设置状态机结束的处理
        /// </summary>
        public Action<Enum> EndHandler { protected get; set; }

        /// <summary>
        /// 运行状态机，请确保所有状态都在状态字典中
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="beginState">开始状态</param>
        /// <exception cref="System.ArgumentNullException">传入参数为空时</exception>
        public void Run(IEnumerable<T> tokens, Enum beginState)
        {
            ExceptionHandler.ArgumentNull("tokens", tokens);
            ExceptionHandler.ArgumentNull("beginState", beginState);

            State = beginState;
            if (StateFunctions.Count > 0)
            {
                foreach (var token in tokens)
                {
                    CallStateFunction(token);
                }
            }
            if (EndHandler != null)
                EndHandler(State);
        }

        /// <summary>
        /// 状态机调用函数
        /// </summary>
        /// <param name="token"></param>
        protected void CallStateFunction(T token)
        {
            if (StateFunctions.ContainsKey(State))
                State = StateFunctions[State](token);
        }
    }
}