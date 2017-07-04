using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MI.Core.Domain;
using MI.Application.Dto;

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
        int UpdateRate(RateInputDto input);
        /// <summary>
        /// 返回汇率信息
        /// </summary>
        /// <returns></returns>
        IList<KeyValueDto<string, string>> GetRates();
    }
}
