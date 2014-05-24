// ReSharper disable once CheckNamespace
namespace Tatan.Data.ObjectQuery
{
    /// <summary>
    /// 
    /// </summary>
    public class FunctionExpression : IExpression
    {
        private const string _symbol = "*";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="function"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static FunctionExpression Create(string function, string field)
        {
            return new FunctionExpression(function, field);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregate"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static FunctionExpression Create(Aggregate aggregate, string field)
        {
            return new FunctionExpression(aggregate.ToString().ToUpper() + "(" + _symbol + ")", field);
        }

        private readonly string _field;
        private readonly string _function;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="function"></param>
        /// <param name="field"></param>
        public FunctionExpression(string function, string field)
        {
            _field = field ?? string.Empty;
            _function = string.IsNullOrEmpty(function) ? _symbol : function;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _function.Replace(_symbol, _field);
        }
    }
}