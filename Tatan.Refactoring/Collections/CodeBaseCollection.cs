namespace Tatan.Refactoring.Collections
{
    using Common.Collections;
    using Common.Exception;

    /// <summary>
    /// 代码基类集合
    /// </summary>
    /// <typeparam name="T">代码类型</typeparam>
    public class CodeBaseCollection<T> : ReadOnlyCollection<T>
    {
        /// <summary>
        /// 获取代码类型
        /// </summary>
        /// <param name="name"></param>
        public T this[string name]
        {
            get
            {
                Assert.ArgumentNotNull("name", name);
                Assert.KeyFound(Collection, name);
                return Collection[name];
            }
            internal set
            {
                Assert.ArgumentNotNull("name", name);
               // Assert.ArgumentNotNull("value", value);
                if (Contains(name))
                    Collection[name] = value;
                else
                    Collection.Add(name, value);
            }
        }
    }
}