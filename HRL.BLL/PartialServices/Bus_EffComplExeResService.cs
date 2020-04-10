using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRL.BLL
{
    public partial class Bus_EffComplExeResService
    {
        /// <summary>
        /// 查询，根据执行项的值executeResIdItem，找到指定记录的值ExecuteResId
        /// </summary>
        /// <param name="executeResIdItem"></param>
        /// <returns></returns>
        public string GetExecuteResIdByExecuteResIdItem(string executeResIdItem)
        {
            var model = LoadEntities(a => a.ExecuteResIdItem == executeResIdItem).FirstOrDefault();
            if (model != null)
            {
                return model.ExecuteResId;
            }
            return string.Empty;
        }
    }
}
