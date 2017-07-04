using AutoMapper;
using MI.Application.Dto;
using MI.Core.Domain;
using MI.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MI.Application
{
    public class LaundryPwdService:ILaundryPwdService
    {
        private readonly IBaseRepository<t_LaundryPwd> _repository;
        public LaundryPwdService(IBaseRepository<t_LaundryPwd> repository)
        {
            _repository = repository;
        }

        public IList<t_LaundryPwdDto> GetEmpRent()
        {
            IList<t_LaundryPwdDto> dtoEmpRent = Mapper.Map<IList<t_LaundryPwdDto>>(_repository.GetAll().ToList());
            return dtoEmpRent;
        }

        /// <summary>
        /// 根据社区，楼栋查询本楼栋的所有洗衣房
        /// </summary>
        /// <param name="community">社区</param>
        /// <param name="building">楼栋</param>
        /// <returns></returns>
        public IList<t_LaundryPwd> GetTariffbyBuilding(string community, string building)
        {
            IList<t_LaundryPwd> list;
            list = _repository.GetAll().Where(u => u.f_Community == community && u.f_Building == building && u.f_RoomType == 2).ToList();
            return list;
        }
        /// <summary>
        /// 根据条件返回列表
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>返回LaundryPwd集合</returns>
        public List<t_LaundryPwd> GetLaundryPwdByWhere(Func<t_LaundryPwd, bool> predicate)
        {
            return _repository.GetAll().Where(predicate).OrderByDescending(u => u.f_Id).ToList();
        }
        /// <summary>
        /// 根据id获取一条数据
        /// </summary>
        /// <param name="iId">主键id</param>
        /// <returns></returns>
        public t_LaundryPwd GetLaundryPwdById(int iId)
        {
            return _repository.GetEntityById(iId);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="oModel">t_LaundryPwd实体</param>
        public void AddLaundryPwdOneData(t_LaundryPwd oModel)
        {
            _repository.Insert(oModel);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="oModel">t_LaundryPwd实体</param>
        public void EditLaundryPwdOneData(t_LaundryPwd oModel)
        {
            _repository.Update(oModel);
           // m_db.Entry(oModel).State = EntityState.Modified;
        }
        /// <summary>
        /// 根据id删除一条数据
        /// </summary>
        /// <param name="iId">id</param>
        public void DeleteLaundryPwd(int iId)
        {
            t_LaundryPwd model = GetLaundryPwdById(iId);
            _repository.Delete(model);
        }
    }
}
