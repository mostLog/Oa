using MI.Core.Domain;
using MI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using MI.Application.Dto;
using MI.Core.Extension;
using System.Data.Entity;

namespace MI.Application
{
    public partial class FlightFeeService : BaseService<FlightFee>, IFlightFeeService
    {
        private readonly IBaseRepository<FlightFee> _repository;
        private readonly IBaseRepository<Employee> _employee;
        private readonly ISTypeService _typeService;
        public FlightFeeService(
            IBaseRepository<FlightFee> repository, 
            IBaseRepository<Employee> employee,
            ISTypeService typeService
            ) :
            base(repository)
        {
            _repository = repository;
            _employee = employee;
            _typeService = typeService;
        }

        public IList<FlightFee> GetConditionByWhere(Expression<Func<FlightFee, bool>> predicate, int pageIndex, int pageSize, out int count)
        {
            var linq = _repository.GetAll().Where(predicate).OrderByDescending(u => u.f_ID);
            count = linq.Count();
            ValidatePagingWhere(linq.Count(), ref pageIndex, pageSize);
            return linq.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        #region 小白-2017-6-20
        /// <summary>
        /// 带条件分页获取数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PageListDto<FlightFeeListDto> GetPagedFlightFeeAllDatas(FlightFeePagedInputDto input)
        {
            var locations = _typeService.GetsTypeByWhere((int)sTypeEnum.上班地点类型);
            var list = _repository.GetAll();
            list = list
                .AsNoTracking()
                .OrderBy(m => m.f_operator)
                .WhereIf(!input.Name.IsNullOrEmpty(), m => m.t_employeeInfo.f_chineseName == input.Name)
                .WhereIf(input.FlightStartDate != null, m => m.f_FlightDate!=null&& m.f_FlightDate >= input.FlightStartDate)
                .WhereIf(input.FlightEndDate != null, m => m.f_FlightDate!=null&& m.f_FlightDate <= input.FlightEndDate)
                .WhereIf(input.IsPay != 2, m => m.f_IsPay == (input.IsPay == 1 ? true : false));

            var flightList = list
                .PageBy(input.iPageIndex, input.iPageSize)
                .ToList()
                .Select(m =>
                {
                    return new FlightFeeListDto
                    {
                        Id = m.f_ID,
                        EmployeeName = m.t_employeeInfo?.f_chineseName ?? string.Empty,
                        Amount = m.f_Amount,
                        FlightDate = m.f_FlightDate,
                        Flight = m.f_Flight,
                        StartEndAdd = m.f_StartEndAdd,
                        IsPay = m.f_IsPay,
                        Location = locations.FirstOrDefault(c => c.f_tID == m.f_AddressId)?.f_value ?? string.Empty,
                        Operator = m.f_operator,
                        OperatorTime = m.f_operatorTime,
                        Remark = m.f_Remark
                    };
                });

            int count = list.Count();

            return new PageListDto<FlightFeeListDto>(flightList, input.iPageIndex, input.iPageSize, count);
        } 
        #endregion
    }
}
