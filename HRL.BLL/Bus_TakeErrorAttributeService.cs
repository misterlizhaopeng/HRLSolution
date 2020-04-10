using HRL.Common;
using HRL.Common.Utlity;
using HRL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRL.BLL
{
    public partial class Bus_TakeErrorAttributeService
    { 
        /// <summary>
        /// 编辑时，获取指定信息；还有显示详细信息Detail
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Bus_TakeErrorAttribute GetModel(string Id)
        {
            var tempModel = this.LoadEntities(t => t.ID == Id).FirstOrDefault();
            if (tempModel == null)
            {
                return new Bus_TakeErrorAttribute();
            }
            return tempModel;
        }

        /// <summary>
        /// 修改或者添加一条数据
        /// </summary>
        /// <param name="model">实体参数</param>
        /// <returns>大于0，操作成功，反之失败</returns>
        public int OperateModel(Bus_TakeErrorAttribute model)
        {
            if (!string.IsNullOrEmpty(model.ID) && model.ID.Length == 36)
            {
                //编辑
                var existModel = this.LoadEntities(t => t.ID == model.ID).FirstOrDefault();
                existModel.AttributeID = model.AttributeID;

            }
            else if (string.IsNullOrEmpty(model.ID))
            {

                //添加
                var addModel = new Bus_TakeErrorAttribute();
                addModel.ID = Guid.NewGuid().ToString();
                addModel.AttributeID = model.AttributeID;
                DbSession.Bus_TakeErrorAttributeRepository.AddEntity(addModel);
            }

            return DbSession.SaveChange();
        }
    }
}
