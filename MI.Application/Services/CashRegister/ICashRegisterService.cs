using MI.Application.Dto;
using MI.Core.Domain;
using System.Collections.Generic;

namespace MI.Application
{
    public interface ICashRegisterService:IBaseService<CashRegister>
    {
        /// <summary>
        /// 分页获取现金登记数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        PageListDto<CashRegisterListDto> GetPagedCashRegisterDatas(CashRegisterPagedInputDto input);
        /// <summary>
        /// 更新汇率信息
        /// </summary>
        /// <returns></returns>
        void UpdateRate(RateInputDto input);
        /// <summary>
        /// 返回汇率信息
        /// </summary>
        /// <returns></returns>
        IList<KeyValueDto<string, string>> GetRates();
    }
}
