using HRL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRL.BLL
{
    public partial class Base_RolesInfoService
    {
        public List<Base_RolesInfo> GetPaging(Base_RolesInfo model, out int count)
        {
            List<Base_RolesInfo> backDtList = new List<Base_RolesInfo>();
            //延迟加载;
            var isAsc = model.orderDirection == "asc" ? true : false;
            var temp = this.LoadEntities(l => l.DelFlag == 0);
            if (!string.IsNullOrEmpty(model.RoleName))
            {
                temp = temp.Where(l => l.RoleName.Contains(model.RoleName));
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
        public Base_RolesInfo GetModel(string id)
        {
            var res = this.LoadEntities(r => r.DelFlag == 0 && r.Id == id).FirstOrDefault();
            if (res == null)
            {
                return new Base_RolesInfo();
            }
            return res;
        }
        /// <summary>
        /// 修改或者添加一条数据
        /// </summary>
        /// <param name="model">实体参数</param>
        /// <returns>大于0，操作成功，反之失败</returns>
        public int OperateModel(Base_RolesInfo model)
        {
            if (!string.IsNullOrEmpty(model.Id) && model.Id.Length == 36)
            {
                //编辑
                var existModel = this.LoadEntities(u => u.Id == model.Id).FirstOrDefault();
                existModel.RoleName = model.RoleName;
                existModel.OrderBy = model.OrderBy;
                #region 修改角色菜单关系
                var existLink = DbSession.Base_RoleMenuInfoRepository.LoadEntities(ur => ur.RoleId == existModel.Id);
                //1. 删除现有角色菜单关系
                foreach (var item in existLink)
                {
                    DbSession.Base_RoleMenuInfoRepository.DeleteEntity(item);
                }
                //2. 添加 新的 角色菜单关系
                if (!string.IsNullOrEmpty(model.menus))
                {
                    var menuList = model.menus.Split(',');
                    foreach (var item in menuList)
                    {
                        Base_RoleMenuInfo base_RoleMenuInfo = new Base_RoleMenuInfo();
                        base_RoleMenuInfo.Id = Guid.NewGuid().ToString();
                        base_RoleMenuInfo.RoleId = model.Id;
                        base_RoleMenuInfo.MenuId = item;
                        DbSession.Base_RoleMenuInfoRepository.AddEntity(base_RoleMenuInfo);
                    }
                }
                #endregion
            }
            else if (string.IsNullOrEmpty(model.Id))
            {

                //添加
                var addModel = new Base_RolesInfo();
                addModel.Id = Guid.NewGuid().ToString();
                addModel.RoleName = model.RoleName;
                addModel.OrderBy = model.OrderBy;
                addModel.DelFlag = 0;
                DbSession.Base_RolesInfoRepository.AddEntity(addModel);
                #region 添加 新的 角色菜单关系
                //1. 添加 新的 角色菜单关系
                if (!string.IsNullOrEmpty(model.menus))
                {
                    var menuList = model.menus.Split(',');
                    foreach (var item in menuList)
                    {
                        Base_RoleMenuInfo base_RoleMenuInfo = new Base_RoleMenuInfo();
                        base_RoleMenuInfo.Id = Guid.NewGuid().ToString();
                        base_RoleMenuInfo.RoleId = model.Id;
                        base_RoleMenuInfo.MenuId = item;
                        DbSession.Base_RoleMenuInfoRepository.AddEntity(base_RoleMenuInfo);
                    }
                }
                #endregion
            }
            return DbSession.SaveChange();
        }

        /// <summary>
        /// 判断是否存在相同记录
        /// </summary>
        /// <param name="model">参数</param>
        /// <returns>true表示存在相同记录，false表示不存在</returns>
        public bool ExistSameRoleInfo(Base_RolesInfo model)
        {
            var userInfo = this.LoadEntities(u => u.RoleName == model.RoleName).FirstOrDefault();
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
    }
}
