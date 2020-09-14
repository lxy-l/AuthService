
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Repository
{
    /// <summary>
    /// 异步仓储接口
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    public interface IRepositoryAsync<TEntity>
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>成功则True，反之False</returns>
        Task<bool> Add(TEntity entity);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="listEntity">实体列表</param>
        /// <returns>受影响行数</returns>
        Task<int> Add(IEnumerable<TEntity> listEntity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>成功则True，反之False</returns>
        Task<bool> DeleteById(object id);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>成功则True，反之False</returns>
        Task<bool> Delete(TEntity entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="listEntity">实体列表</param>
        /// <returns>受影响行数</returns>
        Task<int> Delete(IEnumerable<TEntity> listEntity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="filter">条件</param>
        /// <returns>受影响行数</returns>
        Task<int> DeleteWhere(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>成功则True，反之False</returns>
        Task<bool> Update(TEntity entity);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="whereExpression">条件</param>
        /// <param name="updateExpression">修改字段</param>
        /// <returns>受影响行数</returns>
        Task<int> Update(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity , TEntity >> updateExpression);

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="isAsNoTracking">是否跟踪对象</param>
        /// <returns>实体</returns>
        Task<TEntity> Find(object id, bool isAsNoTracking = false);

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        Task<IQueryable<TEntity>> Get();

        /// <summary>
        /// 符合条件的数据条数
        /// </summary>
        /// <param name="filter">条件</param>
        /// <returns>数量</returns>
        Task<int> Count(Expression<Func<TEntity, bool>> filter = null);
        /// <summary>
        /// 是否存在符合条件的数据
        /// </summary>
        /// <param name="filter">条件</param>
        /// <returns>存在True，反之False</returns>
        Task<bool> Any(Expression<Func<TEntity, bool>> filter = null);
    }
}
