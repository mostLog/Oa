using MI.Application;
using MI.Application.OASession;
using MI.Core.Common;
using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSOfficeSystem_New.Controllers;

namespace TSOfficeSystem_New.Areas.Dormitory.Controllers
{
    /// <summary>
    /// 作者： 吕秀峰 
    /// 创始时间：2017-06-24
    /// 描述：宿舍打扫控制器
    public class HostelCleanController : BaseController
    {
        public IHostelCleanService _Hcs;
        public IDormitoryService _Dormitory;
        public HostelCleanController(IHostelCleanService Hcs, IDormitoryService Dormitory, ISession session, IEmployeeService emoloyee) : base(session, emoloyee)
        {
            _Hcs = Hcs;
            _Dormitory = Dormitory;
        }
        // GET: Dormitory/HostelClean
        /// <summary>
        /// 首页列表
        /// </summary>
        /// <param name="dmw"></param>
        /// <param name="iPageIndex"></param>
        /// <param name="iPageSize"></param>
        /// <returns></returns>
        public ActionResult Index(HostelCleanWhere dmw, int iPageIndex = 1, int iPageSize = 15)
        {
            int count = 0;
            var list = dmw.isValid ? _Hcs.GetConditionByWhere(u =>
              (dmw.f_Community==null || u.t_dormitory.f_Community.ToUpper().Contains(dmw.f_Community.ToUpper().Trim())) &&
              (dmw.f_Building==null || u.t_dormitory.f_Building.ToUpper().Contains(dmw.f_Building.ToUpper().Trim())) &&
              (dmw.f_RoomNo==null || u.t_dormitory.f_RoomNo.Contains(dmw.f_RoomNo.Trim())) &&
              ((dmw.f_Week == 0) || (u.f_Week == dmw.f_Week))
              , iPageIndex, iPageSize, out count).ToList()
            : _Hcs.GetHostelCleanAllData(iPageIndex, iPageSize, out count);
            ViewBag.iPageIndex = iPageIndex;
            ViewBag.iPageSize = iPageSize;
            ViewBag.iCount = count;
            ViewBag.DMW = dmw;
            return View(list);
        }
        /// <summary>
        /// 打印列表
        /// </summary>
        /// <param name="dmw"></param>
        /// <param name="iPageIndex"></param>
        /// <param name="iPageSize"></param>
        /// <returns></returns>
        public ActionResult PrintListView(HostelCleanWhere dmw, int iPageIndex = 1, int iPageSize = 99999)
        {
            int count = 0;
            var list = dmw.isValid ? _Hcs.GetConditionByWhere(u =>
               (dmw.f_Community == null || u.t_dormitory.f_Community.ToUpper().Contains(dmw.f_Community.ToUpper().Trim())) &&
               (dmw.f_Building == null || u.t_dormitory.f_Building.ToUpper().Contains(dmw.f_Building.ToUpper().Trim())) &&
               (dmw.f_RoomNo == null || u.t_dormitory.f_RoomNo.Contains(dmw.f_RoomNo.Trim())) &&
               ((dmw.f_Week == 0) || (u.f_Week == dmw.f_Week))
               , iPageIndex, iPageSize, out count).ToList()
             : _Hcs.GetHostelCleanAllData(iPageIndex, iPageSize, out count);
            ViewBag.iPageIndex = iPageIndex;
            ViewBag.iPageSize = iPageSize;
            ViewBag.iCount = count;
            ViewBag.DMW = dmw;
            return View(list);
        }
        public ActionResult Edit(int Id = 0)
        {
            t_HostelClean model;
            //新增
            if (Id == 0)
            {
                model = new t_HostelClean { t_dormitory = new t_Dormitory() };
            }
            //修改
            else
            {
                model = _Hcs.GetHostelCleanById(Id);
            }
            //社区下拉框
            ViewBag.Community = _Dormitory.GetTariffbyCommunity().Select(u => new SelectListItem { Text = u, Value = "0", Selected = Id != 0 && u == model.t_dormitory.f_Community });
            //楼栋下拉框
            ViewBag.building = _Dormitory.GetTariffbyCommunity(model.t_dormitory.f_Community).Select(u => new SelectListItem { Text = u, Value = "0", Selected = Id != 0 && u == model.t_dormitory.f_Building });
            //房间号下拉框
            ViewBag.RoomNo = _Dormitory.GetTariffbyBuilding(model.t_dormitory.f_Community, model.t_dormitory.f_Building).Select(u => new SelectListItem { Text = u, Value = "0", Selected = Id != 0 && u == model.t_dormitory.f_RoomNo });
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
        /// <param name="community"></param>
        /// <param name="building"></param>
        /// <param name="roomno"></param>
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
        public ActionResult UptateAndAdd(t_HostelClean model)
        {
            ErrorEnum result = ErrorEnum.Error;
            model.f_operator = m_NickName;
            model.f_operatorTime = System.DateTime.Now;
            if (model.f_Id > 0)
            {
                if (_Hcs.EditHostelCleanOneData(model)>0) {
                    result = ErrorEnum.Success;
                }
            }
            else
            {
                if (_Hcs.AddHostelCleanOneData(model) > 0) {
                    result = ErrorEnum.Success;
                }
            }
             
            return Json(result);
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            ErrorEnum result = ErrorEnum.Error;
            if (_Hcs.DeleteHostelClean(Id)>0) {
                result = ErrorEnum.Success;
            } 
            return Json(result);
        }

        /// <summary>
        /// 导出EXCEL
        /// </summary>
        /// <param name="dorm"></param>
        /// <returns></returns>
        public FileResult Export(HostelCleanWhere dmw)
        {
            var list = dmw.isValid ? _Hcs.GetConditionByWhereExportData(u =>
           (dmw.f_Community==null || u.t_dormitory.f_Community.ToUpper().Contains(dmw.f_Community.ToUpper().Trim())) &&
              (dmw.f_Building==null || u.t_dormitory.f_Building.ToUpper().Contains(dmw.f_Building.ToUpper().Trim())) &&
              (dmw.f_RoomNo==null || u.t_dormitory.f_RoomNo.Contains(dmw.f_RoomNo.Trim())) &&
              ((dmw.f_Week == 0) || (u.f_Week == dmw.f_Week))
           ).ToList() : _Hcs.GetHostelCleanAllDataExportData();

            DataSet ds = new DataSet();
            DataTable dt = DataTableExt.ToDataTable<dynamic>(list);
            ds.Tables.Add(dt);

            string fileName = "";
            if (fileName == "")
            {
                fileName = "Export_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            }
            MemoryStream stream = NPOIExcelHelper.ExportExcel(dt, NPOIExcelHelper.ExtendPropertiesType.None, "t_HostelClean");

            return File(stream, "application/vnd.ms-excel", fileName);
        }
    }
}