namespace Tatan.Common.Extension.Function
{
    using System;

    #region 提供固化的扩展方法

    /// <summary>
    /// 固化
    /// <para>author:zhoulitcqq</para>
    /// <para>此方法组不会抛出异常</para>
    /// </summary>
    public static class FixedExtension
    {
        /// <summary>
        /// 二阶固化
        /// <para>f(a,b)+fix(a)=>f(b)</para>
        /// </summary>
        /// <param name="function"></param>
        /// <param name="fix"></param>
        /// <returns></returns>
        public static Func<T2, TResult> 
            Fixed<T1, T2, TResult>(
            this Func<T1, T2, TResult> function, Func<T1> fix)
            => t2 => function(fix(), t2);

        /// <summary>
        /// 二阶固化
        /// <para>f(a,b)+fix(a)=>f(b)</para>
        /// </summary>
        /// <param name="function"></param>
        /// <param name="fix"></param>
        /// <returns></returns>
        public static Func<T2, TResult>
            Fixed<T1, T2, TResult>(
            this Func<T1, T2, TResult> function, T1 fix)
            => t2 => function(fix, t2);

        /// <summary>
        /// 三阶固化
        /// <para>f(a,b,c)+fix(a)=>f(b)(c)</para>
        /// </summary>
        /// <param name="function"></param>
        /// <param name="fix"></param>
        /// <returns></returns>
        public static Func<T2, T3, TResult>
            Fixed<T1, T2, T3, TResult>(
            this Func<T1, T2, T3, TResult> function, Func<T1> fix)
            => (t2, t3) => function(fix(), t2, t3);

        /// <summary>
        /// 三阶固化
        /// <para>f(a,b,c)+fix(a)=>f(b)(c)</para>
        /// </summary>
        /// <param name="function"></param>
        /// <param name="fix"></param>
        /// <returns></returns>
        public static Func<T2, T3, TResult>
            Fixed<T1, T2, T3, TResult>(
            this Func<T1, T2, T3, TResult> function, T1 fix)
            => (t2, t3) => function(fix, t2, t3);

        /// <summary>
        /// 四阶固化
        /// <para>f(a,b,c,d)+fix(a)=>f(b)(c)(d)</para>
        /// </summary>
        /// <param name="function"></param>
        /// <param name="fix"></param>
        /// <returns></returns>
        public static Func<T2, T3, T4, TResult>
            Fixed<T1, T2, T3, T4, TResult>(
            this Func<T1, T2, T3, T4, TResult> function, Func<T1> fix)
            => (t2, t3, t4) => function(fix(), t2, t3, t4);

        /// <summary>
        /// 四阶固化
        /// <para>f(a,b,c,d)+fix(a)=>f(b)(c)(d)</para>
        /// </summary>
        /// <param name="function"></param>
        /// <param name="fix"></param>
        /// <returns></returns>
        public static Func<T2, T3, T4, TResult>
            Fixed<T1, T2, T3, T4, TResult>(
            this Func<T1, T2, T3, T4, TResult> function, T1 fix)
            => (t2, t3, t4) => function(fix, t2, t3, t4);

        /// <summary>
        /// 五阶固化
        /// <para>f(a,b,c,d,e)+fix(a)=>f(b)(c)(d)(e)</para>
        /// </summary>
        /// <param name="function"></param>
        /// <param name="fix"></param>
        /// <returns></returns>
        public static Func<T2, T3, T4, T5, TResult>
            Fixed<T1, T2, T3, T4, T5, TResult>(
            this Func<T1, T2, T3, T4, T5, TResult> function, Func<T1> fix)
            => (t2, t3, t4, t5) => function(fix(), t2, t3, t4, t5);

        /// <summary>
        /// 五阶固化
        /// <para>f(a,b,c,d,e)+fix(a)=>f(b)(c)(d)(e)</para>
        /// </summary>
        /// <param name="function"></param>
        /// <param name="fix"></param>
        /// <returns></returns>
        public static Func<T2, T3, T4, T5, TResult>
            Fixed<T1, T2, T3, T4, T5, TResult>(
            this Func<T1, T2, T3, T4, T5, TResult> function, T1 fix)
            => (t2, t3, t4, t5) => function(fix, t2, t3, t4, t5);
    }

    #endregion
}