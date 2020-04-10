using HRL.IDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HRL.DAL
{
    public class BaseRepository<T> where T : class,new()
    {
        private DbContext dbContext;
        IDbContextFactory DbContextFactory = new EFContextFactory();
        public BaseRepository()
        {
            dbContext = DbContextFactory.GetCurrentContextInstence();
        }
        public T AddEntity(T entity)
        {
            dbContext.Entry<T>(entity).State = EntityState.Added;
            return entity;
        }

        public T DeleteEntity(T entity)
        {
            dbContext.Entry<T>(entity).State = EntityState.Deleted;
            return dbContext.Set<T>().Remove(entity);
        }

        public T UpdateEntity(T entity)
        {
            dbContext.Entry<T>(entity).State = EntityState.Modified;
            return entity;
        }

        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            var entity = dbContext.Set<T>();
            return entity.Where(whereLambda);
        }

        public IQueryable<T> LoadPagingEntities<C>(Expression<Func<T, bool>> whereLambda, int pageSize, int pageIndex, out int count, Expression<Func<T, C>> orderbyLambda, bool isAsc)
        {
            var temp = dbContext.Set<T>().Where(whereLambda);
            count = temp.Count();
            if (isAsc)
            {
                temp = temp.OrderBy<T, C>(orderbyLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else
            {
                temp = temp.OrderByDescending<T, C>(orderbyLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }

            return temp;
        }
    }
}
