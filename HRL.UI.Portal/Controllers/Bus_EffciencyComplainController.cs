using HRL.BLL;
using HRL.Model;
using HRL.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRL.UI.Portal.Controllers
{
    public class Bus_EffciencyComplainController : BaseController
    {
        //有关效能投诉
        // GET: /Bus_EffciencyComplain/
        BjuiJsonResult result = new BjuiJsonResult();
        Bus_EffciencyComplainService service = new Bus_EffciencyComplainService();
        Sys_MultiLangInfoService langService = new Sys_MultiLangInfoService();
        public ActionResult Index(Bus_EffciencyComplain model)
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
            if (!string.IsNullOrEmpty(id) && id.Length == 36)
            {
                //show 编辑
                ViewBag.ShowModel = service.GetModel(id);
            }
            else
            {
                ViewBag.ShowModel = new Bus_EffciencyComplain();
            }
            return View();
        }

        /// <summary>
        /// 添加和编辑的显示页面
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Bus_EffciencyComplain model)
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
            catch (Exception e)
            {
                result.statusCode = "300";
                result.message = GlobalLngResource.GetRes("sysException");
            }
            return Json(result);
        }

        public ActionResult Delete(string id)
        {
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
        public ActionResult Review(string id)
        {
            var model = new Bus_EffciencyComplain() { ID = id, IsPass = TakeErrorStatusEnum.Pass.GetHashCode() };
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






        public ActionResult PicDataShow()
        {
            return View();
        }
        public string PieData()
        {
            return "{\"tooltip\":{\"trigger\": \"item\",\"formatter\": \"{a} <br />{b} : {c} ({d}%)\"},\"legend\": {\"orient\": \"vertical\",\"x\": \"left\",\"data\": [\"行政不作为或行政效率低下\",\"工作态度生硬冷淡\",\"工作推诿或敷衍塞责\",\"违反规定乱收费乱罚款乱摊派\",\"野蛮执法\"]},\"toolbox\": {\"show\": true,\"feature\": {\"mark\": {\"show\": true},\"dataView\": {\"show\": true,\"readOnly\": false},\"magicType\": {\"show\": true,\"type\": [\"pie\",\"funnel\"],\"option\": {\"funnel\": {\"x\": \"25%\",\"width\": \"50%\",\"funnelAlign\": \"left\",\"max\": 1548}}},\"restore\": {\"show\": true},\"saveAsImage\": {\"show\": true}}},\"calculable\": true,\"series\": [{\"name\": \"统计信息\",\"type\": \"pie\",\"radius\": \"55%\",\"center\": [\"50%\",\"60%\"],\"data\": [{\"value\": 35,\"name\": \"行政不作为或行政效率低下\"},{\"value\": 69,\"name\": \"工作态度生硬冷淡\"},{\"value\": 375,\"name\": \"工作推诿或敷衍塞责\"},{\"value\": 30,\"name\": \"违反规定乱收费乱罚款乱摊派\"},{\"value\": 4,\"name\": \"野蛮执法\"}]}]}";
        }
    }
}
