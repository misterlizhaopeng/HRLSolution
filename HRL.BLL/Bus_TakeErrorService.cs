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
    public partial class Bus_TakeErrorService
    {
        Sys_MultiLangInfoService langService = new Sys_MultiLangInfoService();
        public List<Bus_TakeError> GetPaging(Bus_TakeError model, out int count)
        {
            IList<Bus_TakeError> backDtList = new List<Bus_TakeError>();
            //延迟加载;
            var isAsc = model.orderDirection == "asc" ? true : false;
            var temp = this.LoadEntities(l => l.DelFlag == 0);

            //Bus_TakeErrorService takeErrorService = new Bus_TakeErrorService();

            //var temp1 = from t in takeErrorService.LoadEntities(tr => tr.DelFlag == 0)
            //            join ml in langService.LoadEntities(ml => ml.COLUMN_NAME == "Bus_TakeError_FromSource") on t.FromSource equals ml.ID
            //            join ml2 in langService.LoadEntities(ml2 => ml2.COLUMN_NAME == "Bus_TakeError_TypeVal") on t.TypeVal equals ml2.ID
            //            join ml3 in langService.LoadEntities(ml3 => ml3.COLUMN_NAME == "Bus_TakeError_OverQuality") on t.FromSource equals ml3.ID
            //            select new
            //            {
            //                FromSource = ml.COLUMN_NAME,
            //                ActionId = t.ActionId,
            //                TypeVal = ml.COLUMN_NAME,
            //                CreateTime = t.CreateTime,
            //                ResCompany = t.ResCompany,
            //                OverQuality = ml3.COLUMN_NAME
            //            };

            //foreach (var one in temp1.ToList())
            //{
            //    Bus_TakeError takeObj = new Bus_TakeError();
            //    takeObj.FromSource = one.FromSource;
            //    takeObj.ActionId = one.ActionId;
            //    takeObj.TypeVal = one.TypeVal;
            //    takeObj.CreateTime = one.CreateTime;
            //    takeObj.ResCompany = one.ResCompany;
            //    takeObj.OverQuality = one.OverQuality;
            //    backDtList.Add(takeObj);
            //}


            if (!string.IsNullOrEmpty(model.UserId))
            {
                temp = temp.Where(l => l.UserId == model.UserId);
            }
            if (!string.IsNullOrEmpty(model.ResCompany))
            {
                temp = temp.Where(l => l.ResCompany.Contains(model.ResCompany));
            }
            if (!string.IsNullOrEmpty(model.FromSource))
            {
                temp = temp.Where(l => l.FromSource == model.FromSource);
            }
            if (!string.IsNullOrEmpty(model.TypeVal))
            {
                temp = temp.Where(l => l.TypeVal == model.TypeVal);
            }
            if (!string.IsNullOrEmpty(model.OverQuality))
            {
                temp = temp.Where(l => l.OverQuality == model.OverQuality);
            }
            if (model.BeginCreateTime != null && model.EndCreateTime != null)
            {
                temp = temp.Where(l => l.CreateTime >= model.BeginCreateTime && l.CreateTime <= model.EndCreateTime);
            }
            if (!string.IsNullOrEmpty(model.UserId))
            {
                temp = temp.Where(l => l.UserId == model.UserId);
            }

            model.orderField = !string.IsNullOrEmpty(model.orderField) ? model.orderField : "CreateTime";
            temp = temp.OrderByQueryable(model.orderField, model.orderDirection);
            var resList = temp.ToPaging(model.pageCurrent, model.pageSize, out count);

            var sourceList = langService.LoadEntities(s => s.TABLE_NAME == "Bus_TakeError_FromSource");
            var valList = langService.LoadEntities(s => s.TABLE_NAME == "Bus_TakeError_TypeVal");
            var overList = langService.LoadEntities(s => s.TABLE_NAME == "Bus_TakeError_OverQuality");

            foreach (var item in resList)
            {
                var sourceObj = sourceList.Where(p => p.ID == item.FromSource).FirstOrDefault();
                if (sourceObj != null) { item.FromSourceName = sourceObj.COLUMN_NAME; }
                var typeObj = valList.Where(p => p.ID == item.TypeVal).FirstOrDefault();
                if (typeObj != null) { item.TypeValName = typeObj.COLUMN_NAME; }
                var overObj = overList.Where(p => p.ID == item.OverQuality).FirstOrDefault();
                if (overObj != null) { item.OverQualityName = overObj.COLUMN_NAME; }
            }

            return resList;
        }
        /// <summary>
        /// 编辑时，获取指定信息；还有显示详细信息Detail
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Bus_TakeError GetModel(string Id)
        {
            var tempModel = this.LoadEntities(t => t.ID == Id).FirstOrDefault();
            if (tempModel == null)
            {
                return new Bus_TakeError();
            }
            Bus_TakeErrorAttributeService attributeService = new Bus_TakeErrorAttributeService();
            var attrList = from a in attributeService.LoadEntities(a => a.AttributeID == Id)
                           join l in langService.LoadEntities(l => l.TABLE_NAME == "Bus_TakeErrorAttribute_AttributeIDItem") on a.AttributeIDItem equals l.ID
                           select new { ID = a.ID, AttributeID = a.AttributeID, AttributeIDItem = a.AttributeIDItem, AttributeIDItemName = l.COLUMN_NAME };

            List<Bus_TakeErrorAttribute> list = new List<Bus_TakeErrorAttribute>();
            foreach (var item in attrList)
            {
                Bus_TakeErrorAttribute obj = new Bus_TakeErrorAttribute();
                obj.ID = item.ID;
                obj.AttributeID = item.AttributeID;
                obj.AttributeIDItem = item.AttributeIDItem;
                obj.AttributeIDItemName = item.AttributeIDItemName;
                list.Add(obj);
            }

            if (list != null)
            {
                tempModel.AttrList = list;
            }

            var sourceObj = langService.GetModel("Bus_TakeError_FromSource", tempModel.FromSource);
            var typeObj = langService.GetModel("Bus_TakeError_TypeVal", tempModel.TypeVal);
            var overObj = langService.GetModel("Bus_TakeError_OverQuality", tempModel.OverQuality);

            if (sourceObj != null) { tempModel.FromSourceName = sourceObj.COLUMN_NAME; }
            if (typeObj != null) { tempModel.TypeValName = typeObj.COLUMN_NAME; }
            if (overObj != null) { tempModel.OverQualityName = overObj.COLUMN_NAME; }


            return tempModel;
        }

        /// <summary>
        /// 修改或者添加一条数据
        /// </summary>
        /// <param name="model">实体参数</param>
        /// <returns>大于0，操作成功，反之失败</returns>
        public int OperateModel(Bus_TakeError model)
        {
            if (!string.IsNullOrEmpty(model.ID) && model.ID.Length == 36)
            {
                //编辑
                var existModel = this.LoadEntities(t => t.ID == model.ID).FirstOrDefault();
                existModel.FromSource = model.FromSource;
                existModel.ActionId = model.ActionId;
                existModel.TypeVal = model.TypeVal;
                existModel.ResCompany = model.ResCompany;
                existModel.ResCompanyId = model.ResCompanyId;
                existModel.ResPerson = model.ResPerson;
                //existModel.AttributeID = model.AttributeID;
                existModel.DelFlag = 0;
                existModel.AccessCompany = model.AccessCompany;
                existModel.AccessCompanyId = model.AccessCompanyId;
                existModel.IsOverTime = model.IsOverTime;
                existModel.IsSearch = model.IsSearch;
                existModel.OverQuality = model.OverQuality;
                existModel.Remark = model.Remark;
                existModel.LevelRecords = model.LevelRecords;

                #region 性质
                var existAttributes = DbSession.Bus_TakeErrorAttributeRepository.LoadEntities(ur => ur.AttributeID == existModel.ID);
                //删除现有数据
                foreach (var item in existAttributes)
                {
                    DbSession.Bus_TakeErrorAttributeRepository.DeleteEntity(item);
                }

                if (model.AttributeIds != null)
                {
                    foreach (var attrId in model.AttributeIds)
                    {
                        Bus_TakeErrorAttribute attrModel = new Bus_TakeErrorAttribute();
                        attrModel.ID = Guid.NewGuid().ToString();
                        attrModel.AttributeID = existModel.ID;
                        attrModel.AttributeIDItem = attrId;
                        DbSession.Bus_TakeErrorAttributeRepository.AddEntity(attrModel);
                    }
                }
                #endregion
            }
            else if (string.IsNullOrEmpty(model.ID))
            {

                //添加
                var addModel = new Bus_TakeError();
                addModel.ID = Guid.NewGuid().ToString();
                addModel.FromSource = model.FromSource;
                addModel.ActionId = model.ActionId;
                addModel.TypeVal = model.TypeVal;
                addModel.ResCompany = model.ResCompany;
                addModel.ResCompanyId = model.ResCompanyId;
                addModel.ResPerson = model.ResPerson;
                //addModel.AttributeID = model.AttributeID;
                addModel.DelFlag = 0;
                addModel.AccessCompany = model.AccessCompany;
                addModel.CreateTime = DateTime.Now;
                addModel.AccessCompanyId = model.AccessCompanyId;

                addModel.IsOverTime = model.IsOverTime;
                addModel.IsSearch = model.IsSearch;
                addModel.OverQuality = model.OverQuality;
                addModel.Remark = model.Remark;
                addModel.LevelRecords = model.LevelRecords;
                addModel.OrderBy = model.OrderBy;
                addModel.IsPass = 0;
                addModel.UserId = model.UserId;
                DbSession.Bus_TakeErrorRepository.AddEntity(addModel);

                #region 新增性质
                //添加
                if (model.AttributeIds != null)
                {
                    foreach (var attrId in model.AttributeIds)
                    {
                        Bus_TakeErrorAttribute attrModel = new Bus_TakeErrorAttribute();
                        attrModel.ID = Guid.NewGuid().ToString();
                        attrModel.AttributeID = addModel.ID;
                        attrModel.AttributeIDItem = attrId;
                        DbSession.Bus_TakeErrorAttributeRepository.AddEntity(attrModel);
                    }
                }
                #endregion

            }

            return DbSession.SaveChange();
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="IdList">主键编号集合</param>
        /// <returns>大于0，操作成功，反之失败</returns>
        public int Delete(List<string> IdList)
        {
            foreach (var _id in IdList)
            {
                var toDelModel = this.LoadEntities(t => t.ID == _id).FirstOrDefault();
                toDelModel.DelFlag = 1;
            }
            return this.DbSession.SaveChange();
        }

        public int UpdateEntityModel(Bus_TakeError model)
        {
            if (model == null || string.IsNullOrEmpty(model.ID))
            {
                return -1;
            }

            var searchModel = DbSession.Bus_TakeErrorRepository.LoadEntities(s => s.ID == model.ID).FirstOrDefault();
            if (searchModel == null)
            {
                return -1;
            }
            searchModel.IsPass = model.IsPass;
            searchModel.OkTime = DateTime.Now;
            int reTes = DbSession.SaveChange();
            return reTes;
        }

    }
}
