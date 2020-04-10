using HRL.BLL;
using HRL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRL.UI.Portal.Controllers
{
    public class SYS_ResourceDbController : BaseController
    {
        Sys_MultiLangInfoService service = new Sys_MultiLangInfoService();
        BjuiJsonResult result = new BjuiJsonResult();
        #region Update
     
        ///// <summary>
        ///// 语言资源 访问业务入口
        ///// </summary>
        //public ActionResult Index(string MENUCODE)
        //{
        //    string lang = ValueProvider.GetValue("lang").RawValue.ToString();
        //    ViewBag.MenuNameList = lngSysRes.GetSelectMenu();
        //    ViewBag.ResourceCode = MENUCODE;
        //    ViewBag.ResourceList = LoadResource(MENUCODE);
        //    return View();
        //}
        //public ActionResult SaveAllResource(List<CustomKeyValue> customList)
        //{

        //    BjuiJsonResult result = new BjuiJsonResult();
        //    try
        //    {
        //        var res = lngSysRes.UpdateModeList(customList);
        //        if (res > 0)
        //        {
        //            GlobalLngResource.SetMultiLangKey();
        //            result.statusCode = "200";
        //            result.message = GlobalLngResource.GetRes("operSuccess");
        //            return Json(result);
        //        }
        //        result.statusCode = "300";
        //        result.message = GlobalLngResource.GetRes("operFailure");
        //        return Json(result);
        //    }
        //    catch
        //    {
        //        result.statusCode = "300";
        //        result.message = GlobalLngResource.GetRes("operFailure");
        //        return Json(result);
        //    }
        //}
        //public JsonResult SaveResource(List<CustomKeyValue> customList)
        //{
        //    BjuiJsonResult result = new BjuiJsonResult();
        //    if (customList == null)
        //    {
        //        result.statusCode = "300";
        //        result.message = GlobalLngResource.GetRes("noRecords");
        //        return Json(result);
        //    }
        //    CustomKeyValue custom = customList[0];
        //    try
        //    {
        //        var res = this.lngSysRes.UpdateEntityModel(custom);
        //        if (res > 0)
        //        {
        //            GlobalLngResource.SetMultiLangKey();
        //            result.statusCode = "200";
        //            result.message = GlobalLngResource.GetRes("operSuccess");
        //            return Json(result);
        //        }
        //        else
        //        {
        //            result.statusCode = "300";
        //            result.message = GlobalLngResource.GetRes("operFailure");
        //            return Json(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.statusCode = "300";
        //        result.message = GlobalLngResource.GetRes("operFailure");
        //        return Json(result);
        //    }
        //}


        //private List<LNG_SYS_TABLELANG_INFO> LoadResource(string vPageUrl)
        //{
        //    string lang = ValueProvider.GetValue("lang").RawValue.ToString();
        //    var temp = this.lngSysRes.LoadEntities(s => s.TABLE_NAME == vPageUrl && s.COLUMN_CULTURE == lang).ToList();
        //    return temp;
        //}
        #endregion
        #region Add-Delete
        public ActionResult AddLangIndex(FormCollection collection)
        {
            var tableName = Request["tableName"];
            ViewBag.searchForm = collection;
            ViewBag.ResourceCode = tableName;
            ViewBag.resourceList = service.GetLangData(tableName);
            ViewBag.MenuNameList = service.GetSelectMenu();
            return View();
        }

        public ActionResult GetAddLangIndexData()
        {
            string lang = ValueProvider.GetValue("lang").RawValue.ToString();
            var obj = service.LoadEntities(s => s.COLUMN_CULTURE == lang)
                .Select(s => new { s.ID, s.TABLE_NAME, s.TABLE_DESC, s.COLUMN_ID, s.COLUMN_NAME });
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Create(List<LngSysTableLANGVM> resourceParameter, string table_name, string table_desc, string table_descEng)
        {
            if (resourceParameter == null)
            {
                result.statusCode = "300";
                result.message = GlobalLngResource.GetRes("noRecords");
            }
            if (string.IsNullOrEmpty(table_name) || string.IsNullOrEmpty(table_desc) || string.IsNullOrEmpty(table_descEng))
            {
                result.statusCode = "300";
                result.message = GlobalLngResource.GetRes("noRecords");
            }
            try
            {
                var res = this.service.AddModelList(resourceParameter, table_name, table_desc, table_descEng);
                if (res > 0)
                {
                    result.statusCode = "200";
                    result.message = GlobalLngResource.GetRes("operSuccess");
                }
                else
                {
                    result.statusCode = "300";
                    result.message = GlobalLngResource.GetRes("operFailure");
                }
            }
            catch (Exception ex)
            {
                result.statusCode = "300";
                result.message = GlobalLngResource.GetRes("sysException");
            }
            return Json(result);
        }

        public JsonResult Delete(string delids)
        {
            if (string.IsNullOrEmpty(delids))
            {
                result.statusCode = "300";
                result.message = GlobalLngResource.GetRes("WarSelectOne");
            }
            try
            {
                int res = this.service.Delete(delids);
                if (res > 0)
                {
                    result.statusCode = "200";
                    result.message = GlobalLngResource.GetRes("operSuccess");
                }
                else
                {
                    result.statusCode = "300";
                    result.message = GlobalLngResource.GetRes("operFailure");
                }
            }
            catch (Exception ex)
            {
                result.statusCode = "300";
                result.message = GlobalLngResource.GetRes("sysException");
            }

            return Json(result);
        }
        #endregion

    }
}
