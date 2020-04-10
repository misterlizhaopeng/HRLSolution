using HRL.BLL;
using HRL.Common;
using HRL.Common.Utlity;
using HRL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HRL.UI.Portal.Controllers
{
    public class LoginController : Controller
    {
        private Base_UserInfoService base_UserInfoService = new Base_UserInfoService();
        private BjuiJsonResult result = new BjuiJsonResult();
        //
        // GET: /Login/

        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 登录页面
        /// </summary>
        /// <param name="usr"></param>
        /// <param name="pwd"></param>
        /// <param name="validCode"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(string usr, string pwd, string validCode)
        {
            try
            {
                var sCode = Session["Code"] == null ? "" : Session["Code"].ToString();
                //校验验证码
                if (sCode.ToLower() != validCode.ToLower())
                {
                    result.statusCode = "300";
                    result.message = "验证码错误！";
                    return Json(result);
                }
                string userPwd = EncryptHelper.EncryptPwd(pwd);  //校验逻辑
                var userInfo = base_UserInfoService.Login(new Base_UserInfo() { UserName = usr, UserPwd = userPwd });
                if (userInfo == null)
                {
                    result.statusCode = "300";
                    result.message = "账号或密码错误!";
                    return Json(result);
                }
                //放到session里面去
                SessionHelper.SetSession("UserInfo", userInfo);
            }
            catch (Exception ex)
            {
                result.statusCode = "300";
                result.message = GlobalLngResource.GetRes("sysException");
                return Json(result);
            }
            //登录成功跳转到首页
            result.statusCode = "200";
            return Json(result);
        }



    }
}
