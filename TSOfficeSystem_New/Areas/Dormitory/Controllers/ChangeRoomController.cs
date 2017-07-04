using MI.Application;
using MI.Application.ChangeRoomService.Dto;
using MI.Application.Dto;
using MI.Application.OASession;
using MI.Core.Common;
using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TSOfficeSystem_New.Controllers;

namespace TSOfficeSystem_New.Areas.Dormitory.Controllers
{
    /// <summary>
    /// 员工换房控制器
    /// </summary>
    public class ChangeRoomController : BaseController
    {
        public IModifyRecordService _Mse;
        public IChangeRoomService _Crs;
        // ReSharper disable once InconsistentNaming
        public IDormitoryService _Dormitory;
        // ReSharper disable once InconsistentNaming
        public IEmployeeService _Employee;
        public ChangeRoomController(IChangeRoomService crs, IDormitoryService dormitory, IEmployeeService employeeInfoService, IModifyRecordService Mse, ISession session, IEmployeeService emoloyee) : base(session, emoloyee)
        {
            _Crs = crs;
            _Dormitory = dormitory;
            _Employee = employeeInfoService;
            _Mse=  Mse;
        }
        // GET: Dormitory/ChangeRoom
        public ActionResult Index(ChangeRoomWhere cw, int iPageIndex = 1, int iPageSize = 15)
        {
            int count = 0;
            var list = cw.isValid ? _Crs.GetConditionByWhere(u =>
            (cw.Name==null || ((u.t_employeeInfo.f_chineseName.Contains(cw.Name.Trim())) || (u.t_employeeInfo.f_nickname.ToUpper().Contains(cw.Name.ToUpper().Trim())) || (u.t_employeeInfo.f_passportName.ToUpper().Contains(cw.Name.ToUpper().Trim())))
            ) && ((cw.Progress==null) || (u.f_Progress.Contains(cw.Progress))) &&
            ((cw.f_RentDateST == DateTime.MinValue) || (u.f_RentDate >= cw.f_RentDateST)) && ((cw.f_RentDateED == DateTime.MinValue) || (u.f_RentDate <= cw.f_RentDateED))
            , iPageIndex, iPageSize, out count).ToList()
            : _Crs.GetChangeRoomAllData(iPageIndex, iPageSize, out count);
            ViewBag.iPageIndex = iPageIndex;
            ViewBag.iPageSize = iPageSize;
            ViewBag.iCount = count;
            ViewBag.CW = cw;
            return View(list);
        }
        public ActionResult Edit(int Id = 0)
        {
            t_ChangeRoom model = new t_ChangeRoom();
            string mOldDorm = "";
            //新增
            if (Id == 0)
            {
                model = new t_ChangeRoom
                {
                    f_FilingDate = DateTime.Now,
                    f_RentDate = DateTime.Now,
                    t_dormitory = new t_Dormitory(),
                    t_dormitory1 = new t_Dormitory(),
                    t_employeeInfo = new Employee(),
                    f_Registrant = m_NickName
                };
            }
            //修改
            else
            {

                model = _Crs.GetChangeRoomById(Id);

                if (model.f_OldRoomID == null)
                {
                    mOldDorm = "无";
                    // model.f_OldRoomID = 0;
                    model.t_dormitory1 = new t_Dormitory();

                }
                else
                {
                    t_Dormitory dorm = _Dormitory.GetDormitoryById(model.f_OldRoomID.Value);
                    mOldDorm = $"{dorm.f_Community}/{dorm.f_Building}/{dorm.f_RoomNo}";
                }


            }
            //新房间的社区的下拉框
            ViewBag.CCommunity = _Dormitory.GetTariffbyCommunity();
            //新房间的楼栋下拉框
            ViewBag.Cbuilding = _Dormitory.GetTariffbyCommunity(model.t_dormitory?.f_Community);
            //新房间的房间号下拉框
            ViewBag.CRoomNo = _Dormitory.GetTariffbyBuilding(model.t_dormitory?.f_Community, model.t_dormitory?.f_Building);

            ViewBag.RoomNoLaundry = mOldDorm;
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
        public ActionResult UptateAndAdd(t_ChangeRoom model)
        {
            if (model.f_Progress == "已换房")
            {
                Employee oEmp = _Employee.GetEmployeeById(model.f_eid);
                oEmp.f_dormitoryId = model.f_NewRoomId;
                _Employee.UpdateEmployeeInfo(oEmp);
            }
            ErrorEnum resut = ErrorEnum.Error;
            string tips = string.Empty;
            if (model.f_NewRoomId == 0)
            {
                tips = "新房间不能为空";
                return Json(new ResultModel((int)resut, tips));
            }
            model.f_operator = m_NickName;
            model.f_operatorTime = System.DateTime.Now;
            if (model.f_Id > 0)
            {
                _Crs.EditChangeRoomOneData(model);
                resut = ErrorEnum.Success;
            }
            else
            {
                _Crs.AddChangeRoomOneData(model);
                resut = ErrorEnum.Success;
            }
            return Json(new ResultModel((int)resut, tips));
        }


        [HttpPost]
        public ActionResult Delete(int Id)
        {
            ErrorEnum resut = ErrorEnum.Error;
            _Crs.DeleteChangeRoom(Id);
            resut = ErrorEnum.Success;
            return Json(resut);
        }
        public ActionResult SelectByName(string query)
        {
            System.Collections.ArrayList list = new System.Collections.ArrayList();

            IList<Employee> emp = _Employee.SelectByName(query);
            for (int i = 0; i < emp.Count; i++)
            {
                list.Add(emp[i].f_chineseName + ";" + emp[i].f_nickname + ";" + emp[i].f_passportName);
            }
            // return View(emp);
            return this.Json(list);
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