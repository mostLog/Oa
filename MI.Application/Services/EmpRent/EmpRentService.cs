using AutoMapper;
using MI.Application.Dto;
using MI.Application.OASession;
using MI.Application.OASession.Dto;
using MI.Core.Domain;
using MI.Core.Extension;
using MI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MI.Application
{
    /// <summary>
    /// 员工租房服务
    /// </summary>
    public class EmpRentService : BaseService<EmpRent>,IEmpRentService
    {
        //租房表仓储
        private readonly IBaseRepository<EmpRent> _repository;
        //字典表服务
        private readonly ISTypeService _stypeService;
        //换房
        private readonly IBaseRepository<ChangeRoom> _changeRoomRepository;

        private readonly ISession _session;
        public EmpRentService(
            IBaseRepository<EmpRent> repository, 
            IBaseRepository<ChangeRoom> changeRoomRepository,
            ISTypeService stypeService,
            ISession session
            ):base(repository)
        {
            _repository = repository;
            _changeRoomRepository = changeRoomRepository;
            _stypeService = stypeService;
            _session = session;
        }

        public IList<EmpRentDto> GetEmpRent(Func<EmpRent, bool> predicate)
        {
            var dto = _repository.GetAll().Where(predicate).ToList(); 
            IList<EmpRentDto> dtoEmpRent = Mapper.Map<IList<EmpRentDto>>(_repository.GetAll().Where(predicate).ToList());
            return dtoEmpRent;
        }
        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="o_Count">总数</param>
        /// <returns></returns>
        public IList<EmpRentDto> GetEmpRentByWhere(Expression<Func<EmpRent, bool>> predicate, int iPageIndex, int iPageSize, out int o_Count) 
        {
            var lstEmpRent = _repository.GetAll().Where(predicate);
            var empRent = lstEmpRent.OrderByDescending(m => m.f_Id)
                .PageBy(iPageIndex, iPageSize);
            IList<EmpRentDto> dtoEmpRent = Mapper.Map<IList<EmpRentDto>>(empRent.ToList());
            o_Count = lstEmpRent.Count();
            return dtoEmpRent;
        }
        #region 员工租房模块-小白
        /// <summary>
        /// 分页数据查询
        /// 
        /// 小白-2017-6-12
        /// </summary>
        /// <param name="input">参数实体类</param>
        /// <returns></returns>
        public PageListDto<EmpRentListDto> GetEmpRentAllData(EmpRentPagedInputDto input)
        {
            //获取上班地点字典数据
            var workLocations = _stypeService.GetsTypeByWhere((int)sTypeEnum.上班地点类型);
            var lstEmpRent = _repository.GetAll();
            var empRent = lstEmpRent.OrderBy(m => m.f_Id)
                //中文名,护照名,昵称
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), m => m.t_employeeInfo != null && (m.t_employeeInfo.f_chineseName.Contains(input.Name.Trim())) || m.t_employeeInfo.f_nickname.Contains(input.Name.Trim()) || m.t_employeeInfo.f_passportName.Contains(input.Name.Trim()))
                //房间号
                .WhereIf(!string.IsNullOrWhiteSpace(input.RoomNo), m => m.Dormitory != null && m.Dormitory.f_RoomNo.ToUpper().Contains(input.RoomNo.ToUpper()))
                //是否缴费
                .WhereIf(input.IsPayment != 2, m => m.f_IsPayment == (input.IsPayment == 1 ? true : false))
                //楼栋
                .WhereIf(!string.IsNullOrWhiteSpace(input.Building), m => m.Dormitory != null && m.Dormitory.f_Building.ToUpper().Contains(input.Building))
                //缴费日期
                .WhereIf(input.PaymentStartDate != null , m => m.f_PaymentDate != null && m.f_PaymentDate >= input.PaymentStartDate)
                .WhereIf(input.PaymentEndDate!=null, m => m.f_PaymentDate != null && m.f_PaymentDate <= input.PaymentEndDate)
                ;
            
            //数据集合
            IList<EmpRentListDto> dtoEmpRent = empRent
                .PageBy(input.iPageIndex, input.iPageSize)
                .ToList()
                .Select(m =>
            {
                EmpRentListDto dto = new EmpRentListDto();
                dto.Id = m.f_Id;
                dto.EmployeeName = m.t_employeeInfo?.f_chineseName ?? string.Empty;
                dto.DeptName = m.t_employeeInfo?.STypeDepartment?.f_value ?? string.Empty;
                dto.DormitoryName = m.Dormitory?.f_Community ?? string.Empty + "/" + m.Dormitory?.f_Building ?? string.Empty + "/" + m.Dormitory?.f_RoomNo ?? string.Empty;
                dto.Rent = m.f_Rent;
                dto.Grant = m.f_Grant;
                dto.Amount = m.f_Amount;
                dto.PaymentDate = m.f_PaymentDate;
                dto.IsPayment = m.f_IsPayment;
                dto.Payee = m.f_Payee;
                dto.Address = workLocations.FirstOrDefault(c => c.f_tID == m.f_AddressId)?.f_value ?? string.Empty;
                dto.EffectiveDate = m.f_EffectiveDate;
                dto.Operator = m.f_operator;
                dto.OperatorTime = m.f_operatorTime;
                dto.Remark = m.f_Remark;
                return dto;

            }).ToList();
            //数量
            int count = empRent.Count();
            return new PageListDto<EmpRentListDto>(dtoEmpRent, input.iPageIndex, input.iPageSize, count);
        }
        /// <summary>
        /// 补录当月数据
        /// </summary>
        /// <returns></returns>
        public int MakeUpCurrMonthDatas()
        {
            OAUser user = _session.GetCurrUser();
            DateTime nextMonth = DateTime.Now.AddMonths(1);
            DateTime currM = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); 
            DateTime nextM = new DateTime(nextMonth.Year, nextMonth.Month, 1); 
            var lsitChangeRoom =_changeRoomRepository
                .GetAll()
                .Where(p => p.f_NewRoomId != null && p.f_EffectiveMonths != null && p.f_SewRent > 0 && p.f_EffectiveMonths >= currM && p.f_EffectiveMonths < nextM).ToList();
            int count = 0;
            foreach (var ChangeRoom in lsitChangeRoom)
            {
                if (_repository.GetAll().Any(p => p.f_PaymentDate >= currM && p.f_PaymentDate < nextM && p.f_eid == ChangeRoom.f_eid && p.f_DormitoryId == ChangeRoom.f_NewRoomId))
                {
                    continue;
                }
                EmpRent emp = new EmpRent
                {
                    f_DormitoryId = ChangeRoom.f_NewRoomId,
                    f_eid = ChangeRoom.f_eid,
                    f_Rent = 0,
                    f_Grant = 0,
                    f_PaymentDate = ChangeRoom.f_EffectiveMonths,
                    f_Amount = ChangeRoom.f_SewRent,
                    f_IsPayment = false,
                    f_Payee = null,
                    f_operator = user.NickName,
                    f_operatorTime = DateTime.Now,
                    f_AddressId = null
                };
                _repository.Insert(emp);
                count++;
            }
            return count;
        }
        /// <summary>
        /// 生成下月数据
        /// </summary>
        /// <returns></returns>
        public int GenerateNextMonthDatas()
        {
            //登陆用户信息
            OAUser user = _session.GetCurrUser();
            DateTime nextMonth = DateTime.Now.AddMonths(1);
            DateTime nextNextMonth = nextMonth.AddMonths(1);
            DateTime currM = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime nextM = new DateTime(nextMonth.Year, nextMonth.Month, 1);
            DateTime nextNextM = new DateTime(nextNextMonth.Year, nextNextMonth.Month, 1);
            //获取本月租房信息
            var empList = _repository
                .GetAll()
                .Where(c => c.f_PaymentDate >= currM && c.f_PaymentDate < nextM)
                .ToList();

            int count = 0;
            foreach (var item in empList)
            {

                if (_repository.GetAll().Any(c => c.f_PaymentDate >= nextM && c.f_PaymentDate < nextNextM && c.f_Id == item.f_Id))
                {
                    continue;
                }
                ChangeRoom tChangeRoom = _changeRoomRepository
                    .GetAll()
                    .OrderByDescending(p => p.f_FilingDate)
                    .FirstOrDefault(p => p.f_eid == item.f_eid && p.f_NewRoomId != item.f_DormitoryId && p.f_Progress == "已换房");
                EmpRent tNewEmpRent = new EmpRent
                {
                    f_DormitoryId = tChangeRoom != null ? tChangeRoom.f_NewRoomId : item.f_DormitoryId,
                    f_eid = item.f_eid,
                    f_Rent = item.f_Rent,
                    f_Grant = item.f_Grant,
                    f_PaymentDate = nextM,
                    f_Amount = item.f_Amount,
                    f_IsPayment = false,
                    f_Payee = null,
                    f_operator = user.NickName,
                    f_operatorTime = DateTime.Now,
                    f_AddressId = null
                };
                _repository.Insert(tNewEmpRent);
                count++;
            }
            return count;
        }
        #endregion
    }
}
