// ReSharper disable once CheckNamespace
namespace Tatan.Data.ObjectQuery
{
    using System.Text;

    /// <summary>
    /// 条件表达式类
    /// </summary>
    public class WhereExpression : IExpression
    {
        /// <summary>
        /// 数据提供者
        /// </summary>
        public static IDataProvider Provider { get; set; }

        #region 私有字段
        private readonly StringBuilder _experssion; //表达式

        //表达式相关静态字符串
        private const string _eq = "=";
        private const string _ne = "!=";
        private const string _lt = "<";
        private const string _le = "<=";
        private const string _gt = ">";
        private const string _ge = ">=";
        private const string _or = "OR";
        private const string _and = "AND";
        private const string _lb = "(";
        private const string _rb = ")";
        private const string _comma = ",";
        private const string _in = "IN";
        private const string _notin = "NOT IN";
        private const string _exist = "EXISTS";
        private const string _notexist = "NOT EXISTS";
        private const string _is = "IS";
        private const string _notis = "NOT IS";
        private const string _null = "NULL";
        private const string _like = "LIKE";

        #endregion

        #region 私有构造函数
        /// <summary>
        /// 私有构造函数
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="join">连接符</param>
        /// <param name="right">右表达式</param>
        private WhereExpression(string left, string join, object right)
        {
            _experssion = new StringBuilder(left.Length + join.Length + 10);
            if (right is string)
                _experssion.AppendFormat("{0}{1}'{2}'", left, join, right);
            else
                _experssion.AppendFormat("{0}{1}{2}", left, join, right);
        }
        #endregion

        #region 私有静态方法
        /// <summary>
        /// 用and或者or连接表达式
        /// </summary>
        /// <param name="oper">and或者or</param>
        /// <param name="expressions">多个表达式对象</param>
        /// <returns>表达式对象</returns>
        private static IExpression _AndOr(string oper, params IExpression[] expressions)
        {
            if (null == expressions || expressions.Length <= 0)
                return new NullExpression();
            var ret = expressions[0];
            for (var i = 1; i < expressions.Length; i++)
            {
                ret = new WhereExpression(ret.ToString(), oper, expressions[i].ToString());
            }
            if (expressions.Length > 1)
                ret = new WhereExpression(_lb, ret.ToString(), _rb);
            return ret;
        }

        /// <summary>
        /// IN、NOT IN、EXISTS、NOT EXISTS表达式
        /// </summary>
        /// <param name="oper">(not) in/exists</param>
        /// <param name="name">参数名</param>
        /// <param name="values">参数值集合</param>
        /// <returns>表达式对象</returns>
        private static IExpression _NotInExists(string oper, string name, params object[] values)
        {
            if (string.IsNullOrEmpty(name) ||
                null == values ||
                values.Length <= 0)
                return new NullExpression();

            //5表示一个参数值的平均长度
            var ret = new StringBuilder(values.Length * 5);
            ret.AppendFormat("{0}{1}{2}", _lb, Provider.ParameterSymbol, values[0]);
            for (var i = 1; i < values.Length; i++)
            {
                ret.AppendFormat("{0}{1}{2}", _comma, Provider.ParameterSymbol, values[i]);
            }
            ret.Append(_rb);
            return new WhereExpression(name.ToUpper(), oper, ret.ToString());
        }

        /// <summary>
        /// IN、NOT IN、EXISTS、NOT EXISTS表达式
        /// </summary>
        /// <param name="oper">(not) in/exists</param>
        /// <param name="name">参数名</param>
        /// <param name="select">数据查询接口</param>
        /// <returns>表达式对象</returns>
        private static IExpression _NotInExists(string oper, string name, IOQL select)
        {
            if (string.IsNullOrEmpty(name) ||
                null == select)
                return new NullExpression();
            return new WhereExpression(name.ToUpper(), oper, _lb + select.ToString() + _rb);
        }

        /// <summary>
        /// 创建一个比较表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="operand">操作符</param>
        /// <returns>表达式对象</returns>
        private static IExpression _Create(string name, string operand)
        {
            if (string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(operand))
                return new NullExpression();
            return new WhereExpression(name.ToUpper(), operand, Provider.ParameterSymbol + name);
        }

        /// <summary>
        /// 创建一个比较表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="operand">操作符</param>
        /// <param name="value">参数值</param>
        /// <returns>表达式对象</returns>
        private static IExpression _Create(string name, string operand, object value)
        {
            if (string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(operand) ||
                value == null)
                return new NullExpression();
            return new WhereExpression(name.ToUpper(), operand, value);
        }

        /// <summary>
        /// 创建一个比较表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="operand">操作符</param>
        /// <param name="select">数据查询接口</param>
        /// <returns>表达式对象</returns>
        private static IExpression _Create(string name, string operand, IOQL select)
        {
            if (null == select)
                return _Create(name, operand);
            return _Create(name, operand, select.ToString());
        }
        #endregion

        #region 公有静态方法
        /// <summary>
        /// 创建一个等于比较表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>表达式对象</returns>
        public static IExpression Equal(string name)
        {
            return _Create(name, _eq);
        }

        /// <summary>
        /// 创建一个等于比较表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>表达式对象</returns>
        public static IExpression Equal(string name, object value)
        {
            return _Create(name, _eq, value);
        }

        /// <summary>
        /// 创建一个等于比较表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="select">数据查询接口</param>
        /// <returns>表达式对象</returns>
        public static IExpression Equal(string name, IOQL select)
        {
            return _Create(name, _eq, select);
        }

        /// <summary>
        /// 创建一个不等于比较表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>表达式对象</returns>
        public static IExpression NotEqual(string name)
        {
            return _Create(name, _ne);
        }

        /// <summary>
        /// 创建一个不等于比较表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>表达式对象</returns>
        public static IExpression NotEqual(string name, object value)
        {
            return _Create(name, _ne, value);
        }

        /// <summary>
        /// 创建一个不等于比较表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="select">数据查询接口</param>
        /// <returns>表达式对象</returns>
        public static IExpression NotEqual(string name, IOQL select)
        {
            return _Create(name, _ne, select);
        }

        /// <summary>
        /// 创建一个大于比较表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>表达式对象</returns>
        public static IExpression GreatThan(string name)
        {
            return _Create(name, _gt);
        }

        /// <summary>
        /// 创建一个大于比较表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>表达式对象</returns>
        public static IExpression GreatThan(string name, object value)
        {
            return _Create(name, _gt, value);
        }

        /// <summary>
        /// 创建一个大于比较表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="select">数据查询接口</param>
        /// <returns>表达式对象</returns>
        public static IExpression GreatThan(string name, IOQL select)
        {
            return _Create(name, _gt, select);
        }

        /// <summary>
        /// 创建一个大于等于比较表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>表达式对象</returns>
        public static IExpression GreatEqual(string name)
        {
            return _Create(name, _ge);
        }

        /// <summary>
        /// 创建一个大于等于比较表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>表达式对象</returns>
        public static IExpression GreatEqual(string name, object value)
        {
            return _Create(name, _ge, value);
        }

        /// <summary>
        /// 创建一个大于等于比较表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="select">数据查询接口</param>
        /// <returns>表达式对象</returns>
        public static IExpression GreatEqual(string name, IOQL select)
        {
            return _Create(name, _ge, select);
        }

        /// <summary>
        /// 创建一个小于比较表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>表达式对象</returns>
        public static IExpression LessThan(string name)
        {
            return _Create(name, _lt);
        }

        /// <summary>
        /// 创建一个小于比较表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>表达式对象</returns>
        public static IExpression LessThan(string name, object value)
        {
            return _Create(name, _lt, value);
        }

        /// <summary>
        /// 创建一个小于比较表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="select">数据查询接口</param>
        /// <returns>表达式对象</returns>
        public static IExpression LessThan(string name, IOQL select)
        {
            return _Create(name, _lt, select);
        }

        /// <summary>
        /// 创建一个小于等于比较表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>表达式对象</returns>
        public static IExpression LessEqual(string name)
        {
            return _Create(name, _le);
        }

        /// <summary>
        /// 创建一个小于等于比较表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>表达式对象</returns>
        public static IExpression LessEqual(string name, object value)
        {
            return _Create(name, _le, value);
        }

        /// <summary>
        /// 创建一个小于等于比较表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="select">数据查询接口</param>
        /// <returns>表达式对象</returns>
        public static IExpression LessEqual(string name, IOQL select)
        {
            return _Create(name, _le, select);
        }

        /// <summary>
        /// 用and连接表达式
        /// </summary>
        /// <param name="expressions">多个表达式对象</param>
        /// <returns>表达式对象</returns>
        public static IExpression And(params IExpression[] expressions)
        {
            return _AndOr(_and, expressions);
        }

        /// <summary>
        /// 用or连接表达式
        /// </summary>
        /// <param name="expressions">多个表达式对象</param>
        /// <returns>表达式对象</returns>
        public static IExpression Or(params IExpression[] expressions)
        {
            return _AndOr(_or, expressions);
        }

        /// <summary>
        /// IN表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="values">参数值集合</param>
        /// <returns>表达式对象</returns>
        public static IExpression In(string name, params object[] values)
        {
            return _NotInExists(_in, name, values);
        }

        /// <summary>
        /// IN表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="select">数据查询接口</param>
        /// <returns>表达式对象</returns>
        public static IExpression In(string name, IOQL select)
        {
            return _NotInExists(_in, name, select);
        }

        /// <summary>
        /// NOT IN表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="values">参数值集合</param>
        /// <returns>表达式对象</returns>
        public static IExpression NotIn(string name, params object[] values)
        {
            return _NotInExists(_notin, name, values);
        }

        /// <summary>
        /// NOT IN表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="select">数据查询接口</param>
        /// <returns>表达式对象</returns>
        public static IExpression NotIn(string name, IOQL select)
        {
            return _NotInExists(_notin, name, select);
        }

        /// <summary>
        /// EXISTS表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="values">参数值集合</param>
        /// <returns>表达式对象</returns>
        public static IExpression Exists(string name, params object[] values)
        {
            return _NotInExists(_exist, name, values);
        }

        /// <summary>
        /// EXISTS表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="select">数据查询接口</param>
        /// <returns>表达式对象</returns>
        public static IExpression Exists(string name, IOQL select)
        {
            return _NotInExists(_exist, name, select);
        }

        /// <summary>
        /// NOT EXISTS表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="values">参数值集合</param>
        /// <returns>表达式对象</returns>
        public static IExpression NotExists(string name, params object[] values)
        {
            return _NotInExists(_notexist, name, values);
        }

        /// <summary>
        /// NOT EXISTS表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="select">数据查询接口</param>
        /// <returns>表达式对象</returns>
        public static IExpression NotExists(string name, IOQL select)
        {
            return _NotInExists(_notexist, name, select);
        }

        /// <summary>
        /// IS NULL表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>表达式对象</returns>
        public static IExpression IsNull(string name)
        {
            if (string.IsNullOrEmpty(name))
                return new NullExpression();
            return new WhereExpression(name.ToUpper(), _is, _null);
        }

        /// <summary>
        /// IS NULL表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>表达式对象</returns>
        public static IExpression NotIsNull(string name)
        {
            if (string.IsNullOrEmpty(name))
                return new NullExpression();
            return new WhereExpression(name.ToUpper(), _notis, _null);
        }

        /// <summary>
        /// LIKE表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="isBeforeMatch">是否前匹配</param>
        /// <returns>表达式对象</returns>
        public static IExpression Like(string name, object value, bool isBeforeMatch = false)
        {
            if (string.IsNullOrEmpty(name) ||
                value == null)
                return new NullExpression();
            if (Provider.Name == "MySql") //TODO 这里要判断MySql
            {
                string likeValue = string.Format("{0}{1}, '{2}'",
                Provider.ParameterSymbol, value,
                Provider.FuzzyMatchingSymbol);
                if (isBeforeMatch)
                    likeValue = string.Format("'{0}', {1}",
                        Provider.FuzzyMatchingSymbol, likeValue);
                return new WhereExpression(name.ToUpper(), _like, string.Format("CONCAT({0})", likeValue));
            }
            else
            {
                string likeValue = string.Format("{0}{1} {2} '{3}'",
                    Provider.ParameterSymbol, value,
                    Provider.StringSplicingSymbol,
                    Provider.FuzzyMatchingSymbol);
                if (isBeforeMatch)
                    likeValue = string.Format("'{0}' {1} {2}",
                        Provider.FuzzyMatchingSymbol,
                        Provider.StringSplicingSymbol, likeValue);
                return new WhereExpression(name.ToUpper(), _like, likeValue);
            }
        }
        #endregion

        #region IExperssion
        /// <summary>
        /// 获取表达式的字符串描述
        /// </summary>
        /// <returns>字符串描述</returns>
        public override string ToString()
        {
            return _experssion.ToString();
        }
        #endregion

        internal class NullExpression : IExpression
        {
            public override string ToString()
            {
                return "1 = 1";
            }
        }
    }
}