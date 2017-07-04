using MI.Application.Dto;
using MI.Core.Domain;
using System;
using System.Collections.Generic;

namespace MI.Application
{
    public interface ICompanyOfFoodService
    {
        /// <summary>
        /// 获取公司用餐统计信息
        /// </summary>
        /// <returns></returns>
        IList<CompanyOfFood> GetCompanyOfFoods();
        /// <summary>
        /// 公司用餐信息统计
        /// </summary>
        /// <returns></returns>
        int StatInformation();
        /// <summary>
        /// 获取宿舍订餐信息
        /// </summary>
        /// <returns></returns>
        PageListDto<DorOrderListDto> GetDorOrderPagedAllDatas(DorOrderPagedInputDto input);

        List<OrderingEmployeeCollectHistory> GetOrderingEmployeeCollectHistory(DateTime date);

        List<string> GetDistinctDateByOrderingEmployeeCollectHistory();

        void UpdateCompanyOfFood(CompanyOfFood model);

    }
}
