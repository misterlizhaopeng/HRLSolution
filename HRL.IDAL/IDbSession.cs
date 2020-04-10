using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRL.IDAL
{
    public partial interface IDbSession
    {
        DbContext CurrentDbContext { get; set; }
        int ExecuteSql(string sql, params SqlParameter[] param);
        int SaveChange();
    }
}
