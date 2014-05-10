// ReSharper disable once CheckNamespace
namespace Tatan.Data.ObjectQuery
{
    /// <summary>
    /// 
    /// </summary>
    public class CaseWhenExpression : IExpression
    {
        /// <summary>
        /// 别名
        /// </summary>
        private readonly string _alias = string.Empty;

        public override string ToString()
        {
            return base.ToString() + " AS " + _alias;
        }
    }
}
