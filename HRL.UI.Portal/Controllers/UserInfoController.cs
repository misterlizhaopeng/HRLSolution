using HRL.BLL;
using HRL.Common;
using HRL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HRL.UI.Portal.Controllers
{
    public class UserInfoController : BaseController
    {
        Base_UserInfoService service = new Base_UserInfoService();
        BjuiJsonResult result = new BjuiJsonResult();
        //CommonDbLogRecording commonDbLogRecording = new CommonDbLogRecording();
        string tableName = "Base_UserInfo";
        static string[] notfield = new string[] { "UserPwd", "DeptName" };

        #region CRUD

        /// <summary>
        /// 加载页面
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Index(Base_UserInfo model)
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
            //var Id = Request["Id"] ?? string.Empty;
            if (!string.IsNullOrEmpty(id) && id.Length == 36)
            {
                //show 编辑
                ViewBag.ShowModel = service.GetModel(id);
            }
            else
            {
                ViewBag.ShowModel = new Base_UserInfo();
            }


            #region 显示所有角色信息(如果当前人已经选择了指定的角色，默认选中)
            Base_RolesInfoService roleService = new Base_RolesInfoService();
            Base_UserRoleInfoService userRole = new Base_UserRoleInfoService();
            var roles = roleService.LoadEntities(r => r.DelFlag == 0).OrderBy(r => r.OrderBy).Select(r => new { r.Id, r.RoleName }).ToList();
            StringBuilder builder = new StringBuilder();
            var curPerRoles = userRole.LoadEntities(ur => ur.UserId == id).Select(ur => ur.RoleId).Distinct();
            if (curPerRoles != null)
            {
                roles.ForEach(r =>
                {
                    if (curPerRoles.Contains(r.Id))
                    {
                        builder.Append("<input type=\"checkbox\" name=\"roleIds\" checked=\"checked\" value=\"" + r.Id + "\"/>" + r.RoleName);

                    }
                    else
                    {
                        builder.Append("<input type=\"checkbox\" name=\"roleIds\"  value=\"" + r.Id + "\"/>" + r.RoleName);
                    }
                });
            }
            else
            {
                roles.ForEach(r => builder.Append("<input type=\"checkbox\" name=\"roleIds\"  value=\"" + r.Id + "\"/>" + r.RoleName));
            }
            ViewBag.ShowRoles = builder.ToString();
            #endregion

            return View();
        }

        /// <summary>
        /// 添加或者编辑
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Edit(Base_UserInfo model)
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
                //return RedirectToAction("Login", "Login");
                result.statusCode = "300";
                result.message = GlobalLngResource.GetRes("ToLoginAgain");
                return Json(result);
            }
            model.Creator = model.Modified = CurrentUserInfo.Id;
            try
            {
                //只在添加时判断
                if (string.IsNullOrEmpty(model.Id) || model.Id.Length != 36)
                {
                    var existSameUserInfo = service.ExistSameUserInfo(model);
                    if (existSameUserInfo)
                    {
                        result.statusCode = "300";
                        result.message = GlobalLngResource.GetRes("ExistSameUserInfo");
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
            Base_UserInfo delModel = new Base_UserInfo() { Id = id };
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

        #region Export
        /// <summary>
        /// 导出所有
        /// </summary>
        public void ExportAllToExcel()
        {
            try
            {
                var resList = this.service.LoadEntities(s => s.DelFlag == 0).ToList(); // lngMyjhService.LoadLng("0").ToList();
                ExcelHelperEF<Base_UserInfo>.ListToExcel(resList, "ExcelResult" + DateTime.Now.ToString("yyyyMMddhhmmss").Replace("-", ""), tableName, notfield);
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 导出当前页
        /// </summary>
        public void ExportCurrentToExcel(Base_UserInfo model)
        {
            try
            {
                int cou = 0;
                var temp = this.service.GetPaging(model, out cou);
                ExcelHelperEF<Base_UserInfo>.ListToExcel(temp, "ExcelResult" + DateTime.Now.ToString("yyyyMMddhhmmss").Replace("-", ""), tableName, notfield);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}
