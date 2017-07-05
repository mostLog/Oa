using MI.Application.Dto;
using MI.Core.Domain;
using MI.Data.Uow;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MI.Application
{
    [UnitOfWork]
    public interface IFlightFeeService : IBaseService<FlightFee>
    {
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IList<FlightFee> GetConditionByWhere(Expression<Func<FlightFee, bool>> predicate, int pageIndex, int pageSize, out int count);
        /// <summary>
        /// 带条件分页获取数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PageListDto<FlightFeeListDto> GetPagedFlightFeeAllDatas(FlightFeePagedInputDto input);
    }
}
