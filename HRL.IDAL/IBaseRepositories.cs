using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HRL.IDAL
{
    public interface IBaseRepository<T> where T : class,new()
    {
        /// <summary>
        /// 增加一个实体，返回增加的实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T AddEntity(T entity);
        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T DeleteEntity(T entity);
        /// <summary>
        /// 修改一个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T UpdateEntity(T entity);
        /// <summary>
        /// 查找指定条件下的实体集合
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda);
        /// <summary>
        /// 查找分页实体集合
        /// </summary>
        /// <typeparam name="C">排序的字段</typeparam>
        /// <param name="whereLambda">索引条件</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="count">总行数</param>
        /// <param name="orderbyLambda">排序条件</param>
        /// <param name="isAsc"></param> 
        /// <returns>如果是asc为true ，表示升序，反之降序</returns>
        IQueryable<T> LoadPagingEntities<C>(Expression<Func<T, bool>> whereLambda, int pageSize, int pageIndex, out int count, Expression<Func<T, C>> orderbyLambda, bool isAsc);
    }
}
