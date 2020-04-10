using HRL.DAL;
using HRL.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HRL.BLL
{
    public abstract class BaseService<T> : BaseContextDbSession where T : class,new()
    {
        public BaseService()
        {
            SetCurrentRepository();
        }
        public IBaseRepository<T> CurrentRepository;
        /// <summary>
        /// 抽象方法的作用：让子类实例化具体的当前仓储对象 IBaseRepository<T> CurrentRepository,
        /// </summary>
        public abstract void SetCurrentRepository();

        public int AddEntity(T entity)
        {
            var backEntity = CurrentRepository.AddEntity(entity);
            return DbSession.SaveChange();
        }

        public int DeleteEntity(T entity)
        {
            var backEntity = CurrentRepository.DeleteEntity(entity);
            return DbSession.SaveChange();
        }

        public int UpdateEntity(T entity)
        {
            var backEntity = CurrentRepository.UpdateEntity(entity);
            return DbSession.SaveChange();
        }

        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            return CurrentRepository.LoadEntities(whereLambda);
        }

        public IQueryable<T> LoadPagingEntities<C>(Expression<Func<T, bool>> whereLambda, int pageSize, int pageIndex, out int count, Expression<Func<T, C>> orderbyLambda, bool isAsc)
        {
            return CurrentRepository.LoadPagingEntities<C>(whereLambda, pageSize, pageIndex, out count, orderbyLambda, isAsc);
        }

    }
}
