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
    public class FlightFeeController : BaseController
    {
        //机票差额
        private readonly IFlightFeeService _flightFeeService;
        //文件服务
        private readonly IFlightFeeFileService _fileService;
        //类型
        private readonly ISTypeService _typeService;
        //人员信息
        private readonly IEmployeeService _employeeService;
        public FlightFeeController(
            IFlightFeeService flightFeeService,
            IFlightFeeFileService fileService,
            ISTypeService typeService,
            IEmployeeService employeeService,
            ISession session, IEmployeeService emoloyee) : base(session, emoloyee)
        {
            _flightFeeService = flightFeeService;
            _fileService = fileService;
            _typeService = typeService;
            _employeeService = employeeService;
        }
        public ActionResult Index(FlightFeePagedInputDto input)
        {
            ViewBag.InputSearch = input;
            ViewBag.WorkLocation = _typeService.GetsTypeByWhere((int)sTypeEnum.上班地点类型);
            return View(_flightFeeService.GetPagedFlightFeeAllDatas(input));
        }
        /// <summary>
        /// 根据id获取FlightFee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxGetOrPost(MI.Core.Proxy.Type.Post)]
        public JsonResult GetFlightFeeById(int id = 0)
        {
            if (id == 0)
            {
                return Json(new
                {
                    status = 0
                });
            }
            FlightFee model = _flightFeeService.GetObjectById(id);
            return Json(new
            {
                ID=model.f_ID,
                EId=model.f_eid,
                ChineseName = model.t_employeeInfo?.f_chineseName ?? string.Empty,
                Amount=model.f_Amount,
                FlightDate=model.f_FlightDate.ToString("yyyy-MM-dd"),
                Flight=model.f_Flight,
                StartEndAdd=model.f_StartEndAdd,
                IsPay=model.f_IsPay,
                AddressId=model.f_AddressId,
                Remark=model.f_Remark
            });
        }
        /// <summary>
        /// 添加修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxGetOrPost(MI.Core.Proxy.Type.Post)]
        public ActionResult AddOrEdit(FlightFee model)
        {
            int result = 0;
            OAUser loginUser = base._session.GetCurrUser();
            model.f_operator = loginUser.NickName;
            model.f_operatorTime = DateTime.Now;

            if (model.f_ID > 0)
            {
                result = _flightFeeService.UpdateObject(model);
            }
            else
            {
                result = _flightFeeService.AddObject(model);
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
            int result = _flightFeeService.DeleteObject(id);

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
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <returns></returns>
        public FileResult ExportExcel(FlightFeePagedInputDto input)
        {
            ExcelDto dto = _fileService.ExcelExport(input);
            string path = Path.Combine(HttpContext.Server.MapPath("~/Temp/Downloads"), dto.FileName);
            return File(path, "application/vnd.ms-excel", dto.FileName);
        }
    }
}