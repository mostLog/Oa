using MI.Application;
using MI.Application.Dto;
using MI.Application.OASession;
using MI.Core.Common;
using MI.Core.Domain;
using System;
using System.Web.Mvc;
using TSOfficeSystem_New.Controllers;

namespace TSOfficeSystem_New.Areas.Integrated.Controllers
{
    public class ReturnHandoverController : BaseController
    {
        /// <summary>
        /// 工作交接接口
        /// </summary>
        private readonly IReturnHandoverService _returnHandoverService;
        /// <summary>
        /// 人事信息接口
        /// </summary>
        private readonly IEmployeeService _employeeService;
        /// <summary>
        /// 权限类别接口
        /// </summary>
        private readonly ISTypeService _sTypeService;


        public ReturnHandoverController(IReturnHandoverService returnHandoverService, IEmployeeService employeeService, ISTypeService sTypeService, ISession session, IEmployeeService emoloyee) : base(session, emoloyee)
        {
            _returnHandoverService = returnHandoverService;
            _employeeService = employeeService;
            _sTypeService = sTypeService;
        }
        // GET: Integrated/ReturnHandover
        public ActionResult Index(ReturnHandoverDto model)
        {
            var list = _returnHandoverService.GetReturnByWhere(u =>
             (string.IsNullOrWhiteSpace(model.Name)) && ((model.f_ReturnDateST == DateTime.MinValue) || (u.f_StartDate >= model.f_ReturnDateST)) && ((model.f_ReturnDateED == DateTime.MinValue) || (u.f_StartDate <= model.f_ReturnDateED)));
            ViewBag.Emp = _employeeService.GetemployeeInfoByWhere(u => u.STypeDepartment.f_value.Contains("行政") && !(u.STypeWorkStatus.f_value.Contains("离职")));
            ViewBag.RW = model;
            return View(list);
        }

        public ActionResult Edit(int Id)
        {
            var model = _returnHandoverService.GetReturnOneDataById(Id);

            ViewBag.nickname = model.t_employeeInfo.f_chineseName;
            ViewBag.Emp = _employeeService.GetemployeeInfoByWhere(u => u.STypeDepartment.f_value.Contains("行政") && !(u.STypeWorkStatus.f_value.Contains("离职")));
            return View(model);
        }

        public ActionResult Add()
        {
            var model = new ReturnHandover
            {
                f_eid = m_Eid,
                t_employeeInfo = new Employee()
            };
            ViewBag.nickname = _session.GetCurrUser().UserName;;
            ViewBag.Emp = _employeeService.GetemployeeInfoByWhere(u => u.STypeDepartment.f_value.Contains("行政") && !(u.STypeWorkStatus.f_value.Contains("离职")));
            return View(model);
        }

        public ActionResult AddWorkItem(int Id)
        {
            ReturnHandover model = new ReturnHandover();
            model = _returnHandoverService.GetReturnOneDataById(Id);

            ViewBag.nickname = model.t_employeeInfo.f_chineseName;
            ViewBag.Emp = _employeeService.GetemployeeInfoByWhere(u => u.STypeDepartment.f_value.Contains("行政"));
            return View(model);
        }

        [HttpPost]
        public ActionResult AddSubmit(ReturnHandover model)
        {
            ErrorEnum result = ErrorEnum.Error;
            string sReturnChar = string.Empty;
            if (AOUnity.ValidationLength("工作任务", model.f_WorkItem, 1500, out sReturnChar) || AOUnity.ValidationLength("当前移交进度", model.f_CurrentProgress, 100, out sReturnChar) || AOUnity.ValidationLength("处理进度及结果", model.f_AgentProcess, 500, out sReturnChar))
            {
                result = ErrorEnum.LengthOutRange;
                return Json(new ResultModel((int)result, sReturnChar));
            }
            model.f_operatorTime = DateTime.Now;
            if (model.f_Id > 0)
            {
                result = _returnHandoverService.UpdateReturnOneData(model);
            }
            else
            {
                result = _returnHandoverService.AddReturnOneData(model);
            }
            return Json(new ResultModel((int)result));
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            ErrorEnum result = _returnHandoverService.DeleteReturnOneData(Id);
            return Json(result);
        }
    }
}