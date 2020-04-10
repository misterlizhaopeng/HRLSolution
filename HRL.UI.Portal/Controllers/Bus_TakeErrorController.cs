using HRL.BLL;
using HRL.Model;
using HRL.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HRL.UI.Portal.Controllers
{
    public class Bus_TakeErrorController : BaseController
    {
        BjuiJsonResult result = new BjuiJsonResult();
        Bus_TakeErrorService service = new Bus_TakeErrorService();
        Sys_MultiLangInfoService langService = new Sys_MultiLangInfoService();

        #region 列表
        public ActionResult Index(Bus_TakeError model)
        {
            int count = 0;
            try
            {
                if (CurrentUserInfo == null)
                {
                    return RedirectToAction("Login", "Login");
                }
                #region 判断当前角色是否存在 普通人
                var isPerRole = false;
                var roleDesc = EnumDescription<HRL.Model.Enums.Role>.GetVisaTypeEnumDescription((int)HRL.Model.Enums.Role.r_ordPer);
                if (CurrentUserInfo.RoleList.Select(r => r.RoleName).Contains(roleDesc))
                {
                    ViewBag.CurRole = "1";
                    isPerRole = true;
                }
                #endregion
                if (isPerRole)
                {
                    model.UserId = CurrentUserInfo.Id;
                }
                var temp = service.GetPaging(model, out count);

                ViewBag.ShowList = temp;
                ViewBag.total = count;
                ViewBag.Model = model;
                return View();
            }
            catch (Exception ex)
            {
                result.statusCode = "300";
                result.message = GlobalLngResource.GetRes("sysException");
                return Json(result);
            }
        }
        /// <summary>
        /// 添加和编辑的显示页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            Bus_TakeError md = new Bus_TakeError();

            if (!string.IsNullOrEmpty(id) && id.Length == 36)
            {
                //show 编辑
                ViewBag.ShowModel = md = service.GetModel(id);
            }
            else
            {
                ViewBag.ShowModel = new Bus_TakeError();
            }
            return View();
        }

        /// <summary>
        /// 添加和编辑的显示页面
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Bus_TakeError model)
        {
            if (model == null)
            {
                result.statusCode = "300";
                result.message = GlobalLngResource.GetRes("noRecords");
                return Json(result);
            }
            if (!ModelState.IsValid)
            {
                result.statusCode = "300";
                result.message = GlobalLngResource.GetRes("UIInputModelDataError");
                return Json(result);
            }
            if (CurrentUserInfo == null)
            {
                return RedirectToAction("Login", "Login");
            }
            try
            {
                model.UserId = CurrentUserInfo.Id;
                var res = service.OperateModel(model);
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
            catch
            {
                result.statusCode = "300";
                result.message = GlobalLngResource.GetRes("sysException");
            }
            return Json(result);
        }

        public ActionResult Delete(string id)
        {
            //var model = new Bus_TakeError() { ID = id };
            var res = service.Delete(new List<string> { id });
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
            return Json(result);
        }

        public ActionResult Review(string id)
        {
            var model = new Bus_TakeError() { ID = id, IsPass = TakeErrorStatusEnum.Pass.GetHashCode() };
            var res = service.UpdateEntityModel(model);
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
            return Json(result);
        }

        ///// <summary>
        ///// 删除一条记录
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public JsonResult Delete(string id)
        //{
        //    if (string.IsNullOrEmpty(id))
        //    {
        //        result.statusCode = "300";
        //        result.message = GlobalLngResource.GetRes("noRecords");
        //        return Json(result);
        //    }
        //    Bus_TakeError delModel = new Bus_TakeError() { ID = id };
        //    try
        //    {
        //        List<string> idList = new List<string>() { id };
        //        var res = service.Delete(idList);
        //        if (res > 0)
        //        {
        //            result.statusCode = "200";
        //            result.message = GlobalLngResource.GetRes("operSuccess");
        //        }
        //        else
        //        {
        //            result.statusCode = "300";
        //            result.message = GlobalLngResource.GetRes("operFailure");
        //        }
        //    }
        //    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
        //    {
        //        result.statusCode = "300";
        //        result.message = GlobalLngResource.GetRes("sysException");
        //    }
        //    catch (Exception ex)
        //    {
        //        result.statusCode = "300";
        //        result.message = GlobalLngResource.GetRes("sysException");
        //    }
        //    return Json(result);
        //}

        /// <summary>
        /// 显示详细页面
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns>返回实体</returns>
        public ActionResult Detail(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                result.statusCode = "300";
                result.message = GlobalLngResource.GetRes("PleaseSelectOne");
                return Json(result);
            }
            ViewBag.ShowModel = service.GetModel(id);
            return View();
        }

        #endregion
    }
}
