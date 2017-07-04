using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MI.Application.OASession;
using TSOfficeSystem_New.Controllers;
using MI.Application;
using MI.Application.Dto;
using MI.Core.Proxy;
using MI.Core.Domain;
using MI.Application.OASession.Dto;
using MI.Application.File;
using System.IO;

namespace TSOfficeSystem_New.Areas.Integrated.Controllers
{
    /// <summary>
    /// 现金登陆
    /// </summary>
    [AjaxFunc("/Integrated")]
    public class CashRegisterController : BaseController
    {
        private readonly ISTypeService _typeService;
        private readonly ICashRegisterService _cashRegisterService;
        private readonly ICashRegisterFileService _fileService;
        public CashRegisterController(
            ICashRegisterService cashRegisterService,
            ICashRegisterFileService fileService,
            ISTypeService typeService,
            ISession session
            , IEmployeeService emoloyee) : base(session, emoloyee)
        {
            _cashRegisterService = cashRegisterService;
            _cashRegisterService = cashRegisterService;
            _fileService = fileService;
            _typeService = typeService;
        }
        public ActionResult Index(CashRegisterPagedInputDto input)
        {
            ViewBag.InputSearch = input;
            //据点
            ViewBag.Stronghold = _typeService.GetTypeByWhere(u => u.f_type == (int)sTypeEnum.上班地点类型 && (!string.IsNullOrEmpty(u.f_Remark)) && u.f_Remark.Contains("现金"));
            //汇率信息
            ViewBag.Rate = _cashRegisterService.GetRates();
            return View(_cashRegisterService.GetPagedCashRegisterDatas(input));
        }
        /// <summary>
        /// 根据id获取CashRegister
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxGetOrPost(MI.Core.Proxy.Type.Post)]
        public JsonResult GetCashRegisterById(int id = 0)
        {
            if (id == 0)
            {
                return Json(new
                {
                    status = 0
                });
            }
            CashRegister model = _cashRegisterService.GetObjectById(id);
            return Json(new
            {
                CId=model.f_CId,
                Date=model.f_Date?.ToString("yyyy-MM-dd")??string.Empty,
                Items=model.f_Items,
                Income=model.f_Income,
                Expenses=model.f_Expenses,
                HasReceipt=model.f_HasReceipt,
                Remark=model.f_Remark,
                Handled_f_Eid=model.f_Handled_f_Eid
            });
        }
        /// <summary>
        /// 添加修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxGetOrPost(MI.Core.Proxy.Type.Post)]
        public ActionResult AddOrEdit(CashRegister model)
        {
            int result = 0;
            OAUser loginUser = base._session.GetCurrUser();

            if (model.f_CId > 0)
            {
                result = _cashRegisterService.UpdateObject(model);
            }
            else
            {
                model.f_Handled_f_Eid = loginUser.Id;
                result = _cashRegisterService.AddObject(model);
            }
            return Json(1);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxGetOrPost(MI.Core.Proxy.Type.Post)]
        public ActionResult Delete(int id)
        {
            int result = _cashRegisterService.DeleteObject(id);

            return Json(result);
        }
        /// <summary>
        /// 保持汇率信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AjaxGetOrPost(MI.Core.Proxy.Type.Post)]
        public ActionResult UpdateRate(RateInputDto input)
        {
            return Json(_cashRegisterService.UpdateRate(input));
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <returns></returns>
        public FileResult ExportExcel(CashRegisterPagedInputDto input)
        {
            ExcelDto dto = _fileService.ExcelExport(input);

            string path = Path.Combine(HttpContext.Server.MapPath("~/Temp/Downloads"), dto.FileName);

            return File(path, "application/vnd.ms-excel", dto.FileName);
        }
    }
}