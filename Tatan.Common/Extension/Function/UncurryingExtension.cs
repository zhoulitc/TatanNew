namespace Tatan.Common.Extension.Function
{
    using System;

    #region 提供逆柯里化的扩展方法

    /// <summary>
    /// 逆柯里化
    /// <para>author:zhoulitcqq</para>
    /// <para>此方法组不会抛出异常</para>
    /// </summary>
    public static class UncurryingExtension
    {
        /// <summary>
        /// 二阶逆柯里化
        /// <para>f(a)(b)=>f(a,b)</para>
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        public static Func<T1, T2, TResult>
            Uncurrying<T1, T2, TResult>(
            this Func<T1, Func<T2, TResult>> function)
            => (t1, t2) => function(t1)(t2);

        /// <summary>
        /// 三阶逆柯里化
        /// <para>f(a)(b)(c)=>f(a,b,c)</para>
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        public static Func<T1, T2, T3, TResult>
            Uncurrying<T1, T2, T3, TResult>(
            this Func<T1, Func<T2, Func<T3, TResult>>>  function)
            => (t1, t2, t3) => function(t1)(t2)(t3);

        /// <summary>
        /// 四阶逆柯里化
        /// <para>f(a)(b)(c)(d)=>f(a,b,c,d)</para>
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        public static Func<T1, T2, T3, T4, TResult>
            Uncurrying<T1, T2, T3, T4, TResult>(
            this Func<T1, Func<T2, Func<T3, Func<T4, TResult>>>> function)
            => (t1, t2, t3, t4) => function(t1)(t2)(t3)(t4);

        /// <summary>
        /// 五阶逆柯里化
        /// <para>f(a)(b)(c)(d)(e)=>f(a,b,c,d,e)</para>
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        public static Func<T1, T2, T3, T4, T5, TResult>
            Uncurrying<T1, T2, T3, T4, T5, TResult>(
            this Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, TResult>>>>> function)
            => (t1, t2, t3, t4, t5) => function(t1)(t2)(t3)(t4)(t5);
    }

    #endregion
}