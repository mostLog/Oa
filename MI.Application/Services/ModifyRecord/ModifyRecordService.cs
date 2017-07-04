using AutoMapper;
using MI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MI.Core.Domain;
using MI.Application.Dto;
using MI.Core.Common;

namespace MI.Application
{
    public class ModifyRecordService:IModifyRecordService
    {
        private readonly IBaseRepository<t_ModifyRecord> _repository; 
        private readonly IBaseRepository<WorkDistribution> _workDistribution;
        private readonly IBaseRepository<Employee> _employee; 
            private readonly IBaseRepository<Notice> _notice; 
            private readonly IBaseRepository<Grant> _grant;
        private readonly IBaseRepository<FlightFee> _flightFee;
        private readonly IBaseRepository<EmpRent> _empRent;
        private readonly IBaseRepository<SType> _SType;
        private readonly IBaseRepository<NewEmployee> _newEmployee;
        public ModifyRecordService(IBaseRepository<t_ModifyRecord> repository, 
            IBaseRepository<WorkDistribution> workDistribution,
            IBaseRepository<Employee> employee,
            IBaseRepository<Notice> notice,
            IBaseRepository<Grant> grant,
            IBaseRepository<FlightFee> flightFee,
            IBaseRepository<EmpRent> empRent,
            IBaseRepository<SType> SType,
            IBaseRepository<NewEmployee> newEmployee)
        {
            _repository = repository;
            _workDistribution = workDistribution;
            _employee = employee;
            _notice = notice;
            _grant = grant;
            _flightFee = flightFee;
            _empRent = empRent;
            _SType = SType;
            _newEmployee = newEmployee;

        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">model实体</param>
        public void AddModifyRecordData(t_ModifyRecord model)
        {
            _repository.Insert(model);
        }

        public IList<ModifyRecordDto> GetEmpRent()
        {
            IList<ModifyRecordDto> dtoEmpRent = Mapper.Map<IList<ModifyRecordDto>>(_repository.GetAll().ToList());
            return dtoEmpRent;
        }
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">每页多少条数据</param>
        /// <param name="iCount">总数</param>
        /// <returns>返回t_ModifyRecord集合</returns>
        public List<t_ModifyRecord> GetModifyRecordAllData(int iPageIndex, int iPageSize, out int iCount)
        {
            var linq = _repository.GetAll().OrderByDescending(u => u.f_Id);
            iCount = linq.Count();
            return linq.Skip((iPageIndex - 1) * iPageSize).Take(iPageSize).ToList();
        }

        /// <summary>
        /// 根据条件返回列表
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">每页多少数据</param>
        /// <param name="iCount">总数</param>
        /// <returns>返回t_ModifyRecord集合</returns>
        public List<t_ModifyRecord> GetModifyRecordByWhere(Func<t_ModifyRecord, bool> predicate, int iPageIndex, int iPageSize, out int iCount)
        {
            var linq = _repository.GetAll().Where(predicate).OrderByDescending(u => u.f_Id);
            iCount = linq.Count();
            return linq.Skip((iPageIndex - 1) * iPageSize).Take(iPageSize).ToList();
        }
        /// <summary>
        /// 恢复数据
        /// </summary>
        /// <param name="iId">id</param>
        public ErrorEnum Recovery(int iId)
        {
            ErrorEnum error = ErrorEnum.Success;
            t_ModifyRecord data = _repository.GetEntityById(iId);
            Newtonsoft.Json.Linq.JObject json;
            if (data.f_TableName == "工作交接")//t_WorkDistribution
            {
                WorkDistribution model = new WorkDistribution();
                json = Newtonsoft.Json.Linq.JObject.Parse(data.f_OldData);
                if (data.f_ActionStatus == 3)
                {
                    int f_Id = Convert.ToInt32(json["数据ID"]);
                    if (_workDistribution.GetAll().Where(p => p.f_Id == f_Id).FirstOrDefault() != null)
                    {
                        model = _workDistribution.GetAll().Where(p => p.f_Id == f_Id).FirstOrDefault();
                    }
                    else
                    {
                        return error = ErrorEnum.RecordDeleted;
                    }
                }
                model.f_RegisterDate = Convert.ToDateTime(json["登记时间"].ToString());
                model.f_Registered = json["登记人"].ToString();
                model.f_WorkType = Convert.ToInt32(json["工作类别ID"]);
                model.f_Handover = json["交接事项"].ToString();
                if (!string.IsNullOrWhiteSpace(json["待处理人ID"].ToString()))
                    model.f_Treat = Convert.ToInt32(json["待处理人ID"]);
                else
                    model.f_Treat = null;
                if (!string.IsNullOrWhiteSpace(json["紧急事项处理期限"].ToString()))
                {
                    model.f_UrgentDate = Convert.ToDateTime(json["紧急事项处理期限"]);
                }
                else
                {
                    model.f_UrgentDate = null;
                }
                if (!string.IsNullOrWhiteSpace(json["完成时间"].ToString()))
                {
                    model.f_CompleteDate = Convert.ToDateTime(json["完成时间"]);
                }
                else
                {
                    model.f_CompleteDate = null;
                }
                if (!string.IsNullOrWhiteSpace(json["备注"].ToString()))
                {
                    model.f_Remarks = json["备注"].ToString();
                }
                else
                {
                    model.f_Remarks = null;
                }
                if (data.f_ActionStatus == 2)
                {
                    _workDistribution.Insert(model);
                }
                else if (data.f_ActionStatus == 3)
                {
                    _workDistribution.Update(model);
                }
            }
            else if (data.f_TableName == "公告管理")//t_Notice
            {
                Notice model = new Notice();
                json = Newtonsoft.Json.Linq.JObject.Parse(data.f_OldData);
                if (data.f_ActionStatus == 3)
                {
                    int f_Id = Convert.ToInt32(json["数据ID"]);
                    if (_notice.GetAll().Where(p => p.f_Id == f_Id).FirstOrDefault() != null)
                    {
                        model = _notice.GetAll().Where(p => p.f_Id == f_Id).FirstOrDefault();
                    }
                    else
                    {
                        return error = ErrorEnum.RecordDeleted;
                    }
                }
                model.f_Type = json["公告类型"].ToString() == "紧急公告";
                model.f_Content = json["公告内容"].ToString();
                model.f_AddDate = Convert.ToDateTime(json["添加时间"]);
                model.f_Registrant = json["添加人"].ToString();
                model.f_StartDate = Convert.ToDateTime(json["有效开始时间"]);
                model.f_EndDate = Convert.ToDateTime(json["有效结束时间"]);
                if (data.f_ActionStatus == 2)
                {
                    _notice.Insert(model);
                }
                else if (data.f_ActionStatus == 3)
                {
                    _notice.Update(model);
                }
            }
            else if (data.f_TableName == "员工外租补助")//t_Grant
            {
                Grant model = new Grant(); 
                 json = Newtonsoft.Json.Linq.JObject.Parse(data.f_OldData);
                if (data.f_ActionStatus == 3)
                {
                    int f_GrantId = Convert.ToInt32(json["数据ID"]);
                    if (_grant.GetAll().Where(p => p.f_GrantId == f_GrantId).FirstOrDefault() != null)
                    {
                        model = _grant.GetAll().Where(p => p.f_GrantId == f_GrantId).FirstOrDefault();
                    }
                    else
                    {
                        return error = ErrorEnum.RecordDeleted;
                    }
                }
                model.f_eid = int.Parse(json["员工ID"].ToString());
                model.f_amount = Convert.ToDecimal(json["补助金额"].ToString());
                model.f_GrantDate = Convert.ToDateTime(json["補助月份"]);
                model.f_IsPayment = json["是否已发放补助"].ToString() == "√";
                if (!string.IsNullOrWhiteSpace(json["发放人"].ToString()))
                {
                    model.f_Payee = json["发放人"].ToString();
                }
                else
                {
                    model.f_Payee = null;
                }
                if (!string.IsNullOrWhiteSpace(json["操作人"].ToString()))
                {
                    model.f_operator = json["操作人"].ToString();
                }
                else
                {
                    model.f_operator = null;
                }
                if (data.f_ActionStatus == 2)
                {
                    _grant.Insert(model);
                }
                else if (data.f_ActionStatus == 3)
                {
                    _grant.Update(model);
                }
            }
            else if (data.f_TableName == "员工机票差额")//t_FlightFee
            {
                FlightFee model = new FlightFee();
                json = Newtonsoft.Json.Linq.JObject.Parse(data.f_OldData);
                if (data.f_ActionStatus == 3)
                {
                    int f_ID = Convert.ToInt32(json["数据ID"]);
                    if (_flightFee.GetAll().Where(p => p.f_ID == f_ID).FirstOrDefault() != null)
                    {
                        model = _flightFee.GetAll().Where(p => p.f_ID == f_ID).FirstOrDefault();
                    }
                    else
                    {
                        return error = ErrorEnum.RecordDeleted;
                    }
                }
                if (json["员工ID"] != null)
                {
                    if (!string.IsNullOrWhiteSpace(json["员工ID"].ToString()))
                    {
                        model.f_eid = int.Parse(json["员工ID"].ToString());
                    }
                }
                if (json["机票差额"] != null)
                {
                    if (!string.IsNullOrWhiteSpace(json["机票差额"].ToString()))
                    {
                        model.f_Amount = Convert.ToDecimal(json["机票差额"].ToString());
                    }
                }
                if (json["航班日期"] != null)
                {
                    if (!string.IsNullOrWhiteSpace(json["航班日期"].ToString()))
                    {
                        model.f_FlightDate = Convert.ToDateTime(json["航班日期"]);
                    }
                }
                if (json["最后操作时间"] != null)
                {
                    if (!string.IsNullOrWhiteSpace(json["最后操作时间"].ToString()))
                    {
                        model.f_operatorTime = Convert.ToDateTime(json["最后操作时间"]);
                    }
                }
                model.f_Flight = json["航班"].ToString();
                model.f_StartEndAdd = json["起止地点"].ToString();
                model.f_IsPay = json["缴费状态"].ToString() == "√";
                model.f_operator = json["操作人"].ToString();
                if (data.f_ActionStatus == 2)
                {
                    _flightFee.Insert(model);
                }
                else if (data.f_ActionStatus == 3)
                {
                    _flightFee.Update(model);
                }
            }
            else if (data.f_TableName == "员工租房")//t_EmpRent
            {
                EmpRent model = new EmpRent();
                json = Newtonsoft.Json.Linq.JObject.Parse(data.f_OldData);
                if (data.f_ActionStatus == 3)
                {
                    int f_ID = Convert.ToInt32(json["数据ID"]);
                    if (_empRent.GetAll().Where(p => p.f_Id == f_ID).FirstOrDefault() != null)
                    {
                        model = _empRent.GetAll().Where(p => p.f_Id == f_ID).FirstOrDefault();
                    }
                    else
                    {
                        return error = ErrorEnum.RecordDeleted;
                    }
                }
                model.f_eid = int.Parse(json["员工ID"].ToString());
                if (!string.IsNullOrWhiteSpace(json["宿舍ID"].ToString()))
                {
                    model.f_DormitoryId = Convert.ToInt32(json["宿舍ID"].ToString());
                }
                else
                {
                    model.f_DormitoryId = null;
                }
                if (json["租金"] != null)
                {
                    if (!string.IsNullOrWhiteSpace(json["租金"].ToString()))
                    {
                        model.f_Rent = Convert.ToDecimal(json["租金"].ToString());
                    }
                }
                if (json["補貼"] != null)
                {
                    if (!string.IsNullOrWhiteSpace(json["補貼"].ToString()))
                    {
                        model.f_Grant = Convert.ToDecimal(json["補貼"].ToString());
                    }
                }
                if (json["繳費月份"] != null)
                {
                    if (!string.IsNullOrWhiteSpace(json["繳費月份"].ToString()))
                    {
                        model.f_PaymentDate = Convert.ToDateTime(json["繳費月份"].ToString());
                    }
                }
                if (json["應繳費"] != null)
                {
                    if (!string.IsNullOrWhiteSpace(json["應繳費"].ToString()))
                    {
                        model.f_Amount = Convert.ToDecimal(json["應繳費"].ToString());
                    }
                }
                if (json["最后操作时间"] != null)
                {
                    if (!string.IsNullOrWhiteSpace(json["最后操作时间"].ToString()))
                    {
                        model.f_operatorTime = Convert.ToDateTime(json["最后操作时间"]);
                    }
                }
                model.f_IsPayment = json["是否缴费"].ToString() == "√";
                if (!string.IsNullOrWhiteSpace(json["收款人"].ToString()))
                {
                    model.f_Payee = json["收款人"].ToString();
                }
                else
                {
                    model.f_Payee = null;
                }
                if (json["生效日期"] != null)
                {
                    if (!string.IsNullOrWhiteSpace(json["生效日期"].ToString()))
                    {
                        model.f_EffectiveDate = Convert.ToDateTime(json["生效日期"].ToString());
                    }
                }
                if (!string.IsNullOrWhiteSpace(json["操作人"].ToString()))
                {
                    model.f_operator = json["操作人"].ToString();
                }
                else
                {
                    model.f_operator = null;
                }
                if (data.f_ActionStatus == 2)
                {
                    _empRent.Insert(model);
                }
                else if (data.f_ActionStatus == 3)
                {
                    _empRent.Update(model);
                }
            }
            else if (data.f_TableName == "员工管理")//t_employeeInfo
            {
                var periodValue = _SType.GetAll().Where(p => p.f_type == (int)sTypeEnum.班次类型).ToList();
                var deptValue = _SType.GetAll().Where(p => p.f_type == (int)sTypeEnum.部门类型).ToList();
                var workStatus = _SType.GetAll().Where(p => p.f_type == (int)sTypeEnum.上班状态类型).ToList();
                var workLocation = _SType.GetAll().Where(p => p.f_type == (int)sTypeEnum.上班地点类型).ToList();
                var dormitory = _SType.GetAll().Where(p => p.f_type == (int)sTypeEnum.社区类型).ToList();
                var level = _SType.GetAll().Where(p => p.f_type == (int)sTypeEnum.权限类型).ToList();
                var visaType = _SType.GetAll().Where(p => p.f_type == (int)sTypeEnum.签证类型).ToList();
                var Stage = _SType.GetAll().Where(p => p.f_type == (int)sTypeEnum.金流客服等级类型).ToList();
                var group = _SType.GetAll().Where(p => p.f_type == (int)sTypeEnum.组别类型).ToList();

                json = Newtonsoft.Json.Linq.JObject.Parse(data.f_OldData);
                int f_eid = int.Parse(json["员工ID"].ToString());
                Employee emp = _employee.GetEntityById(f_eid);
                if (data.f_ActionStatus == 3 && emp != null)
                {
                    #region 初始化员工信息
                    if (json["班次"] != null)
                    {
                        emp.f_periodType_tID = periodValue.Where(p => json["班次"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID;
                    }
                    if (json["部门"] != null)
                    {
                        emp.f_department_tID = deptValue.Where(p => json["部门"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID ?? 0;
                    }
                    if (json["中文名"] != null)
                    {
                        emp.f_chineseName = json["中文名"].ToString();
                    }
                    if (json["性别"] != null)
                    {
                        emp.f_sex = (byte)(json["性别"].ToString().Equals("男") ? 1 : 0);
                    }
                    if (json["护照ID"] != null)
                    {
                        emp.f_passportID = json["护照ID"].ToString();
                    }
                    if (json["护照名英文/越南文"] != null)
                    {
                        emp.f_passportName = json["护照名英文/越南文"].ToString();
                    }
                    if (json["存储护照照片URL"] != null)
                    {
                        emp.f_PassportURL = json["存储护照照片URL"].ToString();
                    }
                    if (json["昵称"] != null)
                    {
                        emp.f_nickname = json["昵称"].ToString();
                    }
                    if (json["国籍"] != null)
                    {
                        emp.f_international = json["国籍"].ToString();
                    }
                    if (json["QQ号码"] != null)
                    {
                        emp.f_QQNumber = json["QQ号码"].ToString();
                    }
                    if (json["菲律宾联系电话"] != null)
                    {
                        emp.f_TelNoPH = json["菲律宾联系电话"].ToString();
                    }
                    if (json["国内电话"] != null)
                    {
                        emp.f_TelNoCN = json["国内电话"].ToString();
                    }
                    if (json["紧急联系人"] != null)
                    {
                        emp.f_emergencyContactPerson = json["紧急联系人"].ToString();
                    }
                    if (json["紧急联系人电话"] != null)
                    {
                        emp.f_emergencyContactNumber = json["紧急联系人电话"].ToString();
                    }
                    if (json["微信"] != null)
                    {
                        emp.f_Wechat = json["微信"].ToString();
                    }
                    if (json["在职状态"] != null)
                    {
                        emp.f_workStatus_tID = workStatus.Where(p => json["在职状态"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID;
                    }
                    if (json["上班搭车时间"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["上班搭车时间"].ToString()))
                        {
                            DateTime ridewordTime;
                            if (DateTime.TryParse(json["上班搭车时间"].ToString(), out ridewordTime))
                            {
                                emp.f_rideWorkTime = ridewordTime;
                            }
                            else
                            {
                                emp.f_rideWorkTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + json["上班搭车时间"].ToString());
                            }
                        }
                    }
                    if (json["下班搭车时间"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["下班搭车时间"].ToString()))
                        {
                            emp.f_rideOffWorkTime = DateTime.Parse(json["下班搭车时间"].ToString());
                        }
                    }
                    if (json["工作地点"] != null)
                    {
                        emp.f_workLocation_tID = workLocation.Where(p => json["工作地点"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID;
                    }
                    if (json["宿舍表ID"] != null)
                    {
                        emp.f_dormitoryId = int.Parse(json["宿舍表ID"].ToString());
                    }
                    if (json["备注"] != null)
                    {
                        emp.f_Remark = json["备注"].ToString();
                    }
                    if (json["密码"] != null)
                    {
                        emp.f_pwd = json["密码"].ToString();
                    }
                    if (json["等级权限"] != null)
                    {
                        emp.f_level_tID = level.Where(p => json["等级权限"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID ?? 0;
                    }
                    if (json["新人表中的id"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["新人表中的id"].ToString()))
                        {
                            emp.f_nid = int.Parse(json["新人表中的id"].ToString());
                        }
                    }
                    if (json["是否为菲律宾员工"] != null)
                    {
                        emp.f_IsPHStaff = json["是否为菲律宾员工"].ToString().Equals("是") ? true : false;
                    }

                    if (json["薪资"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["薪资"].ToString()))
                        {
                            emp.f_Salary = decimal.Parse(json["薪资"].ToString());
                        }
                    }
                    if (json["OA帐号"] != null)
                    {
                        string accountName = json["OA帐号"].ToString();
                        if (_employee.GetAll().Where(p => p.f_AccountName.Equals(accountName)).FirstOrDefault() != null)
                        {
                            emp.f_AccountName = GetMaxAccountName(emp.f_department_tID);
                        }
                        else
                        {
                            emp.f_AccountName = json["OA帐号"].ToString();
                        }
                    }
                    if (json["创建时间"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["创建时间"].ToString()))
                        {
                            emp.f_CreateDate = DateTime.Parse(json["创建时间"].ToString());
                        }
                    }

                    if (json["签证类型"] != null)
                    {
                        emp.f_VisaType_tID = visaType.Where(p => json["签证类型"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID;
                    }
                    if (json["存储入境章照片URL"] != null)
                    {
                        emp.f_EntrystampURL = json["存储入境章照片URL"].ToString();
                    }
                    if (json["存储身份证照片URL"] != null)
                    {
                        emp.f_IDCardURL = json["存储身份证照片URL"].ToString();
                    }
                    if (json["组长组别"] != null)
                    {
                        emp.f_mutilpleGroup = json["组长组别"].ToString();
                    }
                    if (json["签证到期时间"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["签证到期时间"].ToString()))
                        {
                            emp.f_passportDate = DateTime.Parse(json["签证到期时间"].ToString());
                        }
                    }

                    if (json["入职日期"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["入职日期"].ToString()))
                        {
                            emp.f_HireDate = DateTime.Parse(json["入职日期"].ToString());
                        }
                    }
                    if (json["出生日期"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["出生日期"].ToString()))
                        {
                            emp.f_DateOfBirth = DateTime.Parse(json["出生日期"].ToString());
                        }
                    }
                    if (json["是否已移走"] != null)
                    {
                        emp.f_IsRemove = json["是否已移走"].ToString().Equals("是") ? true : false;
                    }

                    if (json["是否为新员工"] != null)
                    {
                        emp.f_IsNewEmp = json["是否为新员工"].ToString().Equals("是") ? true : false;
                    }
                    if (json["I卡或者I卡证明到期时间"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["I卡或者I卡证明到期时间"].ToString()))
                        {
                            emp.f_ICardDate = DateTime.Parse(json["I卡或者I卡证明到期时间"].ToString());
                        }
                    }
                    if (json["离职日期"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["离职日期"].ToString()))
                        {
                            emp.f_SeparationDate = DateTime.Parse(json["离职日期"].ToString());
                        }
                    }

                    if (json["旧的订餐信息"] != null)
                    {
                        emp.f_OldReservation = json["旧的订餐信息"].ToString();
                    }
                    if (json["金流客服等级"] != null)
                    {
                        emp.f_JLStage = Stage.Where(p => json["金流客服等级"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID ?? 0;
                    }
                    if (json["组别"] != null)
                    {
                        emp.f_JLgroup = group.Where(p => json["组别"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID ?? 0;
                    }
                    if (json["直属主管"] != null)
                    {
                        string director = json["直属主管"].ToString();
                        emp.f_JLdirector_eId = _employee.GetAll().Where(p => director.Equals(p.f_chineseName)).FirstOrDefault()?.f_eid ?? 0;
                    }
                    #endregion
                    _employee.Update(emp);
                }
                else if (data.f_ActionStatus == 2 && emp == null)
                {
                    #region 初始化员工信息
                    emp = new Employee();
                    if (json["班次"] != null)
                    {
                        emp.f_periodType_tID = periodValue.Where(p => json["班次"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID;
                    }
                    if (json["部门"] != null)
                    {
                        emp.f_department_tID = deptValue.Where(p => json["部门"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID ?? 0;
                    }
                    if (json["中文名"] != null)
                    {
                        emp.f_chineseName = json["中文名"].ToString();
                    }
                    if (json["性别"] != null)
                    {
                        emp.f_sex = (byte)(json["性别"].ToString().Equals("男") ? 1 : 0);
                    }
                    if (json["护照ID"] != null)
                    {
                        emp.f_passportID = json["护照ID"].ToString();
                    }
                    if (json["护照名英文/越南文"] != null)
                    {
                        emp.f_passportName = json["护照名英文/越南文"].ToString();
                    }
                    if (json["存储护照照片URL"] != null)
                    {
                        emp.f_PassportURL = json["存储护照照片URL"].ToString();
                    }
                    if (json["昵称"] != null)
                    {
                        emp.f_nickname = json["昵称"].ToString();
                    }
                    if (json["国籍"] != null)
                    {
                        emp.f_international = json["国籍"].ToString();
                    }
                    if (json["QQ号码"] != null)
                    {
                        emp.f_QQNumber = json["QQ号码"].ToString();
                    }
                    if (json["菲律宾联系电话"] != null)
                    {
                        emp.f_TelNoPH = json["菲律宾联系电话"].ToString();
                    }
                    if (json["国内电话"] != null)
                    {
                        emp.f_TelNoCN = json["国内电话"].ToString();
                    }
                    if (json["紧急联系人"] != null)
                    {
                        emp.f_emergencyContactPerson = json["紧急联系人"].ToString();
                    }
                    if (json["紧急联系人电话"] != null)
                    {
                        emp.f_emergencyContactNumber = json["紧急联系人电话"].ToString();
                    }
                    if (json["微信"] != null)
                    {
                        emp.f_Wechat = json["微信"].ToString();
                    }
                    if (json["在职状态"] != null)
                    {
                        emp.f_workStatus_tID = workStatus.Where(p => json["在职状态"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID;
                    }
                    if (json["上班搭车时间"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["上班搭车时间"].ToString()))
                        {
                            DateTime ridewordTime;
                            if (DateTime.TryParse(json["上班搭车时间"].ToString(), out ridewordTime))
                            {
                                emp.f_rideWorkTime = ridewordTime;
                            }
                            else
                            {
                                emp.f_rideWorkTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + json["上班搭车时间"].ToString());
                            }
                        }
                    }
                    if (json["下班搭车时间"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["下班搭车时间"].ToString()))
                        {
                            emp.f_rideOffWorkTime = DateTime.Parse(json["下班搭车时间"].ToString());
                        }
                    }
                    if (json["工作地点"] != null)
                    {
                        emp.f_workLocation_tID = workLocation.Where(p => json["工作地点"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID;
                    }
                    if (json["宿舍表ID"] != null)
                    {
                        emp.f_dormitoryId = int.Parse(json["宿舍表ID"].ToString());
                    }
                    if (json["备注"] != null)
                    {
                        emp.f_Remark = json["备注"].ToString();
                    }
                    if (json["密码"] != null)
                    {
                        emp.f_pwd = json["密码"].ToString();
                    }
                    if (json["等级权限"] != null)
                    {
                        emp.f_level_tID = level.Where(p => json["等级权限"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID ?? 0;
                    }
                    if (json["新人表中的id"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["新人表中的id"].ToString()))
                        {
                            emp.f_nid = int.Parse(json["新人表中的id"].ToString());
                        }
                    }
                    if (json["是否为菲律宾员工"] != null)
                    {
                        emp.f_IsPHStaff = json["是否为菲律宾员工"].ToString().Equals("是") ? true : false;
                    }

                    if (json["薪资"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["薪资"].ToString()))
                        {
                            emp.f_Salary = decimal.Parse(json["薪资"].ToString());
                        }
                    }
                    if (json["OA帐号"] != null)
                    {
                        string accountName = json["OA帐号"].ToString();
                        if (_employee.GetAll().Where(p => p.f_AccountName.Equals(accountName)).FirstOrDefault() != null)
                        {
                            emp.f_AccountName = GetMaxAccountName(emp.f_department_tID);
                        }
                        else
                        {
                            emp.f_AccountName = json["OA帐号"].ToString();
                        }
                    }
                    if (json["创建时间"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["创建时间"].ToString()))
                        {
                            emp.f_CreateDate = DateTime.Parse(json["创建时间"].ToString());
                        }
                    }

                    if (json["签证类型"] != null)
                    {
                        emp.f_VisaType_tID = visaType.Where(p => json["签证类型"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID;
                    }
                    if (json["存储入境章照片URL"] != null)
                    {
                        emp.f_EntrystampURL = json["存储入境章照片URL"].ToString();
                    }
                    if (json["存储身份证照片URL"] != null)
                    {
                        emp.f_IDCardURL = json["存储身份证照片URL"].ToString();
                    }
                    if (json["组长组别"] != null)
                    {
                        emp.f_mutilpleGroup = json["组长组别"].ToString();
                    }
                    if (json["签证到期时间"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["签证到期时间"].ToString()))
                        {
                            emp.f_passportDate = DateTime.Parse(json["签证到期时间"].ToString());
                        }
                    }

                    if (json["入职日期"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["入职日期"].ToString()))
                        {
                            emp.f_HireDate = DateTime.Parse(json["入职日期"].ToString());
                        }
                    }
                    if (json["出生日期"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["出生日期"].ToString()))
                        {
                            emp.f_DateOfBirth = DateTime.Parse(json["出生日期"].ToString());
                        }
                    }
                    if (json["是否已移走"] != null)
                    {
                        emp.f_IsRemove = json["是否已移走"].ToString().Equals("是") ? true : false;
                    }

                    if (json["是否为新员工"] != null)
                    {
                        emp.f_IsNewEmp = json["是否为新员工"].ToString().Equals("是") ? true : false;
                    }
                    if (json["I卡或者I卡证明到期时间"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["I卡或者I卡证明到期时间"].ToString()))
                        {
                            emp.f_ICardDate = DateTime.Parse(json["I卡或者I卡证明到期时间"].ToString());
                        }
                    }
                    if (json["离职日期"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["离职日期"].ToString()))
                        {
                            emp.f_SeparationDate = DateTime.Parse(json["离职日期"].ToString());
                        }
                    }

                    if (json["旧的订餐信息"] != null)
                    {
                        emp.f_OldReservation = json["旧的订餐信息"].ToString();
                    }
                    if (json["金流客服等级"] != null)
                    {
                        emp.f_JLStage = Stage.Where(p => json["金流客服等级"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID ?? 0;
                    }
                    if (json["组别"] != null)
                    {
                        emp.f_JLgroup = group.Where(p => json["组别"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID ?? 0;
                    }
                    if (json["直属主管"] != null)
                    {
                        string director = json["直属主管"].ToString();
                        emp.f_JLdirector_eId = _employee.GetAll().Where(p => director.Equals(p.f_chineseName)).FirstOrDefault()?.f_eid ?? 0;
                    }
                    #endregion 
                    _employee.Insert(emp);
                }
                else
                {
                    error = ErrorEnum.RecordDeleted;
                }
            }
            else if (data.f_TableName == "新人登记")//t_employeeInfo
            {
                var periodValue = _SType.GetAll().Where(p => p.f_type == (int)sTypeEnum.班次类型).ToList();
                var deptValue = _SType.GetAll().Where(p => p.f_type == (int)sTypeEnum.部门类型).ToList();
                var workStatus = _SType.GetAll().Where(p => p.f_type == (int)sTypeEnum.上班状态类型).ToList();
                var workLocation = _SType.GetAll().Where(p => p.f_type == (int)sTypeEnum.上班地点类型).ToList();
                var dormitory = _SType.GetAll().Where(p => p.f_type == (int)sTypeEnum.社区类型).ToList();
                var level = _SType.GetAll().Where(p => p.f_type == (int)sTypeEnum.权限类型).ToList();
                var visaType = _SType.GetAll().Where(p => p.f_type == (int)sTypeEnum.签证类型).ToList();
                var Stage = _SType.GetAll().Where(p => p.f_type == (int)sTypeEnum.金流客服等级类型).ToList();
                var group = _SType.GetAll().Where(p => p.f_type == (int)sTypeEnum.组别类型).ToList();
                var flight = _SType.GetAll().Where(p => p.f_type == (int)sTypeEnum.航班类型).ToList();
                json = Newtonsoft.Json.Linq.JObject.Parse(data.f_OldData);
                int f_eid = int.Parse(json["员工ID"].ToString());
                Employee emp = _employee.GetEntityById(f_eid);
                if (data.f_ActionStatus == 3 && emp != null)
                {
                    #region 初始化员工信息    
                    if (json["班次"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["班次"].ToString()))
                        {
                            emp.f_periodType_tID = periodValue.Where(p => json["班次"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID;
                        }
                    }
                    if (json["部门"] != null)
                    {
                        emp.f_department_tID = deptValue.Where(p => json["部门"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID ?? 0;
                    }
                    if (json["中文名"] != null)
                    {
                        emp.f_chineseName = json["中文名"].ToString();
                    }
                    if (json["性别"] != null)
                    {
                        emp.f_sex = (byte)(json["性别"].ToString().Equals("男") ? 1 : 0);
                    }
                    if (json["护照ID"] != null)
                    {
                        emp.f_passportID = json["护照ID"].ToString();
                    }
                    if (json["护照名英文/越南文"] != null)
                    {
                        emp.f_passportName = json["护照名英文/越南文"].ToString();
                    }
                    if (json["存储护照照片URL"] != null)
                    {
                        emp.f_PassportURL = json["存储护照照片URL"].ToString();
                    }
                    if (json["昵称"] != null)
                    {
                        emp.f_nickname = json["昵称"].ToString();
                    }
                    if (json["国籍"] != null)
                    {
                        emp.f_international = json["国籍"].ToString();
                    }
                    if (json["QQ号码"] != null)
                    {
                        emp.f_QQNumber = json["QQ号码"].ToString();
                    }
                    if (json["菲律宾联系电话"] != null)
                    {
                        emp.f_TelNoPH = json["菲律宾联系电话"].ToString();
                    }
                    if (json["国内电话"] != null)
                    {
                        emp.f_TelNoCN = json["国内电话"].ToString();
                    }
                    if (json["紧急联系人"] != null)
                    {
                        emp.f_emergencyContactPerson = json["紧急联系人"].ToString();
                    }
                    if (json["紧急联系人电话"] != null)
                    {
                        emp.f_emergencyContactNumber = json["紧急联系人电话"].ToString();
                    }
                    if (json["微信"] != null)
                    {
                        emp.f_Wechat = json["微信"].ToString();
                    }
                    if (json["在职状态"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["在职状态"].ToString()))
                        {
                            emp.f_workStatus_tID = workStatus.Where(p => json["在职状态"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID;
                        }
                    }
                    if (json["上班搭车时间"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["上班搭车时间"].ToString()))
                        {
                            DateTime ridewordTime;
                            if (DateTime.TryParse(json["上班搭车时间"].ToString(), out ridewordTime))
                            {
                                emp.f_rideWorkTime = ridewordTime;
                            }
                            else
                            {
                                emp.f_rideWorkTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + json["上班搭车时间"].ToString());
                            }
                        }
                    }
                    if (json["下班搭车时间"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["下班搭车时间"].ToString()))
                        {
                            emp.f_rideOffWorkTime = DateTime.Parse(json["下班搭车时间"].ToString());
                        }
                    }
                    if (json["工作地点"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["工作地点"].ToString()))
                        {
                            emp.f_workLocation_tID = workLocation.Where(p => json["工作地点"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID;
                        }
                    }
                    if (json["宿舍表ID"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["宿舍表ID"].ToString()))
                        {
                            emp.f_dormitoryId = int.Parse(json["宿舍表ID"].ToString());
                        }
                    }
                    if (json["备注"] != null)
                    {
                        emp.f_Remark = json["备注"].ToString();
                    }
                    if (json["密码"] != null)
                    {
                        emp.f_pwd = json["密码"].ToString();
                    }
                    if (json["等级权限"] != null)
                    {
                        emp.f_level_tID = level.Where(p => json["等级权限"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID ?? 0;
                    }
                    if (json["新人表中的id"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["新人表中的id"].ToString()))
                        {
                            emp.f_nid = int.Parse(json["新人表中的id"].ToString());
                        }
                    }
                    if (json["是否为菲律宾员工"] != null)
                    {
                        emp.f_IsPHStaff = json["是否为菲律宾员工"].ToString().Equals("是") ? true : false;
                    }

                    if (json["薪资"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["薪资"].ToString()))
                        {
                            emp.f_Salary = decimal.Parse(json["薪资"].ToString());
                        }
                    }
                    if (json["OA帐号"] != null)
                    {
                        emp.f_AccountName = GetMaxAccountName(emp.f_department_tID);
                    }
                    if (json["创建时间"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["创建时间"].ToString()))
                        {
                            emp.f_CreateDate = DateTime.Parse(json["创建时间"].ToString());
                        }
                    }

                    if (json["签证类型"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["签证类型"].ToString()))
                        {
                            emp.f_VisaType_tID = visaType.Where(p => json["签证类型"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID;
                        }
                    }
                    if (json["存储入境章照片URL"] != null)
                    {
                        emp.f_EntrystampURL = json["存储入境章照片URL"].ToString();
                    }
                    if (json["存储身份证照片URL"] != null)
                    {
                        emp.f_IDCardURL = json["存储身份证照片URL"].ToString();
                    }
                    if (json["组长组别"] != null)
                    {
                        emp.f_mutilpleGroup = json["组长组别"].ToString();
                    }
                    if (json["签证到期时间"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["签证到期时间"].ToString()))
                        {
                            emp.f_passportDate = DateTime.Parse(json["签证到期时间"].ToString());
                        }
                    }

                    if (json["入职日期"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["入职日期"].ToString()))
                        {
                            emp.f_HireDate = DateTime.Parse(json["入职日期"].ToString());
                        }
                    }
                    if (json["出生日期"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["出生日期"].ToString()))
                        {
                            emp.f_DateOfBirth = DateTime.Parse(json["出生日期"].ToString());
                        }
                    }
                    if (json["是否已移走"] != null)
                    {
                        emp.f_IsRemove = json["是否已移走"].ToString().Equals("是") ? true : false;
                    }

                    if (json["是否为新员工"] != null)
                    {
                        emp.f_IsNewEmp = json["是否为新员工"].ToString().Equals("是") ? true : false;
                    }
                    if (json["I卡或者I卡证明到期时间"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["I卡或者I卡证明到期时间"].ToString()))
                        {
                            emp.f_ICardDate = DateTime.Parse(json["I卡或者I卡证明到期时间"].ToString());
                        }
                    }
                    if (json["离职日期"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["离职日期"].ToString()))
                        {
                            emp.f_SeparationDate = DateTime.Parse(json["离职日期"].ToString());
                        }
                    }

                    if (json["旧的订餐信息"] != null)
                    {
                        emp.f_OldReservation = json["旧的订餐信息"].ToString();
                    }
                    if (json["金流客服等级"] != null)
                    {
                        emp.f_JLStage = Stage.Where(p => json["金流客服等级"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID ?? 0;
                    }
                    if (json["组别"] != null)
                    {
                        emp.f_JLgroup = group.Where(p => json["组别"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID ?? 0;
                    }
                    if (json["直属主管"] != null)
                    {
                        string director = json["直属主管"].ToString();
                        emp.f_JLdirector_eId = _employee.GetAll().Where(p => director.Equals(p.f_chineseName)).FirstOrDefault()?.f_eid ?? 0;
                    }

                    NewEmployee nEmp = _newEmployee.GetEntityById(emp.f_nid ?? 0);
                    if (nEmp == null)
                    {
                        return error = ErrorEnum.RecordDeleted;
                    }
                    if (json["航班类型"] != null)
                    {
                        nEmp.f_flightTypt_tID = flight.Where(p => json["航班类型"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID;
                    }
                    if (json["航班起飞时间"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["航班起飞时间"].ToString()))
                        {
                            nEmp.f_flightDate = DateTime.Parse(json["航班起飞时间"].ToString());
                        }
                    }
                    if (json["航班抵达时间"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["航班抵达时间"].ToString()))
                        {
                            nEmp.f_flightArrivalTime = DateTime.Parse(json["航班抵达时间"].ToString());
                        }
                    }
                    if (json["借款金额"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["借款金额"].ToString()))
                        {
                            nEmp.f_LoanAmount = int.Parse(json["借款金额"].ToString());
                        }
                    }
                    if (json["储值卡"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["储值卡"].ToString()))
                        {
                            nEmp.f_GiftCard = int.Parse(json["储值卡"].ToString());
                        }
                    }
                    if (json["电话卡"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["电话卡"].ToString()))
                        {
                            nEmp.f_TelCatd = int.Parse(json["电话卡"].ToString());
                        }
                    }
                    if (json["是否确认"] != null)
                    {
                        nEmp.f_signature = json["是否确认"].ToString().Equals("已确认") ? true : false;
                    }
                    if (json["新人备注"] != null)
                    {
                        nEmp.f_Remark = json["新人备注"].ToString();
                    }
                    #endregion
                    _employee.Update(emp);
                    _newEmployee.Update(nEmp);
                }
                else if (data.f_ActionStatus == 2 && emp == null)
                {
                    #region 初始化员工信息
                    emp = new Employee();
                    if (json["班次"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["班次"].ToString()))
                        {
                            emp.f_periodType_tID = periodValue.Where(p => json["班次"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID;
                        }
                    }
                    if (json["部门"] != null)
                    {
                        emp.f_department_tID = deptValue.Where(p => json["部门"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID ?? 0;
                    }
                    if (json["中文名"] != null)
                    {
                        emp.f_chineseName = json["中文名"].ToString();
                    }
                    if (json["性别"] != null)
                    {
                        emp.f_sex = (byte)(json["性别"].ToString().Equals("男") ? 1 : 0);
                    }
                    if (json["护照ID"] != null)
                    {
                        emp.f_passportID = json["护照ID"].ToString();
                    }
                    if (json["护照名英文/越南文"] != null)
                    {
                        emp.f_passportName = json["护照名英文/越南文"].ToString();
                    }
                    if (json["存储护照照片URL"] != null)
                    {
                        emp.f_PassportURL = json["存储护照照片URL"].ToString();
                    }
                    if (json["昵称"] != null)
                    {
                        emp.f_nickname = json["昵称"].ToString();
                    }
                    if (json["国籍"] != null)
                    {
                        emp.f_international = json["国籍"].ToString();
                    }
                    if (json["QQ号码"] != null)
                    {
                        emp.f_QQNumber = json["QQ号码"].ToString();
                    }
                    if (json["菲律宾联系电话"] != null)
                    {
                        emp.f_TelNoPH = json["菲律宾联系电话"].ToString();
                    }
                    if (json["国内电话"] != null)
                    {
                        emp.f_TelNoCN = json["国内电话"].ToString();
                    }
                    if (json["紧急联系人"] != null)
                    {
                        emp.f_emergencyContactPerson = json["紧急联系人"].ToString();
                    }
                    if (json["紧急联系人电话"] != null)
                    {
                        emp.f_emergencyContactNumber = json["紧急联系人电话"].ToString();
                    }
                    if (json["微信"] != null)
                    {
                        emp.f_Wechat = json["微信"].ToString();
                    }
                    if (json["在职状态"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["在职状态"].ToString()))
                        {
                            emp.f_workStatus_tID = workStatus.Where(p => json["在职状态"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID;
                        }
                    }
                    if (json["上班搭车时间"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["上班搭车时间"].ToString()))
                        {
                            DateTime ridewordTime;
                            if (DateTime.TryParse(json["上班搭车时间"].ToString(), out ridewordTime))
                            {
                                emp.f_rideWorkTime = ridewordTime;
                            }
                            else
                            {
                                emp.f_rideWorkTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + json["上班搭车时间"].ToString());
                            }
                        }
                    }
                    if (json["下班搭车时间"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["下班搭车时间"].ToString()))
                        {
                            emp.f_rideOffWorkTime = DateTime.Parse(json["下班搭车时间"].ToString());
                        }
                    }
                    if (json["工作地点"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["工作地点"].ToString()))
                        {
                            emp.f_workLocation_tID = workLocation.Where(p => json["工作地点"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID;
                        }
                    }
                    if (json["宿舍表ID"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["宿舍表ID"].ToString()))
                        {
                            emp.f_dormitoryId = int.Parse(json["宿舍表ID"].ToString());
                        }
                    }
                    if (json["备注"] != null)
                    {
                        emp.f_Remark = json["备注"].ToString();
                    }
                    if (json["密码"] != null)
                    {
                        emp.f_pwd = json["密码"].ToString();
                    }
                    if (json["等级权限"] != null)
                    {
                        emp.f_level_tID = level.Where(p => json["等级权限"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID ?? 0;
                    }
                    //if (json["新人表中的id"] != null)
                    //{
                    //    if (!string.IsNullOrWhiteSpace(json["新人表中的id"].ToString()))
                    //    {
                    //        emp.f_nid = int.Parse(json["新人表中的id"].ToString());
                    //    }
                    //}
                    if (json["是否为菲律宾员工"] != null)
                    {
                        emp.f_IsPHStaff = json["是否为菲律宾员工"].ToString().Equals("是") ? true : false;
                    }

                    if (json["薪资"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["薪资"].ToString()))
                        {
                            emp.f_Salary = decimal.Parse(json["薪资"].ToString());
                        }
                    }
                    if (json["OA帐号"] != null)
                    {
                        emp.f_AccountName = GetMaxAccountName(emp.f_department_tID);
                    }
                    if (json["创建时间"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["创建时间"].ToString()))
                        {
                            emp.f_CreateDate = DateTime.Parse(json["创建时间"].ToString());
                        }
                    }

                    if (json["签证类型"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["签证类型"].ToString()))
                        {
                            emp.f_VisaType_tID = visaType.Where(p => json["签证类型"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID;
                        }
                    }
                    if (json["存储入境章照片URL"] != null)
                    {
                        emp.f_EntrystampURL = json["存储入境章照片URL"].ToString();
                    }
                    if (json["存储身份证照片URL"] != null)
                    {
                        emp.f_IDCardURL = json["存储身份证照片URL"].ToString();
                    }
                    if (json["组长组别"] != null)
                    {
                        emp.f_mutilpleGroup = json["组长组别"].ToString();
                    }
                    if (json["签证到期时间"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["签证到期时间"].ToString()))
                        {
                            emp.f_passportDate = DateTime.Parse(json["签证到期时间"].ToString());
                        }
                    }

                    if (json["入职日期"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["入职日期"].ToString()))
                        {
                            emp.f_HireDate = DateTime.Parse(json["入职日期"].ToString());
                        }
                    }
                    if (json["出生日期"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["出生日期"].ToString()))
                        {
                            emp.f_DateOfBirth = DateTime.Parse(json["出生日期"].ToString());
                        }
                    }
                    if (json["是否已移走"] != null)
                    {
                        emp.f_IsRemove = json["是否已移走"].ToString().Equals("是") ? true : false;
                    }

                    if (json["是否为新员工"] != null)
                    {
                        emp.f_IsNewEmp = json["是否为新员工"].ToString().Equals("是") ? true : false;
                    }
                    if (json["I卡或者I卡证明到期时间"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["I卡或者I卡证明到期时间"].ToString()))
                        {
                            emp.f_ICardDate = DateTime.Parse(json["I卡或者I卡证明到期时间"].ToString());
                        }
                    }
                    if (json["离职日期"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["离职日期"].ToString()))
                        {
                            emp.f_SeparationDate = DateTime.Parse(json["离职日期"].ToString());
                        }
                    }

                    if (json["旧的订餐信息"] != null)
                    {
                        emp.f_OldReservation = json["旧的订餐信息"].ToString();
                    }
                    if (json["金流客服等级"] != null)
                    {
                        emp.f_JLStage = Stage.Where(p => json["金流客服等级"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID ?? 0;
                    }
                    if (json["组别"] != null)
                    {
                        emp.f_JLgroup = group.Where(p => json["组别"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID ?? 0;
                    }
                    if (json["直属主管"] != null)
                    {
                        string director = json["直属主管"].ToString();
                        emp.f_JLdirector_eId = _employee.GetAll().Where(p => director.Equals(p.f_chineseName)).FirstOrDefault()?.f_eid ?? 0;
                    }

                    NewEmployee nEmp = new NewEmployee();
                    if (json["航班类型"] != null)
                    {
                        nEmp.f_flightTypt_tID = flight.Where(p => json["航班类型"].ToString().Equals(p.f_value)).FirstOrDefault()?.f_tID;
                    }
                    if (json["航班起飞时间"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["航班起飞时间"].ToString()))
                        {
                            nEmp.f_flightDate = DateTime.Parse(json["航班起飞时间"].ToString());
                        }
                    }
                    if (json["航班抵达时间"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["航班抵达时间"].ToString()))
                        {
                            nEmp.f_flightArrivalTime = DateTime.Parse(json["航班抵达时间"].ToString());
                        }
                    }
                    if (json["借款金额"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["借款金额"].ToString()))
                        {
                            nEmp.f_LoanAmount = int.Parse(json["借款金额"].ToString());
                        }
                    }
                    if (json["储值卡"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["储值卡"].ToString()))
                        {
                            nEmp.f_GiftCard = int.Parse(json["储值卡"].ToString());
                        }
                    }
                    if (json["电话卡"] != null)
                    {
                        if (!string.IsNullOrWhiteSpace(json["电话卡"].ToString()))
                        {
                            nEmp.f_TelCatd = int.Parse(json["电话卡"].ToString());
                        }
                    }
                    if (json["是否确认"] != null)
                    {
                        nEmp.f_signature = json["是否确认"].ToString().Equals("已确认") ? true : false;
                    }
                    if (json["新人备注"] != null)
                    {
                        nEmp.f_Remark = json["新人备注"].ToString();
                    }
                    #endregion 
                    _newEmployee.Insert(nEmp);
                    emp.f_nid = nEmp.f_nID;
                    _employee.Insert(emp);
                }
                else
                {
                    error = ErrorEnum.RecordDeleted;
                }
            }
            //最后判断数据是否初始化成功、提交
            if (error == ErrorEnum.Success)
            {
                data.f_IsRecovered = true;
                _repository.Update(data);
            }
            return error;
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
            string sAccoun = _employee.GetAll().Where(u => u.f_AccountName.Contains(sJianXie)).Max(u => u.f_AccountName);
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
    }
}
