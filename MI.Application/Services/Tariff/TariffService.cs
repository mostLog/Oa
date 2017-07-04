using MI.Application.Dto;
using MI.Core.Domain;
using MI.Core.Extension;
using MI.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MI.Application
{
    public partial class TariffService : BaseService<Tariff>, ITariffService
    {
        private readonly IBaseRepository<Tariff> _repository;

        private readonly IBaseRepository<Employee> _employee;
        private readonly ISTypeService _stypeService;
        public TariffService(IBaseRepository<Tariff> repository,
            IBaseRepository<Employee> employee,
            ISTypeService stypeService
            ) :
            base(repository)
        {
            _repository = repository;
            _employee = employee;
            _stypeService = stypeService;
        }
        /// <summary>
        /// 分页查询所有数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public IList<Tariff> GetConditionByWhere(Func<Tariff, bool> predicate, int pageIndex, int pageSize, out int count)
        {
            var linq = _repository.GetAll().Where(predicate).OrderByDescending(u => u.f_Id);
            count = linq.Count();
            ValidatePagingWhere(count, ref pageIndex, pageSize);
            return linq.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public IList<Tariff> GetEmpRent()
        {
            IList<Tariff> dtoEmpRent = _repository.GetAll().ToList();
            return dtoEmpRent;
        }
        /// <summary>
        /// 分页查询部门数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<Tariff> GetPageAllTariff(int iDeptId, int pageIndex, int pageSize, out int count)
        {
            var lstRes = (from s in _employee.GetAll().ToList()
                          where s.f_department_tID == iDeptId
                          select new { s.f_dormitoryId }).Distinct();

            var linq = (from c in _repository.GetAll().ToList()
                        join s in lstRes on c.f_DormitoryId equals s.f_dormitoryId
                        select c).OrderByDescending(c => c.f_Id);
            count = linq.Count();
            ValidatePagingWhere(count, ref pageIndex, pageSize);
            return linq.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
        #region 小白-2017-6-16
        /// <summary>
        /// 条件搜索分页查询
        /// 
        /// 小白-2017-6-16
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PageListDto<TariffListDto> GetPagedAllTariffs(TariffPagedInputDto input)
        {
            //获取上班地点字典数据
            var workLocations = _stypeService.GetsTypeByWhere((int)sTypeEnum.上班地点类型);
            var tariffList = _repository.GetAll();
            tariffList = tariffList
                .OrderBy(m => m.f_Id)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Community), m => m.Dormitory.f_Community.Contains(input.Community))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Building), m => m.Dormitory.f_Building.Contains(input.Building))
                .WhereIf(!string.IsNullOrWhiteSpace(input.RoomNo), m => m.Dormitory.f_RoomNo.Contains(input.RoomNo))
                .WhereIf(input.TariffStartDate != null, m =>m.f_TariffDate!=null&& m.f_TariffDate >= input.TariffStartDate.Value)
                .WhereIf(input.TariffEndDate != null, m => m.f_TariffDate != null && m.f_TariffDate <= input.TariffEndDate.Value)
                .WhereIf(input.IsPayment != 2, m => m.f_IsPayment == (input.IsPayment == 1 ? true : false));

            var list = tariffList
                .PageBy(input.iPageIndex, input.iPageSize)
                .ToList()
                .Select(m =>
                {
                    return new TariffListDto()
                    {
                        Id = m.f_Id,
                        Community = m.Dormitory?.f_Community ?? string.Empty,
                        Building = m.Dormitory?.f_Building ?? string.Empty,
                        RoomNo = m.Dormitory?.f_RoomNo ?? string.Empty,
                        TariffStandard = m.f_TariffStandard,
                        RealBill = m.f_RealBill,
                        Overruns = m.f_Overruns,
                        TariffDate = m.f_TariffDate,
                        Registrant = m.f_Registrant,
                        IsPayment = m.f_IsPayment,
                        Rate = m.f_Rate,
                        Location = workLocations.FirstOrDefault(c => c.f_tID == m.f_AddressId)?.f_value ?? string.Empty,
                        Operator = m.f_operator,
                        OperatorTime = m.f_operatorTime,
                        Remark = m.f_Remark
                    };
                });
            int count = tariffList.Count();
            return new PageListDto<TariffListDto>(list, input.iPageIndex, input.iPageSize, count);
        }
        #endregion
    }
}
