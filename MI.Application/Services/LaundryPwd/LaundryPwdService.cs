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
        private readonly IBaseRepository<LaundryPwd> _repository;
        public LaundryPwdService(IBaseRepository<LaundryPwd> repository)
        {
            _repository = repository;
        }

        public IList<LaundryPwdDto> GetEmpRent()
        {
            IList<LaundryPwdDto> dtoEmpRent = Mapper.Map<IList<LaundryPwdDto>>(_repository.GetAll().ToList());
            return dtoEmpRent;
        }

        /// <summary>
        /// 根据社区，楼栋查询本楼栋的所有洗衣房
        /// </summary>
        /// <param name="community">社区</param>
        /// <param name="building">楼栋</param>
        /// <returns></returns>
        public IList<LaundryPwd> GetTariffbyBuilding(string community, string building)
        {
            IList<LaundryPwd> list;
            list = _repository.GetAll().Where(u => u.f_Community == community && u.f_Building == building && u.f_RoomType == 2).ToList();
            return list;
        }
        /// <summary>
        /// 根据条件返回列表
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>返回LaundryPwd集合</returns>
        public List<LaundryPwd> GetLaundryPwdByWhere(Func<LaundryPwd, bool> predicate)
        {
            return _repository.GetAll().Where(predicate).OrderByDescending(u => u.f_Id).ToList();
        }
        /// <summary>
        /// 根据id获取一条数据
        /// </summary>
        /// <param name="iId">主键id</param>
        /// <returns></returns>
        public LaundryPwd GetLaundryPwdById(int iId)
        {
            return _repository.GetEntityById(iId);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="oModel">LaundryPwd实体</param>
        public void AddLaundryPwdOneData(LaundryPwd oModel)
        {
            _repository.Insert(oModel);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="oModel">LaundryPwd实体</param>
        public void EditLaundryPwdOneData(LaundryPwd oModel)
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
            LaundryPwd model = GetLaundryPwdById(iId);
            _repository.Delete(model);
        }
    }
}
