using HRL.BLL;
using HRL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HRL.UI.Portal.Controllers
{
    public class Base_MenuInfoController : BaseController
    {
        BjuiJsonResult result = new BjuiJsonResult();
        Base_MenuInfoService service = new Base_MenuInfoService();

        #region CRUD
        public ActionResult Index(Base_MenuInfo model)
        {
            int count = 0;
            try
            {
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
            Base_MenuInfo md = new Base_MenuInfo();
            if (!string.IsNullOrEmpty(id) && id.Length == 36)
            {
                //show 编辑
                ViewBag.ShowModel = md = service.GetModel(id);
            }
            else
            {
                ViewBag.ShowModel = new Base_MenuInfo();
            }


            #region 默认情况下， 给一个菜单穿过去已经选择的该父级别菜单
            ViewBag.DefaultMenus = md.ParentId;
            #endregion

            #region 获取操作菜单的的父级（1级和2级）
            var firstTmp = service.LoadEntities(m => m.DelFlag == 0 && m.ParentId == null).OrderBy(m=>m.OrderBy).Select(m => new { m.Id, m.ParentId, m.MenuName });
            var secondTmp = service.LoadEntities(m => firstTmp.Select(me => me.Id).Contains(m.ParentId)).OrderBy(m => m.OrderBy).Select(m => new { m.Id, m.ParentId, m.MenuName });
            var tmpRes = firstTmp.Union(secondTmp);
            StringBuilder builder = new StringBuilder();
            foreach (var item in tmpRes)
            {
                if (md.ParentId == item.Id)
                {
                    builder.AppendLine("{id: '" + item.Id + "', pId: '" + item.ParentId + "', name: '" + item.MenuName + "',checked:true }, ");
                }
                else
                {
                    builder.AppendLine("{id: '" + item.Id + "', pId: '" + item.ParentId + "', name: '" + item.MenuName + "'}, ");
                }
            }
            ViewBag.SelectMenuInfo = builder.ToString();
            #endregion
            return View();
        }

        /// <summary>
        /// 添加和编辑的显示页面
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Base_MenuInfo model)
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
                //只在添加时判断
                // 菜单肯定会存在重复，所以此判断没必要
                //if (string.IsNullOrEmpty(model.Id) || model.Id.Length != 36)
                //{
                //    var existSameRoleInfo = service.ExistSameRoleInfo(model);
                //    if (existSameRoleInfo)
                //    {
                //        result.statusCode = "300";
                //        result.message = GlobalLngResource.GetRes("ExistSameMenuInfo");
                //        return Json(result);
                //    }
                //}

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

        //public ActionResult Delete(string id)
        //{
        //    var model = new Base_MenuInfo() { Id = id };
        //    var res = service.DeleteEntity(model);
        //    if (res > 0)
        //    {
        //        result.statusCode = "200";
        //        result.message = GlobalLngResource.GetRes("operSuccess");
        //    }
        //    else
        //    {
        //        result.statusCode = "300";
        //        result.message = GlobalLngResource.GetRes("operFailure");
        //    }
        //    return Json(result);
        //}

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                result.statusCode = "300";
                result.message = GlobalLngResource.GetRes("noRecords");
                return Json(result);
            }
            Base_MenuInfo delModel = new Base_MenuInfo() { Id = id };
            try
            {
                List<string> idList = new List<string>() { id };
                var res = service.Delete(idList);
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
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                result.statusCode = "300";
                result.message = GlobalLngResource.GetRes("sysException");
            }
            catch (Exception ex)
            {
                result.statusCode = "300";
                result.message = GlobalLngResource.GetRes("sysException");
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

        #endregion
    }
}
