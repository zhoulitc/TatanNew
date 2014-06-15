namespace Tatan.Common.Expression
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using Collections;
    using Exception;

    /// <summary>
    /// 表达数解析器
    /// </summary>
    public static class ExpressionParser
    {
        /// <summary>
        /// 解析一个判断表达数，返回判断字符串以及参数字典
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="symbol"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ParserResult Parse<T>(Expression<Func<T, bool>> expression, string symbol)
        {
            ExceptionHandler.ArgumentNull("symbol", symbol);
            var visitor = new ExpressionParserVisitor(symbol);
            visitor.Visit(expression);
            return visitor.Result.Trim();
        }

        /// <summary>
        /// 解析一个赋值表达式，返回赋值字符串
        /// </summary>
        /// <param name="sets"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static ParserResult Parse(IDictionary<string, object> sets, string symbol)
        {
            ExceptionHandler.ArgumentNull("sets", sets);
            ExceptionHandler.ArgumentNull("symbol", symbol);
            var set = new ParserResult(sets);
            foreach (var key in sets.Keys)
            {
                set.AppendFormat("{0}={1}{2},", key, symbol, key);
            }
            return set.Trim();
        }

        /// <summary>
        /// 解析一个赋值表达式，返回赋值字符串
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static ParserResult Parse(object entity, string symbol)
        {
            ExceptionHandler.ArgumentNull("entity", entity);
            ExceptionHandler.ArgumentNull("symbol", symbol);
            var properties = entity.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var set = new ParserResult();
            foreach (var property in properties)
            {
                set.AppendFormat("{0}={1}{2},", property.Name, symbol, property.Name);
                set.Add(property.Name, property.GetValue(entity));
            }
            return set.Trim();
        }

        #region ParserResult
        /// <summary>
        /// 解析结果
        /// </summary>
        public class ParserResult
        {
// ReSharper disable once InconsistentNaming
            internal StringBuilder _condition;
// ReSharper disable once InconsistentNaming
            internal IDictionary<string, object> _parameters;

            internal ParserResult Trim()
            {
                var result = _condition.ToString();
                if (result.StartsWith("(") && result.EndsWith(")"))
                    _condition = _condition.Remove(0, 1).Remove(_condition.Length - 1, 1);
                if (result.EndsWith(","))
                    _condition = _condition.Remove(result.Length - 1, 1);
                return this;
            }

            internal void SetNull()
            {
                if (_condition.ToString().EndsWith("!="))
                {
                    _condition = _condition.Remove(_condition.Length - 2, 2);
                    _condition.Append(" IS NOT NULL");
                }
                else if (_condition.ToString().EndsWith("="))
                {
                    _condition = _condition.Remove(_condition.Length - 1, 1);
                    _condition.Append(" IS NULL");
                }
            }

            internal ParserResult Append(string value)
            {
                _condition.Append(value);
                return this;
            }

            internal ParserResult AppendFormat(string format, params object[] values)
            {
                _condition.AppendFormat(format, values);
                return this;
            }

            internal void Add(string key, object value)
            {
                if (!_parameters.ContainsKey(key))
                    _parameters.Add(key, value);
            }

            internal int Count
            {
                get { return _parameters.Count; }
            }

            /// <summary>
            /// 
            /// </summary>
            public ParserResult(IDictionary<string, object> parameters = null)
            {
                _condition = new StringBuilder();
                _parameters = parameters == null ? 
                    new Dictionary<string, object>() : 
                    new Dictionary<string, object>(parameters);
            }

            /// <summary>
            /// 表达式串
            /// </summary>
            public string Condition
            {
                get { return _condition.ToString(); }
            }

            /// <summary>
            /// 参数集合
            /// </summary>
            public IEnumerable<KeyValuePair<string, object>> Parameters
            {
                get { return _parameters; }
            }
        }
        #endregion

        private class ExpressionParserVisitor : ExpressionVisitor
        {
            private readonly ParserResult _result;
            private readonly Queue<string> _queue;
            private readonly string _symbol;

            private static readonly ListMap<ExpressionType, string> _types;
            private static readonly string _paramName;

            static ExpressionParserVisitor()
            {
                _paramName = "param";
                _types = new ListMap<ExpressionType, string>(8)
                {
                    {ExpressionType.AndAlso, " AND "},
                    {ExpressionType.OrElse, " OR "},
                    {ExpressionType.Equal, "="},
                    {ExpressionType.NotEqual, "!="},
                    {ExpressionType.LessThan, "<"},
                    {ExpressionType.LessThanOrEqual, "<="},
                    {ExpressionType.GreaterThan, ">"},
                    {ExpressionType.GreaterThanOrEqual, ">="}
                };
            }

            public ParserResult Result 
            {
                get { return _result; }
            }

            public ExpressionParserVisitor(string symbol)
            {
                _result = new ParserResult();
                _queue = new Queue<string>();
                _symbol = symbol;
            }

            protected override Expression VisitBinary(BinaryExpression node)
            {
                _result.Append("(");
                Visit(node.Left);
                if (!_types.Contains(node.NodeType))
                    ExceptionHandler.NotSupported();
                _result.Append(_types[node.NodeType]);
                Visit(node.Right);
                _result.Append(")");
                return node;
            }

            protected override Expression VisitConstant(ConstantExpression node)
            {
                if (_queue.Count > 0)
                {
                    var name = _queue.Dequeue();
                    if (node.Value == null)
                    {
                        _result.SetNull();
                        return node;
                    }
                    _result.Append(_symbol).Append(name);
                    _result.Add(name, GetValue(node.Value));
                    return node;
                }
                _result.Append(node.Value.ToString());
                _queue.Enqueue(node.Value.ToString());
                return node;
            }

            private object GetValue(object value)
            {
                if (value is bool)
                    return ((bool)value) ? 1 : 0;
                return value;
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                if (node.Expression != null && node.Expression.NodeType == ExpressionType.Parameter)
                {
                    _result.Append(node.Member.Name);
                    _queue.Enqueue(_paramName + (_result.Count + 1));
                    return node;
                }
                if (node.Type.Name == "DBNull")
                {
                    _result.SetNull();
                    return node;
                }
                ExceptionHandler.NotSupported();
                return node;
            }
        }
    }
}