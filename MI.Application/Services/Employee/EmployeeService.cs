using AutoMapper;
using MI.Application.Dto;
using MI.Application.OASession;
using MI.Application.OASession.Dto;
using MI.Core.Common;
using MI.Core.Domain;
using MI.Core.Extension;
using MI.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Data.Entity;

namespace MI.Application
{
    public class EmployeeService : IEmployeeService
    {
        // private OAUser _User;
        private readonly IBaseRepository<Employee> _employeeRepository;
        /// <summary>
        /// 新员工
        /// </summary>
        private readonly IBaseRepository<NewEmployee> _newEmployee;
        /// <summary>
        /// 洗衣房
        /// </summary>
        private readonly IBaseRepository<LaundryPwd> _laundryPwd;
        /// <summary>
        /// 类型表
        /// </summary>
        private readonly IBaseRepository<SType> _repSType;
        /// <summary>
        /// 工作交接
        /// </summary>
        private readonly IBaseRepository<WorkDistribution> _workdistribution;
        /// <summary>
        /// 员工宿舍表
        /// </summary>
        private readonly IBaseRepository<Dormitory> _dormitory;
        /// <summary>
        /// 员工订餐表
        /// </summary>
        private readonly IBaseRepository<OrderingEmployees> _orderemp;
        /// <summary>
        /// 返乡机票
        /// </summary>
        private readonly IBaseRepository<ReturnTicket> _returnTicket;
        /// <summary>
        /// 员工信息模型
        /// </summary>
        private readonly IBaseRepository<AllEmployeeInfoViewModel> _allemoloyeeinfoviewModel;
        /// <summary>
        /// 用户服务
        /// </summary>
        private readonly ISession _session;
        /// <summary>
        /// 新人订餐
        /// </summary>
        private readonly IBaseRepository<NewOrderingEmployees> _neworderingEmployees;
        /// <summary>
        /// 宿舍外租
        /// </summary>
        public readonly IBaseRepository<Grant> _grant;

public INewOrderingEmployeesService _newOrderingEmployeesService = null;
        public readonly IOrderingEmployeesService _orderingEmployeesService;
        public IReturnTicketService _tTReturnTicketService;
        /// <param name="employeeRepository"></param>
        /// <param name="empStype"></param>
        /// <summary>
        /// 新人订餐记录表服务
        /// </summary>
        public readonly INewOrderingEmployeesService _INewOrderingService;
        /// <summary>
        /// 员工宿舍订餐
        /// </summary>
        public readonly IOrderingEmployeesService _IOederingService;
        public readonly IBaseRepository<EmpRent> _empRent;
        public readonly IBaseRepository<ChangeRoom> _changeRoom;
        public readonly IBaseRepository<Outside> _outside;
        public readonly IBaseRepository<FlightFee> _flightfee;
        public readonly IBaseRepository<ReturnHandover> _returnhandover;
        /// <summary>
        /// 返乡机票
        /// </summary>
        public EmployeeService(
            IBaseRepository<Employee> employeeRepository,
            IBaseRepository<SType> empStype,
            IBaseRepository<EmployeeDto> empLoueeDto,
            IBaseRepository<Dormitory> dormitory,
            IBaseRepository<OrderingEmployees> orderemp,
            INewOrderingEmployeesService ineworderingserver,
            IOrderingEmployeesService ioederingService,
            IBaseRepository<ReturnTicket> returnticket,
            IBaseRepository<AllEmployeeInfoViewModel> allemoleoyeeinfo,
            IBaseRepository<NewEmployee> newEmployee,
            IBaseRepository<LaundryPwd> laundrypwd,
             IOrderingEmployeesService orderingEmployeesService,
             IReturnTicketService tTReturnTicketService,
            IBaseRepository<WorkDistribution> workdistribution,
            ISession isesson,
            IBaseRepository<NewOrderingEmployees> neworderingemployees,
            IBaseRepository<Grant> grant,
            IBaseRepository<EmpRent> emprent,
            IBaseRepository<ChangeRoom> changeroom,
            IBaseRepository<Outside> outside,
            IBaseRepository<FlightFee> flightfee,
            IBaseRepository<ReturnHandover> returnhandover)
        {
            _employeeRepository = employeeRepository;
            _repSType = empStype;
            _dormitory = dormitory;
            _orderemp = orderemp;
            _INewOrderingService = ineworderingserver;
            _IOederingService = ioederingService;
            _returnTicket = returnticket;
            _allemoloyeeinfoviewModel = allemoleoyeeinfo;
            _newEmployee = newEmployee;
            _laundryPwd = laundrypwd;
            _workdistribution = workdistribution;

            _orderingEmployeesService = orderingEmployeesService;
            _tTReturnTicketService = tTReturnTicketService;
            _session = isesson;
            _neworderingEmployees = neworderingemployees;
            _grant = grant;
            _empRent = emprent;
            _changeRoom = changeroom;
            _outside = outside;
            _flightfee = flightfee;
            _returnhandover = returnhandover;
        }
        public Employee GetEmployeeById(int id)
        {
            Employee employee = _employeeRepository.GetEntityById(id);
            return employee;
        }

        public IList<Employee> GetEmployeeByName(string name)
        {
            return _employeeRepository.GetAll().Where(u => u.f_chineseName.Contains(name) || u.f_nickname.Contains(name) || u.f_passportName.Contains(name) || u.f_AccountName == name).ToList();
        }

        public Employee Login(LoginDto input)
        {

            return _employeeRepository
                .GetAll()
                .Where(m => m.f_AccountName == input.AccountName)
                .FirstOrDefault();
        }
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<Employee> GetemployeeInfoByWhere(Expression<Func<Employee, bool>> predicate)
        {
            return _employeeRepository.GetAll().Where(m => m.STypeDepartment != null).Where(predicate).ToList();
        }
        /// <summary>
        /// 条件分页查询
        /// </summary>
        /// <param name="employeedto"></param>
        /// <param name="iPageIndex"></param>
        /// <param name="iPageSize"></param>
        /// <param name="o_Count"></param>
        /// <param name="bIsNew">是否是新人</param>
        /// <returns></returns>
        public IList<Employee> GetEmployeeByWhere(EmployeeDto employeedto, int iPageIndex, int iPageSize, out int o_Count, bool bIsNew = false)
        {
            bool bDepartment = (employeedto.f_department_tID ?? 0) < 1;
            bool bLevel = (employeedto.level_f_tid ?? 0) < 1;
            bool bName = string.IsNullOrWhiteSpace(employeedto.Name);
            bool bTelNoPH = string.IsNullOrWhiteSpace(employeedto.TelNoPH);
            bool bBuildingOrRoomNo = string.IsNullOrWhiteSpace(employeedto.BuildingOrRoomNo);
            int? workStatus = employeedto.workStatus_f_tid;
            bool bWorkStatus = (employeedto.workStatus_f_tid ?? 0) == 0 || employeedto.workStatus_f_tid == -1;
            if (workStatus != null && workStatus == -2)
            {
                // -2空白的状态
                workStatus = null;
            }
            bool bStartDate = (employeedto.StartDate == DateTime.MinValue);//开始创建时间   
            bool bEndDate = (employeedto.EndDate == DateTime.MinValue);
            var result = _employeeRepository.GetAll().Where(p =>
            (bDepartment || p.f_department_tID == employeedto.f_department_tID) &&
            (bLevel || p.f_level_tID == employeedto.level_f_tid) &&
            (bWorkStatus || p.f_workStatus_tID == workStatus) &&
            (bStartDate || (employeedto.StartDate <= p.f_CreateDate)) &&
            (bEndDate || (p.f_CreateDate <= employeedto.EndDate)) &&
            (bTelNoPH || ((p.f_TelNoPH ?? "").Contains(employeedto.TelNoPH))) &&
            (bBuildingOrRoomNo || (p.Dormitory.f_Building ?? "").Contains(employeedto.BuildingOrRoomNo) || (p.Dormitory.f_RoomNo ?? "").Contains(employeedto.BuildingOrRoomNo)) &&
            (bName || (p.f_chineseName.Contains(employeedto.Name) ||
                      p.f_passportAlis.ToUpper().Contains(employeedto.Name) ||
                      (p.f_nickname ?? "").Contains(employeedto.Name) ||
                      (p.f_AccountName.ToUpper().Contains(employeedto.Name.ToUpper().Trim())))) &&
             (bIsNew ? p.f_IsNewEmp : p.f_IsNewEmp == false)
            ).ToList();
            o_Count = result.Where(p => !string.IsNullOrWhiteSpace(employeedto.feid) ? employeedto.feid.Contains(p.f_eid.ToString()) : true).ToList().Count();
            result = result.Where(p => !string.IsNullOrWhiteSpace(employeedto.feid) ? employeedto.feid.Contains(p.f_eid.ToString()) : true).OrderByDescending(p => p.f_eid).Skip((iPageIndex - 1) * iPageSize).Take(iPageSize).ToList();
            return result;
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="iPageIndex"></param>
        /// <param name="iPageSize"></param>
        /// <param name="o_Count"></param>
        /// <returns></returns>
        public IList<Employee> GetEmployeeByWhere(int iPageIndex, int iPageSize, out int o_Count, bool bIsNew = false)
        {
            var lstEmployee = bIsNew ? _employeeRepository.GetAll().OrderByDescending(j => j.f_eid).PageBy(iPageIndex, iPageSize) :
                _employeeRepository.GetAll().Where(j => j.f_IsNewEmp == false).OrderByDescending(j => j.f_eid).PageBy(iPageIndex, iPageSize);
            IList<Employee> dtoEmployee = Mapper.Map<IList<Employee>>(lstEmployee.ToList());
            o_Count = _employeeRepository.GetAll().OrderByDescending(j => j.f_eid).Count();
            return dtoEmployee;
        }
        /// <summary>
        /// 获取部门
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        public List<SType> GetSectorName(Func<SType, bool> predicate)
        {
            return _repSType.GetAll().Where(predicate).OrderByDescending(j => j.f_tID).ToList();
        }
        /// <summary>
        /// 在职状态
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<SType> GetInServiceStatus(Func<SType, bool> predicate)
        {
            return _repSType.GetAll().Where(predicate).OrderByDescending(j => j.f_tID).ToList();
        }
        /// <summary>
        /// 权限等级
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        public List<SType> GetPrivilegeLevel(Func<SType, bool> predicate)
        {
            return _repSType.GetAll().Where(predicate).OrderByDescending(j => j.f_tID).ToList();
        }
        public List<SType> GetOrderType(Func<SType, bool> predicate)
        {
            return _repSType.GetAll().Where(predicate).OrderByDescending(j => j.f_tID).ToList();
        }
        public void UpdateEmployeeInfo(Employee employeeInfo, bool bType = false)
        {
            AllEmployeeInfoViewModel allEmployeeInfoViewModel = new AllEmployeeInfoViewModel();
            allEmployeeInfoViewModel.EmployeeInfo = employeeInfo;
            UpdateEmployeeInfo(allEmployeeInfoViewModel, bType);
        }
        /// <summary>
        /// 修改员工信息
        /// </summary>
        /// <param name="modelEmployeeInfo"></param>
        /// <param name="bType">是否为个人资料的修改默认否</param>
        /// <param name="isJLKF">是否人员管理的员工修改</param>
        public void UpdateEmployeeInfo(AllEmployeeInfoViewModel allEmployeeInfoViewModel, bool bType = false, bool isJLKF = false)
        {
            Employee mod = _employeeRepository.GetEntityById(allEmployeeInfoViewModel.EmployeeInfo.f_eid);
            #region 由于上下班时间两个字段在数据库中类型为dateTime，但本身只需要存储时分，为防止前台传值的日期不对，所以这里拼上年月日
            if (allEmployeeInfoViewModel.EmployeeInfo.f_rideWorkTime != null && !allEmployeeInfoViewModel.EmployeeInfo.f_IsNewEmp)
            {
                allEmployeeInfoViewModel.EmployeeInfo.f_rideWorkTime = mod.f_rideWorkTime == null ? DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + allEmployeeInfoViewModel.EmployeeInfo.f_rideWorkTime.Value.ToString("HH:mm"))
                    : DateTime.Parse(mod.f_rideWorkTime.Value.ToString("yyyy-MM-dd") + " " + allEmployeeInfoViewModel.EmployeeInfo.f_rideWorkTime.Value.ToString("HH:mm"));
            }
            if (allEmployeeInfoViewModel.EmployeeInfo.f_rideOffWorkTime != null && !allEmployeeInfoViewModel.EmployeeInfo.f_IsNewEmp)
            {
                allEmployeeInfoViewModel.EmployeeInfo.f_rideOffWorkTime = mod.f_rideOffWorkTime == null ? DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + allEmployeeInfoViewModel.EmployeeInfo.f_rideOffWorkTime.Value.ToString("HH:mm"))
                    : DateTime.Parse(mod.f_rideOffWorkTime.Value.ToString("yyyy-MM-dd") + " " + allEmployeeInfoViewModel.EmployeeInfo.f_rideOffWorkTime.Value.ToString("HH:mm"));
            }
            #endregion
            Employee modelEmployeeInfo = allEmployeeInfoViewModel.EmployeeInfo;
            if (modelEmployeeInfo != null)
            {
                if (modelEmployeeInfo.OrderingEmployees != null)
                {
                    mod.OrderingEmployees = modelEmployeeInfo.OrderingEmployees;
                }
                if (modelEmployeeInfo.f_IsNewEmp)
                {
                    string sNotSame = "f_eid,f_chineseName,f_AccountName," + string.Join(", ", CompareType<Employee>(mod, modelEmployeeInfo, new string[] { "f_pwd", "f_passportAlis", "f_JLlevel_tID" }).ToArray());//有变化的字段的字符串
                    JObject oldJson = GetJObjectData(mod, sNotSame);
                    JObject newJson = GetJObjectData(modelEmployeeInfo, sNotSame);
                    //sNotSame = string.Join(", ", CompareType<NewEmployee>(mod.NewEmployee, modelEmployeeInfo?.NewEmployee, new string[] { "f_nID" }).ToArray());
                    ////写入操作记录   
                    //if (sNotSame.Trim() != "f_eid,f_chineseName,f_AccountName,")
                    //{
                    //    GetJObjectData(mod.NewEmployee, sNotSame, ref oldJson);
                    //    GetJObjectData(modelEmployeeInfo?.NewEmployee, sNotSame, ref newJson);
                    //}
                    mod.NewEmployee = modelEmployeeInfo?.NewEmployee;
                }
                else
                {
                    string sNotSame = "f_eid,f_chineseName,f_AccountName," + string.Join(", ", CompareType<Employee>(mod, modelEmployeeInfo, new string[] { "f_pwd", "f_passportAlis", "f_JLlevel_tID" }).ToArray());//有变化的字段的字符串
                    if (bType)//为个人信息修改时，有些字段不比比较
                    {
                        sNotSame = "f_eid,f_chineseName,f_AccountName," + string.Join(", ", CompareType<Employee>(mod, modelEmployeeInfo, new string[] { "f_department_tID", "f_dormitoryId", "f_level_tID", "f_IsPHStaff", "f_workStatus_tID", "f_pwd", "f_passportAlis", "f_JLlevel_tID", "f_JLStage", "f_JLgroup", "f_JLdirector_eId" }).ToArray());//有变化的字段的字符串
                    }
                    if (sNotSame.Trim() != "f_eid,f_chineseName,f_AccountName,")
                    {
                        JObject oJson = GetJObjectData(modelEmployeeInfo, sNotSame);
                        if (!string.IsNullOrWhiteSpace(oJson.ToString()))
                        {
                            //写入操作记录
                            //  AddModifyRecord(GetJObjectData(mod, sNotSame).ToString(), oJson.ToString(), ActionItem.Update, CategoryItem.EmployeeInfo, "员工管理");
                        }
                    }
                }
                mod.f_chineseName = modelEmployeeInfo.f_chineseName;
                mod.f_emergencyContactNumber = modelEmployeeInfo.f_emergencyContactNumber;
                mod.f_emergencyContactPerson = modelEmployeeInfo.f_emergencyContactPerson;
                mod.f_international = modelEmployeeInfo.f_international;
                mod.f_nickname = modelEmployeeInfo.f_nickname;
                mod.f_passportID = modelEmployeeInfo.f_passportID;
                mod.f_passportName = modelEmployeeInfo.f_passportName;
                //mod.f_PassportURL = modelEmployeeInfo.f_PassportURL;
                //mod.f_EntrystampURL = modelEmployeeInfo.f_EntrystampURL;
                mod.f_periodType_tID = modelEmployeeInfo.f_periodType_tID;
                mod.f_QQNumber = modelEmployeeInfo.f_QQNumber;
                mod.f_Remark = modelEmployeeInfo.f_Remark;
                mod.f_rideOffWorkTime = modelEmployeeInfo.f_rideOffWorkTime;
                mod.f_rideWorkTime = modelEmployeeInfo.f_rideWorkTime;
                mod.f_sex = modelEmployeeInfo.f_sex;
                mod.f_TelNoCN = modelEmployeeInfo.f_TelNoCN;
                mod.f_TelNoPH = modelEmployeeInfo.f_TelNoPH;
                mod.f_Wechat = modelEmployeeInfo.f_Wechat;
                mod.f_workLocation_tID = modelEmployeeInfo.f_workLocation_tID;
                mod.f_Salary = modelEmployeeInfo.f_Salary;
                mod.f_VisaType_tID = modelEmployeeInfo.f_VisaType_tID;
                mod.f_passportDate = modelEmployeeInfo.f_passportDate;
                mod.f_DateOfBirth = modelEmployeeInfo.f_DateOfBirth;
                mod.f_HireDate = modelEmployeeInfo.f_HireDate;
                mod.f_ICardDate = modelEmployeeInfo.f_ICardDate;
                mod.f_passportAlis = AOUnity.GetStringByLetters(modelEmployeeInfo.f_passportName);
                mod.f_SeparationDate = modelEmployeeInfo.f_SeparationDate;
                //mod.f_JLStage = modelEmployeeInfo.f_JLStage;
                //mod.f_JLlevel_tID = modelEmployeeInfo.f_JLlevel_tID;
                if (isJLKF)
                {
                    mod.f_JLdirector_eId = modelEmployeeInfo.f_JLdirector_eId;
                    mod.f_JLStage = modelEmployeeInfo.f_JLStage;
                    mod.f_JLgroup = modelEmployeeInfo.f_JLgroup;
                }
                //判断是否为个人信息的修改 2016.7.21 mu,为 个人修改时 以下 信息不能修改
                if (!bType)
                {
                    mod.f_department_tID = modelEmployeeInfo.f_department_tID;//部门
                    mod.f_dormitoryId = modelEmployeeInfo.f_dormitoryId;//宿舍
                    mod.f_level_tID = modelEmployeeInfo.f_level_tID;//等级
                    mod.f_IsPHStaff = modelEmployeeInfo.f_IsPHStaff;//是否为非员工
                    mod.f_workStatus_tID = modelEmployeeInfo.f_workStatus_tID;//在职状态
                }
                //同步订餐信息
                AddOrUpdateOrderingEmployees(allEmployeeInfoViewModel);
                AddReturnTicket(allEmployeeInfoViewModel, true);
                Update(mod);
                
            }
        }
        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="feid"></param>
        public void DeleteEmoloyeeByfeid(int feid)
        {
            Employee employeedata = _employeeRepository.GetEntityById(feid);

            if (employeedata.f_IsNewEmp)
            {
                //写入操作记录
                AOUnity.AddModifyRecord(GetJsonObjectData(employeedata).ToString(), null, ActionItem.Delete, CategoryItem.NewEmployeeInfo,
                    "新人登记",_session.GetCurrUser().NickName);
                RemoveWorkDistribution(employeedata);
            }
            else
            {
                //写入操作记录
                AOUnity.AddModifyRecord(GetEmpJsonObjectData(employeedata).ToString(), null, ActionItem.Delete, CategoryItem.EmployeeInfo, "员工管理", _session.GetCurrUser().NickName);
            }

            if (employeedata.OrderingEmployees.Any())
            {
                employeedata.OrderingEmployees.Clear();
            }
            if (employeedata.NewOrderingEmoloyees.Any())
            {
                employeedata.NewOrderingEmoloyees.Clear();
            }
            if (employeedata.t_grant.Any())
            {
                _grant.Delete(employeedata.t_grant);
            }
            if (employeedata.EmpRents.Any())
            {
                _empRent.Delete(employeedata.EmpRents);
            }
            if (employeedata.ChangeRoom.Any())
            {
                _changeRoom.Delete(employeedata.ChangeRoom);
            }
            if (employeedata.Outside.Any())
            {
                _outside.Delete(employeedata.Outside);
            }
            if (employeedata.Outside1.Any())
            {
                _outside.Delete(employeedata.Outside1);
            }
            if (employeedata.WorkDistribution.Any())
            {
                _workdistribution.Delete(employeedata.WorkDistribution);
            }
            if (employeedata.FlightFee.Any())
            {
                _flightfee.Delete(employeedata.FlightFee);
            }
            if (employeedata.t_ReturnTicket.Any())
            {
                _returnTicket.Delete(employeedata.t_ReturnTicket.ToList());
            }
            if (employeedata.ReturnHandover.Any())
            {
                _returnhandover.Delete(employeedata.ReturnHandover);
            }
            //删除
            _employeeRepository.Delete(employeedata);
        }


        private void Update(Employee model)
        {
            _employeeRepository.Update(model);
        }
        /// <summary>
        /// 同步订餐信息
        /// </summary>
        /// <param name="allEmployeeInfoViewModel"></param>
        private void AddOrUpdateOrderingEmployees(AllEmployeeInfoViewModel allEmployeeInfoViewModel)
        {
            var vLiZhiID = _repSType.GetAll().Where(p => (p.f_type == (int)sTypeEnum.上班状态类型) && ("离职|離職".Contains(p.f_value))).FirstOrDefault()?.f_tID;
            if (allEmployeeInfoViewModel.IsOrderingEmployees && allEmployeeInfoViewModel.EmployeeInfo.f_dormitoryId != null && allEmployeeInfoViewModel.EmployeeInfo.f_dormitoryId.Value > 0 &&
                allEmployeeInfoViewModel.EmployeeInfo.f_workStatus_tID != vLiZhiID &&
                allEmployeeInfoViewModel.LDM_tID != null)
            {
                Dormitory mDormitory = _dormitory.GetEntityById(allEmployeeInfoViewModel.EmployeeInfo.f_dormitoryId.Value);
                int iIndex = 0;
                //新人订餐与员工订餐区分处理
                if (allEmployeeInfoViewModel.IsnewEmployeeInfo)
                {
                    List<NewOrderingEmployees> listM = new List<NewOrderingEmployees>();
                    foreach (string item in allEmployeeInfoViewModel.LDM_tID)
                    {
                        var modelNew = new NewOrderingEmployees
                        {
                            f_LDM_tID = int.Parse(item),
                            f_Community = mDormitory.f_Community,
                            f_Building = mDormitory.f_Building,
                            f_RoomNo = mDormitory.f_RoomNo,
                            f_eID = allEmployeeInfoViewModel.EmployeeInfo.f_eid,
                            f_IsValid = true,
                            f_type_tID =
                                allEmployeeInfoViewModel.LDMType_tID == 0
                                    ? _repSType.GetAll().Where(p => p.f_type == (int)sTypeEnum.订餐要求类型)
                                        .FirstOrDefault()?
                                        .f_tID
                                    : allEmployeeInfoViewModel.LDMType_tID,
                            f_Quantity = allEmployeeInfoViewModel.LDM_Quantity[iIndex],
                            f_EffectiveDate = DateTime.Parse(allEmployeeInfoViewModel.observationDate[iIndex])
                        };
                        listM.Add(modelNew);
                        iIndex++;
                    }
                    _INewOrderingService.UpdateOrderingEmployees(listM);
                }
                else
                {
                    List<OrderingEmployees> listM = new List<OrderingEmployees>();
                    foreach (string item in allEmployeeInfoViewModel.LDM_tID)
                    {
                        var modelNew = new OrderingEmployees
                        {
                            f_LDM_tID = int.Parse(item),
                            f_Community = mDormitory.f_Community,
                            f_Building = mDormitory.f_Building,
                            f_RoomNo = mDormitory.f_RoomNo,
                            f_eID = allEmployeeInfoViewModel.EmployeeInfo.f_eid,
                            f_type_tID =
                                allEmployeeInfoViewModel.LDMType_tID == 0
                                    ? _repSType.GetAll().Where(p => p.f_type == (int)sTypeEnum.订餐要求类型)
                                        .FirstOrDefault()?
                                        .f_tID
                                    : allEmployeeInfoViewModel.LDMType_tID,
                            f_Quantity = allEmployeeInfoViewModel.LDM_Quantity[iIndex]
                        };
                        listM.Add(modelNew);
                        iIndex++;
                    }
                    _orderingEmployeesService.UpdateOrderingEmployees(listM);
                }
            }
            else
            {
                if (allEmployeeInfoViewModel.IsnewEmployeeInfo)
                {
                    _INewOrderingService.AllClear(allEmployeeInfoViewModel.EmployeeInfo.f_eid);
                }
                else
                {
                    _orderingEmployeesService.AllClear(allEmployeeInfoViewModel.EmployeeInfo.f_eid);
                }
            }
        }
        #region 新人添加机票管理信息

        /// <summary>
        /// 新人添加机票管理信息
        /// </summary>
        /// <param name = "allEmployeeInfoViewModel" ></ param >
        /// < param name="bIsEdit"></param>
        #endregion
        /// <summary>
        /// 根据昵称，中文名，护照名查找
        /// </summary>
        /// <param name="sName"></param>
        /// <returns></returns>
        public List<Employee> SelectByName(string sName)
        {
            return _employeeRepository.GetAll().Where(u => u.f_chineseName == sName || u.f_nickname == sName && u.f_passportName == sName).ToList();
        }
        /// <summary>
        /// 根据昵称，中文名，护照名查找
        /// </summary>
        /// <param name="sName"></param>
        /// <returns></returns>
        public List<Employee> GetNames(string sName)
        {
            return _employeeRepository.GetAll().Where(u => u.f_chineseName.Contains(sName) || u.f_nickname.Contains(sName) || u.f_passportName.Contains(sName) || u.f_AccountName == sName).ToList();
        }
        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="feid"></param>
        public void DeleteEmployeeInfoById(int feid)
        {
            Employee employeedata = _employeeRepository.GetEntityById(feid);
            if (employeedata.f_IsNewEmp)
            {
                //写入操作记录
                AOUnity.AddModifyRecord(GetJsonObjectData(employeedata).ToString(), null, ActionItem.Delete, CategoryItem.NewEmployeeInfo,
                    "新人登记",_session.GetCurrUser().NickName);
                RemoveWorkDistribution(employeedata);
            }
            else
            {
                //写入操作记录
                AOUnity.AddModifyRecord(GetEmpJsonObjectData(employeedata).ToString(), null, ActionItem.Delete, CategoryItem.EmployeeInfo, "员工管理",_session.GetCurrUser().NickName);
            }
            _employeeRepository.Delete(employeedata);
        }
        /// <summary>
        /// 将实体转化成json数据，老员工，只记录有改变的字段
        /// </summary>
        /// <param name="oModel">员工实体</param>
        /// <param name="sNotSame">有改变的字段的字符串</param>
        /// <returns>json数据</returns>
        public JObject GetJObjectData(Employee oModel, string sNotSame)
        {
            JObject oJson = new JObject();//json数据

            PropertyInfo[] pisOne = oModel.GetType().GetProperties();
            //遍历旧数据类型，遍历属性，并作比较
            for (int i = 0; i < pisOne.Length; i++)
            {
                //比较,只比较值类型
                if (!pisOne[i].PropertyType.FullName.Contains("MI.Core.Domain"))
                {
                    string sName = pisOne[i].Name;//名称
                    if (sNotSame.Contains(sName))//有改变的字段包含这个字段就转json
                    {
                        //获取属性的值
                        string sValue = pisOne[i].GetValue(oModel, null)?.ToString() ?? "";
                        string sProperty = AOUnity.GetEmpFieldSibling(sName);//属性说明                        
                        if (sName == "f_sex")
                        {
                            oJson.Add(sProperty, oModel.f_sex == 1 ? "男" : "女");
                        }
                        else if (sName == "f_department_tID")
                        {
                            oJson.Add(sProperty, _repSType.GetEntityById(oModel.f_department_tID)?.f_value ?? "");
                        }
                        else if (sName == "f_periodType_tID")
                        {
                            oJson.Add(sProperty, _repSType.GetEntityById((int)oModel.f_periodType_tID)?.f_value ?? "");
                        }
                        else if (sName == "f_dormitoryId")
                        {
                            Dormitory oDormitory = _dormitory.GetEntityById(oModel.f_dormitoryId ?? 0);
                            oJson.Add("宿舍", (oDormitory?.f_Community ?? "") + "/ " + (oDormitory?.f_Building ?? "") + "/ " + (oDormitory?.f_RoomNo ?? ""));
                            oJson.Add(sProperty, oModel.f_dormitoryId);
                        }
                        else if (sName == "f_workLocation_tID")
                        {
                            oJson.Add(sProperty, _repSType.GetEntityById(oModel.f_workLocation_tID ?? 0)?.f_value ?? "");
                        }
                        else if (sName == "f_rideWorkTime")
                        {
                            oJson.Add(sProperty,
                                oModel.f_rideWorkTime != null ? oModel.f_rideWorkTime.Value.ToString("HH:mm") : "");
                        }
                        else if (sName == "f_rideOffWorkTime")
                        {
                            oJson.Add("下班时间", oModel.f_rideOffWorkTime != null ? oModel.f_rideOffWorkTime.Value.ToString("HH:mm") : "");
                        }
                        else if (sName == "f_workStatus_tID")
                        {

                            oJson.Add("在职状态", _repSType.GetEntityById((int)oModel.f_workStatus_tID)?.f_value ?? "");
                        }
                        else if (sName == "f_level_tID")
                        {
                            oJson.Add(sProperty, _repSType.GetEntityById(oModel.f_level_tID)?.f_value ?? "");
                        }
                        else if (sName == "f_JLStage")
                        {
                            oJson.Add(sProperty, _repSType.GetEntityById(oModel.f_JLStage)?.f_value ?? "");
                        }
                        else if (sName == "f_JLgroup")
                        {
                            oJson.Add(sProperty, _repSType.GetEntityById(oModel.f_JLgroup)?.f_value ?? "");
                        }
                        else if (sName == "f_JLdirector_eId")
                        {
                            oJson.Add(sProperty, _employeeRepository.GetEntityById(oModel.f_JLdirector_eId)?.f_chineseName ?? "");
                        }
                        else if (sName == "f_chineseName")
                        {
                            oJson.Add(sProperty, oModel.f_chineseName);
                        }
                        else if (sName == "f_AccountName")
                        {
                            oJson.Add(sProperty, oModel.f_AccountName);
                        }
                        else
                        {
                            oJson.Add(sProperty, sValue);
                        }
                    }
                }
            }
            return oJson;
        }
        #region
        /// <summary>
        /// 比较--两个类型一样的实体类对象的值
        /// </summary>
        /// <param name="oOdlData">旧数据</param>
        /// <param name="oNewData">新数据</param>
        /// <param name="oNoCompare">不需要比较的字段</param>
        /// <returns></returns>
        public static List<string> CompareType<T>(T oOdlData, T oNewData, params string[] oNoCompare)
        {
            List<string> oNameList = new List<string>();//用来存放值不相同的实体类的属性名
            Type typeOne = oOdlData.GetType();
            Type typeTwo = oNewData.GetType();
            PropertyInfo[] pisOne = typeOne.GetProperties(); //获取所有公共属性(Public)
            PropertyInfo[] pisTwo = typeTwo.GetProperties();
            //如果长度为0返回false
            if (pisOne.Length <= 0 || pisTwo.Length <= 0)
            {
                return oNameList;
            }
            //如果长度不一样，返回false
            if (!(pisOne.Length.Equals(pisTwo.Length)))
            {
                return oNameList;
            }
            //遍历旧数据类型，遍历属性，并作比较
            for (int i = 0; i < pisOne.Length; i++)
            {
                //获取属性名
                string oneName = pisOne[i].Name;
                if (pisOne[i].PropertyType.FullName.Contains("MI.Core.Domain"))
                {
                    continue;
                }
                //跳出不需要比较的属性名
                if (string.Concat(oNoCompare).Contains(oneName) || string.Concat(oNameList).Contains(oneName))
                {
                    continue;
                }
                for (int j = 0; j < pisTwo.Length; j++)
                {

                    //比较,只比较值类型
                    if (!pisTwo[j].PropertyType.FullName.Contains("MI.Core.Domain"))
                    {
                        string twoName = pisTwo[j].Name;
                        //获取属性的值
                        object oneValue = pisOne[i].GetValue(oOdlData, null);
                        object twoValue = pisTwo[j].GetValue(oNewData, null);
                        if (oneName.Equals(twoName))
                        {
                            if (oneValue == null)
                            {
                                if (twoValue != null)
                                {
                                    oNameList.Add(oneName); //如果有不一样的就写入list
                                    break;
                                }
                            }
                            else if (oneValue != null)
                            {
                                if (twoValue != null)
                                {
                                    if (!oneValue.Equals(twoValue))
                                    {
                                        oNameList.Add(oneName); //如果有不一样的就写入list
                                        break;
                                    }
                                }
                                else if (twoValue == null)
                                {
                                    oNameList.Add(oneName); //如果有不一样的就写入list
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
            }
            return oNameList;
        }
        #endregion
        /// <summary>
        /// 将实体转化成json数据，新员工，只记录有改变的字段
        /// </summary>
        /// <param name="oModel">员工实体</param>
        /// <param name="sNotSame">有改变的字段的字符串</param>
        /// <returns>json数据</returns>
        public void GetJObjectData(NewEmployee model, string sNotSame, ref JObject oJson)
        {
            PropertyInfo[] pisOne = model.GetType().GetProperties();
            //遍历旧数据类型，遍历属性，并作比较
            for (int i = 0; i < pisOne.Length; i++)
            {
                //比较,只比较值类型
                if (!pisOne[i].PropertyType.FullName.Contains("SB.Models.DataModels"))
                {
                    string sName = pisOne[i].Name;//名称
                    if (sNotSame.Contains(sName))//有改变的字段包含这个字段就转json
                    {
                        //获取属性的值
                        string sValue = pisOne[i].GetValue(model, null)?.ToString() ?? "";
                        if (sName == "f_Remark")
                        {
                            sName = "f_newRemark";
                        }
                        string sProperty = AOUnity.GetEmpFieldSibling(sName);//属性说明                        
                        if (sName == "f_flightTypt_tID")
                        {
                            oJson.Add(sProperty, _repSType.GetEntityById((int)model.f_flightTypt_tID)?.f_value ?? "");
                        }
                        else if (sName == "f_signature")
                        {
                            oJson.Add(sProperty, "true".Equals(sValue) ? "已确认" : "");
                        }
                        else
                        {
                            oJson.Add(sProperty, sValue);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 获取需要打印的集合
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private IEnumerable<dynamic> GetExportList(List<Employee> employeeList)
        {
            int index = 1;
            return employeeList.Select(p => new
            {
                序号 = index++,
                部门 = p.STypeDepartment.f_value ?? "",
                班别 = p.STypePeriodType?.f_value ?? "",
                姓名 = p.f_chineseName ?? "" + " (" + p.f_passportName ?? "" + ")",
                性别 = (p.f_sex.ToString() == "0") ? "女" : "男",
                国籍 = p.f_international ?? "",
                签证类型 = _repSType.GetEntityById(p.f_VisaType_tID ?? 0)?.f_value ?? "无",
                OA帐号 = p.f_AccountName ?? "",
                密码 = p.f_pwd ?? "",
                昵称 = p.f_nickname ?? "",
                蘭卡到期日 = p.f_passportDate?.ToString("yyyy-MM-dd") ?? "",
                I卡及I卡证明到期日 = p.f_ICardDate?.ToString("yyyy-MM-dd") ?? "",
                QQ号码 = p.f_QQNumber ?? "",
                菲线电话 = p.f_TelNoPH ?? "",
                国内电话 = p.f_TelNoCN ?? "",
                社区 = p.Dormitory?.f_Community ?? "",
                楼栋 = p.Dormitory?.f_Building ?? "",
                房号 = p.Dormitory?.f_RoomNo ?? "",
                在职状态 = p.STypeWorkStatus?.f_value ?? "",
                上班地点 = p.STypeWorkLocation?.f_value ?? "",
                上班搭车时间 = p.f_rideWorkTime?.ToString("HH:mm") ?? "",
                下班时间 = p.f_rideOffWorkTime?.ToString("HH:mm") ?? "",
                紧急联系人 = p.f_emergencyContactPerson ?? "",
                紧急联系方式 = p.f_emergencyContactNumber ?? "",
                权限等级 = p.STypeLevel.f_value ?? ""
            }).ToList();
        }
        /// <summary>
        /// excel导出全部数据
        /// </summary>
        /// <returns></returns>
        public List<dynamic> ExportData()
        {
            int iDepartmentTid = 0;
            if (!_employeeRepository.GetEntityById(_session.GetCurrUser().Id).STypeLevel.f_Remark.Contains("Department"))
            {
                iDepartmentTid = _employeeRepository.GetEntityById(_session.GetCurrUser().Id).f_department_tID;
            }
            List<Employee> data = _employeeRepository.GetAll().Where(u => iDepartmentTid == 0 || u.f_department_tID == iDepartmentTid).OrderBy(u => u.f_eid).ToList();
            return GetExportList(data).ToList();
        }
        /// <summary>
        /// 根据条件导出数据
        /// </summary>
        /// <param name="employeedto"></param>
        /// <returns></returns>
        public List<dynamic> ExportData(EmployeeDto employeedto)
        {
            bool boo1 = (employeedto.periodType_f_tid ?? 0) < 1;
            bool boo2 = (employeedto.f_department_tID ?? 0) < 1;
            bool bLevel = (employeedto.level_f_tid ?? 0) < 1;
            bool boo3 = string.IsNullOrWhiteSpace(employeedto.Name);
            if (!boo3)
            {
                employeedto.Name = AOUnity.GetStringByLetters(employeedto.Name).ToUpper().Trim();
            }
            bool boo4 = (employeedto.workStatus_f_tid ?? 0) < 1;
            List<Employee> data = _employeeRepository.GetAll().Where(p =>
            (boo1 || p.f_periodType_tID == employeedto.periodType_f_tid) &&
            (boo2 || p.f_department_tID == employeedto.f_department_tID) &&
            (bLevel || p.f_level_tID == employeedto.level_f_tid) &&
            (boo4 || p.f_workStatus_tID == employeedto.workStatus_f_tid) &&
            ((employeedto.StartDate == DateTime.MinValue) || (employeedto.StartDate <= p.f_CreateDate)) &&
            ((employeedto.EndDate == DateTime.MinValue) || (p.f_CreateDate <= employeedto.EndDate)) &&
            (boo3 || (p.f_chineseName.Contains(employeedto.Name) ||
                      p.f_passportAlis.ToUpper().Contains(employeedto.Name) ||
                      p.f_nickname.Contains(employeedto.Name) ||
                      (p.f_AccountName.ToUpper().Contains(employeedto.Name.ToUpper().Trim())))) &&
             (p.f_IsNewEmp == false)
            ).ToList();
            return GetExportList(data).ToList();
        }
        /// <summary>
        /// 获取对应部门新增的帐号名
        /// </summary>
        /// <param name="iDepartmentId"></param>
        /// <returns></returns>
        public string GetMaxAccountName(int iDepartmentId)
        {
            string sAccountName;
            string sJianXie = AOUnity.GetShorthand(iDepartmentId);
            string sAccoun = _employeeRepository.GetAll().Where(u => u.f_AccountName.Contains(sJianXie)).Max(u => u.f_AccountName);
            if (!string.IsNullOrWhiteSpace(sAccoun))
            {
                int iaccoun = int.Parse(sAccoun.Split(new[] { sJianXie }, StringSplitOptions.RemoveEmptyEntries)[0]) + 1;
                sAccountName = sJianXie + iaccoun.ToString();
            }
            else
            {
                sAccountName = sJianXie + "1001";
            }
            return sAccountName;
        }
        /// <summary>
        /// 添加员工
        /// </summary>
        /// <param name="modelEmployeeInfo"></param>
        public void AddEmployeeInfo(AllEmployeeInfoViewModel allEmployeeInfoViewModel)
        {
            Employee modelEmployeeInfo = allEmployeeInfoViewModel.EmployeeInfo;
            modelEmployeeInfo.f_passportAlis = AOUnity.GetStringByLetters(modelEmployeeInfo.f_passportName);
            if (modelEmployeeInfo.f_IsNewEmp)
            {
                //写入操作记录
                AOUnity.AddModifyRecord(null, GetJsonObjectData(modelEmployeeInfo).ToString(), ActionItem.Add, CategoryItem.NewEmployeeInfo, "新人登记", _session.GetCurrUser().NickName);
            }
            else
            {
                //写入操作记录
                AOUnity.AddModifyRecord(null, GetEmpJsonObjectData(modelEmployeeInfo).ToString(), ActionItem.Add, CategoryItem.EmployeeInfo, "员工管理", _session.GetCurrUser().NickName);
            }
            //同步订餐信息
            AddOrUpdateOrderingEmployees(allEmployeeInfoViewModel);
            _employeeRepository.Insert(modelEmployeeInfo);
            //添加机票管理信息
            AddReturnTicket(allEmployeeInfoViewModel);
            
        }
        /// <summary>
        /// 将实体转化成json数据，新人数据
        /// </summary>
        /// <param name="oModel">员工实体</param>
        /// <returns>json数据</returns>
        public JObject GetJsonObjectData(Employee oModel)
        {
            List<SType> oSTypeList = _repSType.GetAll().Where(p => p.f_type == (int)sTypeEnum.订餐类型).ToList();
            int[] iLDMInfo = oModel.OrderingEmployees?.Select(p => p.f_LDM_tID.Value).ToArray();
            string sLstLDM = iLDMInfo!=null?oSTypeList.Where(u => iLDMInfo.Contains(u.f_tID)).Aggregate("", (current, itme) => current + (itme.f_value + ",")):"";
            Dormitory oDormitory = _dormitory.GetEntityById(oModel.f_dormitoryId ?? 0);
            JObject json = new JObject();

            string periodValue = _repSType.GetAll().Where(p => p.f_type == (int)sTypeEnum.班次类型 && p.f_tID == oModel.f_periodType_tID).ToList().FirstOrDefault()?.f_value ?? "";
            string deptValue = _repSType.GetAll().Where(p => p.f_type == (int)sTypeEnum.部门类型 && p.f_tID == oModel.f_department_tID).ToList().FirstOrDefault()?.f_value ?? "";
            string workStatus = _repSType.GetAll().Where(p => p.f_type == (int)sTypeEnum.上班状态类型 && p.f_tID == oModel.f_workStatus_tID).ToList().FirstOrDefault()?.f_value ?? "";
            string workLocation = _repSType.GetAll().Where(p => p.f_type == (int)sTypeEnum.上班地点类型 && p.f_tID == oModel.f_workLocation_tID).ToList().FirstOrDefault()?.f_value ?? "";
            string level = _repSType.GetAll().Where(p => p.f_type == (int)sTypeEnum.权限类型 && p.f_tID == oModel.f_level_tID).ToList().FirstOrDefault()?.f_value ?? "";
            string visaType = _repSType.GetAll().Where(p => p.f_type == (int)sTypeEnum.签证类型 && p.f_tID == oModel.f_VisaType_tID).ToList().FirstOrDefault()?.f_value ?? "";
            string Stage = _repSType.GetAll().Where(p => p.f_type == (int)sTypeEnum.金流客服等级类型 && p.f_tID == oModel.f_JLStage).ToList().FirstOrDefault()?.f_value ?? "";
            string group = _repSType.GetAll().Where(p => p.f_type == (int)sTypeEnum.组别类型 && p.f_tID == oModel.f_JLgroup).ToList().FirstOrDefault()?.f_value ?? "";
            string director = _employeeRepository.GetAll().Where(p => p.f_eid == oModel.f_JLdirector_eId).FirstOrDefault()?.f_chineseName ?? "";
            json.Add("员工ID", oModel.f_eid);
            json.Add("班次", periodValue);
            json.Add("部门", deptValue);
            json.Add("中文名", oModel.f_chineseName);
            json.Add("性别", oModel.f_sex == 1 ? "男" : "女");
            json.Add("护照ID", oModel.f_passportID);
            json.Add("护照名英文/越南文", oModel.f_passportName);
            json.Add("存储护照照片URL", oModel.f_PassportURL);
            json.Add("昵称", oModel.f_nickname);
            json.Add("国籍", oModel.f_international);
            json.Add("QQ号码", oModel.f_QQNumber);
            json.Add("菲律宾联系电话", oModel.f_TelNoPH);
            json.Add("国内电话", oModel.f_TelNoCN);
            json.Add("紧急联系人", oModel.f_emergencyContactPerson);
            json.Add("紧急联系人电话", oModel.f_emergencyContactNumber);
            json.Add("微信", oModel.f_Wechat);
            json.Add("在职状态", workStatus);
            json.Add("上班搭车时间", oModel.f_rideWorkTime);
            json.Add("下班搭车时间", oModel.f_rideOffWorkTime);
            json.Add("工作地点", workLocation);
            json.Add("宿舍", oDormitory?.f_Community + "/" + oDormitory?.f_Building + "/" + oDormitory?.f_RoomNo);
            json.Add("宿舍表ID", oModel.f_dormitoryId);
            json.Add("备注", oModel.f_Remark);
            json.Add("等级权限", level);
            json.Add("新人表中的id", oModel.f_nid);
            json.Add("是否为菲律宾员工", oModel.f_IsPHStaff ? "是" : "否");
            json.Add("薪资", oModel.f_Salary);
            json.Add("OA帐号", oModel.f_AccountName);
            json.Add("创建时间", oModel.f_CreateDate);
            json.Add("签证类型", visaType);
            json.Add("存储入境章照片URL", oModel.f_EntrystampURL);
            json.Add("签证到期时间", oModel.f_passportDate);
            json.Add("入职日期", oModel.f_HireDate);
            json.Add("出生日期", oModel.f_DateOfBirth);
            json.Add("是否已移走", oModel.f_IsRemove ? "是" : "否");
            json.Add("是否为新员工", oModel.f_IsNewEmp ? "是" : "否");
            json.Add("I卡或者I卡证明到期时间", oModel.f_ICardDate);
            json.Add("存储证件照照片URL", oModel.f_IDCardURL);

            json.Add("航班类型", _repSType.GetEntityById(oModel.NewEmployee?.f_flightTypt_tID ?? 0)?.f_value ?? "");
            json.Add("航班起飞时间", oModel.NewEmployee?.f_flightDate == null ? "" : oModel.NewEmployee?.f_flightDate.Value.ToString("yyyy-MM-dd") + "   " + oModel.NewEmployee?.f_flightDate.Value.ToString("HH:mm"));
            json.Add("航班抵达时间", oModel.NewEmployee?.f_flightArrivalTime == null ? "" : oModel.NewEmployee?.f_flightArrivalTime.Value.ToString("yyyy-MM-dd") + "   " + oModel.NewEmployee?.f_flightArrivalTime.Value.ToString("HH:mm"));
            json.Add("新人餐", sLstLDM ?? "");
            json.Add("借款金额", oModel.NewEmployee?.f_LoanAmount ?? 0);
            json.Add("储值卡", oModel.NewEmployee?.f_GiftCard ?? 0);
            json.Add("电话卡", oModel.NewEmployee?.f_TelCatd ?? 0);
            json.Add("是否确认", oModel.NewEmployee?.f_signature ?? false ? "已确认" : "");
            json.Add("新人备注", oModel.NewEmployee?.f_Remark ?? "");
            return json;
        }
        /// <summary>
        /// 将实体转化成json数据，普通員工
        /// </summary>
        /// <param name="oModel">员工实体</param>
        /// <returns>json数据</returns>
        public JObject GetEmpJsonObjectData(Employee oModel)
        {
            Dormitory oDormitory = _dormitory.GetEntityById(oModel.f_dormitoryId ?? 0);
            string periodValue = _repSType.GetAll().Where(p => p.f_type == (int)sTypeEnum.班次类型 && p.f_tID == oModel.f_periodType_tID).ToList().FirstOrDefault()?.f_value ?? "";
            string deptValue = _repSType.GetAll().Where(p => p.f_type == (int)sTypeEnum.部门类型 && p.f_tID == oModel.f_department_tID).ToList().FirstOrDefault()?.f_value ?? "";
            string workStatus = _repSType.GetAll().Where(p => p.f_type == (int)sTypeEnum.上班状态类型 && p.f_tID == oModel.f_workStatus_tID).ToList().FirstOrDefault()?.f_value ?? "";
            string workLocation = _repSType.GetAll().Where(p => p.f_type == (int)sTypeEnum.上班地点类型 && p.f_tID == oModel.f_workLocation_tID).ToList().FirstOrDefault()?.f_value ?? "";
            string level = _repSType.GetAll().Where(p => p.f_type == (int)sTypeEnum.权限类型 && p.f_tID == oModel.f_level_tID).ToList().FirstOrDefault()?.f_value ?? "";
            string visaType = _repSType.GetAll().Where(p => p.f_type == (int)sTypeEnum.签证类型 && p.f_tID == oModel.f_VisaType_tID).ToList().FirstOrDefault()?.f_value ?? "";
            string Stage = _repSType.GetAll().Where(p => p.f_type == (int)sTypeEnum.金流客服等级类型 && p.f_tID == oModel.f_JLStage).ToList().FirstOrDefault()?.f_value ?? "";
            string group = _repSType.GetAll().Where(p => p.f_type == (int)sTypeEnum.组别类型 && p.f_tID == oModel.f_JLgroup).ToList().FirstOrDefault()?.f_value ?? "";
            string director = _employeeRepository.GetAll().Where(p => p.f_eid == oModel.f_JLdirector_eId).FirstOrDefault()?.f_chineseName ?? "";
            JObject json = new JObject();
            json.Add("员工ID", oModel.f_eid);
            json.Add("班次", periodValue);
            json.Add("部门", deptValue);
            json.Add("中文名", oModel.f_chineseName);
            json.Add("性别", oModel.f_sex == 1 ? "男" : "女");
            json.Add("护照ID", oModel.f_passportID);
            json.Add("护照名英文/越南文", oModel.f_passportName);
            json.Add("存储护照照片URL", oModel.f_PassportURL);
            json.Add("昵称", oModel.f_nickname);
            json.Add("国籍", oModel.f_international);
            json.Add("QQ号码", oModel.f_QQNumber);
            json.Add("菲律宾联系电话", oModel.f_TelNoPH);
            json.Add("国内电话", oModel.f_TelNoCN);
            json.Add("紧急联系人", oModel.f_emergencyContactPerson);
            json.Add("紧急联系人电话", oModel.f_emergencyContactNumber);
            json.Add("微信", oModel.f_Wechat);
            json.Add("在职状态", workStatus);
            json.Add("上班搭车时间", oModel.f_rideWorkTime);
            json.Add("下班搭车时间", oModel.f_rideOffWorkTime);
            json.Add("工作地点", workLocation);
            json.Add("宿舍", oDormitory?.f_Community + "/" + oDormitory?.f_Building + "/" + oDormitory?.f_RoomNo);
            json.Add("宿舍表ID", oModel.f_dormitoryId);
            json.Add("备注", oModel.f_Remark);
            json.Add("等级权限", level);
            json.Add("新人表中的id", oModel.f_nid);
            json.Add("是否为菲律宾员工", oModel.f_IsPHStaff ? "是" : "否");
            json.Add("薪资", oModel.f_Salary);
            json.Add("OA帐号", oModel.f_AccountName);
            json.Add("创建时间", oModel.f_CreateDate);
            json.Add("签证类型", visaType);
            json.Add("存储入境章照片URL", oModel.f_EntrystampURL);
            json.Add("签证到期时间", oModel.f_passportDate);
            json.Add("入职日期", oModel.f_HireDate);
            json.Add("出生日期", oModel.f_DateOfBirth);
            json.Add("是否已移走", oModel.f_IsRemove ? "是" : "否");
            json.Add("是否为新员工", oModel.f_IsNewEmp ? "是" : "否");
            json.Add("I卡或者I卡证明到期时间", oModel.f_ICardDate);
            json.Add("离职日期", oModel.f_SeparationDate);
            json.Add("旧的订餐信息", oModel.f_OldReservation);
            json.Add("金流客服等级", Stage);
            json.Add("组别", group);
            json.Add("直属主管", director);
            json.Add("存储证件照照片URL", oModel.f_IDCardURL);
            return json;
        }

        private void AddReturnTicket(AllEmployeeInfoViewModel allEmployeeInfoViewModel, bool bIsEdit = false)
        {
            if (!allEmployeeInfoViewModel.IsnewEmployeeInfo) return;
            ReturnTicket modelReturnTicket = _returnTicket.GetAll().FirstOrDefault(p => p.f_eId == allEmployeeInfoViewModel.EmployeeInfo.f_eid) ?? new ReturnTicket();
            if (modelReturnTicket.f_Id == 0)
            {
                bIsEdit = false;
            }
            string sTime = string.Empty;
            if (allEmployeeInfoViewModel.EmployeeInfo?.NewEmployee?.f_flightDate != null)
            {
                sTime = allEmployeeInfoViewModel.EmployeeInfo?.NewEmployee?.f_flightDate.Value.ToString("HH:mm~");
            }
            if (allEmployeeInfoViewModel.EmployeeInfo?.NewEmployee?.f_flightArrivalTime != null)
            {
                if (!sTime.Contains("~"))
                {
                    sTime = "~";
                }
                sTime += allEmployeeInfoViewModel.EmployeeInfo?.NewEmployee?.f_flightArrivalTime.Value.ToString("HH:mm");
            }
            bool bIsLog = false;//标识修改时是否需要写入操作记录
            if (modelReturnTicket.f_FromDate != allEmployeeInfoViewModel.EmployeeInfo?.NewEmployee?.f_flightDate || modelReturnTicket.f_FromAirlineType_Id != allEmployeeInfoViewModel.EmployeeInfo?.NewEmployee?.f_flightTypt_tID
                || modelReturnTicket.f_FromFlight != sTime || modelReturnTicket.f_FromTerminal != _repSType.GetAll().FirstOrDefault(p => p.f_tID == modelReturnTicket.f_FromAirlineType_Id)?.f_Remark)
            {
                bIsLog = true;
            }
            modelReturnTicket.f_eId = allEmployeeInfoViewModel.EmployeeInfo?.f_eid;
            //modelReturnTicket.t_employeeInfo = null;
            modelReturnTicket.f_FromDate = allEmployeeInfoViewModel.EmployeeInfo?.NewEmployee?.f_flightDate;
            modelReturnTicket.f_FromAirlineType_Id = allEmployeeInfoViewModel.EmployeeInfo?.NewEmployee?.f_flightTypt_tID;
            modelReturnTicket.f_FromFlight = sTime;
            modelReturnTicket.f_FromRemark = "新人来菲机票";
            modelReturnTicket.f_Operation = _session.GetCurrUser().NickName;
            modelReturnTicket.f_OperationId = _session.GetCurrUser().Id;
            modelReturnTicket.f_OperationDate = DateTime.Now;
            modelReturnTicket.f_FromTerminal = _repSType.GetAll().FirstOrDefault(p => p.f_tID == modelReturnTicket.f_FromAirlineType_Id)?.f_Remark;
            modelReturnTicket.f_IsNewEmp = true;
            if (bIsEdit)
            {
                _tTReturnTicketService.EditReturnTicketOneData(modelReturnTicket, null, bIsLog);
            }
            else
            {
                _tTReturnTicketService.AddReturnTicketOneData(modelReturnTicket);
            }
        }
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns>返回LaundryPwd集合</returns>
        public List<LaundryPwd> GetLaundryPwdAllData()
        {
            return _laundryPwd.GetAll().OrderByDescending(u => u.f_Id).ToList();
        }
        /// <summary>
        /// 移走新人
        /// </summary>
        /// <param name="iEid"></param>
        public void RemovalEmployeeInfoById(int iEid)
        {
            var modelEmployeeInfo = _employeeRepository.GetEntityById(iEid);
            string sReservation = string.Empty;//旧订餐
            if (modelEmployeeInfo != null)
            {
                //删除员工宿舍订餐信息
                var list = modelEmployeeInfo.OrderingEmployees.Where(p => p.f_eID == modelEmployeeInfo.f_eid).ToList();
                if (list.Any())
                {
                    foreach (var item in list)
                    {
                        sReservation += _repSType.GetEntityById((int)item.f_LDM_tID).f_value + ",";
                        _orderemp.Delete(_orderemp.GetEntityById((int)item.f_LDM_tID));
                    }
                }
                //删除新人宿舍订餐信息
                var newlist = modelEmployeeInfo.NewOrderingEmoloyees.Where(p => p.f_eID == modelEmployeeInfo.f_eid).ToList();
                var LDM = _repSType.GetAll().Where(p => p.f_type == (int)sTypeEnum.订餐类型).ToList();//查出中中晚宵夜的tID
                var typeList = _repSType.GetAll().Where(p => p.f_type == (int)sTypeEnum.订餐要求类型).ToList();//查出中中晚宵夜
                if (newlist.Any())
                {
                    var orderers = newlist.GroupBy(p => new { p.f_LDM_tID }).ToList();
                    List<NewOrderingEmployees> sortList;
                    foreach (var one in orderers)
                    {
                        sortList = newlist.Where(p => p.f_LDM_tID == one.Key.f_LDM_tID).OrderBy(p => p.f_EffectiveDate).ToList();
                        sReservation += LDM.FirstOrDefault(p => p.f_tID == one.Key.f_LDM_tID).f_value + "\n";
                        foreach (var data in sortList)
                        {
                            sReservation += data.f_EffectiveDate.Value.ToString("MM/dd") + "*" + typeList.FirstOrDefault(p => p.f_tID == data.f_type_tID).f_value + "*" + data.f_Quantity + "份\n";
                        }
                    }
                }
                RemoveWorkDistribution(modelEmployeeInfo);
                DateTime? dDate = modelEmployeeInfo.f_rideWorkTime;
                DateTime? dTime = modelEmployeeInfo.f_rideOffWorkTime;
                //如果有报道时间 就计算上下班时间
                if (dTime != null && dTime != DateTime.MinValue)
                {
                    modelEmployeeInfo.f_rideWorkTime = DateTime.Parse(dDate?.ToString("yyyy-MM-dd ") + dTime.Value.AddHours(-1).ToString("HH:mm"));//上班搭车时间
                    modelEmployeeInfo.f_rideOffWorkTime = modelEmployeeInfo.f_rideWorkTime.Value.AddHours(9);//下班搭车时间
                    if (modelEmployeeInfo.NewEmployee != null)
                    {
                        modelEmployeeInfo.NewEmployee.f_RegistrationTime = DateTime.Parse(dDate?.ToString("yyyy-MM-dd ") + dTime.Value.ToString("HH:mm"));
                    }
                }
                else
                {
                    modelEmployeeInfo.f_rideWorkTime = DateTime.Parse("1990-01-01 09:00");
                    modelEmployeeInfo.f_rideOffWorkTime = DateTime.Parse("1990-01-01 18:00");
                }
                modelEmployeeInfo.f_workStatus_tID = _repSType.GetAll().FirstOrDefault(p => p.f_type == (int)sTypeEnum.上班状态类型 && p.f_value == "在职")?.f_tID ?? 0; //默认在职的状态 
                modelEmployeeInfo.f_IsRemove = true;//状态变为移走
                modelEmployeeInfo.f_IsNewEmp = false;//状态为  不是新员工
                modelEmployeeInfo.f_HireDate = DateTime.Now.Date;
                modelEmployeeInfo.f_OldReservation = sReservation;
                modelEmployeeInfo.f_CreateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                Update(modelEmployeeInfo);
            }
        }
        /// <summary>
        /// //移除所有新人餐的交接記錄
        /// </summary>
        /// <param name="modelEmployeeInfo"></param>
        private void RemoveWorkDistribution(Employee modelEmployeeInfo)
        {
            int iWorkType = _repSType.GetAll().Where(u => u.f_type == (int)sTypeEnum.工作类别 && "新人订餐管理".Equals(u.f_value)).FirstOrDefault()?.f_tID ?? 0;
            var notice = _workdistribution.GetAll().FirstOrDefault(u => u.f_WorkType == iWorkType && modelEmployeeInfo.f_eid.ToString().Equals(u.f_Remarks));
            if (notice != null)
            {
                _workdistribution.Delete(notice);
            }
        }
        /// <summary>
        /// 查询已被移走的新人数据
        /// </summary>
        /// <param name="queryModel">查询条件</param>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">每页多少数据</param>
        /// <param name="o_iCount">总数</param>
        /// <returns>返回t_employeeInfo集合</returns>
        public List<Employee> GetNewEmpRemove(EmployeeDto employeedto, int iPageIndex, int iPageSize, out int o_iCount)
        {
            if (!_employeeRepository.GetEntityById(_session.GetCurrUser().Id).STypeLevel.f_Remark.Contains("Department"))
            {
                //不包括 Department 。表示不能查看其他部门 只能查看自己部门
                employeedto.f_department_tID = _employeeRepository.GetEntityById(_session.GetCurrUser().Id).f_department_tID;
            }
            var result = _employeeRepository.GetAll().Where(u =>
                (employeedto.StartDate == DateTime.MinValue || (u.NewEmployee.f_flightDate >= employeedto.StartDate && u.NewEmployee.f_flightArrivalTime >= employeedto.StartDate)) &&
                (employeedto.EndDate == DateTime.MinValue || (u.NewEmployee.f_flightDate <= employeedto.EndDate && u.NewEmployee.f_flightArrivalTime <= employeedto.EndDate)) &&
                ((employeedto.f_department_tID ?? 0) < 1 || u.f_department_tID == employeedto.f_department_tID) &&
                (u.f_IsRemove) &&
                ((employeedto.Name ?? "").Trim() == "" || u.f_AccountName.Contains(employeedto.Name) || u.f_chineseName.Contains(employeedto.Name) || u.f_passportName.Contains(employeedto.Name) || u.f_nickname.Contains(employeedto.Name)));
            o_iCount = result.Count();
            //ipageIndex是否超出总页码
            AOUnity.ValidatePagingWhere(result.Count(), ref iPageIndex, iPageSize);
            return result.OrderBy(p => p.NewEmployee.f_flightDate).ThenBy(p => p.f_eid).Skip((iPageIndex - 1) * iPageSize).Take(iPageSize).ToList();
        }
        /// <summary>
        /// 获取已被移走的新人数据
        /// </summary>
        /// <param name="queryModel">查询条件</param>
        /// <returns>返回t_employeeInfo集合</returns>
        public List<Employee> GetNewEmpRemove(EmployeeDto employeedto)
        {
            if (!_employeeRepository.GetEntityById(_session.GetCurrUser().Id).STypeLevel.f_Remark.Contains("Department"))
            {
                //不包括 Department 。表示不能查看其他部门 只能查看自己部门
                employeedto.f_department_tID = _employeeRepository.GetEntityById(_session.GetCurrUser().Id).f_department_tID;
            }
            var result = _employeeRepository.GetAll().Where(u =>
                (employeedto.StartDate == DateTime.MinValue || (u.NewEmployee.f_flightDate >= employeedto.StartDate && u.NewEmployee.f_flightArrivalTime >= employeedto.StartDate)) &&
                (employeedto.EndDate == DateTime.MinValue || (u.NewEmployee.f_flightDate <= employeedto.EndDate && u.NewEmployee.f_flightArrivalTime <= employeedto.EndDate)) &&
                ((employeedto.f_department_tID ?? 0) < 1 || u.f_department_tID == employeedto.f_department_tID) &&
                (u.f_IsRemove) &&
                ((employeedto.Name ?? "").Trim() == "" || u.f_AccountName.Contains(employeedto.Name) || u.f_chineseName.Contains(employeedto.Name) || u.f_passportName.Contains(employeedto.Name) || u.f_nickname.Contains(employeedto.Name)));
            return result.OrderBy(p => p.NewEmployee.f_flightDate).ThenBy(p => p.f_eid).ToList();
        }
        /// <summary>
        /// 打印以移走新人数据
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="iPageIndex"></param>
        /// <param name="iPageSize"></param>
        /// <returns></returns>
        public List<dynamic> ExportRemoveNewEmp(EmployeeDto employeedto)
        {
            List<Employee> data = GetNewEmpRemove(employeedto).ToList();
            int index = 1;
            return data.Select(p => new
            {
                序号 = index++,
                国籍 = p.f_international,
                部门 = p.STypeDepartment.f_value,
                姓名 = p.f_chineseName + " (" + p.f_passportName + ")",
                OA帐号 = p.f_AccountName,
                密码 = p.f_pwd,
                性别 = (p.f_sex.ToString() == "0") ? "女" : "男",
                航班类型 = _repSType.GetEntityById(p.NewEmployee?.f_flightTypt_tID ?? 0)?.f_value ?? "无",
                航班起飞日期 = p.NewEmployee?.f_flightDate == null ? "" : p.NewEmployee?.f_flightDate.Value.ToString("yyyy-MM-dd") + p.NewEmployee?.f_flightDate.Value.ToString("HH:mm"),
                航班抵达日期 = p.NewEmployee?.f_flightArrivalTime == null ? "" : p.NewEmployee?.f_flightArrivalTime.Value.ToString("yyyy-MM-dd") + p.NewEmployee?.f_flightArrivalTime.Value.ToString("HH:mm"),
                报到地址 = p.STypeWorkLocation?.f_value ?? "暂无",
                报到日期 = ((p.f_rideWorkTime != null && "1990-01-01" != p.f_rideWorkTime.Value.ToString("yyyy-MM-dd")) ? p.f_rideWorkTime.Value.ToString("yyyy-MM-dd") : "") + " " + ((p.f_rideOffWorkTime != null && "1990-01-01" != p.f_rideWorkTime.Value.ToString("yyyy-MM-dd")) ? p.f_rideOffWorkTime.Value.ToString("HH:mm") : ""),
                社区楼栋房号 = p.Dormitory?.f_Community + "/ " + p.Dormitory?.f_Building + "/ " + p.Dormitory?.f_RoomNo,
                新人餐 = p.f_OldReservation,
                借款金额 = p.NewEmployee?.f_LoanAmount,
                储值卡 = p.NewEmployee?.f_GiftCard,
                电话卡 = p.NewEmployee?.f_TelCatd,
                确认签字 = p.NewEmployee?.f_signature ?? false ? "已确认" : "",
                是否告知培训部 = (p.NewEmployee?.f_isInform ?? false) ? "√" : "",
                备注 = p.NewEmployee?.f_Remark
            }).ToList<dynamic>();
        }
        /// <summary>
        /// 获取所有班车数据
        /// </summary>
        /// <param name="f_department_Id">部门id</param>
        /// <param name="iPageIndex"></param>
        /// <param name="iPageSize"></param>
        /// <param name="o_Count"></param>
        /// <returns></returns>
        public List<Employee> GetShuttleBus(int iPageIndex, int iPageSize, out int o_Count)
        {
            var result = _employeeRepository.GetAll().Where(j => j.f_IsNewEmp == false);
            o_Count = result.Count();
            //ipageIndex是否超出总页码
            AOUnity.ValidatePagingWhere(result.Count(), ref iPageIndex, iPageSize);
            return result.OrderBy(j => j.f_rideWorkTime).ThenBy(j => j.f_eid).Skip((iPageIndex - 1) * iPageSize).Take(iPageSize).ToList();
        }
        /// <summary>
        /// 根据部门id获取班车数据
        /// </summary>
        /// <param name="f_department_Id">部门id</param>
        /// <param name="iPageIndex"></param>
        /// <param name="iPageSize"></param>
        /// <param name="o_Count"></param>
        /// <returns></returns>
        public List<Employee> GetShuttleBus(int f_department_Id, int iPageIndex, int iPageSize, out int o_Count)
        {
            var result = _employeeRepository.GetAll().Where(p => p.f_department_tID == f_department_Id && p.f_IsNewEmp == false);
            o_Count = result.Count();
            //ipageIndex是否超出总页码
            AOUnity.ValidatePagingWhere(result.Count(), ref iPageIndex, iPageSize);
            return result.OrderBy(p => p.f_rideWorkTime).ThenBy(p => p.f_eid).Skip((iPageIndex - 1) * iPageSize).Take(iPageSize).ToList();
        }
        /// <summary>
        /// 班车汇总
        /// </summary>
        /// <param name="sCommunityStr">楼栋</param>
        /// <param name="sWorkLocationStr">上班地点</param>
        /// <param name="isY">是否包含返乡，Y包含</param>
        /// <returns></returns>
        public List<TrafficStatisticsDto> GetBusSummary(string[] sCommunityStr, string[] sWorkLocationStr, string isY)
        {
            if (isY == "Y")
            {
                //上班
                var lisResult = _employeeRepository.GetAll().Where(p => p.f_rideWorkTime != null && p.f_dormitoryId != null && p.STypeWorkLocation != null &&
                (p.STypeWorkStatus.f_value.Contains("在职") || p.STypeWorkStatus.f_value.Contains("返乡") || p.STypeWorkStatus.f_value.Contains("在職") || p.STypeWorkStatus.f_value.Contains("返鄉"))
                && p.f_IsNewEmp == false && sCommunityStr.Contains(p.Dormitory.f_Community) && sWorkLocationStr.Contains(p.STypeWorkLocation.f_value)).ToList()
                    .GroupBy(p => p.f_rideWorkTime?.ToShortTimeString()).Select(p => TResult(p, 1, sCommunityStr, sWorkLocationStr)).OrderBy(u => u.RideTime).ToList();
                //下班
                lisResult.AddRange(_employeeRepository.GetAll().Where(p => p.f_rideOffWorkTime != null && p.f_dormitoryId != null && p.STypeWorkLocation != null &&
                (p.STypeWorkStatus.f_value.Contains("在职") || p.STypeWorkStatus.f_value.Contains("返乡") || p.STypeWorkStatus.f_value.Contains("在職") || p.STypeWorkStatus.f_value.Contains("返鄉"))
                && p.f_IsNewEmp == false && sCommunityStr.Contains(p.Dormitory.f_Community) && sWorkLocationStr.Contains(p.STypeWorkLocation.f_value)).ToList()
                    .GroupBy(p => p.f_rideOffWorkTime?.ToShortTimeString()).Select(p => TResult(p, 2, sCommunityStr, sWorkLocationStr)).OrderBy(u => u.RideTime).ToList());

                return lisResult;

            }
            else
            {
                //上班
                var lisResult = _employeeRepository.GetAll().Where(p => p.f_rideWorkTime != null && p.f_dormitoryId != null && p.STypeWorkLocation != null &&
                (p.STypeWorkStatus.f_value.Contains("在职") || p.STypeWorkStatus.f_value.Contains("在職"))
                && p.f_IsNewEmp == false && sCommunityStr.Contains(p.Dormitory.f_Community) && sWorkLocationStr.Contains(p.STypeWorkLocation.f_value)).ToList()
                    .GroupBy(p => p.f_rideWorkTime?.ToShortTimeString()).Select(p => TResult(p, 1, sCommunityStr, sWorkLocationStr)).OrderBy(u => u.RideTime).ToList();
                //下班
                lisResult.AddRange(_employeeRepository.GetAll().Where(p => p.f_rideOffWorkTime != null && p.f_dormitoryId != null && p.STypeWorkLocation != null &&
                (p.STypeWorkStatus.f_value.Contains("在职") || p.STypeWorkStatus.f_value.Contains("在職"))
                && p.f_IsNewEmp == false && sCommunityStr.Contains(p.Dormitory.f_Community) && sWorkLocationStr.Contains(p.STypeWorkLocation.f_value)).ToList()
                    .GroupBy(p => p.f_rideOffWorkTime?.ToShortTimeString()).Select(p => TResult(p, 2, sCommunityStr, sWorkLocationStr)).OrderBy(u => u.RideTime).ToList());
                return lisResult;
            }
        }
        /// <summary>
        /// 手动分组统计
        /// </summary>
        /// <param name="groupData">分组之后的数据</param>
        /// <param name="stype">上班1or下班2</param>
        /// <param name="sCommunityStr">要统计的社区数组</param>
        /// <param name="sWorkLocationStr">要统计的工作地点数组</param>
        /// <returns></returns>
        private TrafficStatisticsDto TResult(IGrouping<string, Employee> groupData, int stype, IReadOnlyList<string> sCommunityStr, IReadOnlyList<string> sWorkLocationStr)
        {
            DateTime time;
            DateTime.TryParse(groupData.Key, out time);
            //声明班车搭车始实体
            TrafficStatisticsDto traffic = new TrafficStatisticsDto(stype, time, groupData.Count(), sCommunityStr.Count, sWorkLocationStr.Count);
            //通过循环统计人数
            for (int i = 0; i < sCommunityStr.Count; i++)
            {
                traffic.Community[i] = groupData.Count(c => c.Dormitory.f_Community.Trim() == sCommunityStr[i].Trim());
            }
            for (int i = 0; i < sWorkLocationStr.Count; i++)
            {
                traffic.WorkLocation[i] = groupData.Count(c => c.STypeWorkLocation.f_value.Trim() == sWorkLocationStr[i].Trim());
            }
            traffic.isComprising =
                groupData.Any(p => p.STypeWorkStatus.f_value.Contains("返乡") || p.STypeWorkStatus.f_value.Contains("返鄉"));
            return traffic;
        }
        /// <summary>
        /// 人事数据统计
        /// </summary>
        /// <returns></returns>
        public List<Employee> GetPersonnelData()
        {
            return _employeeRepository.GetAll().Where(p => p.STypeWorkStatus != null).ToList();
        }
        /// <summary>
        /// 新人登记打印数据
        /// </summary>
        /// <param name="employeedto"></param>
        /// <returns></returns>
        public List<Employee> GetPageListexcel(EmployeeDto employeedto, bool bIsNew = false)
        {
            bool bDepartment = (employeedto.f_department_tID ?? 0) < 1;
            bool bLevel = (employeedto.level_f_tid ?? 0) < 1;
            bool bName = string.IsNullOrWhiteSpace(employeedto.Name);
            bool bTelNoPH = string.IsNullOrWhiteSpace(employeedto.TelNoPH);
            bool bBuildingOrRoomNo = string.IsNullOrWhiteSpace(employeedto.BuildingOrRoomNo);
            int? workStatus = employeedto.workStatus_f_tid;
            bool bWorkStatus = (employeedto.workStatus_f_tid ?? 0) == 0 || employeedto.workStatus_f_tid == -1;
            if (workStatus != null && workStatus == -2)
            {
                // -2空白的状态
                workStatus = null;
            }
            bool bStartDate = (employeedto.StartDate == DateTime.MinValue);//开始创建时间   
            bool bEndDate = (employeedto.EndDate == DateTime.MinValue);
            var result = _employeeRepository.GetAll().Where(p =>
            (bDepartment || p.f_department_tID == employeedto.f_department_tID) &&
            (bLevel || p.f_level_tID == employeedto.level_f_tid) &&
            (bWorkStatus || p.f_workStatus_tID == workStatus) &&
            (bStartDate || (employeedto.StartDate <= p.f_CreateDate)) &&
            (bEndDate || (p.f_CreateDate <= employeedto.EndDate)) &&
            (bTelNoPH || ((p.f_TelNoPH ?? "").Contains(employeedto.TelNoPH))) &&
            (bBuildingOrRoomNo || (p.Dormitory.f_Building ?? "").Contains(employeedto.BuildingOrRoomNo) || (p.Dormitory.f_RoomNo ?? "").Contains(employeedto.BuildingOrRoomNo)) &&
            (bName || (p.f_chineseName.Contains(employeedto.Name) ||
                      p.f_passportAlis.ToUpper().Contains(employeedto.Name) ||
                      (p.f_nickname ?? "").Contains(employeedto.Name) ||
                      (p.f_AccountName.ToUpper().Contains(employeedto.Name.ToUpper().Trim())))) &&
             (bIsNew ? p.f_IsNewEmp : p.f_IsNewEmp == false)
            ).ToList();
            return result;
        }
        /// <summary>
        /// 新人登记打印数据
        /// </summary>
        /// <param name="employeedto"></param>
        /// <returns></returns>
        public List<Employee> GetPageListexcel()
        {
            return _employeeRepository.GetAll().Where(j => j.f_IsNewEmp == true).ToList();
        }
        /// <summary>
        ///  修改护照或者入境章图片路径
        /// </summary>
        /// <param name="oMdel">员工信息</param>
        /// <param name="sUrl">存储的url</param>
        /// <param name="sT">护照or入境章</param>
        /// <returns></returns>
        public void UpdetePassporUrl(Employee oMdel, string sUrl, string sT)
        {
            if (oMdel != null && !string.IsNullOrWhiteSpace(sUrl))
            {
                if (sT == "p")
                {
                    oMdel.f_PassportURL = sUrl;
                }
                else if (sT == "e")
                {
                    oMdel.f_EntrystampURL = sUrl;
                }
                else
                {
                    oMdel.f_IDCardURL = sUrl;
                }
                Update(oMdel);
            }
        }
        /// <summary>
        /// 获得所有员工信息
        /// </summary>
        /// <returns></returns>
        public IList<Employee> GetAllEmployeeData()
        {
            return _employeeRepository.GetAll().ToList();
        }

        /// <summary>
        /// 获取洗衣房以及密码 
        /// </summary>
        /// <param name="iId">洗衣房ID</param>
        /// <returns></returns>
        public string GetLaundryPwd(int iId)
        {
            return _laundryPwd.GetEntityById(iId)?.f_NoAndPwd ?? "";
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public void EditEmployeePwd(ModifyPwdModel model)
        {
            //根据用户ID和用户名获得满条件的第一个实体对象
            var oldModel = _employeeRepository.GetAll().FirstOrDefault(m => m.f_eid == model.eid && m.f_pwd == model.oldPwd);
            if (oldModel != null)
            {
                oldModel.f_pwd = model.newPwd;
            }
            _employeeRepository.Update(oldModel);
        }
    }
}
