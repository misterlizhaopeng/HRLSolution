using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRL.Model.Enums
{
    public enum TakeErrorEnum
    {
        /// <summary>
        /// 来源
        /// </summary>
        [DescriptionAttribute("来源")]
        Bus_TakeError_FromSource = 0,
        /// <summary>
        /// 类别
        /// </summary>
        Bus_TakeError_TypeVal = 1,
        /// <summary>
        /// 责任单位、承办单位（下拉菜单）
        /// </summary>
        Bus_TakeError_ResCompanyId = 2,
        /// <summary>
        /// 责任人
        /// </summary>
        Bus_TakeError_ResPerson = 3,
        /// <summary>
        /// 完成质量
        /// </summary>
        Bus_TakeError_OverQuality = 4,
        /// <summary>
        /// 性质选择项
        /// </summary>
        Bus_TakeErrorAttribute_AttributeIDItem = 5
    }
}
