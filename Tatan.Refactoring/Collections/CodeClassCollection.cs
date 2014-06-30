using System.Linq;

namespace Tatan.Refactoring.Collections
{
    using Codes;

    /// <summary>
    /// 代码类集合
    /// </summary>
    public class CodeClassCollection : CodeBaseCollection<CodeClass>
    {
        /// <summary>
        /// 判断一个文件中的类集合中是否存在至少一个公共类
        /// </summary>
        public bool HasPublicClass
        {
            get
            {
                return Collection.Any(pair => pair.Value.Accessibility == CodeAccessibility.Public);
            }
        }
    }
}