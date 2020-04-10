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
    public class Base_RolesInfoController : BaseController
    {

        BjuiJsonResult result = new BjuiJsonResult();
        Base_RolesInfoService service = new Base_RolesInfoService();

        #region CRUD
        public ActionResult Index(Base_RolesInfo model)
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
            if (!string.IsNullOrEmpty(id) && id.Length == 36)
            {
                //show 编辑
                ViewBag.ShowModel = service.GetModel(id);
            }
            else
            {
                ViewBag.ShowModel = new Base_RolesInfo();
            }
            #region 给角色分配菜单
            Base_MenuInfoService base_MenuInfoService = new Base_MenuInfoService();
            var tmpRes = base_MenuInfoService.LoadEntities(m => m.DelFlag == 0).OrderBy(m => m.OrderBy).Select(m => new { m.Id, m.MenuName, m.ParentId });
            StringBuilder builder = new StringBuilder();
            if (tmpRes != null)
            {
                #region 找指定角色的所有已经选中的菜单menuIds
                Base_RoleMenuInfoService base_RoleMenuInfoService = new Base_RoleMenuInfoService();
                var menuIds = base_RoleMenuInfoService.LoadEntities(rm => rm.RoleId == id).Select(rm => rm.MenuId);
                #endregion

                #region 默认情况下，一个角色对应多条菜单的赋值
                StringBuilder builderDefaultMenus = new StringBuilder();
                foreach (var item in menuIds)
                {
                    builderDefaultMenus.Append(item + ",");
                }
                var dfStr = builderDefaultMenus.ToString();
                if (dfStr.Length > 0)
                {
                    ViewBag.DefaultMenus = dfStr.Remove(dfStr.Length - 1, 1);
                } 
                #endregion

                foreach (var item in tmpRes)
                {
                    if (menuIds != null && menuIds.Contains(item.Id))
                    {
                       
                        //选中已经选中的菜单
                        //    { id: 'd', pId: 'w', name: 'd',checked:true },
                        builder.AppendLine("{id: '" + item.Id + "', pId: '" + item.ParentId + "', name: '" + item.MenuName + "',checked:true }, ");
                    }
                    else
                    {
                        builder.AppendLine("{id: '" + item.Id + "', pId: '" + item.ParentId + "', name: '" + item.MenuName + "' }, ");
                    }
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
        public ActionResult Edit(Base_RolesInfo model)
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
                if (string.IsNullOrEmpty(model.Id) || model.Id.Length != 36)
                {
                    var existSameRoleInfo = service.ExistSameRoleInfo(model);
                    if (existSameRoleInfo)
                    {
                        result.statusCode = "300";
                        result.message = GlobalLngResource.GetRes("ExistSameRoleInfo");
                        return Json(result);
                    }
                }
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
        //    var model = new Base_RolesInfo() { Id = id };
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
            Base_RolesInfo delModel = new Base_RolesInfo() { Id = id };
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
