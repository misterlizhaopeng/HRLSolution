using HRL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRL.BLL
{
    public partial class Bus_EffciencyComplainService
    {
        Sys_MultiLangInfoService langService = new Sys_MultiLangInfoService();
        public List<Bus_EffciencyComplain> GetPaging(Bus_EffciencyComplain model, out int count)
        {
            List<Bus_EffciencyComplain> backDtList = new List<Bus_EffciencyComplain>();
            Bus_EffComplExeResService effComplResService = new Bus_EffComplExeResService();
            //延迟加载;
            var isAsc = model.orderDirection == "asc" ? true : false;
            var temp = LoadEntities(l => l.DelFlag == 0);
            //按照“投诉件来源”"等值"查询
            if (!string.IsNullOrEmpty(model.ComplainFrmSourceId))
            {
                temp = temp.Where(l => l.ComplainFrmSourceId == model.ComplainFrmSourceId);
            }
            //按照“投诉类型”"等值"查询
            if (!string.IsNullOrEmpty(model.ComplainType))
            {
                temp = temp.Where(l => l.ComplainType == model.ComplainType);
            }
            //按照“处理情况”"等值"查询,id(multiid)
            if (!string.IsNullOrEmpty(model.ExecuteRes))
            {
                #region MyRegion
                var complainType = effComplResService.GetExecuteResIdByExecuteResIdItem(model.ExecuteRes);
                #endregion
                temp = temp.Where(l => l.ID == complainType);
            }
            //添加个人权限
            if (!string.IsNullOrEmpty(model.UserId))
            {
                temp = temp.Where(l => l.UserId == model.UserId);
            }
            //按照“投诉受理时间”进行时间查询
            if (model.BeginCreateTime != null && model.EndCreateTime != null)
            {
                temp = temp.Where(l => l.ComplainAcceptTime >= model.BeginCreateTime && l.ComplainAcceptTime <= model.EndCreateTime);
            }
            model.orderField = !string.IsNullOrEmpty(model.orderField) ? model.orderField : "CreateTime";
            temp = temp.OrderByQueryable(model.orderField, model.orderDirection);
            var resList = temp.ToPaging(model.pageCurrent, model.pageSize, out count);




            //var valList = langService.LoadEntities(s => s.TABLE_NAME == "Bus_TakeError_TypeVal");
            //var overList = langService.LoadEntities(s => s.TABLE_NAME == "Bus_TakeError_OverQuality");

            //foreach (var item in resList)
            //{
            //    var sourceObj = sourceList.Where(p => p.ID == item.FromSource).FirstOrDefault();
            //    if (sourceObj != null) { item.FromSourceName = sourceObj.COLUMN_NAME; }
            //    var typeObj = valList.Where(p => p.ID == item.TypeVal).FirstOrDefault();
            //    if (typeObj != null) { item.TypeValName = typeObj.COLUMN_NAME; }
            //    var overObj = overList.Where(p => p.ID == item.OverQuality).FirstOrDefault();
            //    if (overObj != null) { item.OverQualityName = overObj.COLUMN_NAME; }
            //}


            var compList = langService.LoadEntities(s => s.TABLE_NAME == "Bus_EffciencyComplain_CompanyInfoId");
            var comperList = langService.LoadEntities(s => s.TABLE_NAME == "Bus_EffciencyComplain_CompanyPerId");
            var sourceList = langService.LoadEntities(s => s.TABLE_NAME == "Bus_EffciencyComplain_ComplainFrmSource");
            var typeObjList = langService.LoadEntities(s => s.TABLE_NAME == "Bus_TakeErrorAttribute_AttributeIDItem");

            foreach (var item in resList)
            {
                var comp = compList.Where(p => p.ID == item.CompanyInfoId).FirstOrDefault();
                if (comp != null) { item.CompanyInfoIdName = comp.COLUMN_NAME; }

                var comper = comperList.Where(p => p.ID == item.CompanyPerId).FirstOrDefault();
                if (comper != null) { item.CompanyPerIdName = comper.COLUMN_NAME; }

                var sourceObj = sourceList.Where(p => p.ID == item.ComplainFrmSourceId).FirstOrDefault();
                if (sourceObj != null) { item.ComplainFrmSourceIdName = sourceObj.COLUMN_NAME; }

                var typeObj = typeObjList.Where(p => p.ID == item.ComplainType).FirstOrDefault();
                if (typeObj != null) { item.ComplainTypeName = typeObj.COLUMN_NAME; }

            
                //if (sourceObj != null) { item.ComplainFrmSourceIdName = sourceObj.COLUMN_NAME; }
               
            }
            return resList;
        }
        /// <summary>
        /// 编辑时，获取指定信息；还有显示详细信息Detail
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Bus_EffciencyComplain GetModel(string Id)
        {
            var tempModel = LoadEntities(t => t.ID == Id).FirstOrDefault();
            if (tempModel == null)
            {
                return new Bus_EffciencyComplain();
            }

            Bus_EffComplExeResService attributeService = new Bus_EffComplExeResService();
            var attrList = from a in attributeService.LoadEntities(a => a.ExecuteResId == Id)
                           //join l in langService.LoadEntities(l => l.TABLE_NAME == "Bus_EffComplExeRes_ExecuteResIdItem") on a.ExecuteResId equals l.ID
                           join l in langService.LoadEntities(l => l.TABLE_NAME == "Bus_EffComplExeRes_ExecuteResIdItem") on a.ExecuteResIdItem equals l.ID
                           select new { ID = a.ID, ExecuteResId = a.ExecuteResId, ExecuteResIdItem = a.ExecuteResIdItem, ExecuteResIdItemName = l.COLUMN_NAME };

            List<Bus_EffComplExeRes> list = new List<Bus_EffComplExeRes>();
            foreach (var item in attrList)
            {
                Bus_EffComplExeRes obj = new Bus_EffComplExeRes();
                obj.ID = item.ID;
                obj.ExecuteResId = item.ExecuteResId;
                obj.ExecuteResIdItem = item.ExecuteResIdItem;
                obj.ExecuteResIdItemName = item.ExecuteResIdItemName;
                list.Add(obj);
            }

            if (list != null)
            {
                tempModel.AttrList = list;
            }
            var comp = langService.GetModel("Bus_EffciencyComplain_CompanyInfoId", tempModel.CompanyInfoId);
            var comper = langService.GetModel("Bus_EffciencyComplain_CompanyPerId", tempModel.CompanyPerId);
            var sourceObj = langService.GetModel("Bus_EffciencyComplain_ComplainFrmSource", tempModel.ComplainFrmSourceId);
            var typeObj = langService.GetModel("Bus_TakeErrorAttribute_AttributeIDItem", tempModel.ComplainType);
            //            var overObj = langService.GetModel("Bus_TakeError_OverQuality", tempModel.OverQuality);
            if (comp != null) { tempModel.CompanyInfoIdName = comp.COLUMN_NAME; }
            if (comper != null) { tempModel.CompanyPerIdName = comper.COLUMN_NAME; }
            if (sourceObj != null) { tempModel.ComplainFrmSourceIdName = sourceObj.COLUMN_NAME; }
            if (typeObj != null) { tempModel.ComplainTypeName = typeObj.COLUMN_NAME; }
            // if (overObj != null) { tempModel.OverQualityName = overObj.COLUMN_NAME; }
            return tempModel;
        }
        /// <summary>
        /// 修改或者添加一条数据
        /// </summary>
        /// <param name="model">实体参数</param>
        /// <returns>大于0，操作成功，反之失败</returns>
        public int OperateModel(Bus_EffciencyComplain model)
        {
            if (!string.IsNullOrEmpty(model.ID) && model.ID.Length == 36)
            {
                //编辑
                var existModel = this.LoadEntities(t => t.ID == model.ID).FirstOrDefault();
                existModel.CompanyInfo = model.CompanyInfo;
                existModel.CompanyInfoId = model.CompanyInfoId;
                existModel.CompanyPerId = model.CompanyPerId;
                existModel.OtherPer = model.OtherPer;
                existModel.ComplainFrmSourceId = model.ComplainFrmSourceId;
                existModel.ComplainAcceptTime = model.ComplainAcceptTime;
                existModel.DelFlag = 0;
                existModel.ComplainCode = model.ComplainCode;
                existModel.ComplainProfileContent = model.ComplainProfileContent;
                existModel.ComplainType = model.ComplainType;
                existModel.IsOverTime = model.IsOverTime;
                existModel.IsSearch = model.IsSearch;
                existModel.MassesSatisfy = model.MassesSatisfy;
                existModel.LevelRecords = model.LevelRecords;
                existModel.Phone = model.Phone;
                existModel.AreaCode = model.AreaCode;
                existModel.Tel = model.Tel;
                existModel.ComplainAreaCode = model.ComplainAreaCode;
                existModel.ComplainTel = model.ComplainTel;


                #region 修改处理情况
                var existExecuteRes = DbSession.Bus_EffComplExeResRepository.LoadEntities(ur => ur.ExecuteResId == existModel.ID);
                //删除现有数据
                foreach (var item in existExecuteRes)
                {
                    DbSession.Bus_EffComplExeResRepository.DeleteEntity(item);
                }

                if (model.ExecuteResIds != null)
                {
                    foreach (var attrId in model.ExecuteResIds)
                    {
                        Bus_EffComplExeRes attrModel = new Bus_EffComplExeRes();
                        attrModel.ID = Guid.NewGuid().ToString();
                        attrModel.ExecuteResId = existModel.ID;
                        attrModel.ExecuteResIdItem = attrId;
                        DbSession.Bus_EffComplExeResRepository.AddEntity(attrModel);
                    }
                }
                #endregion
            }
            else if (string.IsNullOrEmpty(model.ID))
            {

                //添加
                var addModel = new Bus_EffciencyComplain();
                addModel.ID = Guid.NewGuid().ToString();
                addModel.CompanyInfo = model.CompanyInfo;
                addModel.CompanyInfoId = model.CompanyInfoId;
                addModel.CompanyPerId = model.CompanyPerId;
                addModel.OtherPer = model.OtherPer;
                addModel.ComplainFrmSourceId = model.ComplainFrmSourceId;
                addModel.ComplainAcceptTime = model.ComplainAcceptTime;
                addModel.DelFlag = 0;
                addModel.ComplainCode = model.ComplainCode;
                addModel.ComplainProfileContent = model.ComplainProfileContent;
                addModel.ComplainType = model.ComplainType;
                addModel.IsOverTime = model.IsOverTime;
                addModel.IsSearch = model.IsSearch;
                addModel.MassesSatisfy = model.MassesSatisfy;
                addModel.LevelRecords = model.LevelRecords;
                addModel.UserId = model.UserId;
                addModel.CreateTime = DateTime.Now;
                addModel.Creator = model.UserId;
                addModel.Phone = model.Phone;
                addModel.AreaCode = model.AreaCode;
                addModel.Tel = model.Tel;
                addModel.ComplainAreaCode = model.ComplainAreaCode;
                addModel.ComplainTel = model.ComplainTel;

                DbSession.Bus_EffciencyComplainRepository.AddEntity(addModel);

                #region 新增处理情况
                //添加
                if (model.ExecuteResIds != null)
                {
                    foreach (var attrId in model.ExecuteResIds)
                    {
                        Bus_EffComplExeRes attrModel = new Bus_EffComplExeRes();
                        attrModel.ID = Guid.NewGuid().ToString();
                        attrModel.ExecuteResId = addModel.ID;
                        attrModel.ExecuteResIdItem = attrId;
                        DbSession.Bus_EffComplExeResRepository.AddEntity(attrModel);
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
            return DbSession.SaveChange();
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateEntityModel(Bus_EffciencyComplain model)
        {
            if (model == null || string.IsNullOrEmpty(model.ID))
            {
                return -1;
            }

            var searchModel = DbSession.Bus_EffciencyComplainRepository.LoadEntities(s => s.ID == model.ID).FirstOrDefault();
            if (searchModel == null)
            {
                return -1;
            }
            searchModel.IsPass = model.IsPass;
            searchModel.ModifyTime = DateTime.Now;
            return DbSession.SaveChange();
        }
    }
}
