using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MI.Core.Domain;
using MI.Data;
using System.Data.Entity;
using MI.Application.Dto;
using MI.Core.Extension;

namespace MI.Application
{
    /// <summary>
    /// 餐饮管理模块服务
    /// </summary>
    public class CompanyOfFoodService : ICompanyOfFoodService
    {
        private readonly ISTypeService _stypeService;
        private readonly IBaseRepository<CompanyOfFood> _companyOfFoodRepository;
        private readonly IBaseRepository<Employee> _employeeRepository;
        private readonly IBaseRepository<OrderingEmployees> _orderingEmployeeRepository;
        private readonly IBaseRepository<NewEmployee> _newEmployeeRepository;
        private readonly IBaseRepository<NewOrderingEmployees> _newOrderingEmployeeRepository;
        private readonly IBaseRepository<OrderingEmployeeCollectHistory> _orderingEmployeeCollectHistoryRepository;

        public CompanyOfFoodService(
            IBaseRepository<CompanyOfFood> companyOfFoodRepository,
            IBaseRepository<Employee> employeeRepository,
            IBaseRepository<OrderingEmployees> orderingEmployeeRepository,
            IBaseRepository<NewEmployee> newEmployeeRepository,
            IBaseRepository<NewOrderingEmployees> newOrderingEmployeeRepository,
            IBaseRepository<OrderingEmployeeCollectHistory> orderingEmployeeCollectHistoryRepository,
            ISTypeService stypeService
            )
        {
            _companyOfFoodRepository = companyOfFoodRepository;
            _orderingEmployeeRepository = orderingEmployeeRepository;
            _employeeRepository = employeeRepository;
            _newEmployeeRepository = newEmployeeRepository;
            _newOrderingEmployeeRepository = newOrderingEmployeeRepository;
            _orderingEmployeeCollectHistoryRepository = orderingEmployeeCollectHistoryRepository;
            _stypeService = stypeService;
        }
        /// <summary>
        /// 返回统计数据
        /// </summary>
        /// <returns></returns>
        public IList<CompanyOfFood> GetCompanyOfFoods()
        {
            var dataList = new List<CompanyOfFood>();
            //部门
            dataList.AddRange(_companyOfFoodRepository
                .GetAll()
                .AsNoTracking()
                .Where(c => c.SType.f_type == 20)
                .ToList());
            //上班地点
            dataList.AddRange(_companyOfFoodRepository
                .GetAll()
                .AsNoTracking()
                .Where(c => c.SType.f_type == 4)
                .ToList());
            //荷 官
            dataList.AddRange(_companyOfFoodRepository
                .GetAll()
                .AsNoTracking()
                .Where(c => c.SType.f_type == 2)
                .ToList());
            return dataList;
        }
        /// <summary>
        /// 宿舍订餐信息分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PageListDto<DorOrderListDto> GetDorOrderPagedAllDatas(DorOrderPagedInputDto input)
        {
            IList<SType> typeList=_stypeService.GetsTypeByWhere((int)sTypeEnum.订餐类型);
            IList<SType> requireTypeList = _stypeService.GetsTypeByWhere((int)sTypeEnum.订餐要求类型);
            IList<DorOrderListDto> resultList = new List<DorOrderListDto>();
            #region 新员工
            var newlist = _newOrderingEmployeeRepository
                .GetAll()
                .AsNoTracking()
                .Where(c=>c.t_employeeInfo.f_IsRemove==false)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), c => c.t_employeeInfo.f_chineseName.Contains(input.Name.Trim()) || c.t_employeeInfo.f_nickname.Contains(input.Name.Trim()) || c.t_employeeInfo.f_passportName.Contains(input.Name.Trim()))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Dormitory), c => c.f_Community.Contains(input.Dormitory) || c.f_Building.Contains(input.Dormitory) || c.f_RoomNo.Contains(input.Dormitory))
                .GroupBy(c => c.f_eID);

            var newDtoList = newlist
                .OrderBy(c => c.Key)
                //.PageBy(input.iPageIndex, input.iPageSize)
                .Select(c => c.Key.Value)
                .ToList();
            int newCount = newlist.Count();
            foreach (var item in newDtoList)
            {
                var orderList = _newOrderingEmployeeRepository.GetAll().Where(m => m.f_eID == item).ToList();
                var dto = new DorOrderListDto() { Ordings = new List<string>() };
                //通过订餐类型遍历
                for (int i = 0; i < typeList.Count; i++)
                {
                    //
                    var order = orderList.FirstOrDefault(m => m.f_LDM_tID.Value == typeList[i].f_tID);
                    string orderInfo = string.Empty;
                    if (order != null)
                    {
                        if (string.IsNullOrEmpty(dto.Name))
                        {
                            dto.Name = order.t_employeeInfo?.f_chineseName + "(" + order.t_employeeInfo?.f_nickname + ")";
                            dto.IsNewEmployee = order.t_employeeInfo?.f_IsNewEmp??false;
                            dto.Community = order.f_Community;
                            dto.Building = order.f_Building;
                            dto.RoomNo = order.f_RoomNo;
                            dto.IsValid = order.f_IsValid;
                        }
                        //订餐类型
                        string t = typeList.FirstOrDefault(m => m.f_tID == order.f_LDM_tID)?.f_value ?? string.Empty;
                        //订餐要求
                        string rt = requireTypeList.FirstOrDefault(m => m.f_tID == order.f_type_tID)?.f_value ?? string.Empty;

                        orderInfo = (order.f_EffectiveDate?.ToString("MM/dd*") ?? string.Empty) + (!string.IsNullOrEmpty(rt) ? rt + "*" : string.Empty) + (order.f_Quantity != 0 ? order.f_Quantity + "份" : string.Empty);
                    }
                    dto.Ordings.Add(orderInfo);
                }
                resultList.Add(dto);
            }
            #endregion


            #region 普通员工
            //分组分页查询前10条数据的员工id
            var list = _orderingEmployeeRepository
                .GetAll()
                .AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), c => c.t_Employee.f_chineseName.Contains(input.Name.Trim()) || c.t_Employee.f_nickname.Contains(input.Name.Trim()) || c.t_Employee.f_passportName.Contains(input.Name.Trim()))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Dormitory), c => c.f_Community.Contains(input.Dormitory) || c.f_Building.Contains(input.Dormitory) || c.f_RoomNo.Contains(input.Dormitory))
                .GroupBy(c => c.f_eID);

            var dtoList = list
                .OrderBy(c => c.Key)
                //.PageBy(input.iPageIndex, input.iPageSize)
                .Select(c => c.Key.Value)
                .ToList();
            foreach (var item in dtoList)
            {
                var orderList = _orderingEmployeeRepository.GetAll().Where(m => m.f_eID == item).ToList();
                var dto = new DorOrderListDto() { Ordings = new List<string>() };
                //通过订餐类型遍历
                for (int i = 0; i < typeList.Count; i++)
                {
                    //
                    var order = orderList.FirstOrDefault(m => m.f_LDM_tID.Value == typeList[i].f_tID);
                    string orderInfo = string.Empty;
                    if (order != null)
                    {
                        if (string.IsNullOrEmpty(dto.Name))
                        {
                            dto.Name = order.t_Employee?.f_chineseName + "(" + order.t_Employee?.f_nickname + ")";
                            dto.IsNewEmployee = order.t_Employee?.f_IsNewEmp??false;
                            dto.Community = order.f_Community;
                            dto.Building = order.f_Building;
                            dto.RoomNo = order.f_RoomNo;
                            dto.IsValid = order.f_IsValid;
                        }
                        //订餐类型
                        string t = typeList.FirstOrDefault(m => m.f_tID == order.f_LDM_tID)?.f_value ?? string.Empty;
                        //订餐要求
                        string rt = requireTypeList.FirstOrDefault(m => m.f_tID == order.f_type_tID)?.f_value ?? string.Empty;

                        orderInfo = (order.f_EffectiveDate?.ToString("MM/dd*") ?? string.Empty) + (!string.IsNullOrEmpty(rt) ? rt + "*" : string.Empty) + (order.f_Quantity != 0 ? order.f_Quantity + "份" : string.Empty);
                    }
                    dto.Ordings.Add(orderInfo);
                }
                resultList.Add(dto);
            }

            #endregion

            int count = list.Count()+newCount;
            return new PageListDto<DorOrderListDto>(
                resultList
                .OrderByDescending(p => p.IsNewEmployee)
                .Skip((input.iPageIndex-1)*input.iPageSize)
                .Take(input.iPageSize), input.iPageIndex,input.iPageSize, count);
        }
        /// <summary>
        /// 统计公司用餐信息
        /// </summary>
        /// <returns></returns>

        public int StatInformation()
        {
            List<CompanyOfFood> lstCompanyOfFood = new List<CompanyOfFood>();
            //获取所有部门
            var list = _stypeService.GetsTypeByWhere(20);
            string sBTime = _stypeService.GetTypeByWhere(p => p.f_type == 21 && p.f_value == "早餐时间").FirstOrDefault()?.f_Remark?.Trim();
            string sLTime = _stypeService.GetTypeByWhere(p => p.f_type == 21 && p.f_value == "中餐时间").FirstOrDefault()?.f_Remark?.Trim();
            string sDTime = _stypeService.GetTypeByWhere(p => p.f_type == 21 && p.f_value == "晚餐时间").FirstOrDefault()?.f_Remark?.Trim();
            string sMTime = _stypeService.GetTypeByWhere(p => p.f_type == 21 && p.f_value == "宵夜时间").FirstOrDefault()?.f_Remark?.Trim();
            foreach (var item in list)
            {
                CompanyOfFood mCo = item
                    .CompanyOfFoods
                    .FirstOrDefault() ?? new CompanyOfFood();
                List<Employee> employee;
                /*
                  p.STypeWorkStatus 在职状态。f_value判断是否在职
                  p.STypeWorkLocation 工作地点。f_Remark判断工作地点是否需要统计餐饮
                 */
                if (item.f_value.Contains("-"))
                {
                    //ex： 金流-早班
                    //特殊处理分开计算早晚班
                    string[] val = item.f_value.Split('-');
                    string sBuMen = val[0];
                    string sBanCi = val[1];
                    employee = _employeeRepository.GetAll().Where(p => p.f_IsNewEmp == false && p.STypeWorkStatus.f_value.Contains("在职") && p.STypeWorkLocation.f_Remark.Contains("餐饮") && p.STypeDepartment.f_value == sBuMen && p.STypePeriodType.f_value.Contains(sBanCi)).ToList();
                }
                else
                {
                    employee = _employeeRepository.GetAll().Where(p => p.f_IsNewEmp == false && p.STypeWorkStatus.f_value.Contains("在职") && p.STypeWorkLocation.f_Remark.Contains("餐饮") && p.STypeDepartment.f_value == item.f_value).ToList();
                }
                mCo.f_type_tID = item.f_tID;
                mCo.f_breakfast = employee.Count(p => CheckTime(p.f_rideWorkTime, p.f_rideOffWorkTime, sBTime));
                mCo.f_lunch = employee.Count(p => CheckTime(p.f_rideWorkTime, p.f_rideOffWorkTime, sLTime));
                mCo.f_dinner = employee.Count(p => CheckTime(p.f_rideWorkTime, p.f_rideOffWorkTime, sDTime));
                mCo.f_Midnight = employee.Count(p => CheckTime(p.f_rideWorkTime, p.f_rideOffWorkTime, sMTime));

                lstCompanyOfFood.Add(mCo);
            }
            list = _stypeService.GetTypeByWhere(p => p.f_type == 4 &&p.f_Remark!=null&&p.f_Remark.Contains("餐饮")).ToList();
            foreach (var item in list)
            {
                CompanyOfFood mCo = item
                    .CompanyOfFoods
                    .FirstOrDefault()?? new CompanyOfFood();
                List<Employee> employees = _employeeRepository
                    .GetAll()
                    .Where(p => p.f_IsNewEmp == false && p.STypeWorkStatus.f_value.Contains("在职") && p.STypeWorkLocation.f_Remark.Contains("餐饮") && p.f_workLocation_tID == item.f_tID)
                    .ToList();
                mCo.f_type_tID = item.f_tID;
                mCo.f_breakfast = employees.Count(p => CheckTime(p.f_rideWorkTime, p.f_rideOffWorkTime, sBTime));
                mCo.f_lunch = employees.Count(p => CheckTime(p.f_rideWorkTime, p.f_rideOffWorkTime, sLTime));
                mCo.f_dinner = employees.Count(p => CheckTime(p.f_rideWorkTime, p.f_rideOffWorkTime, sDTime));
                mCo.f_Midnight = employees.Count(p => CheckTime(p.f_rideWorkTime, p.f_rideOffWorkTime, sMTime));
                lstCompanyOfFood.Add(mCo);
            }
            {
                //荷官 
                var tSType = _stypeService.GetTypeByWhere(p => p.f_type == 2 && p.f_value == "荷官").FirstOrDefault();
                if (tSType != null)
                {
                    CompanyOfFood mCo = tSType.CompanyOfFoods.FirstOrDefault();
                    lstCompanyOfFood.Add(mCo);
                }
            }
            int result = 0;
            foreach (var item in lstCompanyOfFood)
            {
                if (item.f_cID==0)
                {
                    result=_companyOfFoodRepository.Insert(item);
                }else
                {
                    result=_companyOfFoodRepository.Update(item);
                }
            }
            return result;
        }

        public List<OrderingEmployeeCollectHistory> GetOrderingEmployeeCollectHistory(DateTime date)
        {
            //新人订餐统计
            var idList = _orderingEmployeeRepository.GetAll().Select(p => p.f_eID).ToList();//员工订餐表中的ID集合，用于筛选员工订餐和新人订餐中都有数据的员工
            var mealType = _stypeService.GetsTypeByWhere((int)sTypeEnum.订餐类型).ToList();
            int lunchId = mealType.FirstOrDefault(p => p.f_value == "中餐").f_tID;
            int dinnerId = mealType.FirstOrDefault(p => p.f_value == "晚餐").f_tID;
            int midnightId = mealType.FirstOrDefault(p => p.f_value == "宵夜").f_tID;
            var observationType = _stypeService.GetsTypeByWhere((int)sTypeEnum.订餐要求类型).ToList();
            int luchRegularId = observationType.FirstOrDefault(p => p.f_value == "常规").f_tID;
            int luchOnlyRicePlasticBagId = observationType.FirstOrDefault(p => p.f_value == "只要米饭且塑料袋装").f_tID;
            int luchOnlyRiceBoxId = observationType.FirstOrDefault(p => p.f_value == "只要米飯且餐盒装").f_tID;
            int luchNoMeatId = observationType.FirstOrDefault(p => p.f_value == "不要肉").f_tID;
            var nEmpData = _newOrderingEmployeeRepository.GetAll().ToList()
                .Where(p => p.f_EffectiveDate != null && p.f_IsValid && p.f_EffectiveDate.Value.ToString("yyyy-MM-dd") == date.ToString("yyyy-MM-dd") && !idList.Contains(p.f_eID.Value))
                .GroupBy(p => new {
                    p.f_Community,
                    p.f_Building,
                    p.f_RoomNo,
                    p.f_type_tID,
                    p.f_LDM_tID,
                    p.f_EffectiveDate
                }).Select(g => new OrderingEmployeeCollectHistory
                {
                    f_Community = g.Key.f_Community,
                    f_Building = g.Key.f_Building,
                    f_RoomNo = g.Key.f_RoomNo,
                    f_Date = date,
                    f_LuchRegular = g.Where(w => w.f_LDM_tID == lunchId && w.f_type_tID == luchRegularId).Sum(x => x.f_Quantity),
                    f_LuchOnlyRicePlasticBag = g.Where(w => w.f_LDM_tID == lunchId && w.f_type_tID == luchOnlyRicePlasticBagId).Sum(x => x.f_Quantity),
                    f_LuchOnlyRiceBox = g.Where(w => w.f_LDM_tID == lunchId && w.f_type_tID == luchOnlyRiceBoxId).Sum(x => x.f_Quantity),
                    f_LuchNoMeat = g.Where(w => w.f_LDM_tID == lunchId && w.f_type_tID == luchNoMeatId).Sum(x => x.f_Quantity),
                    f_DinnerRegular = g.Where(w => w.f_LDM_tID == dinnerId && w.f_type_tID == luchRegularId).Sum(x => x.f_Quantity),
                    f_DinnerOnlyRicePlasticBag = g.Where(w => w.f_LDM_tID == dinnerId && w.f_type_tID == luchOnlyRicePlasticBagId).Sum(x => x.f_Quantity),
                    f_DinnerOnlyRiceBox = g.Where(w => w.f_LDM_tID == dinnerId && w.f_type_tID == luchOnlyRiceBoxId).Sum(x => x.f_Quantity),
                    f_DinnerNoMeat = g.Where(w => w.f_LDM_tID == dinnerId && w.f_type_tID == luchNoMeatId).Sum(x => x.f_Quantity),
                    f_MidnightRegular = g.Where(w => w.f_LDM_tID == midnightId && w.f_type_tID == luchRegularId).Sum(x => x.f_Quantity),
                    f_MidnightOnlyRicePlasticBag = g.Where(w => w.f_LDM_tID == midnightId && w.f_type_tID == luchOnlyRicePlasticBagId).Sum(x => x.f_Quantity),
                    f_MidnightOnlyRiceBox = g.Where(w => w.f_LDM_tID == midnightId && w.f_type_tID == luchOnlyRiceBoxId).Sum(x => x.f_Quantity),
                    f_MidnightNoMeat = g.Where(w => w.f_LDM_tID == midnightId && w.f_type_tID == luchNoMeatId).Sum(x => x.f_Quantity)
                }).ToList();
            //将统计后的新人数据与普通员工的订餐统计数据整合
            return (from n in nEmpData select n).Union(from nor in _orderingEmployeeCollectHistoryRepository.GetAll().Where(p => p.f_Date == date) select nor).GroupBy(p => new {
                p.f_Community,
                p.f_Building,
                p.f_RoomNo
            }).Select(g => new OrderingEmployeeCollectHistory
            {
                f_Community = g.Key.f_Community,
                f_Building = g.Key.f_Building,
                f_RoomNo = g.Key.f_RoomNo,
                f_Date = date,
                f_LuchRegular = g.Sum(x => x.f_LuchRegular),
                f_LuchOnlyRicePlasticBag = g.Sum(x => x.f_LuchOnlyRicePlasticBag),
                f_LuchOnlyRiceBox = g.Sum(x => x.f_LuchOnlyRiceBox),
                f_LuchNoMeat = g.Sum(x => x.f_LuchNoMeat),
                f_DinnerRegular = g.Sum(x => x.f_DinnerRegular),
                f_DinnerOnlyRicePlasticBag = g.Sum(x => x.f_DinnerOnlyRicePlasticBag),
                f_DinnerOnlyRiceBox = g.Sum(x => x.f_DinnerOnlyRiceBox),
                f_DinnerNoMeat = g.Sum(x => x.f_DinnerNoMeat),
                f_MidnightRegular = g.Sum(x => x.f_MidnightRegular),
                f_MidnightOnlyRicePlasticBag = g.Sum(x => x.f_MidnightOnlyRicePlasticBag),
                f_MidnightOnlyRiceBox = g.Sum(x => x.f_MidnightOnlyRiceBox),
                f_MidnightNoMeat = g.Sum(x => x.f_MidnightNoMeat),
            }).ToList();
        }

        public List<string> GetDistinctDateByOrderingEmployeeCollectHistory()
        {
            return _orderingEmployeeCollectHistoryRepository.GetAll().Select(p => p.f_Date).OrderBy(p => p).ToList().Distinct().Select(p => p?.ToString("yyyy-MM-dd")).ToList();
        }

        /// <summary>
        /// 根据上下班时间 和 用餐时间范围，对比是否满足
        /// </summary>
        /// <param name="rideWorkTime">上班时间</param>
        /// <param name="rideOffWorkTime">下班时间</param>
        /// <param name="sRangeTime">用餐时间范围</param>
        /// <returns></returns>
        private bool CheckTime(DateTime? rideWorkTime, DateTime? rideOffWorkTime, string sRangeTime)
        {
            if (rideWorkTime == null || rideOffWorkTime == null || sRangeTime == null || !sRangeTime.Contains("~"))
            {
                return false;
            }
            string[] sTime = sRangeTime.Split('~');
            DateTime startD = DateTime.Parse(sTime[0]);
            DateTime endD = DateTime.Parse(sTime[1]);
            if (startD>endD)
            {
                endD = endD.AddDays(1);
            }
            //获取当前时间
            DateTime now = DateTime.Now;
            //
            DateTime newRideWorkTime = new DateTime(now.Year,now.Month,now.Day,rideWorkTime.Value.Hour,rideWorkTime.Value.Minute,rideWorkTime.Value.Second);
            DateTime newRideOffWorkTime = new DateTime(now.Year, now.Month, now.Day, rideOffWorkTime.Value.Hour, rideOffWorkTime.Value.Minute, rideOffWorkTime.Value.Second);

            return (newRideWorkTime >= startD && newRideWorkTime <= endD)||(newRideOffWorkTime>=startD&&newRideOffWorkTime<=endD);
        }

        public int UpdateCompanyOfFood(CompanyOfFood model)
        {
            return _companyOfFoodRepository.Update(model);
        }
    }
}
