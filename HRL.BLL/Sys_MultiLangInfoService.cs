using HRL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRL.BLL
{
    public partial class Sys_MultiLangInfoService
    {
        #region Update
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        public IDictionary<string, string> GetSelectMenu()
        {
            string lan = "zh-cn";// HttpContext.Current.Request.Cookies["language"].Value;
            var temp = DbSession.Sys_MultiLangInfoRepository.LoadEntities(s => s.COLUMN_CULTURE == lan)
                .Select(s => new { s.TABLE_NAME, s.TABLE_DESC })
                .Distinct();
            var res = temp.ToList();
            IDictionary<string, string> dic = new Dictionary<string, string>();
            res.ForEach(s => { dic.Add(s.TABLE_NAME, s.TABLE_DESC); });
            return dic;
        }
        //public int UpdateEntityModel(CustomKeyValue model)
        //{
        //    int ID = -1;
        //    //int.TryParse(model.Key, out ID);
        //    int.TryParse(model.ID, out ID);
        //    if (ID <= 0)
        //    {
        //        return -1;
        //    }
        //    //var searchModel = DbSession.LNG_SYS_TABLE_INFORepository.LoadEntities(s => s.ID == ID).FirstOrDefault();
        //    var searchModel = DbSession.LNG_SYS_TABLELANG_INFORepository.LoadEntities(s => s.ID == ID).FirstOrDefault();
        //    if (searchModel == null)
        //    {
        //        return -1;
        //    }
        //    searchModel.COLUMN_NAME = model.Value;
        //    int reTes = DbSession.SaveChange();
        //    return reTes;
        //}
        //public int UpdateModeList(List<CustomKeyValue> customList)
        //{
        //    var passIDList = customList.Select(s => Convert.ToDecimal(s.ID)).Distinct();
        //    var searchModelList = this.LoadEntities(s => passIDList.Contains(s.ID));
        //    foreach (var item in searchModelList)
        //    {
        //        if (item.ID <= 0)
        //        {
        //            return -1;
        //        }
        //        var _key = string.Empty;
        //        _key = item.ID.ToString();
        //        var newVal = customList.Where(a => a.ID == _key).Select(b => b.Value).FirstOrDefault();
        //        if (newVal == null)
        //        {
        //            return -1;
        //        }
        //        item.COLUMN_NAME = newVal;
        //        //this.DbSession.LNG_SYS_TABLE_INFORepository.UpdateEntity(item);
        //        this.DbSession.LNG_SYS_TABLELANG_INFORepository.UpdateEntity(item);
        //    }
        //    return DbSession.SaveChange();
        //}
        #endregion

        #region Add-Delete
        /// <summary>
        /// 加载资源模块，no paging
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<Sys_MultiLangInfo> GetLangData(string tableName)
        {
            var loadDataList = DbSession.Sys_MultiLangInfoRepository.LoadEntities(s => s.TABLE_NAME == tableName);//&& s.COLUMN_CULTURE == lan);
            if (loadDataList == null)
            {
                return null;
            }
            return loadDataList.ToList();
        }
        public Sys_MultiLangInfo GetModel(string tableName, string Id)
        {
            var list = DbSession.Sys_MultiLangInfoRepository.LoadEntities(s => s.TABLE_NAME == tableName && s.ID == Id).ToList();

            if (list == null) return null;

            return list.FirstOrDefault();
        }
        public int AddModelList(List<LngSysTableLANGVM> resourceParameter, string table_name, string table_desc, string table_descEng)
        {
            /*
                table_name:AA
                table_desc:BB
                resourceParameter[0].Column_ID:AAA
                resourceParameter[0].Chinese:AAA
                resourceParameter[0].English:AAA
             */
            foreach (var item in resourceParameter)
            {
                if (string.IsNullOrEmpty(item.COLUMN_ID))
                {
                    continue;
                }
                Sys_MultiLangInfo model = new Sys_MultiLangInfo();
                model.ID = Guid.NewGuid().ToString();
                model.TABLE_NAME = table_name;
                model.TABLE_DESC = table_desc;
                model.COLUMN_ID = item.COLUMN_ID;
                model.COLUMN_NAME = item.Chinese;
                model.COLUMN_CULTURE = "zh-cn";
                DbSession.Sys_MultiLangInfoRepository.AddEntity(model);

                //LNG_SYS_TABLELANG_INFO modelEnglish = new LNG_SYS_TABLELANG_INFO();
                //modelEnglish.ID = this.GetSequence("SEQ_LNG_SYS_TABLELANG_INFO");
                //modelEnglish.TABLE_NAME = table_name;
                //modelEnglish.TABLE_DESC = table_descEng;
                //modelEnglish.COLUMN_ID = item.COLUMN_ID;
                //modelEnglish.COLUMN_NAME = item.English;
                //modelEnglish.COLUMN_CULTURE = "en-us";
                //DbSession.LNG_SYS_TABLELANG_INFORepository.AddEntity(modelEnglish);
            }
            var res = DbSession.SaveChange();
            return res;
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="delids"></param>
        /// <returns></returns>
        public int Delete(string delids)
        {
            var delList = delids.Split(',').ToList();
            foreach (var item in delList)
            {
                Sys_MultiLangInfo model = new Sys_MultiLangInfo();
                model.ID = item;
                DbSession.Sys_MultiLangInfoRepository.DeleteEntity(model);
            }
            var res = DbSession.SaveChange();
            return res;
        }
        #endregion
    }
}
