using MI.Application;
using MI.Application.OASession;
using MI.Core.Common;
using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSOfficeSystem_New.Controllers;

namespace TSOfficeSystem_New.Areas.Dormitory.Controllers
{
    public class ModifyRecordController: BaseController
    {
        IModifyRecordService _CMR;
        IEmployeeService _employeeInfoService;
        public ModifyRecordController(IModifyRecordService cmr,ISession session, IEmployeeService employeeInfoService) : base(session, employeeInfoService)
        {
            _CMR = cmr;
            _employeeInfoService = employeeInfoService;
        }
        // GET: Dormitory/ModifyRecord
        public ActionResult Index(ModifyRecordWhere modify)
        {
            ViewBag.Modify = modify;
            return View();
        }
        /// <summary>
        /// 获取操作记录
        /// </summary>
        /// <returns></returns>
        public ActionResult HtmlView(ModifyRecordWhere modify, int Count, int iPageIndex = 1, int iPageSize = 15)
        {
            int count = 0;
            List<t_ModifyRecord> qr;
            if (modify.isValid)
            {
                qr = _CMR.GetModifyRecordByWhere(p =>
                (modify.f_ChangeTime == DateTime.MinValue || p.f_ChangeTime.Date >= modify.f_ChangeTime.Date)
                && (modify.f_EndChangeTime == DateTime.MinValue || p.f_ChangeTime.Date <= modify.f_EndChangeTime.Date)
                && (modify.f_ActionStatus == 0 || (p.f_ActionStatus == modify.f_ActionStatus))
                && (modify.f_ItemCategory == 0 || (p.f_ItemCategory == modify.f_ItemCategory))
                && (string.IsNullOrWhiteSpace(modify.f_Content) || ((p.f_OldData ?? "").Contains(modify.f_Content) || (p.f_NewData ?? "").Contains(modify.f_Content))), iPageIndex, iPageSize, out count).ToList();
            }
            else
            {
                qr = _CMR.GetModifyRecordAllData(iPageIndex, iPageSize, out count).ToList();
            }
            ViewBag.count = Count;
            return View(qr);
        }
        public ActionResult Recovery(int Id)
        {
            ErrorEnum resut = ErrorEnum.Error;
            resut = _CMR.Recovery(Id);
            return Json(resut);
        }
    }
}