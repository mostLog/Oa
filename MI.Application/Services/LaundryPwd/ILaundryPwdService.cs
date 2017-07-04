using MI.Application.Dto;
using MI.Core.Domain;
using System;
using System.Collections.Generic;

namespace MI.Application
{
    public interface ILaundryPwdService
    {
        IList<t_LaundryPwdDto> GetEmpRent();
        IList<t_LaundryPwd> GetTariffbyBuilding(string community, string building);
        /// <summary>
        /// 根据条件返回列表
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>返回LaundryPwd集合</returns>
        List<t_LaundryPwd> GetLaundryPwdByWhere(Func<t_LaundryPwd, bool> predicate);
        /// <summary>
        /// 根据id获取一条数据
        /// </summary>
        /// <param name="iId">主键id</param>
        /// <returns></returns>
        t_LaundryPwd GetLaundryPwdById(int iId);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="oModel">t_LaundryPwd实体</param>
        void EditLaundryPwdOneData(t_LaundryPwd oModel);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="oModel">t_LaundryPwd实体</param>
        void AddLaundryPwdOneData(t_LaundryPwd oModel);
        /// <summary>
        /// 根据id删除一条数据
        /// </summary>
        /// <param name="iId">id</param>
        void DeleteLaundryPwd(int iId);
    }
}
