using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MI.Application.Dto;

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

        int UpdateCompanyOfFood(CompanyOfFood model);

    }
}
