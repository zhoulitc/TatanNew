// ReSharper disable once CheckNamespace
namespace Tatan.Data
{
    using Common.Exception;

    /// <summary>
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public static class DataEntityExtension
    {
        /// <summary>
        /// 插入一条实体记录到数据库中，需要先设置一个默认的数据源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool Insert<T>(this T entity)
            where T : class, IDataEntity
        {
            var source = DataSource.Default;
            Assert.ArgumentNotNull(nameof(source), source);
            return source.Tables.Get<T>().Insert(entity);
        }

        /// <summary>
        /// 删除一条实体记录到数据库中，需要先设置一个默认的数据源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool Delete<T>(this T entity)
            where T : class, IDataEntity
        {
            var source = DataSource.Default;
            Assert.ArgumentNotNull(nameof(source), source);
            return source.Tables.Get<T>().Delete(entity);
        }

        /// <summary>
        /// 更新一条实体记录到数据库中，需要先设置一个默认的数据源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool Update<T>(this T entity)
            where T : class, IDataEntity
        {
            var source = DataSource.Default;
            Assert.ArgumentNotNull(nameof(source), source);
            return source.Tables.Get<T>().Update(entity);
        }
    }
}