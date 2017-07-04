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
    public class WorkDistributionController : BaseController
    {
        /// <summary>
        /// 工作交接接口
        /// </summary>
        private readonly IWorkDistributionService _workDistributionService;
        /// <summary>
        /// 人事信息接口
        /// </summary>
        private readonly IEmployeeService _employeeService;
        /// <summary>
        /// 权限类别接口
        /// </summary>
        private readonly ISTypeService _sTypeService;


        public WorkDistributionController(IWorkDistributionService workDistributionService, IEmployeeService employeeService, ISTypeService sTypeService, ISession session, IEmployeeService emoloyee) : base(session, emoloyee)
        {
            _workDistributionService = workDistributionService;
            _employeeService = employeeService;
            _sTypeService = sTypeService;
        }
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns></returns>
        // GET: Integrated/WorkDistribution
        public ActionResult Index(WorkDistributionDto model, int iPageIndex = 1, int iPageSize = 15)
        {
            int count = 0;
            var data = model.isValid ?
                _workDistributionService.GetWorkByWhere(u =>
              ((model.f_Treat == null || model.f_Treat == 0) || u.f_Treat == model.f_Treat) && (model.f_IsComplete == null || u.f_IsComplete == model.f_IsComplete) &&
              ((model.f_WorkType == 0 || u.f_WorkType == model.f_WorkType) &&
              (string.IsNullOrWhiteSpace(model.f_Registered) || u.f_Registered.Contains(model.f_Registered)) && (model.f_Id == 0 || u.f_Id == model.f_Id) &&
              (string.IsNullOrEmpty(model.f_Handover) || u.f_Handover.Contains(model.f_Handover))
              ), iPageIndex, iPageSize, out count) : _workDistributionService.GetWorkDistributionAllData(iPageIndex, iPageSize, out count);
            ViewBag.iPageIndex = iPageIndex;
            ViewBag.iPageSize = iPageSize;
            ViewBag.iCount = count;
            ViewBag.model = model;
            ViewBag.WorkType = _sTypeService.GetTypeByWhere(u => u.f_type == (int)sTypeEnum.工作类别);
            ViewBag.Emp = _employeeService.GetemployeeInfoByWhere(u => u.STypeDepartment.f_value.Contains("行政") && !(u.STypeWorkStatus.f_value.Contains("离职")));
            return View(data);
        }
        [HttpPost]
        public ActionResult Edit(int Id = 0)
        {
            WorkDistribution model = new WorkDistribution();
            model = Id == 0 ? new WorkDistribution { f_RegisterDate = DateTime.Now, f_UrgentDate = null, f_CompleteDate = null, f_IsComplete = false } : _workDistributionService.GetWorkOneDataById(Id);
            ViewBag.WorkType = _sTypeService.GetTypeByWhere(u => u.f_type == (int)sTypeEnum.工作类别);
            ViewBag.Emp = _employeeService.GetemployeeInfoByWhere(u => u.STypeDepartment.f_value.Contains("行政") && !(u.STypeWorkStatus.f_value.Contains("离职")));
            return View(model);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UptateAndAdd(WorkDistribution model)
        {
            ErrorEnum result = ErrorEnum.Error;
            if (model.f_Id > 0)
            {
                result = _workDistributionService.UpdataOneData(model);
            }
            else
            {
                result = _workDistributionService.AddWorkOneData(model);
            }
            return Json(new ResultModel((int)result));
        }
        [HttpPost]
        public ActionResult Delete(int Id) 
        {
            ErrorEnum result = _workDistributionService.DeleteById(Id);
            return Json(result);
        }
    }
}