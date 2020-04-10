using HRL.IDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRL.DAL
{
    /// <summary>
    /// 业务层访问数据层 统一入口
    /// </summary>
    public partial class DbSession : IDbSession
    {
        private IDbContextFactory dbContextFactory = new EFContextFactory();
        public DbContext CurrentDbContext { get; set; }
        public DbSession()
        {
            CurrentDbContext = dbContextFactory.GetCurrentContextInstence();
        }
        public int ExecuteSql(string sql, params  SqlParameter[] param)
        {
            return CurrentDbContext.Database.ExecuteSqlCommand(sql, param);
        }

        public int SaveChange()
        {
            return this.CurrentDbContext.SaveChanges();
        }
    }
}
