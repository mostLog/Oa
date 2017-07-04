using MI.Application.Dto;
using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MI.Application
{
    public interface ITariffService:IBaseService<Tariff>
    {
        IList<Tariff> GetEmpRent();
        /// <summary>
        /// 条件搜索分页查询
        /// 
        /// 小白-2017-6-16
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PageListDto<TariffListDto> GetPagedAllTariffs(TariffPagedInputDto input);
        /// <summary>
        /// 分页查询部门数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        List<Tariff> GetPageAllTariff(int iDeptId, int pageIndex, int pageSize, out int count);
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IList<Tariff> GetConditionByWhere(Func<Tariff, bool> predicate, int pageIndex, int pageSize, out int count);

        
       
    }
}
