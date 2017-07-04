using MI.Application;
using MI.Application.Dto;
using MI.Application.File;
using MI.Application.OASession;
using MI.Application.OASession.Dto;
using MI.Core.Domain;
using MI.Core.Proxy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using TSOfficeSystem_New.Controllers;

namespace TSOfficeSystem_New.Areas.Dormitory.Controllers
{
    [AjaxFunc("/Dormitory")]
    public class GrantController : BaseController
    {
        //补助服务
        private readonly IGrantService _grantService;
        //员工信息服务
        private readonly IEmployeeService _employeeService;
        //文件服务
        private readonly IGrantFileService _fileService;
        public GrantController(
            IGrantService grantService,
            IGrantFileService fileService,
            IEmployeeService employeeService,
            ISession session, IEmployeeService emoloyee) : base(session, emoloyee)
        {
            _grantService = grantService;
            _fileService = fileService;
            _employeeService = employeeService;
        }
        public ActionResult Index(GrantPagedInputDto input)
        {
            //搜索条件
            ViewBag.InputSearch = input;

            return View(_grantService.GetPagedAllGrants(input));
        }
        /// <summary>
        /// 根据id获取Grant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxGetOrPost(MI.Core.Proxy.Type.Post)]
        public JsonResult GetGrantById(int id = 0)
        {
            if (id == 0)
            {
                return Json(new
                {
                    status = 0
                });
            }
            Grant model = _grantService.GetObjectById(id);
            return Json(new
            {
                Id = model.f_GrantId,
                ChineseName = model.t_employeeInfo?.f_chineseName,
                EId = model.t_employeeInfo?.f_eid,
                Amount = model.f_amount,
                GrantDate = model.f_GrantDate.ToString("yyyy-MM-dd"),
                IsPayment = model.f_IsPayment,
                Payee = model.f_Payee,
            });
        }
        /// <summary>
        /// 添加修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxGetOrPost(MI.Core.Proxy.Type.Post)]
        public ActionResult AddOrEdit(Grant model)
        {
            int result = 0;
            OAUser loginUser = base._session.GetCurrUser();
            model.f_operator = loginUser.NickName;
            model.f_operatorTime = DateTime.Now;
           
            if (model.f_GrantId > 0)
            {
                result = _grantService.UpdateObject(model);
            }
            else
            {
                model.f_Payee = loginUser.NickName;
                result = _grantService.AddObject(model);
            }

            return Json(result);
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
            int result = _grantService.DeleteObject(id);

            return Json(result);
        }

        [HttpPost]
        [AjaxGetOrPost(MI.Core.Proxy.Type.Post)]
        public JsonResult GetNames(string query)
        {
            IList<Employee> emp = _employeeService.GetEmployeeByName(query);
            var rst = emp.Select(u => new
            {
                id = u.f_eid,
                name =
                $"{u.f_chineseName}--{u.f_nickname}--{u.f_passportName}"
            });
            return Json(new { success = true, data = rst });
        }
        [HttpPost]
        [AjaxGetOrPost(MI.Core.Proxy.Type.Post)]
        public JsonResult GenerateLastMonthDatas()
        {
            return Json(_grantService.GenerateCurrentMonthData());
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <returns></returns>
        public FileResult ExportExcel(GrantPagedInputDto input)
        {
            ExcelDto dto = _fileService.ExcelExport(input);
            string path = Path.Combine(HttpContext.Server.MapPath("~/Temp/Downloads"), dto.FileName);
            return File(path, "application/vnd.ms-excel", dto.FileName);
        }
    }
}