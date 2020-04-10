using HRL.Common;
using HRL.Common.Utlity;
using HRL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRL.BLL
{
    public partial class Base_UserInfoService
    {

        /// <summary>
        /// 判断是否存在相同记录
        /// </summary>
        /// <param name="model">参数</param>
        /// <returns>true表示存在相同记录，false表示不存在</returns>
        public bool ExistSameUserInfo(Base_UserInfo model)
        {
            var userInfo = this.LoadEntities(u => u.UserName == model.UserName && u.DelFlag == 0).FirstOrDefault();
            if (userInfo != null)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 登录时，获取指定人员信息
        /// </summary>
        /// <param name="base_UserInfo"></param>
        /// <returns></returns>
        public Base_UserInfo Login(Base_UserInfo base_UserInfo)
        {
            var temp = this.LoadEntities(u => u.UserName == base_UserInfo.UserName &&
                u.UserPwd == base_UserInfo.UserPwd && u.DelFlag == 0).FirstOrDefault();
            if (temp==null)
            {
                return null;
            }
            Base_UserRoleInfoService base_UserRoleInfoService = new Base_UserRoleInfoService();
            Base_RolesInfoService base_RolesInfoService = new Base_RolesInfoService();
            var curPerRoles = from ur in base_UserRoleInfoService.LoadEntities(ur => true)
                              join r in base_RolesInfoService.LoadEntities(r => r.DelFlag == 0) on ur.RoleId equals r.Id
                              where ur.UserId == temp.Id
                              select r;
            if (curPerRoles != null)
            {
                temp.RoleList = curPerRoles.ToList();
            }
            else
            {
                temp.RoleList = new List<Base_RolesInfo>();
            }
            return temp;
        }
        public List<Base_UserInfo> GetPaging(Base_UserInfo model, out int count)
        {
            List<Base_UserInfo> backDtList = new List<Base_UserInfo>();
            //延迟加载;
            var isAsc = model.orderDirection == "asc" ? true : false;
            var temp = this.LoadEntities(l => l.DelFlag == 0);
            if (!string.IsNullOrEmpty(model.UserName))
            {
                temp = temp.Where(l => l.UserName.Contains(model.UserName));
            }
            model.orderField = !string.IsNullOrEmpty(model.orderField) ? model.orderField : "CreateTime";
            temp = temp.OrderByQueryable(model.orderField, model.orderDirection);
            var resList = temp.ToPaging(model.pageCurrent, model.pageSize, out count);
            return resList;
        }
        /// <summary>
        /// 编辑时，获取指定信息；还有显示详细信息Detail
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Base_UserInfo GetModel(string Id)
        {
            var tempModel = this.LoadEntities(u => u.Id == Id).FirstOrDefault();
            if (tempModel == null)
            {
                return new Base_UserInfo();
            }
            return tempModel;
        }
        /// <summary>
        /// 修改或者添加一条数据
        /// </summary>
        /// <param name="model">实体参数</param>
        /// <returns>大于0，操作成功，反之失败</returns>
        public int OperateModel(Base_UserInfo model)
        {
            if (!string.IsNullOrEmpty(model.Id) && model.Id.Length == 36)
            {
                //编辑
                var existModel = this.LoadEntities(u => u.Id == model.Id).FirstOrDefault();
                existModel.UserName = model.UserName;
                existModel.UserPwd = EncryptHelper.EncryptPwd(model.UserPwd);
                existModel.Phone = model.Phone;
                existModel.OrderBy = model.OrderBy;
                existModel.Addrs = model.Addrs;
                existModel.BirthDay = model.BirthDay;
                existModel.Bz = model.Bz;
                existModel.DeptId = model.DeptId;
                existModel.ModifyTime = DateTime.Now;
                existModel.Modified = model.Modified;
                existModel.Email = model.Email;
                #region 修改用户角色关系
                var existUserRoles = DbSession.Base_UserRoleInfoRepository.LoadEntities(ur => ur.UserId == existModel.Id);
                //1. 删除现有用户角色关系
                foreach (var item in existUserRoles)
                {
                    DbSession.Base_UserRoleInfoRepository.DeleteEntity(item);
                }
                //2. 添加 新的用户角色关系
                if (model.RoleIds != null)
                {
                    foreach (var roleId in model.RoleIds)
                    {
                        Base_UserRoleInfo userRoleModel = new Base_UserRoleInfo();
                        userRoleModel.Id = Guid.NewGuid().ToString();
                        userRoleModel.RoleId = roleId;
                        userRoleModel.UserId = existModel.Id;
                        DbSession.Base_UserRoleInfoRepository.AddEntity(userRoleModel);
                    }
                }

                #endregion
            }
            else if (string.IsNullOrEmpty(model.Id))
            {

                //添加
                var addModel = new Base_UserInfo();
                addModel.Id = Guid.NewGuid().ToString();
                addModel.UserName = model.UserName;
                addModel.UserPwd = EncryptHelper.EncryptPwd(model.UserPwd);
                addModel.Phone = model.Phone;
                addModel.OrderBy = model.OrderBy;
                addModel.Addrs = model.Addrs;
                addModel.BirthDay = model.BirthDay;
                addModel.Bz = model.Bz;
                addModel.DelFlag = 0;
                addModel.Creator = model.Creator;
                addModel.CreateTime = DateTime.Now;
                addModel.DeptId = model.DeptId;
                addModel.Email = model.Email;
                DbSession.Base_UserInfoRepository.AddEntity(addModel);
                #region 添加用户角色关系
                //1. 添加 新的用户角色关系
                if (model.RoleIds != null)
                {
                    foreach (var roleId in model.RoleIds)
                    {
                        Base_UserRoleInfo userRoleModel = new Base_UserRoleInfo();
                        userRoleModel.Id = Guid.NewGuid().ToString();
                        userRoleModel.RoleId = roleId;
                        userRoleModel.UserId = addModel.Id;
                        DbSession.Base_UserRoleInfoRepository.AddEntity(userRoleModel);
                    }
                }

                #endregion
            }


            return DbSession.SaveChange();
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
