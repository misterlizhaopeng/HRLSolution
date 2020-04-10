using HRL.BLL;
using HRL.Common;
using HRL.Common.Utlity;
using HRL.Model;
using HRL.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HRL.UI.Portal.Controllers
{
    public class HRLController : BaseController
    {
        private Base_RolesInfoService service = new Base_RolesInfoService();
        private Base_MenuInfoService menuInfoService = new Base_MenuInfoService();
        private Base_UserInfoService base_UserInfoService = new Base_UserInfoService();
        private BjuiJsonResult result = new BjuiJsonResult();
        private Base_UserInfo curPerModel = SessionHelper.GetSession("UserInfo") as Base_UserInfo;


        //当前登录人
        //  string curPerId =  // "BFDE28D8-580F-4FAD-9EB0-DD8980838A58";// string.Empty;//UnipecERPBridge.Common.SessionHelper.GetSession("sysUserNo").ToString();
        public ActionResult Edit()
        {
            return Json(new { statusCode = 200, message = "修改成功！" });
        }
        public ActionResult PerDetail()
        {
            return View();
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Login", "Login");
        }
        /// <summary>
        /// 主页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (CurrentUserInfo == null)
            {
                return RedirectToAction("Login", "Login");

            }
            var res = service.LoadEntities(r => true);
            ViewBag.LoadMenus = menuInfoService.LoadMenusInfo(CurrentUserInfo.Id);
            ViewBag.curPer = curPerModel == null ? "" : curPerModel.UserName;
            var _per = Convert.ToString(Session["qPer"]);
            if (_per == "Manager")
            {
                ViewBag.qPer = "Admin";
            }
            if (_per == "Per")
            {
                ViewBag.qPer = "User";
            }

            #region 判断当前角色是否存在 普通人
            var roleDesc = EnumDescription<HRL.Model.Enums.Role>.GetVisaTypeEnumDescription((int)HRL.Model.Enums.Role.r_ordPer);
            if (CurrentUserInfo.RoleList.Select(r => r.RoleName).Contains(roleDesc))
            {
                ViewBag.CurRole = "1";
            }
            #endregion
            return View();
        }


        public ActionResult PersonLoad() { return View(); }


        public ActionResult Main()
        {

            var roleDesc = EnumDescription<HRL.Model.Enums.Role>.GetVisaTypeEnumDescription((int)HRL.Model.Enums.Role.r_ordPer);
            if (CurrentUserInfo.RoleList.Select(r => r.RoleName).Contains(roleDesc))
            {
                //普通用户
                ViewBag.role="2";
            }
            else
            {
                //管理员角色
                ViewBag.role = "1";
            }
         
            return View();
        }


        public ActionResult Test()
        {
            return View();
        }

        public int Delete() { return 1; }

        public ActionResult ModifyPwd()
        {
            return View();
        }

        public ActionResult ComeSoon() { return View(); }
    }
}
