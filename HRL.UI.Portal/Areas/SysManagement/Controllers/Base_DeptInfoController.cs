using HRL.BLL;
using HRL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRL.UI.Portal.Areas.SysManagement.Controllers
{
    public class Base_DeptInfoController : Controller
    {
        BjuiJsonResult result = new BjuiJsonResult();
        Base_DeptInfoService Base_DeptInfoService = new Base_DeptInfoService();
        public ActionResult Index()
        {

            return View();
        }

        public JsonResult GetData()
        {
            var parentId = string.IsNullOrEmpty(Request["id"]) ? null : Request["id"];
            IQueryable<Base_DeptInfo> res = null;
            IQueryable<Base_DeptInfo> resCompare = Base_DeptInfoService.LoadEntities(d => true);
            if (parentId == null)
            {
                res = Base_DeptInfoService.LoadEntities(d => d.ParentId == null);
            }
            else
            {
                res = Base_DeptInfoService.LoadEntities(d => d.ParentId == parentId);
            }

            var tmmRes1 = from r in res
                          select new
                          {
                              r.Id,
                              r.DeptName,
                              childCount = (from ri in resCompare
                                            where ri.ParentId == r.Id
                                            select ri).Select(d => d.Id).Count()
                          };
            var tmp = tmmRes1.ToList().Select(d => new { id = d.Id, name = d.DeptName, isParent = d.childCount > 0 ? true : false });
            return Json(tmp, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取一个节点的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetNodeData(string id)
        {
            var res = Base_DeptInfoService.LoadEntities(d => d.Id == id).FirstOrDefault();
            return Json(res);
        }
        public ActionResult Edit(string id)
        {
            var m = Base_DeptInfoService.LoadEntities(d => d.Id == id).FirstOrDefault();
            if (m != null)
            {
                return View(m);
            }
            else
            {
                return View(new Base_DeptInfo());
            }

        }
        [HttpPost]
        public ActionResult Edit(Base_DeptInfo model)
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
            try
            {
                var res = Base_DeptInfoService.OperateModel(model);
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

        public ActionResult AddSub(string ParentId)
        {
            return View(new Base_DeptInfo() { ParentId = ParentId });

        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {
            var res = Base_DeptInfoService.LoadEntities(d => d.Id == id).Select(d=>d.ParentId).FirstOrDefault();
            if (string.IsNullOrEmpty(res))
            {
                result.statusCode = "300";
                result.message = GlobalLngResource.GetRes("noDelRoot");
                return Json(result);
            }
            Base_DeptInfoService.GetIds(id);
            var r = Base_DeptInfoService.Delete(Base_DeptInfoService.DelList, id);
            if (r > 0)
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


        [HttpPost]

        public ActionResult AddSub(Base_DeptInfo model)
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
            try
            {
                var res = Base_DeptInfoService.AddSub(model);
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



    }
}
