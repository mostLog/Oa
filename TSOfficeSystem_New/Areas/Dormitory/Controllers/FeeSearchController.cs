using MI.Application;
using MI.Application.Dto;
using MI.Application.OASession;
using MI.Core;
using MI.Core.Common;
using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSOfficeSystem_New.Controllers;

namespace TSOfficeSystem_New.Areas.Dormitory.Controllers
{
    public class FeeSearchController : BaseController
    {
        private readonly IEmpRentService _empRentService;
        private readonly ISTypeService _sTypeService;
        private readonly IEmployeeService _employeeService;
        private readonly IGrantService _grantService;
        private readonly ITariffService _TDS;
        private readonly IFlightFeeService _FFS;
        private readonly IReturnTicketService _RTS;
        private readonly IDormitoryMaintenanceService _DMS;
        private readonly ICertAgencyService _CAS;
        private readonly IBedLinenService _BLS;
        private readonly IHostelCleanService _HCS;

        public FeeSearchController(IEmpRentService empRentService,
            ISTypeService sTypeService,
            IEmployeeService employeeService,
            IGrantService grantService,
            ITariffService TDS,
            IFlightFeeService FFS,
            IReturnTicketService RTS,
            IDormitoryMaintenanceService DMS,
            ICertAgencyService CAS,
            IBedLinenService BLS,
            IHostelCleanService HCS,
        ISession session, IEmployeeService emoloyee) : base(session, emoloyee)
        {
            _empRentService = empRentService;
            _sTypeService = sTypeService;
            _employeeService = employeeService;
            _grantService = grantService;
            _TDS = TDS;
            _FFS = FFS;
            _RTS = RTS;
            _DMS = DMS;
            _CAS = CAS;
            _BLS = BLS;
            _HCS = HCS;
        }

        /// <summary>
        /// 查询员工租房信息
        /// </summary>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">页大小</param> 
        /// <returns></returns>
        public ActionResult EmpRentListData(int iPageIndex = 1, int iPageSize = 15)
        {
            int count = 0;
            if (m_RankStr.Contains("SectorALL"))
            {
                Employee employee = _employeeService.GetEmployeeById(m_Eid);
                var list = _empRentService.GetEmpRentByWhere(u =>
                (m_Eid == 0 || (u.t_employeeInfo.f_department_tID == employee.f_department_tID)
                ), iPageIndex, iPageSize, out count).ToList();
                ViewBag.iPageIndex = iPageIndex;
                ViewBag.iPageSize = iPageSize;
                ViewBag.iCount = count;
                ViewBag.listWorkLocation = _sTypeService.GetsTypeByWhere((int)sTypeEnum.上班地点类型);
                return View(list);
            }
            else
            {
                var list = _empRentService.GetEmpRentByWhere(u => (m_Eid == 0 || (u.f_eid == m_Eid)), iPageIndex, iPageSize, out count).ToList();
                ViewBag.iPageIndex = iPageIndex;
                ViewBag.iPageSize = iPageSize;
                ViewBag.iCount = count;
                ViewBag.listWorkLocation = _sTypeService.GetsTypeByWhere((int)sTypeEnum.上班地点类型);
                return View(list);
            }

        }

        /// <summary>
        /// 查询外租补助信息
        /// </summary>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">页大小</param>
        /// <returns></returns>
        public ActionResult RentSubListData(int iPageIndex = 1, int iPageSize = 15)
        {
            int o_Count = 0;
            if (m_RankStr.Contains("SectorALL"))
            {
                Employee employee = _employeeService.GetEmployeeById(m_Eid);
                var list = _grantService.GetGrantByWhere(c => (m_Eid == 0 || (c.t_employeeInfo.f_department_tID == employee.f_department_tID)), iPageIndex, iPageSize, out o_Count).ToList();
                ViewBag.iPageIndex = iPageIndex;
                ViewBag.iPageSize = iPageSize;
                ViewBag.iCount = o_Count;
                return View(list);
            }
            else
            {
                var list = _grantService.GetGrantByWhere(c => (m_Eid == 0 || (c.f_eid == m_Eid)), iPageIndex, iPageSize, out o_Count).ToList();
                ViewBag.iPageIndex = iPageIndex;
                ViewBag.iPageSize = iPageSize;
                ViewBag.iCount = o_Count;
                return View(list);
            }
        }

        /// <summary>
        /// 电费超支
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult TariffList(TariffDto model, int iPageIndex = 1, int iPageSize = 15)
        {
            int count = 0;
            int e_Id = base._session.GetCurrUser().Id;
            Employee emp = _employeeService.GetEmployeeById(e_Id);
            if (m_RankStr.Contains("SectorALL"))
            {
                if (e_Id == 0)
                {
                    return null;
                }
                else
                {
                    var list = _TDS.GetPageAllTariff(emp.f_department_tID, iPageIndex, iPageSize, out count).ToList();
                    ViewBag.iPageIndex = iPageIndex;
                    ViewBag.iPageSize = iPageSize;
                    ViewBag.iCount = count;
                    ViewBag.TW = model;
                    ViewBag.listWorkLocation = _sTypeService.GetTypeByWhere(p => p.f_type == (int)sTypeEnum.上班地点类型);
                    return View(list);
                }
            }
            else
            {
                var list = _TDS.GetConditionByWhere(u =>
                 (e_Id == 0 || (u.f_DormitoryId == emp.f_dormitoryId)
                 )
                  , iPageIndex, iPageSize, out count).ToList();
                ViewBag.iPageIndex = iPageIndex;
                ViewBag.iPageSize = iPageSize;
                ViewBag.iCount = count;
                ViewBag.TW = model;
                ViewBag.listWorkLocation = _sTypeService.GetTypeByWhere(p => p.f_type == (int)sTypeEnum.上班地点类型);
                return View(list);
            }
        }

        /// <summary>
        /// 机票差额
        /// </summary>
        /// <param name="model"></param>
        /// <param name="iPageIndex"></param>
        /// <param name="iPageSize"></param>
        /// <returns></returns>
        public ActionResult FlightFeeList(FlightFeeDto model, int iPageIndex = 1, int iPageSize = 15)
        {
            int count = 0;
            int e_Id = base._session.GetCurrUser().Id;
            Employee emp = _employeeService.GetEmployeeById(e_Id);
            if (m_RankStr.Contains("SectorALL"))
            {
                if (e_Id == 0)
                {
                    return null;
                }
                else
                {
                    var list = _FFS.GetConditionByWhere(u =>
                     (e_Id == 0 || (u.t_employeeInfo.f_department_tID == emp.f_department_tID)
                     ), iPageIndex, iPageSize, out count).ToList();
                    ViewBag.iPageIndex = iPageIndex;
                    ViewBag.iPageSize = iPageSize;
                    ViewBag.iCount = count;
                    ViewBag.GW = model;
                    ViewBag.listWorkLocation = _sTypeService.GetTypeByWhere(p => p.f_type == (int)sTypeEnum.上班地点类型);
                    return View(list);
                }
            }
            else
            {
                var list = _FFS.GetConditionByWhere(u => (e_Id == 0 || (u.f_eid == e_Id))
                  , iPageIndex, iPageSize, out count).ToList();
                ViewBag.iPageIndex = iPageIndex;
                ViewBag.iPageSize = iPageSize;
                ViewBag.iCount = count;
                ViewBag.GW = model;
                ViewBag.listWorkLocation = _sTypeService.GetTypeByWhere(p => p.f_type == (int)sTypeEnum.上班地点类型);
                return View(list);
            }
        }

        /// <summary>
        /// 机票查询
        /// </summary>
        /// <param name="rtw"></param>
        /// <param name="AlertS"></param>
        /// <param name="iPageIndex"></param>
        /// <param name="iPageSize"></param>
        /// <returns></returns>
        public ActionResult ReturnTicketList(TicketWhereDto rtw, string AlertS = "", int iPageIndex = 1, int iPageSize = 15)
        {
            List<ReturnTicket> returnticketLIst = new List<ReturnTicket>();
            int count = 0;
            int e_Id = base._session.GetCurrUser().Id;
            rtw.f_eId = e_Id;
            if (m_RankStr.Contains("SectorALL"))
            {
                returnticketLIst = _RTS.GetReturnTicketListBySector(e_Id);
            }
            else
            {
                returnticketLIst = _RTS.GetReturnTicketListBySector(e_Id);
            }
            ViewBag.AirlineType = _sTypeService.GetTypeByWhere(p => p.f_type == (int)sTypeEnum.航班类型);
            ViewBag.iPageIndex = iPageIndex;
            ViewBag.iPageSize = iPageSize;
            ViewBag.iCount = count;
            ViewBag.RTW = rtw;
            ViewBag.AlertS = AlertS;
            return View(returnticketLIst);
        }

        /// <summary>
        /// 维修查询
        /// </summary>
        /// <returns></returns>
        public ActionResult RepairFees(string keyWords, int iPageIndex = 1, int iPageSize = 15)
        {
            int e_Id = base._session.GetCurrUser().Id;
            Employee emp = _employeeService.GetEmployeeById(e_Id);
            if (e_Id != 0)
            {
                int count = 0;
                if (m_RankStr.Contains("SectorALL"))
                {
                    var list = _DMS.GetConditionByDeptId(emp.f_department_tID, iPageIndex, iPageSize, out count);
                    ViewBag.iPageIndex = iPageIndex;
                    ViewBag.iPageSize = iPageSize;
                    ViewBag.iCount = count;
                    return View(list);
                }
                else
                {
                    int iDid = (emp?.f_dormitoryId) ?? 0;
                    var list = _DMS.GetConditionByWhere(p => p.f_DormitoryId == iDid, iPageIndex, iPageSize, out count);
                    ViewBag.iPageIndex = iPageIndex;
                    ViewBag.iPageSize = iPageSize;
                    ViewBag.iCount = count;
                    return View(list);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///  送机查询
        /// </summary>
        /// <param name="scw"></param>
        /// <param name="iPageIndex"></param>
        /// <param name="iPageSize"></param>
        /// <returns></returns>
        public ActionResult SendCarList(SendCarDto scw, int iPageIndex = 1, int iPageSize = 15)
        {
            int e_Id = base._session.GetCurrUser().Id;
            Employee emp = _employeeService.GetEmployeeById(e_Id);
            int count = 0;
            if (m_RankStr.Contains("SectorALL"))
            {
                var list = _RTS.GetSendCarByDeptid(emp.f_department_tID, iPageIndex, iPageSize, out count);
                ViewBag.AirlineType = _sTypeService.GetTypeByWhere(p => p.f_type == (int)sTypeEnum.航班类型);
                ViewBag.iPageIndex = iPageIndex;
                ViewBag.iPageSize = iPageSize;
                ViewBag.iCount = count;
                ViewBag.Rtw = scw;
                return View(list);
            }
            else
            {
                var list = _RTS.GetSendCarByEid(m_Eid, iPageIndex, iPageSize, out count);
                ViewBag.AirlineType = _sTypeService.GetTypeByWhere(p => p.f_type == (int)sTypeEnum.航班类型);
                ViewBag.iPageIndex = iPageIndex;
                ViewBag.iPageSize = iPageSize;
                ViewBag.iCount = count;
                ViewBag.Rtw = scw;
                return View(list);
            }
        }

        /// <summary>
        ///  接机查询
        /// </summary>
        /// <param name="scw"></param>
        /// <param name="iPageIndex"></param>
        /// <param name="iPageSize"></param>
        /// <returns></returns>
        public ActionResult PickupList(PickupChaDto pcw, int iPageIndex = 1, int iPageSize = 15)
        {
            int e_Id = base._session.GetCurrUser().Id;
            Employee emp = _employeeService.GetEmployeeById(e_Id);
            int count = 0;
            if (m_RankStr.Contains("SectorALL"))
            {
                var list = _RTS.GetPickupByDeptid(emp.f_department_tID, iPageIndex, iPageSize, out count);
                ViewBag.AirlineType = _sTypeService.GetTypeByWhere(p => p.f_type == (int)sTypeEnum.航班类型);
                ViewBag.iPageIndex = iPageIndex;
                ViewBag.iPageSize = iPageSize;
                ViewBag.iCount = count;
                ViewBag.Rtw = pcw;
                return View(list);
            }
            else
            {
                var list = _RTS.GetPickupByEid(m_Eid, iPageIndex, iPageSize, out count);
                ViewBag.AirlineType = _sTypeService.GetTypeByWhere(p => p.f_type == (int)sTypeEnum.航班类型);
                ViewBag.iPageIndex = iPageIndex;
                ViewBag.iPageSize = iPageSize;
                ViewBag.iCount = count;
                ViewBag.Rtw = pcw;
                return View(list);
            }

        }
        /// <summary>
        /// 下载 
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <returns></returns>
        public ActionResult Filedown(string FileName, string ToOrFrom, int f_eId = 0)
        {
            if (string.IsNullOrWhiteSpace(FileName) || string.IsNullOrWhiteSpace(ToOrFrom) || f_eId <= 0)
            {
                return null;
            }

            FileName = EncryptHelper.AESDecrypt(FileName);

            string sPath = Server.MapPath("~/TicketFile/") + FileName;
            if (ToOrFrom == "f_FileName")
            {
                sPath = Server.MapPath("~/File/CertAgencyFile/") + FileName;
            }
            if (System.IO.File.Exists(sPath))
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(sPath, FileMode.OpenOrCreate);
                    byte[] bytes = new byte[(int)fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                    fs.Close();
                    Response.ContentType = "application/octet-stream";
                    //通知浏览器下载文件而不是打开
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8));
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    Response.End();
                    //修改资料下载提示
                    if (ToOrFrom == "f_FileName")
                    {
                        CertAgency model = _CAS.GetCertAgencyById(f_eId);
                        model.f_DownTips = false;

                        _CAS.EditCertAgencyData(model);
                    }
                    else
                    {
                        ReturnTicket model = _RTS.GetReturnTicketById(f_eId);
                        if (ToOrFrom == "f_ToFile")
                        {
                            model.f_ToIsTips = false;
                        }
                        else if (ToOrFrom == "f_FromFile")
                        {
                            model.f_FromIsTips = false;
                        }
                        _RTS.EditReturnTicketOneData(model);
                    }

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Dispose();
                    }
                }
            }
            else
            {
                if (ToOrFrom == "f_FileName")
                {
                    return RedirectToAction("CertAgencyList", new { AlertS = "该资源已丢失，请联系行政人员" });
                }
                return RedirectToAction("ReturnTicketList", new { AlertS = "该资源已丢失，请联系行政人员" });
            }
            return new EmptyResult();
        }
        /// <summary>
        /// 证件查询
        /// </summary>
        /// <returns></returns>
        public ActionResult CertAgencyList()
        {
            int e_Id = base._session.GetCurrUser().Id;
            List<CertAgencyDto> list = new List<CertAgencyDto>();
            if (m_RankStr.Contains("SectorALL"))
            {
                list = _CAS.GetAgencyBySector(e_Id);
            }
            else
            {
                list = _CAS.GetAgencyByEid(e_Id);
            }
            return View(list);
        }

        /// <summary>
        /// 按员工查询自己的床单送洗记录
        /// </summary>
        /// <returns></returns>
        public ActionResult BedLinenList()
        {
            int e_Id = base._session.GetCurrUser().Id;
            return View(_BLS.GetBedLinenListByEid(e_Id));
        }

        /// <summary>
        /// 添加BedLinen数据视图
        /// </summary>
        /// <param name="iId"></param>
        /// <returns>返回一个 修改视图 页面</returns>
        public ActionResult BedLinenListEdit(int iId)
        {
            int e_Id = base._session.GetCurrUser().Id;
            DateTime oDate;
            if (DateTime.Now.Day >= 1 && DateTime.Now.Day <= 9)
            {
                oDate = DateTime.Now;
            }
            else
            {
                oDate = DateTime.Now.AddMonths(1);
            }
            BedLinen model = iId == 0 ? (new BedLinen { f_BunkNo = 0, f_SheetsNo = 0, f_QuiltNo = 0, f_PillowcaseNo = 0, f_Cont = 0, f_AddBunkNo = 0, f_AddSheetsNo = 0, f_AddQuiltNo = 0, f_AddPillowcaseNo = 0, f_eid = e_Id, f_RegistrationDate = oDate }) : _BLS.GetBedLinenById(iId);
            if (iId > 0)
            {
                if (model.f_eid != e_Id)
                {
                    return Json("该条信息不是您添加，不许修改");
                }
            }
            //床位类型
            ViewBag.typeBedLinenlist = _sTypeService.GetTypeByWhere(p => p.f_type == (int)sTypeEnum.床位类型);
            return View(model);
        }

        /// <summary>
        /// 添加动作
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(BedLinen model)
        {
            int e_Id = base._session.GetCurrUser().Id;
            ErrorEnum resut = ErrorEnum.Error;
            if (e_Id != 0)
            {
                model.f_eid = e_Id;
                if (DateTime.Now.Day >= 1 && DateTime.Now.Day <= 9)
                {
                    model.f_RegistrationDate = DateTime.Now;
                }
                else
                {
                    model.f_RegistrationDate = DateTime.Now.AddMonths(1);
                }
                if (model.f_Id > 0)
                {
                    resut = _BLS.EditBedLinenOneData(model);
                }
                else
                {
                    resut = _BLS.AddBedLinenOneData(model);
                }
            }
            else
            {
                return Json(ErrorEnum.NickNameIsNull);
            }
            return Json(resut);
        }

        /// <summary>
        /// 查询个人宿舍打扫信息
        /// </summary>
        /// <param name="dmw"></param>
        /// <param name="iPageIndex"></param>
        /// <param name="iPageSize"></param>
        /// <returns></returns>
        public ActionResult HostelCleanList(HostelCleanDto dmw, int iPageIndex = 1, int iPageSize = 15)
        {
            int e_Id = base._session.GetCurrUser().Id;
            List<t_HostelClean> ohostelist = new List<t_HostelClean>();
            Employee oUser = _employeeService.GetEmployeeById(e_Id);
            int count = 0;
            if (oUser.f_dormitoryId != null && oUser.f_dormitoryId != 0)
            {
                dmw.f_Community = oUser.t_Dormitory.f_Community;
                dmw.f_Building = oUser.t_Dormitory.f_Building;
                dmw.f_RoomNo = oUser.t_Dormitory.f_RoomNo;
                ohostelist = _HCS.GetConditionByWhere(u =>
               (u.t_dormitory.f_Community.ToUpper().Contains(dmw.f_Community.ToUpper().Trim())) &&
               (u.t_dormitory.f_Building.ToUpper().Contains(dmw.f_Building.ToUpper().Trim())) &&
               (u.t_dormitory.f_RoomNo.Contains(dmw.f_RoomNo.Trim()))
              , iPageIndex, iPageSize, out count).ToList();
            }
            ViewBag.iPageIndex = iPageIndex;
            ViewBag.iPageSize = iPageSize;
            ViewBag.iCount = count;
            ViewBag.DMW = dmw;
            return View(ohostelist);
        }
    }
}