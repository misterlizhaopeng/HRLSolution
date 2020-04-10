using HRL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRL.BLL
{
    public partial class Base_DeptInfoService
    {
        public List<string> DelList = new List<string>();
        /// <summary>
        /// 修改或者添加一条数据
        /// </summary>
        /// <param name="model">实体参数</param>
        /// <returns>大于0，操作成功，反之失败</returns>
        public int OperateModel(Base_DeptInfo model)
        {
            if (!string.IsNullOrEmpty(model.Id) && model.Id.Length == 36)
            {
                //编辑
                var existModel = this.LoadEntities(u => u.Id == model.Id).FirstOrDefault();
                existModel.DeptName = model.DeptName;
                existModel.Bz = model.Bz;
                existModel.OrderBy = model.OrderBy;

            }
            else if (string.IsNullOrEmpty(model.Id))
            {

                //添加
                var addModel = new Base_DeptInfo();
                addModel.Id = Guid.NewGuid().ToString();
                addModel.DeptName = model.DeptName;
                addModel.OrderBy = model.OrderBy;
                addModel.Bz = model.Bz;
                DbSession.Base_DeptInfoRepository.AddEntity(addModel);
            }
            return DbSession.SaveChange();
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddSub(Base_DeptInfo model)
        {
            //添加
            var addModel = new Base_DeptInfo();
            addModel.Id = Guid.NewGuid().ToString();
            addModel.DeptName = model.DeptName;
            addModel.OrderBy = model.OrderBy;
            addModel.ParentId = model.ParentId;
            addModel.Bz = model.Bz;
            DbSession.Base_DeptInfoRepository.AddEntity(addModel);
            return DbSession.SaveChange();
        }

        public bool GetIds(string parentId)
        {
            var res = DbSession.Base_DeptInfoRepository.LoadEntities(d => d.ParentId == parentId).Select(d => new { d.Id });
            if (res == null)
            {
                return false;
            }
            foreach (var item in res)
            {
                DelList.Add(item.Id);
                GetIds(item.Id);

            }
            return true;
        }
        public int Delete(List<string> ids, string id)
        {
            DbSession.Base_DeptInfoRepository.DeleteEntity(new Base_DeptInfo { Id = id });
            foreach (var item in ids)
            {
                DbSession.Base_DeptInfoRepository.DeleteEntity(new Base_DeptInfo { Id = item });
            }
            return DbSession.SaveChange();
        }
        private int DeleteFolder(string parentId)
        {
            var delList = DbSession.Base_DeptInfoRepository.LoadEntities(d => d.Id == parentId);
            foreach (var item in delList)
            {
                DbSession.Base_DeptInfoRepository.DeleteEntity(item);
            }
            return DbSession.SaveChange();

        }
    }
}
