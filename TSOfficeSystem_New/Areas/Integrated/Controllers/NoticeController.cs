using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MI.Application.OASession;
using TSOfficeSystem_New.Controllers;
using MI.Application;
using MI.Core.Domain;
using MI.Core.Common;

namespace TSOfficeSystem_New.Areas.Integrated.Controllers
{
    public class NoticeController : BaseController
    {
        private readonly INoticeService _noticeService;
        public NoticeController(ISession session,
            INoticeService noticeService, IEmployeeService emoloyee) : base(session, emoloyee)
        {
            _noticeService = noticeService;
        }

        // GET: Integrated/Notice
        public ActionResult Index()
        {
            Session["notice"] = 0;
            return View(_noticeService.GetNoticeAllData());
        }
        public ActionResult Edit(int iId = 0)
        {
            //新增
            var oModel = iId == 0 ? new Notice { f_StartDate = DateTime.Now, f_EndDate = DateTime.Now } : _noticeService.GetNoticeById(iId);
            return View(oModel);
        }

        [HttpPost]
        public ActionResult UptateAndAdd(Notice model)
        {
            string m_NickName = base._session.GetCurrUser().NickName;
            ErrorEnum resut = ErrorEnum.Error;
            if (model.f_Id > 0)
            {
                if (model.f_Content.Length > 1500)
                {
                    return Json(ErrorEnum.LengthOutRange);
                }
                resut = _noticeService.EditNoticeOneData(model);
            }
            else
            {
                if (m_NickName != null)
                {
                    model.f_Registrant = m_NickName;
                }
                else
                {
                    return Json(ErrorEnum.NickNameIsNull);
                }
                if (model.f_Content.Length > 1500)
                {
                    return Json(ErrorEnum.LengthOutRange);
                }
                model.f_AddDate = DateTime.Now;
                resut = _noticeService.AddNoticeOneData(model);
            }

            return Json(resut);
        }
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            ErrorEnum resut = ErrorEnum.Error;
            resut = _noticeService.DeleteNotice(Id);
            return Json(resut);
        }
        /// <summary>
        /// 置顶
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult IsTop(int Id, string istop)
        {
            ErrorEnum resut = ErrorEnum.Error;
            resut = _noticeService.IsTop(Id, istop);
            return Json(resut);
        }
    }
}