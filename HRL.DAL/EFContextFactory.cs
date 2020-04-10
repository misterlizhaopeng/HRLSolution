using HRL.IDAL;
using HRL.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace HRL.DAL
{
    public class EFContextFactory : IDbContextFactory
    {
        //保证一次请求，返回一个实例
        public DbContext GetCurrentContextInstence()
        {
            DbContext dbContext = (DbContext)CallContext.GetData(typeof(EFContextFactory).FullName);
            if (dbContext == null)
            {
                dbContext = new HRLEntities();
                CallContext.SetData(typeof(EFContextFactory).FullName, dbContext);
            }
            return dbContext;
        }
    }
}
