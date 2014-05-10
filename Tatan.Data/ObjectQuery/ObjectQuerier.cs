namespace Tatan.Data.ObjectQuery
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// 对象查询器
    /// <para>Select[->Top][->Distinct][->{_}]</para>
    /// <para>From[->{_}][->Join]</para>
    /// <para>[->Where][->GroupBy[->Having]][->OrderBy]</para>
    /// <para>[->From][->{_}....]</para>
    /// <remarks>中括号表示步骤可选，大括号表示方法可无限叠加</remarks>
    /// </summary>
    public static class ObjectQuerier
    {
        /// <summary>
        /// 表数据选择
        /// </summary>
        /// <param name="fields">选择字段集合</param>
        /// <returns>选择接口</returns>
        public static ISelect Select(params string[] fields)
        {
            return new InternalSelect(fields);
        }

        #region ISelect
        internal class InternalSelect : ISelect
        {
            #region 私有字段
            private string _distinct;       //distinct子句
            private string _top;            //top子句
            private StringBuilder _list;    //list子句
            #endregion

            #region 构造函数
            /// <summary>
            /// 内联构造函数
            /// </summary>
            /// <param name="fields">字段集合</param>
            public InternalSelect(IEnumerable<string> fields)
            {
                _distinct = string.Empty;
                _top = string.Empty;
                _list = new StringBuilder();
                AddList(fields);
            }
            #endregion

            #region 公有方法

            #region ISelect
            /// <summary>
            /// 追加字段名
            /// </summary>
            /// <param name="field">字段名</param>
            /// <param name="fields">其他字段名</param>
            /// <returns>选择接口</returns>
            public ISelect _(string field, params string[] fields)
            {
                if (!string.IsNullOrEmpty(field) && field.ToUpper() != _distinct.ToUpper())
                    _list.Append(" , " + field);
                AddList(fields);
                return this;
            }

            /// <summary>
            /// 追加CaseWhen表达式
            /// </summary>
            /// <param name="expression">表达式</param>
            /// <param name="expressions">其他表达式</param>
            /// <returns>选择接口</returns>
            public ISelect _(CaseWhenExpression expression, params CaseWhenExpression[] expressions)
            {
                AddExpression(expression, expressions);
                return this;
            }

            /// <summary>
            /// 追加函数表达式
            /// </summary>
            /// <param name="expression">表达式</param>
            /// <param name="expressions">其他表达式</param>
            /// <returns>选择接口</returns>
            public ISelect _(FunctionExpression expression, params FunctionExpression[] expressions)
            {
                AddExpression(expression, expressions);
                return this;
            }

            /// <summary>
            /// 设置包含集合
            /// </summary>
            /// <param name="table">表名</param>
            /// <param name="tables">其他表名</param>
            /// <returns>集合接口</returns>
            public IFrom From(string table, params string[] tables)
            {
                Adjust(_list);
                if (string.IsNullOrEmpty(table) && (null == tables || 0 == tables.Length))
                    return new NullFrom();
                return new InternalFrom(GetList(), table, tables);
            }

            /// <summary>
            /// 为对应字段设置不重复选项
            /// </summary>
            /// <param name="field">字段名</param>
            /// <returns>选择接口</returns>
            public ISelect Distinct(string field)
            {
                if (!string.IsNullOrEmpty(_distinct))
                    return this;

                _distinct = field;
                int index = _list.ToString().IndexOf(_distinct);
                if (index < 0)
                    return this;

                _list = _list.Replace(string.Format((index == 0) ? "{0} , " : " , {0}", _distinct), string.Empty);
                return this;
            }

            /// <summary>
            /// 设置TOP语句
            /// </summary>
            /// <param name="n">前n行或者n%行</param>
            /// <param name="isPercent">是否为百分比</param>
            /// <returns>选择接口</returns>
            public ISelect Top(uint n, bool isPercent = false)
            {
                _top = string.Format("TOP {0} {1} ", n, isPercent ? "PERCENT" : string.Empty);
                return this;
            }

            /// <summary>
            /// 返回Empty，没有表名无法查询
            /// </summary>
            /// <returns>返回空字符串</returns>
            public override string ToString()
            {
                return string.Empty;
            }
            #endregion

            #endregion

            #region 私有方法
            //追加字段
            private void AddList(IEnumerable<string> fields)
            {
                if (null == fields)
                    return;
                foreach (var f in fields)
                    if (f.ToUpper() != _distinct.ToUpper())
                        _list.AppendFormat(",{0}", f);
            }

            //追加表达式
            private void AddExpression(IExpression expression, IEnumerable<IExpression> expressions)
            {
                if (expression != null)
                    _list.AppendFormat(" , {0}", expression.ToString());
                if (null == expressions)
                    return;
                foreach (var e in expressions)
                {
                    if (e != null)
                        _list.AppendFormat(" , {0}", e.ToString());
                }
            }

            //获取完整的select语句
            private string GetList()
            {
                return string.Format("{0} {1} {2}",
                    _top,
                    string.IsNullOrEmpty(_distinct) ? string.Empty : string.Format("DISTINCT {0} , ", _distinct),
                    (0 == _list.Length) ? "*" : _list.ToString()).Trim();
            }

            //调整集合
            private static void Adjust(StringBuilder s)
            {
                if (s.Length > 0 && ' ' == s[0])
                    s.Remove(0, 3);
            }
            #endregion
        }
        #endregion

        #region IFrom
        internal class InternalFrom : IFrom
        {
            #region 私有字段
            private readonly string _select;                        //select子句
            private readonly StringBuilder _froms = new StringBuilder();     //from子句
            private readonly StringBuilder _joins = new StringBuilder();     //join子句
            private bool _joined;
            #endregion

            #region 构造函数
            /// <summary>
            /// 内联构造函数
            /// </summary>
            /// <param name="select">select列表</param>
            /// <param name="table">表名</param>
            /// <param name="tables">其他表名</param>
            public InternalFrom(string select, string table, params string[] tables)
            {
                _select = select;
                _froms.Append(table);
                foreach (string e in tables)
                    if (!string.IsNullOrEmpty(e))
                        _froms.AppendFormat(",{0}", e);
            }
            #endregion

            #region 公有方法

            #region IOQL
            /// <summary>
            /// 获取OQL对应的SQL
            /// </summary>
            /// <returns>SQL语句</returns>
            public override string ToString()
            {
                return string.Format("SELECT {0} FROM {1} {2}", _select, _froms, _joins).Trim();
            }
            #endregion

            #region IFrom
            /// <summary>
            /// 追加表
            /// </summary>
            /// <param name="table">表名</param>
            /// <param name="tables">其他表名</param>
            /// <returns>集合接口</returns>
            public IFrom _(string table, params string[] tables)
            {
                if (table != null)
                    _froms.AppendFormat(" , {0}", table);
                if (tables != null)
                    foreach (string e in tables)
                        if (e != null)
                            _froms.AppendFormat(" , {0}", e);
                return this;
            }

            /// <summary>
            /// 追加数据查询集合
            /// </summary>
            /// <param name="select">数据查询集合</param>
            /// <param name="selects">其他数据查询集合</param>
            /// <returns>集合接口</returns>
            public IFrom _(IOQL select, params IOQL[] selects)
            {
                if (select != null)
                    _froms.AppendFormat(" , ({0})", select.ToString());
                if (selects == null) return this;
                foreach (IOQL s in selects)
                    if (s != null)
                        _froms.AppendFormat(" , ({0})", s.ToString());
                return this;
            }

            /// <summary>
            /// 连接两个集合
            /// </summary>
            /// <param name="left">左表</param>
            /// <param name="join">连接类型枚举</param>
            /// <param name="right">右表</param>
            /// <param name="expression">ON表达式</param>
            /// <returns>集合接口</returns>
            public IFrom Join(string left, Join join, string right, IExpression expression)
            {
                if (_joined || string.IsNullOrEmpty(left) || string.IsNullOrEmpty(right) || join == ObjectQuery.Join.Cross)
                    return this;

                RemoveTable(left);
                RemoveTable(right);
                _joins.AppendFormat(" {0} {1} JOIN {2} ON {3} ", left, join.ToString().ToUpper(), right, expression.ToString());
                _joined = true;
                return this;
            }

            /// <summary>
            /// 检索条件
            /// </summary>
            /// <param name="expression">表达式对象</param>
            /// <returns>数据查询接口</returns>
            public IWhere Where(IExpression expression)
            {
                if (null == expression)
                    return new InternalWhere(_select, _froms.ToString(), " WHERE ", "1 = 1");
                return new InternalWhere(_select, _froms.ToString(), _joins + " WHERE ", expression.ToString());
            }
            #endregion

            #endregion

            #region 私有方法
            //从from子句中移除一个表名
            private void RemoveTable(string table)
            {
                var index = _froms.ToString().IndexOf(table);
                if (index == 0)
                {
                    _froms.Remove(0, table.Length);
                }
                else if (index > 0)
                {
                    _froms.Remove(index - 3, table.Length + 3);
                }
            }
            #endregion
        }

        internal class NullFrom : IFrom
        {
            public IFrom _(string table, params string[] tables) 
            {
                return this; 
            }

            public IFrom _(IOQL select, params IOQL[] selects) 
            { 
                return this; 
            }

            public IFrom Join(string left, Join join, string right, IExpression expression) 
            { 
                return this; 
            }

            public IWhere Where(IExpression expression) 
            {
                return new NullWhere(); 
            }
        
            public override string ToString() 
            {
                return string.Empty; 
            }
        }
        #endregion

        #region IWhere
        internal class InternalWhere : IWhere
        {
            #region 私有字段
            private readonly string _list;          //select子句
            private readonly string _froms;         //from子句
            private readonly string _joins;         //join子句
            private readonly string _where;         //where子句
            private readonly StringBuilder _groupBy;//groupby子句
            private readonly StringBuilder _orderBy;//orderby子句
            #endregion

            #region 构造函数
            /// <summary>
            /// 内联构造函数
            /// </summary>
            /// <param name="list">select列表</param>
            /// <param name="froms">from集合</param>
            /// <param name="joins">join子句</param>
            /// <param name="expression">表达式</param>
            public InternalWhere(string list, string froms, string joins, string expression)
            {
                _list = list;
                _froms = froms;
                _joins = joins;
                _where = expression;
                _groupBy = new StringBuilder();
                _orderBy = new StringBuilder();
            }
            #endregion

            #region 公有方法

            #region IOQL
            /// <summary>
            /// 获取OQL对应的SQL
            /// </summary>
            /// <returns>SQL语句</returns>
            public override string ToString()
            {
                return string.Format("SELECT {0} FROM {1} {2} {3} {4} {5}", _list, _froms, _joins, _where, _groupBy, _orderBy).Trim();
            }
            #endregion

            #region IWhere
            /// <summary>
            /// 设置分组子句
            /// </summary>
            /// <param name="field">字段名</param>
            /// <param name="fields">其他字段名</param>
            /// <returns>数据查询接口</returns>
            public IWhere GroupBy(string field, params string[] fields)
            {
                if (_groupBy.Length > 0)
                    return this;

                if (!string.IsNullOrEmpty(field))
                    _groupBy.Append(field);
                if (fields != null && fields.Length > 0)
                    foreach (string f in fields)
                        if (!string.IsNullOrEmpty(field))
                            _groupBy.AppendFormat(",{0}", f);

                if (_groupBy.Length > 0)
                    _groupBy.Insert(0, "GROUP BY ");
                return this;
            }

            /// <summary>
            /// 设置分组条件子句，需要先设置分组子句
            /// </summary>
            /// <param name="expression">条件表达式</param>
            /// <returns>数据查询接口</returns>
            public IWhere Having(IExpression expression)
            {
                if (_groupBy.Length == 0 || _groupBy.ToString().IndexOf(" HAVING ") > 0)
                    return this;

                if (null != expression)
                    _groupBy.AppendFormat(" HAVING {0} ", expression.ToString());
                return this;
            }

            /// <summary>
            /// 设置排序子句
            /// </summary>
            /// <param name="sort">排序类型</param>
            /// <param name="field">字段名</param>
            /// <param name="fields">其他字段名</param>
            /// <returns>数据查询接口</returns>
            public IWhere OrderBy(string sort, string field, params string[] fields)
            {
                if (_orderBy.Length > 0)
                    return this;

                if (!string.IsNullOrEmpty(field))
                    _orderBy.Append(field);
                if (fields != null && fields.Length > 0)
                    foreach (string f in fields)
                        if (!string.IsNullOrEmpty(field))
                            _orderBy.AppendFormat(" , {0}", f);
                if (_orderBy.Length > 0)
                {
                    _orderBy.AppendFormat(" {0}", "desc" == sort ? "DESC" : "ASC");
                    _orderBy.Insert(0, "ORDER BY ");
                }
                return this;
            }

            /// <summary>
            /// 包含集合操作
            /// </summary>
            /// <param name="oper">集合操作类型</param>
            /// <param name="select">数据查询接口</param>
            /// <param name="selects">其他数据查询接口</param>
            /// <returns>集合接口</returns>
            public IFrom From(SetMerge oper, IOQL select, params IOQL[] selects)
            {
                if (null == select && (null == selects || 0 == selects.Length))
                    return new NullFrom();

                var setEnum = oper.ToString().ToUpper();
                if (oper == SetMerge.UnionAll)
                    setEnum = "UNION ALL";

                var sets = new StringBuilder();
                if (select != null)
                    sets.AppendFormat("{0} ({1}) ", setEnum, select.ToString());
                if (selects != null)
                    foreach (var s in selects)
                        if (s != null)
                            sets.AppendFormat("{0} ({1}) ", setEnum, s.ToString());

                return new InternalFrom(_list, string.Format("({0}) {1}", ToString(), sets));
            }
            #endregion

            #endregion
        }

        internal class NullWhere : IWhere
        {
            public IWhere GroupBy(string field, params string[] fields)
            {
                return this;
            }

            public IWhere Having(IExpression expression)
            {
                return this;
            }

            public IWhere OrderBy(string sort, string field, params string[] fields)
            {
                return this;
            }

            public IFrom From(SetMerge oper, IOQL select, params IOQL[] selects)
            {
                return new NullFrom();
            }

            public override string ToString()
            {
                return string.Empty;
            }
        }
        #endregion
    }    
}