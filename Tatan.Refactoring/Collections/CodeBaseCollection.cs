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
                ExceptionHandler.ArgumentNull("name", name);
                ExceptionHandler.KeyNotFound(Collection, name);
                return Collection[name];
            }
            internal set
            {
                ExceptionHandler.ArgumentNull("name", name);
                ExceptionHandler.ArgumentNull("value", value);
                if (Contains(name))
                    Collection[name] = value;
                else
                    Collection.Add(name, value);
            }
        }
    }
}