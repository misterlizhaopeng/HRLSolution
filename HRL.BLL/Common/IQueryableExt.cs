using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HRL.BLL
{
    public static class IQueryableExt
    {
        /// <summary>
        /// 调用此方法之前，进行动态筛选
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="allItems">分页对象</param>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="count">筛选之后的总行数</param>
        /// <returns></returns>
        public static List<T> ToPaging<T>(this IQueryable<T> allItems, int pageCurrent, int pageSize, out int count)
        {
            count = allItems.Count();
            var temp = allItems.Skip((pageCurrent - 1) * pageSize).Take(pageSize);
            if (temp.Count() == 0)
            {
                return new List<T>();
            }
            return temp.ToList();
        }

        /// <summary>
        /// 动态字段排序
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="query">排序对象</param>
        /// <param name="orderField">排序指定列</param>
        /// <param name="isDesc">升降序，asc或者desc</param>
        /// <returns></returns>
        public static IQueryable<T> OrderByQueryable<T>(this IQueryable<T> query, string orderField, string isDesc)
        {
            if (!string.IsNullOrEmpty(orderField))
            {
                //创建表达式变量参数
                var parameter = Expression.Parameter(typeof(T), "parameter");
                //根据属性名获取属性
                var property = typeof(T).GetProperty(orderField);
                //创建一个访问属性的表达式
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                isDesc = isDesc ?? "asc";
                string OrderName = isDesc == "desc" ? "OrderByDescending" : "OrderBy";
                MethodCallExpression resultExp = Expression.Call(typeof(Queryable), OrderName, new Type[] { typeof(T), property.PropertyType }, query.Expression, Expression.Quote(orderByExp));
                query = query.Provider.CreateQuery<T>(resultExp);
            }
            return query;
        }
    }
}
