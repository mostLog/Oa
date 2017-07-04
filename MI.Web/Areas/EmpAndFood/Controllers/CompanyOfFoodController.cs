using MI.Application;
using System.Web.Mvc;
using MI.Core.Proxy;
using MI.Web.Controllers;
using MI.Application.OASession;
using MI.Application.Dto;
using System;
using MI.Core.Domain;

namespace MI.Web.Areas.EmpAndFood.Controllers
{
    [AjaxFunc("/EmpAndFood")]
    public class CompanyOfFoodController : BaseController
    {
        //餐饮管理服务
        private readonly ICompanyOfFoodService _service;
        private readonly ISTypeService _stypeService;
        public CompanyOfFoodController(
            ICompanyOfFoodService service, 
            ISTypeService stypeService,
            ISession session, IEmployeeService emoloyee) : base(session, emoloyee)
        {
            _service = service;
            _stypeService = stypeService;
        }
        public ActionResult Index()
        {
            ViewBag.WorkAddressCount = _stypeService.GetTypeByWhere(p => p.f_type == (int)sTypeEnum.上班地点类型 && p.f_Remark != null && p.f_Remark.Contains("餐饮")).Count;
            return View(_service.GetCompanyOfFoods());
        }
        /// <summary>
        /// 宿舍订餐
        /// </summary>
        /// <returns></returns>
        public ActionResult DormitoryOrderingIndex(DorOrderPagedInputDto input)
        {
            ViewBag.InputSearch = input;
            ViewBag.OrderType = _stypeService.GetsTypeByWhere((int)sTypeEnum.订餐类型);
            ViewBag.StatDate = _service.GetDistinctDateByOrderingEmployeeCollectHistory();
            return View(_service.GetDorOrderPagedAllDatas(input));
        }
        /// <summary>
        /// 员工订餐统计(sp)
        /// </summary>
        /// <returns></returns>
        public ActionResult GetEmployeeOfFood(DateTime date)
        {
            date = date.Date;
            var data = _service.GetOrderingEmployeeCollectHistory(date);
            return View(data);
        }

        [HttpPost]
        [AjaxGetOrPost(MI.Core.Proxy.Type.Post)]

        public JsonResult UpdateCompanyOfFood(CompanyOfFood model)
        {
            _service.UpdateCompanyOfFood(model);
            return Json(1);
        }
        /// <summary>
        /// 统计信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AjaxGetOrPost(MI.Core.Proxy.Type.Post)]
        public ActionResult StatInfo()
        {
            int result=_service.StatInformation();
            return Json(result);
        }
    }
}