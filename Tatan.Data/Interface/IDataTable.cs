// ReSharper disable once CheckNamespace
namespace Tatan.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Builder;

    /// <summary>
    /// 数据表
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public interface IDataTable
    {
        /// <summary>
        /// 获取表名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 获取数据源
        /// </summary>
        IDataSource DataSource { get; }

        /// <summary>
        /// 获取表的Sql构造器
        /// </summary>
        SqlBuilder SqlBuilder { get; }

        /// <summary>
        /// 添加一个属于此表的实体
        /// <para>使用此方法还需要使用Add方法才可以将元素加入到集合中</para>
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <exception cref="System.ArgumentNullException">参数为空时抛出</exception>
        /// <returns>数据项</returns>
        bool Insert<T>(T entity) 
            where T : class, IDataEntity;

        /// <summary>
        /// 移除一个实体
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="System.ArgumentNullException">参数为空时抛出</exception>
        /// <returns></returns>
        bool Delete(string id);

        /// <summary>
        /// 移除一个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="System.ArgumentNullException">参数为空时抛出</exception>
        /// <returns></returns>
        bool Delete<T>(T entity) 
            where T : class, IDataEntity;

        /// <summary>
        /// 根据模式表达式，移除指定的记录
        /// </summary>
        /// <param name="condition">表达式</param>
        /// <exception cref="System.ArgumentNullException">参数为空时抛出</exception>
        /// <returns>删除条数</returns>
        int Delete<T>(Expression<Func<T, bool>> condition)
            where T : class, IDataEntity;

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="System.ArgumentNullException">参数为空时抛出</exception>
        /// <returns></returns>
        bool Update<T>(T entity) 
            where T : class, IDataEntity;

        /// <summary>
        /// 根据模式表达式，更新指定的记录
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="condition"></param>
        /// <exception cref="System.ArgumentNullException">参数为空时抛出</exception>
        /// <returns></returns>
        int Update<T>(object entity, Expression<Func<T, bool>> condition)
            where T : class, IDataEntity;

        /// <summary>
        /// 根据模式表达式，更新指定的记录
        /// </summary>
        /// <param name="sets"></param>
        /// <param name="condition"></param>
        /// <exception cref="System.ArgumentNullException">参数为空时抛出</exception>
        /// <returns></returns>
        int Update<T>(IDictionary<string, object> sets, Expression<Func<T, bool>> condition) 
            where T : class, IDataEntity;

        /// <summary>
        /// 获取此表的记录数
        /// </summary>
        /// <returns></returns>
        long Count();

        /// <summary>
        /// 获取此表的记录数
        /// </summary>
        /// <exception cref="System.ArgumentNullException">参数为空时抛出</exception>
        /// <returns></returns>
        long Count<T>(Expression<Func<T, bool>> condition)
            where T : class, IDataEntity;

        /// <summary>
        /// 构建一个新实体
        /// </summary>
        /// <param name="id">唯一标识</param>
        /// <param name="creator">创建者</param>
        /// <param name="createdTime">创建时间</param>
        /// <returns></returns>
        T NewEntity<T>(string id, string creator = null, string createdTime = null)
            where T : class, IDataEntity;


        /// <summary>
        /// 查询实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IReadOnlyList<T> Query<T>(Func<IDataQuery<T>, IDataQuery<T>> queryFunction)
            where T : class, IDataEntity;

        /// <summary>
        /// 查询唯一实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        T Query<T>(string id)
            where T : class, IDataEntity;
    }
}