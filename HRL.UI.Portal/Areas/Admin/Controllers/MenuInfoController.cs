using HRL.UI.Portal.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRL.UI.Portal.Areas.Admin.Controllers
{
    public class MenuInfoController : BaseController
    {
        //
        // GET: /Admin/MenuInfo/

        public ActionResult Index()
        {
            //string
            //String str = "abc";
            //System.IO.BinaryReader.Equals("", "");
            //Console.WriteLine(str);
            return View();
        }


        #region Test测试
        public ActionResult TestDataGrid()
        {
            return View();
        }

        #endregion
    }
}
