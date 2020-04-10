using HRL.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRL.UI.Portal.Controllers
{
    public class BaseController : Controller
    {
        private bool _IsCheck = true;

        //private IDictionary<string, string> curConfigRoles;

        //public IDictionary<string, string> CurConfigRoles
        //{
        //    get
        //    {
        //        //r_leaderOfCom:单位领导,r_leaderOfDis:区领导,r_ordPer:普通个人,r_systemMa:系统管理
        //        string role = ConfigurationManager.AppSettings["role"];
        //        string[]rs=role.Split(',');
        //        this.curConfigRoles = new Dictionary<string, string>();
        //        foreach (var item in rs)
        //        {
        //            string[]itemRole=item.Split(':');
        //            this.curConfigRoles.Add(itemRole[0], itemRole[1]);
        //        }
        //        return curConfigRoles;

        //    }
        //}
        private Base_UserInfo _CurrentUserInfo;

        public Base_UserInfo CurrentUserInfo
        {
            get
            {
                if (_CurrentUserInfo == null)
                {
                    _CurrentUserInfo = (Base_UserInfo)Session["UserInfo"];
                }
                return _CurrentUserInfo;
            }
            set { _CurrentUserInfo = value; }
        }

        public bool IsCheck
        {
            get { return _IsCheck; }
            set { _IsCheck = value; }
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            //校验用户是否已经登录
            var userInfo = this.Session["UserInfo"];
            if (userInfo == null)
            {
                //跳转到登录页面
                this.Response.Redirect("/");
                return;
            }
        }
    }
}