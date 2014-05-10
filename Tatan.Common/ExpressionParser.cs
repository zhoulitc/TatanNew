namespace Tatan.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Text;
    using Exception;

    /// <summary>
    /// 表达数解析器
    /// </summary>
    public static class ExpressionParser
    {
        private static readonly ExpressionParserVisitor _visitor = new ExpressionParserVisitor();

        /// <summary>
        /// 解析一个判断表达数，返回判断字符串
        /// </summary>
        /// <param name="expression"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string Parse<T>(Expression<Func<T, bool>> expression)
        {
            _visitor.Reset();
            _visitor.Visit(expression);
            return _visitor.Result;
        }

        /// <summary>
        /// 解析一个赋值表达式，返回赋值字符串
        /// </summary>
        /// <param name="sets"></param>
        /// <returns></returns>
        public static string Parse(IDictionary<string, object> sets)
        {
            var set = new StringBuilder(sets.Count*20);
            foreach (var key in sets.Keys)
            {
                set.AppendFormat("{0}={1}{2},", key, "", key);
            }
            if (set.Length > 0)
                set.Remove(set.Length - 1, 1);
            return set.ToString();
        }

        private class ExpressionParserVisitor : ExpressionVisitor
        {
            private readonly StringBuilder _builder = new StringBuilder();

            public string Result 
            {
                get
                {
                    var result = _builder.ToString();
                    if (result.StartsWith("(") && result.EndsWith(")"))
                        result = result.Substring(1, result.Length - 2);
                    return result;
                }
            }

            public void Reset()
            {
                _builder.Clear();
            }

            protected override Expression VisitBinary(BinaryExpression node)
            {
                _builder.Append("(");
                Visit(node.Left);
                switch (node.NodeType)
                {
                    case ExpressionType.AndAlso:
                        _builder.Append(" AND ");
                        break;
                    case ExpressionType.OrElse:
                        _builder.Append(" OR ");
                        break;
                    case ExpressionType.Equal:
                        _builder.Append("=");
                        break;
                    case ExpressionType.NotEqual:
                        _builder.Append("!=");
                        break;
                    case ExpressionType.LessThan:
                        _builder.Append("<");
                        break;
                    case ExpressionType.LessThanOrEqual:
                        _builder.Append("<=");
                        break;
                    case ExpressionType.GreaterThan:
                        _builder.Append(">");
                        break;
                    case ExpressionType.GreaterThanOrEqual:
                        _builder.Append(">=");
                        break;
                    default:
                        ExceptionHandler.NotSupported();
                        break;
                }
                Visit(node.Right);
                _builder.Append(")");
                return node;
            }

            private bool EndsWithCompare()
            {
                var expression = _builder.ToString();
                return expression.EndsWith("=") || expression.EndsWith("!=") ||
                       expression.EndsWith(">") || expression.EndsWith(">=") ||
                       expression.EndsWith("<") || expression.EndsWith("<=");
            }

            protected override Expression VisitConstant(ConstantExpression node)
            {
                if (node.Value == null)
                {
                    _builder.Append("NULL");
                    return node;
                }
                if (EndsWithCompare())
                {
                    switch (Type.GetTypeCode(node.Value.GetType()))
                    {
                        case TypeCode.Boolean:
                            _builder.Append(((bool) node.Value) ? 1 : 0);
                            break;
                        case TypeCode.DBNull:
                            _builder.Append("NULL");
                            break;
                        case TypeCode.String:
                        case TypeCode.Char:
                            _builder.Append("'");
                            _builder.Append(node.Value);
                            _builder.Append("'");
                            break;
                        case TypeCode.Object:
                            ExceptionHandler.NotSupported();
                            break;
                        default:
                            _builder.Append(node.Value);
                            break;
                    }
                    return node;
                }
                _builder.Append(node.Value);
                return node;
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                if (node.Expression != null && node.Expression.NodeType == ExpressionType.Parameter)
                {
                    _builder.Append(node.Member.Name);
                    return node;
                }
                ExceptionHandler.NotSupported();
                return node;
            }
        }
    }
}