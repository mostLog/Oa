using MI.Application.Dto;
using MI.Core.Domain;
using MI.Core.Proxy;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MI.Application
{
    public interface IEmpRentService:IBaseService<EmpRent>
    {
        IList<EmpRentDto> GetEmpRent(Func<EmpRent, bool> predicate);

        IList<EmpRentDto> GetEmpRentByWhere(Expression<Func<EmpRent, bool>> predicate, int pageIndex, int pageSize,out int o_Count);  
        /// <summary>
        /// 分页数据查询
        /// 
        /// 小白-2017-6-12
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PageListDto<EmpRentListDto> GetEmpRentAllData(EmpRentPagedInputDto input);
        /// <summary>
        /// 补录当月数据
        /// </summary>
        /// <returns></returns>
        int MakeUpCurrMonthDatas();
        /// <summary>
        /// 生成下月数据
        /// </summary>
        /// <returns></returns>
        int GenerateNextMonthDatas();
    }
}
