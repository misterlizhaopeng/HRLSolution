using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRL.Model
{
    public partial class Bus_EffciencyComplain : Pager
    {
        public List<string> ExecuteResIds { get; set; }
        public List<Bus_EffComplExeRes> AttrList { get; set; }




        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyInfoIdName { get; set; }
        /// <summary>
        /// 单位人员名称
        /// </summary>
        public string CompanyPerIdName { get; set; }
        /// <summary>
        /// 来源名称
        /// </summary>
        public string ComplainFrmSourceIdName { get; set; }
        /// <summary>
        /// 投诉类型
        /// </summary>
        public string ComplainTypeName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string State { get; set; }



        public Nullable<System.DateTime> BeginCreateTime { get; set; }

        public Nullable<System.DateTime> EndCreateTime { get; set; }
    }
}
