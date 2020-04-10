using HRL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRL.BLL
{
    public partial class Base_MenuInfoService
    {
        #region Basic CRUD
        public List<Base_MenuInfo> GetPaging(Base_MenuInfo model, out int count)
        {
            List<Base_MenuInfo> backDtList = new List<Base_MenuInfo>();
            //延迟加载;
            var isAsc = model.orderDirection == "asc" ? true : false;
            var temp = this.LoadEntities(l => l.DelFlag == 0);
            if (!string.IsNullOrEmpty(model.MenuName))
            {
                temp = temp.Where(l => l.MenuName.Contains(model.MenuName));
            }
            model.orderField = !string.IsNullOrEmpty(model.orderField) ? model.orderField : "OrderBy";
            temp = temp.OrderByQueryable(model.orderField, model.orderDirection);
            var resList = temp.ToPaging(model.pageCurrent, model.pageSize, out count);
            return resList;
        }
        /// <summary>
        /// 编辑时，获取指定信息；还有显示详细信息Detail
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Base_MenuInfo GetModel(string id)
        {
            var res = this.LoadEntities(r => r.DelFlag == 0 && r.Id == id).FirstOrDefault();
            if (res == null)
            {
                return new Base_MenuInfo();
            }
            return res;
        }
        /// <summary>
        /// 修改或者添加一条数据
        /// </summary>
        /// <param name="model">实体参数</param>
        /// <returns>大于0，操作成功，反之失败</returns>
        public int OperateModel(Base_MenuInfo model)
        {
            if (!string.IsNullOrEmpty(model.Id) && model.Id.Length == 36)
            {
                //编辑
                var existModel = this.LoadEntities(u => u.Id == model.Id).FirstOrDefault();
                existModel.MenuName = model.MenuName;
                existModel.MenuUrl = model.MenuUrl;
                existModel.OrderBy = model.OrderBy;
                existModel.ParentId = model.menus;
                existModel.ModifyTime = DateTime.Now;
            }
            else if (string.IsNullOrEmpty(model.Id))
            {

                //添加
                var addModel = new Base_MenuInfo();
                addModel.Id = Guid.NewGuid().ToString();
                addModel.MenuName = model.MenuName;
                addModel.MenuUrl = model.MenuUrl;
                addModel.OrderBy = model.OrderBy;
                addModel.DelFlag = 0;
                addModel.ParentId = model.menus;
                DbSession.Base_MenuInfoRepository.AddEntity(addModel);

            }
            return DbSession.SaveChange();
        }

        /// <summary>
        /// 判断是否存在相同记录
        /// </summary>
        /// <param name="model">参数</param>
        /// <returns>true表示存在相同记录，false表示不存在</returns>
        public bool ExistSameRoleInfo(Base_MenuInfo model)
        {
            var userInfo = this.LoadEntities(u => u.MenuName == model.MenuName && u.DelFlag == 0).FirstOrDefault();
            if (userInfo != null)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="IdList">主键编号集合</param>
        /// <returns>大于0，操作成功，反之失败</returns>
        public int Delete(List<string> IdList)
        {
            foreach (var _id in IdList)
            {
                var toDelModel = this.LoadEntities(u => u.Id == _id).FirstOrDefault();
                toDelModel.DelFlag = 1;
            }
            return this.DbSession.SaveChange();
        }

        #endregion

        #region init-Services
        private Base_UserInfoService base_UserInfoService = new Base_UserInfoService();
        private Base_UserMenuInfoService base_UserMenuInfoService = new Base_UserMenuInfoService();


        private Base_UserRoleInfoService base_UserRoleInfoService = new Base_UserRoleInfoService();
        private Base_RoleMenuInfoService base_RoleMenuInfoService = new Base_RoleMenuInfoService();
        #endregion

        /// <summary>
        /// 加载菜单信息
        /// </summary>
        /// <param name="StrUserId">根据用户id</param>
        /// <param name="ParentMenuId">父类id</param>
        /// <returns></returns>
        public List<Base_MenuInfo> GetLeftMenu(string StrUserId, string ParentMenuId)
        {
            StringBuilder StrRightMenu = new StringBuilder();
            //当前人所有的角色
            //var oneUserMenus = from u in base_UserInfoService.LoadEntities(u => true)
            //                   join m in base_UserMenuInfoService.LoadEntities(um => true) on u.Id equals m.UserId
            //                   where u.Id == StrUserId
            //                   select m;

            var oneUserMenus = (from ur in base_UserRoleInfoService.LoadEntities(ur => true)
                                join rm in base_RoleMenuInfoService.LoadEntities(rm => true) on ur.RoleId equals rm.RoleId
                                where ur.UserId == StrUserId
                                select new { rm.MenuId }).Distinct();

            //找所有的菜单信息
            var tempMenuRes = from m in this.LoadEntities(m => m.DelFlag == 0)
                              join mid in oneUserMenus on m.Id equals mid.MenuId
                              select m;
            if (!string.IsNullOrEmpty(ParentMenuId))
            {
                tempMenuRes = tempMenuRes.Where(m => m.ParentId == ParentMenuId);
            }
            else
            {
                tempMenuRes = tempMenuRes.Where(m => m.ParentId == null);
            }
            tempMenuRes = tempMenuRes.OrderBy(m => m.OrderBy);
            if (tempMenuRes == null)
            {
                return new List<Base_MenuInfo>();
            }
            return tempMenuRes.ToList();
        }
        /// <summary>
        /// 加载页面菜单
        /// </summary>
        /// <returns></returns>
        public string LoadMenusInfo(string curPer)
        {
            StringBuilder sbMenu = new StringBuilder();
            List<Base_MenuInfo> pMenuList = this.GetLeftMenu(curPer, "");
            string menuName = string.Empty;
            string subMenu = string.Empty;
            var activeNav = System.Configuration.ConfigurationManager.AppSettings["ActiveName"].ToString();
            for (int i = 0; i < pMenuList.Count; i++)
            {
                menuName = pMenuList[i].MenuName.ToString();

                //第一个横向菜单,自动展开子菜单
                //if (i != 0)
                //{
                //    sbMenu.Append("<li><a href=\"javascript:;\" data-toggle=\"slidebar\"><i class=\"fa fa-check-square-o\"></i>" + menuName + "</a>");
                //    sbMenu.Append("<li><a href=\"javascript:;\" data-toggle=\"slidebar\"><i class=\"fa fa-check-square-o\"></i>" + menuName + "</a>");
                //}
                //else
                //{
                if (activeNav == menuName)
                {
                    sbMenu.Append("<li class=\"active\"><a href=\"javascript:;\" data-toggle=\"slidebar\"><i class=\"fa fa-check-square-o\"></i>" + menuName + "</a>");
                }
                else
                {
                    sbMenu.Append("<li><a href=\"javascript:;\" data-toggle=\"slidebar\"><i class=\"fa fa-check-square-o\"></i>" + menuName + "</a>");
                }

                // }
                subMenu = GetOneSubMenu(curPer, pMenuList[i].Id, menuName);
                sbMenu.Append("<div class=\"items hide\" data-noinit=\"true\">");
                sbMenu.Append(subMenu);
                sbMenu.Append("</div></li>");
                //if (menuName.Trim() != "系统管理" )//|| menuName.Trim() != "参数设置"
                //{   }
                //else
                //{
                //    // 
                //    //第一个横向菜单,自动展开子菜单
                //    if (i != 0)
                //    {
                //        sbMenu.Append("<li><a href=\"#\" data-toggle=\"slidebar\"><i class=\"fa fa-database\"></i> " + menuName + "</a>");
                //    }
                //    else
                //    {
                //        //  sbMenu.Append("<li class=\"active\"><a href=\"javascript:;\" data-toggle=\"slidebar\"><i class=\"fa fa-check-square-o\"></i>" + menuName + "</a>");
                //        sbMenu.Append("<li class=\"active\"><a href=\"#\" data-toggle=\"slidebar\"><i class=\"fa fa-database\"></i> " + menuName + "</a>");
                //    }
                //    subMenu = GetOneSubMenu(curPer, pMenuList[i].Id, menuName);
                //    sbMenu.Append("<div class=\"items hide\" data-noinit=\"true\">");
                //    sbMenu.Append("<ul class=\"menu-items\" data-faicon=\"database\">");
                //    sbMenu.Append(subMenu);
                //    sbMenu.Append("</ul></div></li>");
                //}
            }
            return sbMenu.ToString();
        }
        /// <summary>
        /// 添加一级子菜单
        /// </summary>
        /// <param name="strUserId">登录用户ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="parName">上级菜单名称</param>
        /// <returns></returns>
        private string GetOneSubMenu(string strUserId, string menuId, string parName)
        {
            StringBuilder sbMenu = new StringBuilder();
            List<Base_MenuInfo> oneMenuList = this.GetLeftMenu(strUserId, menuId);
            string menuName = string.Empty;
            string subMenu = string.Empty;
            for (int i = 0; i < oneMenuList.Count; i++)
            {

                menuName = oneMenuList[i].MenuName.ToString();

                sbMenu.Append("<ul class=\"menu-items\" data-faicon=\"folder-o\" data-tit=" + menuName + ">");
                subMenu = GetTwoSubMenu(strUserId, oneMenuList[i].Id);
                sbMenu.Append(subMenu);
                sbMenu.Append("</ul>");
                //   if (parName != "系统管理")
                //{}
                //else
                //{
                //    sbMenu.Append("<li><a href=\"" + oneMenuList[i].MenuUrl + "\" data-options=\"{id:'table-layout" + oneMenuList[i].Id + "', faicon:'refresh'}\">" + menuName + "</a></li>");
                //}
            }
            return sbMenu.ToString();

        }
        /// <summary>
        /// 添加二级子菜单
        /// </summary>
        /// <param name="strUserId">登录用户ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="parName">上级菜单名称</param>
        /// <returns></returns>
        private string GetTwoSubMenu(string strUserId, string menuId)
        {
            StringBuilder sbMenu = new StringBuilder();
            List<Base_MenuInfo> twoMenuList = this.GetLeftMenu(strUserId, menuId);
            string menuName = string.Empty; ;
            for (int i = 0; i < twoMenuList.Count; i++)
            {

                menuName = twoMenuList[i].MenuName.ToString();
                sbMenu.Append("<li><a href=\"" + twoMenuList[i].MenuUrl + "\" data-options=\"{id:'doc-file+" + twoMenuList[i].Id + "', faicon:'list'}\">" + menuName + "</a></li>");
            }
            return sbMenu.ToString();
        }
    }
}
