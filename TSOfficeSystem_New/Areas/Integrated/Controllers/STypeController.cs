using MI.Application;
using MI.Application.OASession;
using MI.Core.Common;
using MI.Core.Domain;
using System.Web.Mvc;
using TSOfficeSystem_New.Controllers;

namespace TSOfficeSystem_New.Areas.Integrated.Controllers
{
    public class STypeController : BaseController
    {
        private readonly ISTypeService _stypeService;
        public STypeController(ISTypeService stypeService, ISession session, IEmployeeService emoloyee) : base(session, emoloyee)
        {
            _stypeService = stypeService;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="iType"></param>
        /// <returns></returns>
        // GET: Integrated/SType
        public ActionResult Index(int iType = 0)
        {
            ViewBag.iType = iType.ToString();
            if (iType == (int)sTypeEnum.上车地点配置)
            {
                ViewBag.shequ = _stypeService.GetsTypeByWhere((int)sTypeEnum.社区类型);
            }
            return View(_stypeService.GetsTypeByWhere(iType));
        }

        [HttpPost]
        public ActionResult Edit(int Id = 0)
        {
            SType model = new SType();
            model = Id == 0 ? new SType { f_type = 0 } : _stypeService.GetsTypeById(Id);
            if (model.f_type == (int)sTypeEnum.上车地点配置)
            {
                ViewBag.shequ = _stypeService.GetsTypeByWhere((int)sTypeEnum.社区类型);
            }
            return View(model);
        }

        public ActionResult UptateAndAdd(SType model)
        {
            ErrorEnum result = ErrorEnum.Error;
            if (model.f_tID > 0)
            {
                result = _stypeService.EditOneData(model);
            }
            else
            {
                result = _stypeService.AddsTypeOneData(model);
            }

            return Json(new ResultModel((int)result));
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            ErrorEnum result = ErrorEnum.Error;
            string tables = _stypeService.DeletesType(Id);
            if (string.IsNullOrWhiteSpace(tables))
            {
                result = ErrorEnum.Success;
            }
            else
            {
                result = ErrorEnum.Quote;
            }
            return Json(new ResultModel((int)result,tables,""));
        }
    }
}