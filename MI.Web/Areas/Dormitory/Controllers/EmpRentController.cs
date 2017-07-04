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
    /// <summary>
    /// 员工租房
    /// 
    /// 小白 2017-06-12
    /// </summary>
    [AjaxFunc("/Dormitory")]
    public class EmpRentController : BaseController
    {
        //员工租房服务
        private readonly IEmpRentService _empRentService;
        private readonly IEmpRentFileService _excelExportService;
        //字典表服务
        private readonly ISTypeService _sTypeService;
        //楼栋服务
        private readonly IDormitoryService _dormitoryService;
        //员工信息服务
        private readonly IEmployeeService _employeeService;
        public EmpRentController(
            IEmpRentService empRentService, 
            ISTypeService sTypeService,
            IDormitoryService dormitoryService,
            IEmployeeService employeeService,
            IEmpRentFileService excelExportService,
            ISession session, IEmployeeService emoloyee) : base(session, emoloyee)
        {
            _empRentService = empRentService;
            _sTypeService = sTypeService;
            _dormitoryService = dormitoryService;
            _employeeService = employeeService;
            _excelExportService = excelExportService;
        }
        public ActionResult Index(EmpRentPagedInputDto input)
        {
            ViewBag.WorkLoaction = _sTypeService.GetsTypeByWhere((int)sTypeEnum.上班地点类型);
            ViewBag.SearchInput = input;
            //返回分页数据集合
            return View(_empRentService.GetEmpRentAllData(input));
        }
        /// <summary>
        /// 搜索功能
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[HttpPost]
        //public ActionResult Index(EmpRentPagedInputDto input)
        //{
        //    ViewBag.SearchInput = input;
        //    ViewBag.WorkLoaction = _sTypeService.GetsTypeByWhere((int)sTypeEnum.上班地点类型);
        //    return View("Index", _empRentService.GetEmpRentAllData(input));
        //}
        /// <summary>
        /// 根据id获取EmpRent
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxGetOrPost(MI.Core.Proxy.Type.Post)]
        public JsonResult GetEmpRentById(int id=0)
        {
            if (id==0)
            {
                return Json(new {
                    status=0
                });
            }
            EmpRent model= _empRentService.GetObjectById(id);
            t_Dormitory dormitory = model.t_dormitory;
            var workLocations=_sTypeService.GetsTypeByWhere((int)sTypeEnum.上班地点类型);
           
            return Json(new {
                Id=model.f_Id,
                ChineseName=model.t_employeeInfo?.f_chineseName,
                EId=model.t_employeeInfo?.f_eid,
                RoomNoLaundry= dormitory!=null? $"{dormitory.f_Community}/{dormitory.f_Building}/{dormitory.f_RoomNo}":string.Empty,
                DormitoryId=model.f_DormitoryId,
                Rent=model.f_Rent,
                Grant=model.f_Grant,
                Amount=model.f_Amount,
                PaymentDate=model.f_PaymentDate.Value.ToString("yyyy-MM-dd"),
                IsPayment=model.f_IsPayment,
                Payee=model.f_Payee,
                AddressId= model.f_AddressId,
                Remark=model.f_Remark
            });
        }
        [HttpPost]
        [AjaxGetOrPost(MI.Core.Proxy.Type.Post)]
        public ActionResult AddOrEdit(EmpRent model)
        {
            int result = 0;
            OAUser loginUser = base._session.GetCurrUser();
            model.f_operator = loginUser.NickName;
            model.f_operatorTime = DateTime.Now;
            if (model.f_AddressId == 0)
            {
                model.f_AddressId = null;
            }
            if (model.f_Id > 0)
            {
                model.f_EffectiveDate = DateTime.Now.Date;
                result=_empRentService.UpdateObject(model);
            }
            else
            {
                result=_empRentService.AddObject(model);
            }
           
            return Json(result);
        }
        [HttpPost]
        [AjaxGetOrPost(MI.Core.Proxy.Type.Post)]
        public ActionResult Delete(int id)
        {
            int result=_empRentService.DeleteObject(id);
            
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
        /// 根据员工ID关联宿舍
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxGetOrPost(MI.Core.Proxy.Type.Post)]
        public JsonResult GetDormityId(int eid)
        {
            Employee emp = _employeeService.GetEmployeeById(eid);
            if (emp.f_dormitoryId == null)
            {
                var rst = new { id = 0, name = "无" };
                return Json(new { success = true, data = rst });
            }
            else
            {
                t_Dormitory dorm = _dormitoryService.GetDormitoryById(emp.f_dormitoryId.Value);
                var rst = new { id = dorm.f_DormitoryId, name = $"{dorm.f_Community}/{dorm.f_Building}/{dorm.f_RoomNo}" };
                return Json(new { success = true, data = rst });
            }
        }
        [HttpPost]
        [AjaxGetOrPost(MI.Core.Proxy.Type.Post)]
        public JsonResult MakeUpCurrMonthDatas()
        {
            return Json(_empRentService.MakeUpCurrMonthDatas());
        }
        [HttpPost]
        [AjaxGetOrPost(MI.Core.Proxy.Type.Post)]
        public JsonResult GenerateNextMonthDatas()
        {
            return Json(_empRentService.GenerateNextMonthDatas());
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <returns></returns>
        public FileResult ExportExcel(EmpRentPagedInputDto input)
        {
            ExcelDto dto= _excelExportService.ExcelExport(input);

            string path = Path.Combine(HttpContext.Server.MapPath("~/Temp/Downloads"),dto.FileName);

            return File(path, "application/vnd.ms-excel", dto.FileName);
        }

    }
}