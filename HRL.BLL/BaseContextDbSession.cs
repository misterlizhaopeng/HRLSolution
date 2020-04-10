using HRL.DAL;
using HRL.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace HRL.BLL
{
    public class BaseContextDbSession
    {
        private IDbSession dbSession;
        protected IDbSession DbSession
        {
            get
            {
                dbSession = (DbSession)CallContext.GetData(typeof(BaseContextDbSession).FullName);
                if (dbSession == null)
                {
                    dbSession = new DbSession();
                    CallContext.SetData(typeof(BaseContextDbSession).FullName, dbSession);
                }
                return dbSession;
            }
        }

        /// <summary>
        /// 序列值，表示下一个
        /// </summary>
        /// <param name="TableName">序列名称</param>
        /// <returns>返回一个自增id的新值</returns>
        public int GetSequence(string tableName)
        {
            return DbSession.CurrentDbContext.Database.SqlQuery<int>("select max(Id) from " + tableName).FirstOrDefault();
        }
    }
}
