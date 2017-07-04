using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MI.Application.Dto;
using MI.Core.Domain;

namespace MI.Application
{
    public interface ICertAgencyService
    {
        /// <summary>
        /// 根据员工id查询证件信息
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        List<CertAgencyDto> GetAgencyByEid(int eid);
        /// <summary>
        /// 查询主管所在部门的所有证件信息
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        List<CertAgencyDto> GetAgencyBySector(int eid);

        /// <summary>
        /// 根据Id获得证件信息
        /// </summary>
        /// <param name="iId"></param>
        /// <returns></returns>
        CertAgency GetCertAgencyById(int iId);

        /// <summary>
        /// 修改证件办理
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        void EditCertAgencyData(CertAgency model);

        /// <summary>
        /// 新增证件办理
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        void AddCertAgencyData(CertAgency model);

        /// <summary>
        /// 删除证件办理信息
        /// </summary>
        /// <param name="iId">要删除的ID</param>
        /// <returns></returns>
        void DeleteCertAgency(int iId);

        /// <summary>
        /// 根据条件获得证件办理数据
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">页大小</param>
        /// <param name="o_Count">总数</param>
        /// <returns></returns>
        IList<CertAgency> GetCertAgencyByWhere(CertAgencyWhereDto caw, int iPageIndex, int iPageSize, out int o_Count);
    }
}
