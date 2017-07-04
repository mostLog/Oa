using MI.Application.Dto;
using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application
{
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
