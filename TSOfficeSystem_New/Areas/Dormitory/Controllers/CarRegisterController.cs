using MI.Application;
using MI.Application.OASession;
using MI.Core.Common;
using MI.Core.Domain;
using System.Linq;
using System.Web.Mvc;
using TSOfficeSystem_New.Controllers;

namespace TSOfficeSystem_New.Areas.Dormitory.Controllers
{
    /// <summary>
    /// 车辆管理控制器
    /// </summary>
    public class CarRegisterController : BaseController
    {

        private readonly ICategoryService _empRentService;
        private readonly ISTypeService _ISTTypeService;

        public CarRegisterController(ICategoryService empRentService, ISTypeService ISTTypeService, ISession session, IEmployeeService emoloyee) : base(session, emoloyee)
        {
            _empRentService = empRentService;
            _ISTTypeService = ISTTypeService;
        }
        // GET: Dormitory/RegisterSearch
        public ActionResult Index(int itype = 0)
        {
            ViewBag.listTypeDepartment = _ISTTypeService.GetsTypeByWhere((int)sTypeEnum.车辆类型);
            // GetAll().Where(f=>f.f_type== (int)sTypeEnum.车辆类型);
            ViewBag.itype = itype;
            var linq = itype == 0 ? _empRentService.GetAll() : _empRentService.GetAll().Where(f => f.f_CarType == itype).ToList();
            return View(linq);
        }
        public ActionResult Edit(int Id = 0)
        {
            CarRegister model = new CarRegister();
            //新增
            if (Id == 0)
            {
                model = new CarRegister { f_IsIssued = false };
            }
            //修改
            else
            {
                model = _empRentService.GetCarRegisterById(Id);
            }
            ViewBag.listTypeDepartment = _ISTTypeService.GetsTypeByWhere((int)sTypeEnum.车辆类型);
            return View(model);
        }
        [HttpPost]
        public ActionResult UptateAndAdd(CarRegister model)
        {
            ErrorEnum resut = ErrorEnum.Error;
            string sReturnChar = string.Empty;
            if (AOUnity.ValidationLength("车主信息", model.f_CarOwner, 30, out sReturnChar))
            {
                //resut = STypeError.LengthOutRange;
                return Json(new ResultModel((int)resut, sReturnChar));
            }
            if (model.f_ID > 0)
            {
                _empRentService.EditCarRegisterOneData(model);
                resut = ErrorEnum.Success;
            }
            else
            {
                _empRentService.AddCarRegisterOneData(model);
                resut = ErrorEnum.Success;
            }

            return Json(new ResultModel((int)resut));
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            CarRegister register = new CarRegister();
            register = _empRentService.GetCarRegisterById(id);
            ErrorEnum resut = ErrorEnum.Error;
            _empRentService.DeleteCarRegister(register);
             resut = ErrorEnum.Success;
            return Json(resut);
        }
    }
}