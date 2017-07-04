using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application
{
    public interface IDormitorySummaryService
    {
        /// <summary>
        /// 获取住宿汇总数据
        /// </summary>
        /// <param name="sCommunity">社区</param>
        /// <param name="sBuilding">楼栋</param>
        /// <returns></returns>
        List<DormitorySummaryViewModel> getDormitorySummary(string sCommunity, string sBuilding);
        /// <summary>
        ///  获取住宿汇总数据
        /// </summary>
        /// <param name="iCommunity">社区ID</param>
        /// <param name="iBuilding">楼栋ID</param>
        /// <returns></returns>
        List<DormitorySummaryViewModel> getDormitorySummary(int iCommunity, int iBuilding);
        /// <summary>
        /// 新增情侣/夫妻关系
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        void AddCoupleRegister(CoupleRegister model);
        /// <summary>
        /// 检测员工ID是否正常，是否存在重复
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string CheckCoupleRegister(CoupleRegister model);
        /// <summary>
        /// 获取情侣关系登记
        /// </summary>
        /// <returns></returns>
        List<CoupleRegister> GetCoupleRegister();

        /// <summary>
        /// 获取情侣关系登记（分页）
        /// </summary>
        /// <returns></returns>
        List<CoupLeManageView> GetCoupleRegister(string keyWords, int iPageIndex, int iPageSize, out int o_Count);
        /// <summary>
        /// 修改情侣关系登记
        /// </summary>
        /// <param name="model"></param>
        void UpdateCoupleRegister(CoupleRegister model);
        /// <summary>
        /// 删除情侣关系登记
        /// </summary>
        /// <param name="iCid"></param>
        void DeleteCoupleRegister(int iCid);

        /// <summary>
        /// 删除夫妻双方有一个不在职的夫妻关系
        /// </summary>
        void DeleteCoupleNotServ();
    }
}
