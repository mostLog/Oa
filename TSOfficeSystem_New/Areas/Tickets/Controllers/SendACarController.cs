using MI.Application;
using MI.Application.Dto;
using MI.Application.OASession;
using MI.Core.Common;
using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSOfficeSystem_New.Controllers;

namespace TSOfficeSystem_New.Areas.Tickets.Controllers
{
    public class SendACarController : BaseController
    {
        private readonly IReturnTicketService _returnTicketService;
        //类型服务
        private readonly ISTypeService _sTypeService;
        //宿舍服务
        private readonly IDormitoryService _dormitoryService;
        //员工服务
        private readonly IEmployeeService _employeeService;
        public SendACarController(
            IReturnTicketService returnTicketService, 
            ISTypeService sTypeService, 
            IDormitoryService dormitoryService, 
            IEmployeeService employeeService, 
            ISession session, 
            IEmployeeService emoloyee) : base(session, emoloyee)
        {
            _returnTicketService = returnTicketService;
            _sTypeService = sTypeService;
            _dormitoryService = dormitoryService;
            _employeeService = employeeService;
        }

        /// <summary>
        /// 根据条件查询送机派车所有数据
        /// </summary>
        /// <param name="sc">查询条件</param>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">页大小</param>
        /// <returns></returns>
        public ActionResult SendACarList(SendCarDto sc, int iPageIndex = 1, int iPageSize = 15)
        {
            int o_Count = 0;
            bool bIsSendACar = string.IsNullOrWhiteSpace(sc.IsSendACar) || (sc.IsSendACar == "2");
            bool bIsSend = ((!bIsSendACar) && sc.IsSendACar == "1");
            var list = _returnTicketService.GetSendCarByWhere(m => (m.f_ToDate != null)
                       && ((sc.Name == null || sc.Name.Trim() == string.Empty) || (m.t_employeeInfo.f_chineseName != null && m.t_employeeInfo.f_chineseName.Contains(sc.Name.Trim())))
                       && (sc.DateStart == DateTime.MinValue || (m.f_ToDate >= sc.DateStart)) && (sc.DateEnd == DateTime.MinValue || (m.f_ToDate <= sc.DateEnd))
                        && ((sc.National == null || sc.National.Trim() == string.Empty) || (sc.National == "0") || ((m.t_employeeInfo.f_international != null && m.t_employeeInfo.f_international.Contains(sc.National.Trim()))))
                       && (bIsSendACar || (m.f_ToIsSendACar == bIsSend)), iPageIndex, iPageSize, out o_Count);
            ViewBag.iPageIndex = iPageIndex;
            ViewBag.iPageSize = iPageSize;
            ViewBag.iCount = o_Count;
            ViewBag.Sc = sc;
            ViewBag.Nation = _sTypeService.GetsTypeByWhere((int)sTypeEnum.国籍);
            ViewBag.FlightType = _sTypeService.GetsTypeByWhere((int)sTypeEnum.航班类型);
            ViewBag.DepartmentType = _sTypeService.GetsTypeByWhere((int)sTypeEnum.部门类型);
            return View(list);
        }

        /// <summary>
        /// 根据条件查询接机派车所有数据
        /// </summary>
        /// <param name="sc">查询条件</param>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">页大小</param>
        /// <returns></returns>
        public ActionResult PickupList(PickupDto sc, int iPageIndex = 1, int iPageSize = 15) 
        {
            int o_Count = 0;
            bool bIsPickup = string.IsNullOrWhiteSpace(sc.FromIsSendACar) || (sc.FromIsSendACar == "2");
            bool bIsPick = ((!bIsPickup) && sc.FromIsSendACar == "1");
            var list = _returnTicketService.GetPickupByWhere(m => (m.f_FromDate != null) && ((sc.Name == null || sc.Name.Trim() == string.Empty) || (m.t_employeeInfo.f_chineseName != null && m.t_employeeInfo.f_chineseName.Contains(sc.Name.Trim())))
                               && (sc.FromDateST == DateTime.MinValue || (m.f_FromDate >= sc.FromDateST)) && (sc.FromDateED == DateTime.MinValue || (m.f_FromDate <= sc.FromDateED))
                               && ((sc.National == null || sc.National.Trim() == string.Empty) || (sc.National == "0") || ((m.t_employeeInfo.f_international != null && m.t_employeeInfo.f_international.Contains(sc.National.Trim()))))
                               && (bIsPickup || (m.f_FromIsSendACar == bIsPick)), iPageIndex, iPageSize, out o_Count);
            ViewBag.iPageIndex = iPageIndex;
            ViewBag.iPageSize = iPageSize;
            ViewBag.iCount = o_Count;
            ViewBag.Sc = sc;
            ViewBag.Nation = _sTypeService.GetsTypeByWhere((int)sTypeEnum.国籍);
            ViewBag.FlightType = _sTypeService.GetsTypeByWhere((int)sTypeEnum.航班类型);
            ViewBag.DepartmentType = _sTypeService.GetsTypeByWhere((int)sTypeEnum.部门类型);
            return View(list);
        }

        /// <summary>
        /// 根据条件导出送机派车数据
        /// </summary>
        /// <param name="sc"></param>
        /// <returns></returns>
        public ActionResult ExportSendCar(SendCarDto sc)
        {
            bool bIsSendACar = string.IsNullOrWhiteSpace(sc.IsSendACar) || (sc.IsSendACar == "2");
            bool bIsSend = ((!bIsSendACar) && sc.IsSendACar == "1");
            var list = _returnTicketService.GetSendCarByWhereExport(m => (m.f_ToDate != null) && ((sc.Name == null || sc.Name.Trim() == string.Empty) || (m.t_employeeInfo.f_chineseName != null && m.t_employeeInfo.f_chineseName.Contains(sc.Name.Trim())))
                               && (sc.DateStart == DateTime.MinValue || (m.f_ToDate >= sc.DateStart)) && (sc.DateEnd == DateTime.MinValue || (m.f_ToDate <= sc.DateEnd))
                               && ((sc.National == null || sc.National.Trim() == string.Empty) || (sc.National == "0") || ((m.t_employeeInfo.f_international != null && m.t_employeeInfo.f_international.Contains(sc.National.Trim()))))
                               && (bIsSendACar || (m.f_ToIsSendACar == bIsSend)));
            DataSet ds = new DataSet();
            DataTable dt= DataTableExt.ToDataTable<dynamic>(list);
            ds.Tables.Add(dt);

            string sFileName = "Export_接机派车" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            MemoryStream stream = NPOIExcelHelper.ExportExcel(dt, NPOIExcelHelper.ExtendPropertiesType.None, "t_ReturnTicket");
            return File(stream, "application/vnd.ms-excel", sFileName);
        }

        /// <summary>
        /// 根据条件导出接机派车数据
        /// </summary>
        /// <param name="sc"></param>
        /// <returns></returns>
        public FileResult ExportPickup(PickupDto sc)
        {
            bool bIsSendACar = string.IsNullOrWhiteSpace(sc.FromIsSendACar) || (sc.FromIsSendACar == "2");
            bool bIsSend = ((!bIsSendACar) && sc.FromIsSendACar == "1");
            var list = _returnTicketService.GetPickupByWhereExport(m => (m.f_FromDate != null) && ((sc.Name == null || sc.Name.Trim() == string.Empty) || (m.t_employeeInfo.f_chineseName != null && m.t_employeeInfo.f_chineseName.Contains(sc.Name.Trim())))
                               && (sc.FromDateST == DateTime.MinValue || (m.f_FromDate >= sc.FromDateST)) && (sc.FromDateED == DateTime.MinValue || (m.f_FromDate <= sc.FromDateED))
                               && ((sc.National == null || sc.National.Trim() == string.Empty) || (sc.National == "0") || ((m.t_employeeInfo.f_international != null && m.t_employeeInfo.f_international.Contains(sc.National.Trim()))))
                               && (bIsSendACar || (m.f_FromIsSendACar == bIsSend))); 
            DataSet ds = new DataSet();
            DataTable dt = DataTableExt.ToDataTable<dynamic>(list);
            ds.Tables.Add(dt);

            string sFileName = "Export_送机派车" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            MemoryStream stream = NPOIExcelHelper.ExportExcel(dt, NPOIExcelHelper.ExtendPropertiesType.None, "t_ReturnTicket");
            return File(stream, "application/vnd.ms-excel", sFileName);
        }

        /// <summary>
        /// 批量派车标记
        /// </summary>
        /// <param name="tagList">传过来的参数</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SendTag(string tagList)
        {
            ErrorEnum eReuslt = ErrorEnum.Fail;
            eTipsEnum eTips = eTipsEnum.FailOperation;
            try
            {
                ReturnTicket model = new ReturnTicket();
                string[] tags = tagList.Split(',');
                for (int i = 0; i < tags.Length; i++)
                {
                    model = _returnTicketService.GetReturnTicketById(Convert.ToInt32(tags[i]));
                    ReturnTicket oldModel = new ReturnTicket()
                    {
                        f_eId = model.f_eId,
                        f_Id = model.f_Id,
                        f_ToCode = model.f_ToCode,
                        f_FromCode = model.f_FromCode,
                        f_ToDropOffTime = model.f_ToDropOffTime,
                        f_ToRemark = model.f_ToRemark,
                        f_FromRemark = model.f_FromRemark,
                        f_ToIsSendACar = model.f_ToIsSendACar,
                        f_FromIsSendACar = model.f_FromIsSendACar
                    };
                    //判断是否派车
                    bool bFlag = model.f_ToIsSendACar;
                    if (bFlag)
                    {
                        model.f_ToIsSendACar = false;
                        model.f_ToCode = null;
                    }
                    else
                    {
                        model.f_ToIsSendACar = true;
                        //获得返乡日期
                        DateTime dtToDate = model.f_ToDate ?? DateTime.Now;
                        //获得最大编号
                        int iNumber = GetSendCarMaxNumber(dtToDate);
                        model.f_ToCode = iNumber + 1;
                    }
                    int result=_returnTicketService.EditReturnTicketOneData(model, oldModel, true, true);
                    if (result > 0)
                    {
                        eReuslt = ErrorEnum.Success;
                        eTips = eTipsEnum.SuccessfulOperation;
                    }
                }
            }
            catch
            {
                eReuslt = ErrorEnum.Error;
                eTips = eTipsEnum.Error;
            }
            return Json(new ResultModel((int)eReuslt, eTips.Description()));
        }


        /// <summary>
        /// 批量派车标记
        /// </summary>
        /// <param name="tagList">传过来的参数</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PickupTag(string tagList)
        {
            ErrorEnum eReuslt = ErrorEnum.Fail;
            eTipsEnum eTips = eTipsEnum.FailOperation;
            try
            {
                ReturnTicket model = new ReturnTicket();
                string[] tags = tagList.Split(',');
                for (int i = 0; i < tags.Length; i++)
                {
                    model = _returnTicketService.GetReturnTicketById(Convert.ToInt32(tags[i]));
                    ReturnTicket oldModel = new ReturnTicket()
                    {
                        f_eId = model.f_eId,
                        f_Id = model.f_Id,
                        f_ToCode = model.f_ToCode,
                        f_FromCode = model.f_FromCode,
                        f_ToDropOffTime = model.f_ToDropOffTime,
                        f_ToRemark = model.f_ToRemark,
                        f_FromRemark = model.f_FromRemark,
                        f_ToIsSendACar = model.f_ToIsSendACar,
                        f_FromIsSendACar = model.f_FromIsSendACar
                    };
                    //判断是否派车
                    bool bFlag = model.f_FromIsSendACar;
                    if (bFlag)
                    {
                        model.f_FromIsSendACar = false;
                        model.f_FromCode = null;
                    }
                    else
                    {
                        model.f_FromIsSendACar = true;
                        //获得返乡日期
                        DateTime dtFormDate = model.f_FromDate ?? DateTime.Now;
                        //获得最大编号
                        int iNumber = GetPickupMaxNumber(dtFormDate);
                        model.f_FromCode = iNumber + 1;
                    }
                    int result = _returnTicketService.EditReturnTicketOneData(model, oldModel, true, true);
                    if (result > 0)
                    {
                        eReuslt = ErrorEnum.Success;
                        eTips = eTipsEnum.SuccessfulOperation;
                    }
                }
            }
            catch
            {
                eReuslt = ErrorEnum.Error;
                eTips = eTipsEnum.Error;
            }
            return Json(new ResultModel((int)eReuslt, eTips.Description()));
        }
        #region   判断返乡日期在那个范围 获得最大编号
        /// <summary>
        /// 判断送机返乡日期在那个范围 获得最大编号
        /// </summary>
        /// <param name="dtToDate">返乡日期</param>
        /// <returns></returns>
        public int GetSendCarMaxNumber(DateTime dtToDate) 
        {
            int strSendCode = 0;
            if (dtToDate.Day > 20)
            {
                //获得返乡日期当月最后一天
                int lastDay = DateTime.DaysInMonth(dtToDate.Year, dtToDate.Month);
                DateTime dtStartDate = new DateTime(dtToDate.Year, dtToDate.Month, 21);
                DateTime dtEndDate = new DateTime(dtToDate.Year, dtToDate.Month, lastDay);
                strSendCode = _returnTicketService.GetSendMaxToCode(dtStartDate, dtEndDate);
            }
            else if (dtToDate.Day > 10)
            {
                DateTime dtStartDate = new DateTime(dtToDate.Year, dtToDate.Month, 11);
                DateTime dtEndDate = new DateTime(dtToDate.Year, dtToDate.Month, 20);
               strSendCode = _returnTicketService.GetSendMaxToCode(dtStartDate, dtEndDate);
            }
            else
            {
                DateTime dtStartDate = new DateTime(dtToDate.Year, dtToDate.Month, 1);
                DateTime dtEndDate = new DateTime(dtToDate.Year, dtToDate.Month, 10);
                strSendCode = _returnTicketService.GetSendMaxToCode(dtStartDate, dtEndDate);
            }
            return strSendCode;
        }
        #endregion

        #region   判断返乡日期在那个范围 获得最大编号
        /// <summary>
        /// 判断接机返乡日期在那个范围 获得最大编号
        /// </summary>
        /// <param name="dtToDate">返乡日期</param>
        /// <returns></returns>
        public int GetPickupMaxNumber(DateTime dtFormDate)  
        {
            int strSendCode = 0;
            if (dtFormDate.Day > 20)
            {
                //获得返乡日期当月最后一天
                int lastDay = DateTime.DaysInMonth(dtFormDate.Year, dtFormDate.Month);
                DateTime dtStartDate = new DateTime(dtFormDate.Year, dtFormDate.Month, 21);
                DateTime dtEndDate = new DateTime(dtFormDate.Year, dtFormDate.Month, lastDay);
                strSendCode = _returnTicketService.GetPickupMaxToCode(dtStartDate, dtEndDate);
            }
            else if (dtFormDate.Day > 10)
            {
                DateTime dtStartDate = new DateTime(dtFormDate.Year, dtFormDate.Month, 11);
                DateTime dtEndDate = new DateTime(dtFormDate.Year, dtFormDate.Month, 20);
                strSendCode = _returnTicketService.GetPickupMaxToCode(dtStartDate, dtEndDate);
            }
            else
            {
                DateTime dtStartDate = new DateTime(dtFormDate.Year, dtFormDate.Month, 1);
                DateTime dtEndDate = new DateTime(dtFormDate.Year, dtFormDate.Month, 10);
                strSendCode = _returnTicketService.GetPickupMaxToCode(dtStartDate, dtEndDate); 
            }
            return strSendCode;
        }
        #endregion

        /// <summary>
        /// 保存送机派车的出发时间和备注
        /// </summary>
        /// <param name="id">自增Id</param>
        /// <param name="toDropOffTime">出发时间</param>
        /// <param name="remark">备注信息</param>
        /// <returns></returns>
        public ActionResult SendSaveData(int id, string toDropOffTime, string remark)
        {
            ErrorEnum eReuslt = ErrorEnum.Fail;
            eTipsEnum eTips = eTipsEnum.FailOperation;
            string sReturnChar = string.Empty;
            if (AOUnity.ValidationLength("备注", remark, 100, out sReturnChar))
            {
                eReuslt = ErrorEnum.LengthOutRange;
                return Json(new ResultModel((int)eReuslt, sReturnChar));
            }
            try
            {
                var model= _returnTicketService.GetReturnTicketById(id);
                ReturnTicket oldModel = new ReturnTicket()
                {
                    f_eId = model.f_eId,
                    f_Id = model.f_Id,
                    f_ToCode = model.f_ToCode,
                    f_FromCode = model.f_FromCode,
                    f_ToDropOffTime = model.f_ToDropOffTime,
                    f_ToRemark = model.f_ToRemark,
                    f_FromRemark = model.f_FromRemark,
                    f_ToIsSendACar = model.f_ToIsSendACar,
                    f_FromIsSendACar = model.f_FromIsSendACar
                };
                model.f_ToDropOffTime = toDropOffTime;
                model.f_ToRemark = remark;
                int result = _returnTicketService.EditReturnTicketOneData(model, oldModel, true, true);
                if (result > 0)
                {
                    eReuslt = ErrorEnum.Success;
                    eTips = eTipsEnum.SuccessfulOperation;
                }
            }
            catch
            {
                eReuslt = ErrorEnum.Error;
                eTips = eTipsEnum.Error;            
            }
            return Json(new ResultModel((int)eReuslt, eTips.Description()));
        }

        /// <summary>
        /// 保存接机派车的备注
        /// </summary>
        /// <param name="id">自增Id</param>
        /// <param name="remark">备注信息</param>
        /// <returns></returns>
        public ActionResult PickupSaveData(int id, string remark)
        {
            ErrorEnum eReuslt = ErrorEnum.Fail;
            eTipsEnum eTips = eTipsEnum.FailOperation;
            string sReturnChar = string.Empty;
            if (AOUnity.ValidationLength("备注", remark, 100, out sReturnChar))
            {
                eReuslt = ErrorEnum.LengthOutRange;
                return Json(new ResultModel((int)eReuslt, sReturnChar));
            }
            try
            {
                var model = _returnTicketService.GetReturnTicketById(id);
                ReturnTicket oldModel = new ReturnTicket()
                {
                    f_eId = model.f_eId,
                    f_Id = model.f_Id,
                    f_ToCode = model.f_ToCode,
                    f_FromCode = model.f_FromCode,
                    f_ToDropOffTime = model.f_ToDropOffTime,
                    f_ToRemark = model.f_ToRemark,
                    f_FromRemark = model.f_FromRemark,
                    f_ToIsSendACar = model.f_ToIsSendACar,
                    f_FromIsSendACar = model.f_FromIsSendACar
                };
                model.f_FromRemark = remark;

                int result = _returnTicketService.EditReturnTicketOneData(model, oldModel, true, true);
                if (result > 0)
                {
                    eReuslt = ErrorEnum.Success;
                    eTips = eTipsEnum.SuccessfulOperation;
                }
            }
            catch
            {
                eReuslt = ErrorEnum.Error;
                eTips = eTipsEnum.Error;
            }
            return Json(new ResultModel((int)eReuslt, eTips.Description()));
        }
    }
}