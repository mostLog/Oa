using MI.Application;
using MI.Application.Dto;
using MI.Application.OASession;
using MI.Core.Common;
using MI.Core.Domain;
using MI.Web.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace MI.Web.EmpAndFood.Controllers
{
    public class StatisticsRideController : BaseController
    {
        /// <summary>
        /// 人事管理服务
        /// </summary>
        public readonly IEmployeeService IEmololyee;
        /// <summary>
        /// 类型服务
        /// </summary>
        public readonly ISTypeService ISTypeSerevice;
        public StatisticsRideController(IEmployeeService ies, ISTypeService ise,ISession session, IEmployeeService emoloyee) : base(session, emoloyee)
        {
            IEmololyee = ies;
            ISTypeSerevice = ise;
        }
        // GET: EmpAndFood/TrafficStatistics
        /// <summary>
        /// 获取员工班车数据
        /// </summary>
        /// <param name="f_department_Id">部门</param>
        /// <param name="iPageIndex"></param>
        /// <param name="iPageSize"></param>
        /// <returns></returns>
        public ActionResult Index(string f_department_Id="-", int iPageIndex = 1, int iPageSize = 50)
        {
            int iCount;
            List<Employee> lisData = f_department_Id.Trim() == "-" ? IEmololyee.GetShuttleBus(iPageIndex, iPageSize, out iCount) : IEmololyee.GetShuttleBus(Convert.ToInt32(f_department_Id), iPageIndex, iPageSize, out iCount);

            //部门
            ViewBag.listDepartment = IEmololyee.GetSectorName(p => p.f_type == (int)sTypeEnum.部门类型);
            ViewBag.iPageIndex = iPageIndex;
            ViewBag.iPageSize = iPageSize;
            ViewBag.iCount = iCount;
            ViewBag.department = f_department_Id;
            return View(lisData);
        }
        /// <summary>
        /// 获取员工班车汇总数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Satistics(string isY)
        {
            if (isY != "Y" && isY != "N")
            {
                return null;
            }
            string[] arrCommunity = IEmololyee.GetSectorName(p => p.f_type == (int)sTypeEnum.社区类型 && (p.f_Remark ?? "").Contains("true")).Select(p => p.f_value).ToArray();
            string[] arrWorkLocation = IEmololyee.GetSectorName(p => p.f_type == (int)sTypeEnum.上班地点类型 && (p.f_Remark ?? "").Contains("班车")).Select(p => p.f_value).ToArray();
            ViewBag.arrCommunity = arrCommunity;
            ViewBag.arrWorkLocation = arrWorkLocation;
            ViewBag.IsY = isY == "Y";
            return View(IEmololyee.GetBusSummary(arrCommunity, arrWorkLocation, isY));
        }

        /// <summary>
        /// 导出EXCEL
        /// </summary>
        /// <returns></returns>
        public FileResult Export(string isY)
        {
            string[] arrCommunity = IEmololyee.GetSectorName(p => p.f_type == (int)sTypeEnum.社区类型 && (p.f_Remark ?? "").Contains("true")).Select(p => p.f_value).ToArray();
            string[] arrWorkLocation = IEmololyee.GetSectorName(p => p.f_type == (int)sTypeEnum.上班地点类型 && (p.f_Remark ?? "").Contains("班车")).Select(p => p.f_value).ToArray();
            var lisData = IEmololyee.GetBusSummary(arrCommunity, arrWorkLocation, isY);
            DataSet ds = new DataSet();
            DataTable dt = GetDataTableExt(lisData, arrCommunity, arrWorkLocation);
            ds.Tables.Add(dt);
            string sFileName = (isY == "Y" ? "班车资料汇总包括返乡" : "班车资料汇总") + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            MemoryStream stream = NPOIExcelHelper.ExportExcel(dt, NPOIExcelHelper.ExtendPropertiesType.None, "t_employeeInfo");
            return File(stream, "application/vnd.ms-excel", sFileName);
        }
        /// <summary>
        /// 把list数据转为datatable
        /// </summary>
        /// <param name="listStatisticsRideViewModel">list数据</param>
        /// <param name="arrCommunity">社区集合</param>
        /// <param name="arrWorkLocation">上班地点集合</param>
        /// <returns></returns>
        private DataTable GetDataTableExt(List<TrafficStatisticsDto> listStatisticsRideViewModel, IReadOnlyList<string> arrCommunity, IReadOnlyList<string> arrWorkLocation)
        {
            DataTable dtStatisticsRide = new DataTable();
            dtStatisticsRide.Columns.Add("序号");
            dtStatisticsRide.Columns.Add("类别");
            dtStatisticsRide.Columns.Add("搭车时间");
            foreach (var sCommunity in arrCommunity)
            {
                dtStatisticsRide.Columns.Add(sCommunity);
            }
            foreach (var sWorkLocation in arrWorkLocation)
            {
                dtStatisticsRide.Columns.Add(sWorkLocation);
            }
            dtStatisticsRide.Columns.Add("合计");
            int iIndex = 1;
            foreach (var statisticsRideViewModel in listStatisticsRideViewModel)
            {
                DataRow drStatisticsRide = dtStatisticsRide.NewRow();
                drStatisticsRide["序号"] = iIndex;
                drStatisticsRide["类别"] = statisticsRideViewModel.srType == 1 ? "上班" : "下班";
                drStatisticsRide["搭车时间"] = statisticsRideViewModel.RideTime.Value.ToString("HH:mm");
                for (int i = 0; i < arrCommunity.Count; i++)
                {
                    drStatisticsRide[arrCommunity[i]] = statisticsRideViewModel.Community[i];
                }
                for (int i = 0; i < arrWorkLocation.Count; i++)
                {
                    drStatisticsRide[arrWorkLocation[i]] = statisticsRideViewModel.WorkLocation[i];
                }
                drStatisticsRide["合计"] = statisticsRideViewModel.Count;
                dtStatisticsRide.Rows.Add(drStatisticsRide);
                iIndex++;
            }
            return dtStatisticsRide;
        }
    }
}