using System;
using System.Collections.Generic;
using System.Linq;
using MI.Application.Dto;
using MI.Data;
using MI.Core.Domain;
using AutoMapper;
using MI.Core.Common;
using Newtonsoft.Json.Linq;
using System.Data.Entity.Validation;
using System.Data.Entity;
using MI.Application.OASession;
using System.Linq.Expressions;

namespace MI.Application
{
    public class ReturnTicketService : BaseService<ReturnTicket>, IReturnTicketService
    {
        /// <summary>
        /// 机票登记
        /// </summary>
        private readonly IBaseRepository<ReturnTicket> _repository;
        //类型服务
        private readonly ISTypeService _sTypeService;
        //登记查询服务
        private readonly IRegistTipService _registTipService;
        //操作记录服务
        private readonly IModifyRecordService _modifyRecordService;
        /// <summary>
        /// 人员表
        /// </summary>
        private readonly IBaseRepository<Employee> _Employee;
        /// <summary>
        /// 类型表
        /// </summary>
        private readonly IBaseRepository<SType> _SType;
        private readonly ISession _session;
        private readonly IBaseRepository<Employee> _ELS;


        public ReturnTicketService(
            IBaseRepository<ReturnTicket> repository,
            ISTypeService sTypeService,
            IRegistTipService registTipService,
            IModifyRecordService modifyRecordService,
            IBaseRepository<Employee> ELS,
            IBaseRepository<Employee> employee,
            IBaseRepository<SType> stype,
            ISession session) :
            base(repository)
        {
            _repository = repository;
            _sTypeService = sTypeService;
            _registTipService = registTipService;
            _modifyRecordService = modifyRecordService;
            _session = session;
            _ELS = ELS;
            _Employee = employee;
            _SType = stype;
        }
        /// <summary>
        /// 根据条件查询送机派车所有数据
        /// </summary>
        /// <param name="sc">查询条件</param>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">页大小</param>
        /// <param name="o_Count">总数</param>
        /// <returns></returns>
        public IList<ReturnTicketDto> GetSendCarByWhere(Expression<Func<ReturnTicket, bool>> predicate, int iPageIndex, int iPageSize, out int o_Count)
        {
            var returnTicket = _repository.GetAll().Where(predicate).OrderBy(m => m.f_ToDate)
                               .ThenBy(m => m.f_ToFlight)
                               .ThenBy(m => m.f_ToTerminal).ToList();
            var lstResult = returnTicket.OrderByDescending(u => u.f_Id).Skip((iPageIndex - 1) * iPageSize).Take(iPageSize);
            IList<ReturnTicketDto> dtoReturnTicket = Mapper.Map<IList<ReturnTicketDto>>(lstResult);
            o_Count = returnTicket.Count();
            return dtoReturnTicket;
        }

        /// <summary>
        /// 根据条件查询接机派车所有数据
        /// </summary>
        /// <param name="sc">查询条件</param>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">页大小</param>
        /// <param name="o_Count">总数</param>
        /// <returns></returns>
        public IList<ReturnTicketDto> GetPickupByWhere(Expression<Func<ReturnTicket, bool>> predicate, int iPageIndex, int iPageSize, out int o_Count)
        {
            var returnTicket = _repository.GetAll().Where(predicate).OrderBy(m => m.f_FromDate)
                               .ThenBy(m => m.f_FromFlight)
                               .ThenBy(m => m.f_FromTerminal).ToList();
            var lstResult = returnTicket.OrderByDescending(u => u.f_Id).Skip((iPageIndex - 1) * iPageSize).Take(iPageSize);
            IList<ReturnTicketDto> dtoReturnTicket = Mapper.Map<IList<ReturnTicketDto>>(lstResult);
            o_Count = returnTicket.Count();
            return dtoReturnTicket;

        }

        /// <summary>
        /// 根据条件导出送机派车数据
        /// </summary>
        /// <param name="sc">查询条件</param>
        /// <returns></returns>
        public List<dynamic> GetSendCarByWhereExport(Expression<Func<ReturnTicket, bool>> predicate)
        {
            var lstNation = _sTypeService.GetsTypeByWhere((int)sTypeEnum.国籍);
            var lstFlight = _sTypeService.GetsTypeByWhere((int)sTypeEnum.航班类型);
            var lstDepartment = _sTypeService.GetsTypeByWhere((int)sTypeEnum.部门类型);
            var returnTicket = _repository.GetAll().Where(predicate).ToList();
            IEnumerable<dynamic> lstResult = from p in returnTicket
                                             select new
                                             {
                                                 序號NO = p.f_ToCode == null ? "" : "A-" + p.f_ToCode.ToString(),
                                                 部門 = lstDepartment.FirstOrDefault(u => u.f_tID == (p.t_employeeInfo?.f_department_tID ?? 0))?.f_value ?? "",
                                                 姓名Name = p.t_employeeInfo?.f_chineseName ?? "",
                                                 性别Sex = p.t_employeeInfo?.f_sex == 1 ? "M" : "F",
                                                 國籍 = p.t_employeeInfo?.f_international ?? "",
                                                 昵稱 = p.t_employeeInfo?.f_nickname ?? "New Employee",
                                                 社區Community = p.t_employeeInfo?.Dormitory?.f_Community,
                                                 樓棟Building = p.t_employeeInfo?.Dormitory?.f_Building,
                                                 房號Room = p.t_employeeInfo?.Dormitory?.f_RoomNo,
                                                 日期Date = p.f_ToDate?.ToString("yyyy-MM-dd") ?? "",
                                                 航空Flight = lstFlight.FirstOrDefault(u => u.f_tID == p.f_ToAirlineType_Id)?.f_value ?? "",
                                                 航廈Terminal = lstFlight.FirstOrDefault(u => u.f_tID == p.f_ToAirlineType_Id)?.f_Remark ?? "",
                                                 班機時間Time = p.f_ToFlight,
                                                 地點 = p.f_ToAddress,
                                                 出發時間Departure = p.f_ToDropOffTime ?? "",
                                                 国内号码 = p.t_employeeInfo?.f_TelNoCN ?? "",
                                                 菲律宾号码 = p.t_employeeInfo?.f_TelNoPH ?? "",
                                                 備註Remark = p.f_ToRemark ?? "",
                                                 是否派车 = p.f_ToIsSendACar ? "√" : "×"
                                             };
            List<dynamic> list = lstResult.ToList();
            return list;
        }

        /// <summary>
        /// 根据条件导出送机派车数据
        /// </summary>
        /// <param name="sc">查询条件</param>
        /// <returns></returns>
        public List<dynamic> GetPickupByWhereExport(Expression<Func<ReturnTicket, bool>> predicate)
        {
            var lstNation = _sTypeService.GetsTypeByWhere((int)sTypeEnum.国籍);
            var lstFlight = _sTypeService.GetsTypeByWhere((int)sTypeEnum.航班类型);
            var lstDepartment = _sTypeService.GetsTypeByWhere((int)sTypeEnum.部门类型);
            var returnTicket = _repository.GetAll().Where(predicate).ToList();
            IEnumerable<dynamic> lstResult = from p in returnTicket
                                             select new
                                             {
                                                 序號NO = p.f_FromCode == null ? "" : "A-" + p.f_FromCode.ToString(),
                                                 部門 = lstDepartment.FirstOrDefault(u => u.f_tID == (p.t_employeeInfo?.f_department_tID ?? 0))?.f_value ?? "",
                                                 姓名Name = p.t_employeeInfo?.f_chineseName ?? "",
                                                 性别Sex = p.t_employeeInfo?.f_sex == 1 ? "M" : "F",
                                                 國籍 = p.t_employeeInfo?.f_international ?? "",
                                                 昵稱 = p.t_employeeInfo?.f_nickname ?? "New Employee",
                                                 社區Community = p.t_employeeInfo?.Dormitory?.f_Community,
                                                 樓棟Building = p.t_employeeInfo?.Dormitory?.f_Building,
                                                 房號Room = p.t_employeeInfo?.Dormitory?.f_RoomNo,
                                                 日期Date = p.f_FromDate?.ToString("yyyy-MM-dd") ?? "",
                                                 航空Flight = lstFlight.FirstOrDefault(u => u.f_tID == p.f_FromAirlineType_Id)?.f_value ?? "",
                                                 航廈Terminal = lstFlight.FirstOrDefault(u => u.f_tID == p.f_FromAirlineType_Id)?.f_Remark ?? "",
                                                 班機時間Time = p.f_FromFlight,
                                                 地點 = p.f_FromAddress,
                                                 国内号码 = p.t_employeeInfo?.f_TelNoCN ?? "",
                                                 菲律宾号码 = p.t_employeeInfo?.f_TelNoPH ?? "",
                                                 備註Remark = p.f_FromRemark ?? "",
                                                 是否派车 = p.f_FromIsSendACar ? "√" : "×"
                                             };
            List<dynamic> list = lstResult.ToList();
            return list;
        }
        /// <summary>
        /// 根据id获取一条数据
        /// </summary>
        /// <param name="iId">传过来的参数</param>
        /// <returns></returns>
        public ReturnTicket GetReturnTicketById(int iId)
        {
            return _repository.GetEntityById(iId);
        }

        /// <summary>
        /// 返回送机的当前时间段最大编号
        /// </summary>
        /// <param name="dtStartDate">开始时间</param>
        /// <param name="dtEndDate">结束时间</param>
        /// <returns></returns>
        public int GetSendMaxToCode(DateTime dtStartDate, DateTime dtEndDate)
        {
            var linq = _repository.GetAll().Where(
                    u => (u.f_ToDate != null) && (u.f_ToDate >= dtStartDate) && (u.f_ToDate <= dtEndDate)).Max(u => u.f_ToCode);
            return linq ?? 0;
        }

        /// <summary>
        /// 返回送机的当前时间段最大编号
        /// </summary>
        /// <param name="dtStartDate">开始时间</param>
        /// <param name="dtEndDate">结束时间</param>
        /// <returns></returns>
        public int GetPickupMaxToCode(DateTime dtStartDate, DateTime dtEndDate)
        {
            var linq = _repository.GetAll().Where(
                    u => (u.f_FromDate != null) && (u.f_FromDate >= dtStartDate) && (u.f_FromDate <= dtEndDate)).Max(u => u.f_FromCode);
            return linq ?? 0;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="oModel">新model实体</param>
        /// <param name="oldModel">原来model实体</param>
        /// <param name="bIsLogs"></param>
        /// <param name="isSendCar">是否需要写入操作记录</param>
        public void EditReturnTicketOneData(ReturnTicket oModel, ReturnTicket oldModel = null, bool bIsLogs = true, bool isSendCar = false)
        {
            ReturnTicket oOldData = GetReturnTicketById(oModel.f_Id);
            //处理登记查询提示   改变t_RegistTip(登记查询提示表)f_OperatorTime时间  根据登记查询表中文名和员工ID
            DoRegistTip(oModel, oModel.f_eId, "t_ReturnTicket", "机票查询");
            if (!string.IsNullOrWhiteSpace(oModel.f_ToDate.ToString()))
            {
                DoRegistTip(oModel, oModel.f_eId, "t_ReturnTicket", "送机查询");
            }
            if (!string.IsNullOrWhiteSpace(oModel.f_FromDate.ToString()))
            {
                DoRegistTip(oModel, oModel.f_eId, "t_ReturnTicket", "接机查询");
            }
            AddModifyRecord(GetSendCarJObjectData(oldModel), GetSendCarJObjectData(oModel), ActionItem.Update, CategoryItem.SendACar, "派车安排");
        }
        /// <summary>
        /// 将实体转化成json数据，新人数据
        /// </summary>
        /// <param name="oModel">员工实体</param>
        /// <returns>json数据</returns>
        public string GetJObjectData(ReturnTicket oModel)
        {
            return new JObject
            {
                {"员工姓名", _ELS.GetEntityById(oModel.f_eId??0)?.f_chineseName??""},
                {"返乡日期", oModel.f_ToDate?.ToString("yyyy-MM-dd")??""},
                {"返乡航空公司", _sTypeService.GetsTypeById(oModel.f_ToAirlineType_Id??0)?.f_value??""},
                {"返乡航班时间", oModel.f_ToFlight??""},
                {"返乡备注", oModel.f_ToRemark??""},
                {"返菲日期", oModel.f_FromDate?.ToString("yyyy-MM-dd")??""},
                {"返菲航空公司", _sTypeService.GetsTypeById(oModel.f_FromAirlineType_Id??0)?.f_value??""},
                {"返菲航班时间", oModel.f_FromFlight??""},
                {"返菲备注", oModel.f_FromRemark??""}
            }.ToString();
        }

        /// <summary>
        /// 往登记查询提示表写操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">实体</param>
        /// <param name="eId">员工Id</param>
        /// <param name="strTable">登记查询表名</param>
        /// <param name="strTableName">登记查询表中文名</param>
        public virtual void DoRegistTip<T>(T model, int? eId, string strTable, string strTableName)
        {
            if (eId != null)
            {
                RegistTip regmodel = _registTipService.GetRegistTipByCondition(eId.Value, strTableName);
                if (regmodel == null)
                {
                    regmodel = new RegistTip
                    {
                        f_eId = eId.Value,
                        f_Table = strTable,
                        f_TableName = strTableName,
                        f_TipState = 1,
                        f_OperatorTime = DateTime.Now
                    };
                    _registTipService.AddRegistTipData(regmodel);
                }
                else
                {
                    regmodel = new RegistTip
                    {
                        f_eId = eId.Value,
                        f_Table = strTable,
                        f_TableName = strTableName,
                        f_TipState = 1,
                        f_OperatorTime = DateTime.Now
                    };
                    _registTipService.EditRegistTipData(regmodel);
                }
            }
        }

        /// <summary>
        /// 添加一条操作记录数据
        /// </summary>
        /// <param name="oldEntity">旧数据</param>
        /// <param name="newEntity">新数据</param>
        /// <param name="action">操作类型（修改，新增，删除）</param>
        /// <param name="category">修改项目</param>
        /// <param name="Name">操作者</param>
        /// <param name="tableName">操作的表名</param>
        /// <returns></returns>
        public void AddModifyRecord(string oldEntity, string newEntity, ActionItem action, CategoryItem category, string tableName)
        {
            ModifyRecord record = new ModifyRecord
            {
                f_ActionStatus = (int)action,
                f_ItemCategory = (int)category,
                f_ByIP = AOUnity.GetClientIp(),
                f_ByWho = _session.GetCurrUser().NickName,//获得昵称
                f_NewData = newEntity,
                f_OldData = oldEntity,
                f_ChangeTime = DateTime.Now,
                f_TableName = tableName
            };
            _modifyRecordService.AddModifyRecordData(record);
        }

        /// <summary>
        /// 将实体转化成json数据，派车安排
        /// </summary>
        /// <param name="oModel">实体</param>
        /// <returns>json数据</returns>
        public string GetSendCarJObjectData(ReturnTicket oModel)
        {
            if (oModel != null)
            {
                return new JObject
            {
                {"员工姓名", oModel.t_employeeInfo?.f_chineseName??""},
                {"送机已派车", oModel.f_ToIsSendACar ? "已派车":""},
                {"送机序號", oModel.f_ToCode == null ? "":"A-" + oModel.f_ToCode.Value.ToString()},
                {"接机已派车", oModel.f_FromIsSendACar ? "已派车":""},
                {"接机序號", oModel.f_FromCode == null ? "":"B-" + oModel.f_FromCode.Value.ToString()},
                {"出發時間", oModel.f_ToDropOffTime??""},
                {"出發备注", oModel.f_ToRemark??""},
                {"返菲备注", oModel.f_FromRemark??""}
            }.ToString();
            }
            return "";
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="oModel">model实体</param>
        /// <param name="bIsLogs">是否需要写入操作记录</param>
        public void AddReturnTicketOneData(ReturnTicket oModel, bool bIsLogs = true)
        {
            //处理登记查询提示
             DoRegistTip(oModel, oModel.f_eId, "t_ReturnTicket", "机票查询");
            if (!string.IsNullOrWhiteSpace(oModel.f_ToDate.ToString()))
            {
                DoRegistTip(oModel, oModel.f_eId, "t_ReturnTicket", "送机查询");
            }
            if (!string.IsNullOrWhiteSpace(oModel.f_FromDate.ToString()))
            {
                DoRegistTip(oModel, oModel.f_eId, "t_ReturnTicket", "接机查询");
            }
            if (bIsLogs)
            {
                AddModifyRecord(null, GetJObjectData(oModel).ToString(), ActionItem.Add, CategoryItem.ReturnTicket, "机票登记");
            }
            _repository.Insert(oModel);
        }
        /// <summary>
        /// 查询主管所在部门的所有机票信息
        /// </summary>
        /// <param name="iEid">员工id</param>
        /// <returns>返回一个ReturnTicket集合</returns>
        public List<ReturnTicket> GetReturnTicketListBySector(int iEid)
        {
            Employee emp = _ELS.GetAll().FirstOrDefault(u => u.f_eid == iEid);
            List<ReturnTicket> returnTicketList = new List<ReturnTicket>();
            if (emp != null)
            {
                var linq =
                    _repository.GetAll().Where(
                        u => u.t_employeeInfo.f_department_tID == emp.f_department_tID && u.f_IsNewEmp == false).ToList();
                returnTicketList = linq.OrderBy(u =>
                {
                    int iId = u.t_employeeInfo.f_eid;
                    if (u.t_employeeInfo.f_eid == iEid)
                    {
                        iId = 0;
                    }
                    return iId;
                }).ThenByDescending(p => p.f_ToDate).ToList();
            }
            return returnTicketList;
        }

        /// <summary>
        /// 查询送机派车根据员工ID
        /// </summary>
        /// <param name="iEid">员工id</param>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">每页几条数据</param>
        /// <param name="iCount">总数据</param>
        /// <returns>返回一个ReturnTicket集合</returns>
        public List<ReturnTicket> GetSendCarByEid(int iEid, int iPageIndex, int iPageSize, out int iCount)
        {
            var linq = from u in _repository.GetAll().ToList()
                       where u.f_ToDate != null && u.f_eId == iEid
                       orderby u.f_ToDate, u.f_ToFlight ?? "".Split('~').Last(), u.f_ToTerminal, u.t_employeeInfo?.Dormitory?.f_Community
                       select u;
            iCount = linq.Count();
            ValidatePagingWhere(iCount, ref iPageIndex, iPageSize);
            return linq.OrderByDescending(u => u.f_Id).Skip((iPageIndex - 1) * iPageSize).Take(iPageSize).ToList();
        }

        /// <summary>
        /// 查询送机派车根据部门ID
        /// </summary>
        /// <param name="iDeptid">部门id</param>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">每页几条数据</param>
        /// <param name="iCount">总数据</param>
        /// <returns>返回一个ReturnTicket集合</returns>
        public List<ReturnTicket> GetSendCarByDeptid(int iDeptid, int iPageIndex, int iPageSize, out int iCount)
        {
            var linq = from u in _repository.GetAll().ToList()
                       where u.f_ToDate != null && (u.f_eId != null && u.t_employeeInfo.f_department_tID == iDeptid)
                       orderby u.f_ToDate, u.f_ToFlight ?? "".Split('~').Last(), u.f_ToTerminal, u.t_employeeInfo?.Dormitory?.f_Community
                       select u;

            iCount = linq.Count();
            linq = linq.OrderBy(u =>
            {
                int iId = u.t_employeeInfo.f_eid;
                if (u.t_employeeInfo.f_eid == _session.GetCurrUser().Id)
                {
                    iId = 0;
                }
                return iId;
            });
            ValidatePagingWhere(iCount, ref iPageIndex, iPageSize);
            return linq.OrderByDescending(u => u.f_Id).Skip((iPageIndex - 1) * iPageSize).Take(iPageSize).ToList();
        }

        /// <summary>
        /// 查询接机派车根据员工ID
        /// </summary>
        /// <param name="iEid">员工id</param>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">每页几条数据</param>
        /// <param name="iCount">总数据</param>
        /// <returns>返回一个ReturnTicket集合</returns>
        public List<ReturnTicket> GetPickupByEid(int iEid, int iPageIndex, int iPageSize, out int iCount)
        {
            var linq = _repository.GetAll().Where(u => (u.f_FromDate != null) && (u.f_eId != null && u.f_eId == iEid)).ToList().OrderBy(u => u.f_FromDate).ThenBy(u => u.f_FromFlight ?? "".Split('~').Last()).ThenBy(u => u.f_FromTerminal).ThenBy(u => u.t_employeeInfo?.Dormitory?.f_Community);

            iCount = linq.Count();
            ValidatePagingWhere(iCount, ref iPageIndex, iPageSize);
            return linq.OrderByDescending(u => u.f_Id).Skip((iPageIndex - 1) * iPageSize).Take(iPageSize).ToList();
        }

        /// <summary>
        /// 查询接机派车根据部门ID
        /// </summary>
        /// <param name="iDeptid">部门id</param>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">每页几条数据</param>
        /// <param name="iCount">总数据</param>
        /// <returns>返回一个ReturnTicket集合</returns>
        public List<ReturnTicket> GetPickupByDeptid(int iDeptid, int iPageIndex, int iPageSize, out int iCount)
        {
            var linq = _repository.GetAll().Where(u => (u.f_FromDate != null) && (u.f_eId != null && u.t_employeeInfo.f_department_tID == iDeptid)).ToList().OrderBy(u => u.f_FromDate).ThenBy(u => u.f_FromFlight ?? "".Split('~').Last()).ThenBy(u => u.f_FromTerminal).ThenBy(u => u.t_employeeInfo?.Dormitory?.f_Community);
            iCount = linq.Count();
            linq = linq.OrderBy(u =>
            {
                int iId = u.t_employeeInfo.f_eid;
                if (u.t_employeeInfo.f_eid == _session.GetCurrUser().Id)
                {
                    iId = 0;
                }
                return iId;
            });
            ValidatePagingWhere(iCount, ref iPageIndex, iPageSize);
            return linq.OrderByDescending(u => u.f_Id).Skip((iPageIndex - 1) * iPageSize).Take(iPageSize).ToList();
        }
        /// <summary>
        /// 获取类型名称的集合
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<SType> GetNameByStype(Func<SType, bool> predicate)
        {
            return _SType.GetAll().Where(predicate).OrderByDescending(j => j.f_tID).ToList();
        }
        /// <summary>
        /// 获取所有分页数据
        /// </summary>
        /// <param name="iPageIndex"></param>
        /// <param name="iPageSize"></param>
        /// <param name="iCount"></param>
        /// <returns></returns>
        public List<ReturnTicket> GetReturnTicketAllData(int iPageIndex, int iPageSize, out int iCount)
        {
            List<ReturnTicket> dataReturnTice = _repository.GetAll().Where(u => !u.f_IsNewEmp).OrderBy(u=>u.f_ToDate).ThenBy(u=>u.f_Id).ToList();
            iCount = dataReturnTice.Count();
            //分页iPageIndex是否超出页码
            ValidatePagingWhere(iCount, ref iPageIndex, iPageSize);
            return dataReturnTice.Skip((iPageIndex - 1) * iPageSize).Take(iPageSize).ToList();
        }
        /// <summary>
        /// 条件获取分页数据
        /// </summary>
        /// <param name="iPageIndex"></param>
        /// <param name="iPageSize"></param>
        /// <param name="iCount"></param>
        /// <returns></returns>
        public List<ReturnTicket> GetReturnTicketByWhere(TicketWhereDto ticketDto, int iPageIndex, int iPageSize, out int iCount)
        {
            List<ReturnTicket> dataReturnTice = _repository.GetAll().Where(u =>
            (ticketDto.f_departmentId == 0 || u.t_employeeInfo.f_department_tID == ticketDto.f_departmentId) &&
            (ticketDto.f_FromDate == null || u.f_FromDate == ticketDto.f_FromDate) &&
            (ticketDto.f_ToDate == null || u.f_ToDate == ticketDto.f_ToDate) &&
            (string.IsNullOrEmpty(ticketDto.Name) || (u.t_employeeInfo.f_nickname.Contains(ticketDto.Name) || u.t_employeeInfo.f_chineseName.Contains(ticketDto.Name))) &&
                   (string.IsNullOrEmpty(ticketDto.f_international) || ticketDto.f_international == "0" || u.t_employeeInfo.f_international.Contains(ticketDto.f_international)) &&
                   (ticketDto.f_ToAirlineType_Id == 0 || (u.f_ToAirlineType_Id == ticketDto.f_ToAirlineType_Id || u.f_FromAirlineType_Id == ticketDto.f_ToAirlineType_Id)) &&
                   (ticketDto.f_eId == 0 || u.f_eId == ticketDto.f_eId) &&
                   (!u.f_IsNewEmp)
            ).OrderBy(u => u.f_ToDate).ThenBy(u => u.t_employeeInfo.f_international).ThenBy(u => u.f_Id).ToList();
            iCount = dataReturnTice.Count();
            ValidatePagingWhere(iCount, ref iPageIndex, iPageSize);
            return dataReturnTice.Skip((iPageIndex - 1) * iPageSize).Take(iPageSize).ToList();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void Deletet_ReturnTicket(int id)
        {
            ReturnTicket model = _repository.GetEntityById(id);
            _repository.Delete(model);
        }
        /// <summary>
        /// 根据条件导出
        /// </summary>
        /// <param name="ticketDto">按条件查询</param>
        /// <returns></returns>
        public List<dynamic> ExportDataByWhere(TicketWhereDto ticketDto)
        {
            List<ReturnTicket> list = _repository.GetAll().Where(u =>
            (ticketDto.f_departmentId == 0 || u.t_employeeInfo.f_department_tID == ticketDto.f_departmentId) &&
            (ticketDto.f_FromDate == null || u.f_FromDate == ticketDto.f_FromDate) &&
            (ticketDto.f_ToDate == null || u.f_ToDate == ticketDto.f_ToDate) &&
            (string.IsNullOrEmpty(ticketDto.Name) || (u.t_employeeInfo.f_nickname.Contains(ticketDto.Name) || u.t_employeeInfo.f_chineseName.Contains(ticketDto.Name))) &&
                   (string.IsNullOrEmpty(ticketDto.f_international) || u.t_employeeInfo.f_international.Contains(ticketDto.f_international)) &&
                   (ticketDto.f_ToAirlineType_Id == 0 || (u.f_ToAirlineType_Id == ticketDto.f_ToAirlineType_Id || u.f_FromAirlineType_Id == ticketDto.f_ToAirlineType_Id)) &&
                   (ticketDto.f_eId == 0 || u.f_eId == ticketDto.f_eId) &&
                   (u.f_IsNewEmp == false)
            ).OrderBy(u => u.f_ToDate).ThenBy(u => u.t_employeeInfo.f_international).ThenBy(u => u.f_Id).ToList();
            return GetExportList(list).ToList();
        }
        /// <summary>
        /// 导出所有
        /// </summary>
        /// <returns></returns>
        public List<dynamic> ExportData()
        {
            List<ReturnTicket> data = _repository.GetAll().Where(u => u.f_IsNewEmp == false).OrderBy(u => u.f_ToDate).ThenBy(u => u.t_employeeInfo.f_international).ThenBy(u => u.f_Id).ToList();
            return GetExportList(data).ToList();
        }
        /// <summary>
        /// 获取需要打印的集合
        /// </summary>
        /// <param name="list">t_ReturnTicket的list集合</param>
        /// <returns>返回一个匿名集合</returns>
        private IEnumerable<dynamic> GetExportList(List<ReturnTicket> list)
        {
            int iCount = 0;

            return list.Select(p => new
            {
                序号 = iCount++,
                昵称 = p.t_employeeInfo?.f_nickname ?? (p.t_employeeInfo?.f_chineseName ?? ""),
                国籍 = p.t_employeeInfo?.f_international ?? "",
                部门 = _SType.GetEntityById((int)p.t_employeeInfo?.f_department_tID) != null ? _SType.GetEntityById((int)p.t_employeeInfo?.f_department_tID)?.f_value : "",
                返乡日期 = p.f_ToDate?.ToString("yyyy-MM-dd") ?? "",
                返乡航空公司 = p.f_ToAirlineType_Id != null ? _SType.GetEntityById((int)p.f_ToAirlineType_Id)?.f_value : "",
                返乡航班起飞与抵达时间 = p.f_ToFlight ?? "",
                地点 = p.f_ToAddress ?? "",
                返乡备注 = p.f_ToRemark??"",
                返菲日期 = p.f_FromDate?.ToString("yyyy-MM-dd") ?? "",
                返菲航空公司 = p.f_FromAirlineType_Id!= null ? _SType.GetEntityById((int)p.f_FromAirlineType_Id)?.f_value :"",
                返菲航班起飞与抵达时间 = p.f_FromFlight ?? "",
                地点1 = p.f_FromAddress ?? "",
                返菲备注 = p.f_FromRemark??"",
                操作人 = p.f_Operation??"",
                操作时间 = p.f_OperationDate!=null? p.f_OperationDate.ToString("yyyy-MM-dd"):"",
            }).ToList();
        }

        
    }
}
