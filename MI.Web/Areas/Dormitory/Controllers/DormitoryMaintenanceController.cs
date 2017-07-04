using MI.Application;
using MI.Application.OASession;
using MI.Application.OASession.Dto;
using MI.Core;
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
    /// <summary>
    /// 作者： 吕秀峰 
    /// 创始时间：2017-06-22
    /// 描述：宿舍维修Controller
    public class DormitoryMaintenanceController : BaseController
    {
        public IDormitoryMaintenanceService _Crs;
        public IDormitoryService _Dormitory;
        public ISTypeService _sType;
        public DormitoryMaintenanceController(
            IDormitoryMaintenanceService crs,
            IDormitoryService dormitory, 
            ISTypeService sType, 
            ISession session, IEmployeeService emoloyee) : base(session, emoloyee)
        {
            _Crs = crs;
            _Dormitory = dormitory;
            _sType = sType;

        }
        // GET: Dormitory/DormitoryMaintenance
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="dmw"></param>
        /// <param name="iPageIndex"></param>
        /// <param name="iPageSize"></param>
        /// <returns></returns>
        public ActionResult Index(DormitoryMaintenanceWhere dmw, int iPageIndex = 1, int iPageSize = 15)
        {
            int count = 0;
            var list = dmw.isValid ? _Crs.GetConditionByWheres(u =>
              (dmw.f_Community == null || u.t_dormitory.f_Community.ToUpper().Contains(dmw.f_Community.ToUpper().Trim())) &&
              (dmw.f_Building == null || u.t_dormitory.f_Building.ToUpper().Contains(dmw.f_Building.ToUpper().Trim())) &&
              (dmw.f_RoomNo == null || u.t_dormitory.f_RoomNo.Contains(dmw.f_RoomNo.Trim())) &&
              (dmw.f_operator == null || u.f_operator.Contains(dmw.f_operator.Trim())) &&
              (dmw.f_serviceWay == null || u.f_serviceWay == dmw.f_serviceWay),
               iPageIndex, iPageSize, out count).ToList()
            : _Crs.GetDormitoryMaintenanceAllData(iPageIndex, iPageSize, out count);
            ViewBag.iPageIndex = iPageIndex;
            ViewBag.iPageSize = iPageSize;
            ViewBag.iCount = count;
            ViewBag.DMW = dmw;
            //宿舍对接配置
            ViewBag.ButtItem = _sType.GetTypeByWhere(p => p.f_type == (int)sTypeEnum.宿舍维修对接);
            return View(list);
        }
        /// <summary>
        /// 打印列表
        /// </summary>
        /// <param name="dmw"></param>
        /// <param name="iPageIndex"></param>
        /// <param name="iPageSize"></param>
        /// <returns></returns>
        public ActionResult PrintListView(DormitoryMaintenanceWhere dmw, int iPageIndex = 1, int iPageSize = 9999)
        {
            int count = 0;
            var list = dmw.isValid ? _Crs.GetConditionByWheres(u =>
             (dmw.f_Community == null || u.t_dormitory.f_Community.ToUpper().Contains(dmw.f_Community.ToUpper().Trim())) &&
             (dmw.f_Building == null || u.t_dormitory.f_Building.ToUpper().Contains(dmw.f_Building.ToUpper().Trim())) &&
             (dmw.f_RoomNo == null || u.t_dormitory.f_RoomNo.Contains(dmw.f_RoomNo.Trim())) &&
             (dmw.f_operator == null || u.f_operator.Contains(dmw.f_operator.Trim())) &&
             (dmw.f_serviceWay == null || u.f_serviceWay == dmw.f_serviceWay),
              iPageIndex, iPageSize, out count).ToList()
           : _Crs.GetDormitoryMaintenanceAllData(iPageIndex, iPageSize, out count);
            ViewBag.iPageIndex = iPageIndex;
            ViewBag.iPageSize = iPageSize;
            ViewBag.iCount = count;
            ViewBag.DMW = dmw;
            //宿舍对接配置
            ViewBag.ButtItem = _sType.GetTypeByWhere(p => p.f_type == (int)sTypeEnum.宿舍维修对接);
            return View(list);
        }
        public ActionResult Edit(int Id = 0)
        {
            DormitoryMaintenance model;
            //新增
            if (Id == 0)
            {
                model = new DormitoryMaintenance
                {
                    f_Fitness = false,
                    //  f_Registrant = m_NickName,
                    t_dormitory = new t_Dormitory()
                };
            }
            //修改
            else
            {
                model = _Crs.GetDormitoryMaintenanceById(Id);
            }
            //社区下拉框
            ViewBag.LCommunity = _Dormitory.GetTariffbyCommunity();
            //楼栋下拉框
            ViewBag.Lbuilding = _Dormitory.GetTariffbyCommunity(model.t_dormitory.f_Community);
            //房间号下拉框
            ViewBag.LRoomNo = _Dormitory.GetTariffbyBuilding(model.t_dormitory.f_Community, model.t_dormitory.f_Building);
            //宿舍对接配置
            ViewBag.ButtItem = _sType.GetTypeByWhere(p => p.f_type == (int)sTypeEnum.宿舍维修对接);
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
            int mDorId = 0;
            if (dormi != null)
            {
                mDorId = dormi.f_DormitoryId;
            }
            return Json(mDorId);
        }
        [HttpPost]
        public ActionResult UptateAndAdd(DormitoryMaintenance model)
        {
            ErrorEnum resut = ErrorEnum.Error;
            OAUser loginUser = base._session.GetCurrUser();
            model.f_operator = loginUser.NickName;
            model.f_operatorTime = System.DateTime.Now;
            if (model.f_Id > 0)
            {
                _Crs.EditDormitoryMaintenanceOneData(model);
                resut = ErrorEnum.Success;
            }
            else
            {
                _Crs.AddDormitoryMaintenanceOneData(model);
                resut = ErrorEnum.Success;
            }
            return Json(resut);
        }
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            ErrorEnum resut = ErrorEnum.Error;
            _Crs.DeleteDormitoryMaintenance(Id);
            //if (_Crs.Commit() > 0)
                resut = ErrorEnum.Success;
            return Json(resut);
        }
    }
}