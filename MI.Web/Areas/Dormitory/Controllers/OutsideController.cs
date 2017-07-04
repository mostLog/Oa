using MI.Application;
using MI.Application.OASession;
using MI.Core.Common;
using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSOfficeSystem_New.Controllers;

namespace TSOfficeSystem_New.Areas.Dormitory.Controllers
{
    public class OutsideController : BaseController
    {
        public IOutsideService _Oss;
        public IDormitoryService _Dormitory;
        public IEmployeeService _Employee;
        private readonly ISTypeService _sTypeService;
        /// <summary>
        /// 订餐服务
        /// </summary>
        private readonly IOrderingEmployeesService _orderingEmployeesService;
        public OutsideController(IOutsideService oss, IDormitoryService dormitory,
           IEmployeeService employee, ISTypeService sTypeService,
            IOrderingEmployeesService orderingEmployeesService ,ISession session, IEmployeeService emoloyee) : base(session, emoloyee)
        {
            _Oss = oss;
            _Dormitory = dormitory;
            _Employee = employee;
            _sTypeService = sTypeService;
            _orderingEmployeesService = orderingEmployeesService;
        }
        // GET: Dormitory/Outside
        public ActionResult Index(OutsideWhere ow, int iPageIndex = 1, int iPageSize = 15)
        {
            int count = 0;
            var list = ow.isValid ? _Oss.GetConditionByWhere(u =>
                   (ow.Name==null || ((u.t_employeeInfo.f_chineseName.Contains(ow.Name.Trim()) ) ||
                                                           (u.t_employeeInfo.f_nickname.ToUpper().Contains(ow.Name.ToUpper().Trim()) ) ||
                                                           (u.t_employeeInfo.f_passportName.ToUpper().Contains(ow.Name.ToUpper().Trim())))
              ), iPageIndex, iPageSize, out count).ToList()
            : _Oss.GetOutsideAllData(iPageIndex, iPageSize, out count);
            ViewBag.iPageIndex = iPageIndex;
            ViewBag.iPageSize = iPageSize;
            ViewBag.iCount = count;
            ViewBag.OW = ow;
            return View(list);
        }
        public ActionResult Edit(int Id = 0)
        {
            t_Outside model;
            string mDept = "";
            string mOldDorm = "";
            //新增
            if (Id == 0)
            {
                model = new t_Outside
                {
                    f_FilingDate = DateTime.Now,
                    f_Registrant = m_NickName,
                    t_employeeInfo = new Employee(),
                    t_employeeInfo1 = new Employee(),
                    t_dormitory = new t_Dormitory()
                };
            }
            //修改
            else
            {
                model = _Oss.GetOutsideById(Id);
                if (model.f_DormitoryId == null)
                {
                    mOldDorm = "无";
                    model.t_dormitory = new t_Dormitory();
                }
                else
                {
                    t_Dormitory dorm = _Dormitory.GetDormitoryById((int)model.f_DormitoryId);
                    mOldDorm = $"{dorm.f_Community}/{dorm.f_Building}/{dorm.f_RoomNo}";
                }

                IList<SType> lstype = _sTypeService.GetTypeByWhere(p => (p.f_type == (int)sTypeEnum.部门类型) && p.f_tID == model.f_DeptId);
                mDept = lstype[0].f_value;
            }

            ViewBag.RoomNoLaundry = mOldDorm;
            ViewBag.DeptName = mDept;
            ////原社区下拉框
            //ViewBag.LCommunity = Dormitory.GetTariffbyCommunity();
            ////原楼栋下拉框
            //ViewBag.Lbuilding = Dormitory.GetTariffbyCommunity(model.t_dormitory.f_Community);
            ////原房间号下拉框
            //ViewBag.LRoomNo = Dormitory.GetTariffbyBuilding(model.t_dormitory.f_Community, model.t_dormitory.f_Building);
            return View(model);

        }
        /// <summary>
        /// 根据社区查询楼栋
        /// </summary>
        /// <param name="community">社区</param>
        [HttpPost]
        public ActionResult Community(string community)
        {
            return Json(_Dormitory.GetTariffbyCommunity(community));
        }
        /// <summary>
        /// 根据社区，楼栋查询社区的所有房间号
        /// </summary>
        /// <param name="community">社区</param>
        /// <param name="building">楼栋</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Building(string community, string building)
        {
            return Json(_Dormitory.GetTariffbyBuilding(community, building));
        }
        /// <summary>
        /// 根据社区,楼栋,房间号查询房间id
        /// </summary>
        /// <param name="community">社区</param>
        /// <param name="building">楼栋</param>
        /// <param name="roomno">房间号</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RoomNo(string community, string building, string roomno)
        {
            t_Dormitory dormi = _Dormitory.GetTariffbyRoomNo(community, building, roomno);
            int m_dorId = 0;
            if (dormi != null)
            {
                m_dorId = dormi.f_DormitoryId;
            }
            return Json(m_dorId);
        }
        [HttpPost]
        public ActionResult UptateAndAdd(t_Outside model)
        {
            ErrorEnum resut = ErrorEnum.Error;
            model.f_operator = m_NickName;
            model.f_operatorTime = System.DateTime.Now;
            if (model.f_Id > 0)
            {
                if (_Oss.EditOutsideOneData(model) > 0) {
                    Employee emp = _Employee.GetEmployeeById(model.f_eid);
                    emp.f_dormitoryId = null;
                    _Employee.UpdateEmployeeInfo(emp);
                    _orderingEmployeesService.AllClear(model.f_eid);
                    resut = ErrorEnum.Success;
                };

            }
            else
            {
                if (_Oss.AddOutsideOneData(model)>0) {
                    Employee emp = _Employee.GetEmployeeById(model.f_eid);
                    emp.f_dormitoryId = null;
                    _Employee.UpdateEmployeeInfo(emp);
                    _orderingEmployeesService.AllClear(model.f_eid);
                    resut = ErrorEnum.Success;
                }

            }              
            return Json(resut);
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            ErrorEnum resut = ErrorEnum.Error;
            if (_Oss.DeleteOutside(Id)>0) {
                resut = ErrorEnum.Success;
            }
            return Json(resut);
        }

        /// <summary>
        /// 快速获取人员信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetNames(string query)
        {
            List<Employee> emp = _Employee.GetNames(query);
            var rst = emp.Select(u => new {
                id = u.f_eid,
                name =
                $"{u.f_chineseName}--{u.f_nickname}--{u.f_passportName}"
            });
            return Json(new { success = true, data = rst });
        }

        /// <summary>
        /// 根据员工ID关联部门
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        public JsonResult GetDept(int eid)
        {
            Employee emp = _Employee.GetEmployeeById(eid);
            #region  待扩展时用
            //if (emp.f_department_tID == null)
            //{
            //    var rst = new { id = 0, name = "无" };
            //    return Json(new { success = true, data = rst });
            //}
            //else
            //{
            //    List<t_sType> lstype = STypeService.QueryByCondition(p => (p.f_type ==(int) sTypeEnum.部门类型) &&  p.f_tID==emp.f_department_tID);
            //    var rst = new { id = emp.f_department_tID, name =lstype[0].f_value };
            //    return Json(new { success = true, data = rst });
            //}
            #endregion
            IList<SType> lstype = _sTypeService.GetTypeByWhere(p => (p.f_type == (int)sTypeEnum.部门类型) && p.f_tID == emp.f_department_tID);
            var rst = new { id = emp.f_department_tID, name = lstype[0].f_value };
            return Json(new { success = true, data = rst });
        }

        /// <summary>
        /// 根据员工ID关联宿舍
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetDormityId(int eid)
        {
            Employee emp = _Employee.GetEmployeeById(eid);
            if (emp.f_dormitoryId == null)
            {
                var rst = new { id = 0, name = "无" };
                return Json(new { success = true, data = rst });
            }
            else
            {
                t_Dormitory dorm = _Dormitory.GetDormitoryById(emp.f_dormitoryId.Value);
                var rst = new { id = dorm.f_DormitoryId, name = $"{dorm.f_Community}/{dorm.f_Building}/{dorm.f_RoomNo}" };
                return Json(new { success = true, data = rst });
            }
        }
    }
}