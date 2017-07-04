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
    /// 床单送洗控制器
    /// 创建人：吕秀峰
    /// 创建时间：2017-06-26
    /// </summary>
    public class BedLinenController : BaseController
    {
        private IBedLinenService _BedLinen;
        private ICategoryService _Category;
        public BedLinenController(IBedLinenService bedLinen, ICategoryService icrs, IEmployeeService employeeInfoService,ISession session, IEmployeeService emoloyee) : base(session, emoloyee)
        {
            _BedLinen = bedLinen;
            _Category = icrs;
        }
        // GET: Dormitory/BedLinen
        // GET: Dormitory/_bedLinen
        /// <summary>
        /// 主页显示
        /// </summary>
        /// <param name="queryModel">添加</param>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">每页多少数据</param>
        /// <returns></returns>
        public ActionResult Index(BedLinenWhere queryModel, int iPageIndex = 1, int iPageSize = 15)
        {
            int iCount = 0;
            List<BedLinenViewModel> list = _BedLinen.GetBedLinenAllData(queryModel, iPageIndex, iPageSize, out iCount);
            ViewBag.iPageIndex = iPageIndex;
            ViewBag.iPageSize = iPageSize;
            ViewBag.iCount = iCount;
            ViewBag.queryModel = queryModel;
            return View(list);
        }

        /// <summary>
        /// 修改视图
        /// </summary>
        /// <param name="iId">主键id</param>
        /// <returns>返回一个 修改视图 页面</returns>
        public ActionResult Edit(int iId = 0)
        {
            DateTime oDate;
            if (DateTime.Now.Day >= 1 && DateTime.Now.Day <= 9)
            {
                oDate = DateTime.Now;
            }
            else
            {
                oDate = DateTime.Now.AddMonths(1);
            }
            BedLinen model = iId == 0 ? (new BedLinen { f_BunkNo = 0, f_SheetsNo = 0, f_QuiltNo = 0, f_PillowcaseNo = 0, f_Cont = 0, f_AddBunkNo = 0, f_AddSheetsNo = 0, f_AddQuiltNo = 0, f_AddPillowcaseNo = 0, f_eid = m_Eid, f_RegistrationDate = oDate }) : _BedLinen.GetBedLinenById(iId);
            //床位类型
            ViewBag.typeBedLinenlist = _Category.GetsTypeByWhere(p => p.f_type == (int)sTypeEnum.床位类型);
            return View(model);
        }
        [HttpPost]
        public ActionResult UptateAndAdd(BedLinen model)
        {
            ErrorEnum resut = ErrorEnum.Error;
            string sReturnChar = string.Empty;
            if (AOUnity.ValidationLength("备注信息", model.f_Remarks, 30, out sReturnChar))
            {
                resut = ErrorEnum.LengthOutRange;
                return Json(new ResultModel((int)resut, sReturnChar));
            }
            if (model.f_Id > 0)
            {
                if (_BedLinen.EditBedLinenOneData(model)>0) {
                    resut = ErrorEnum.Success;
                }
            }
            else
            {
                if (m_Eid != 0)
                {
                    if (_BedLinen.AddBedLinenOneData(model)>0) {
                        resut = ErrorEnum.Success;
                    }
                }
                else
                {
                    resut = ErrorEnum.NickNameIsNull;
                    return Json(new ResultModel((int)resut));
                }
            }
            return Json(new ResultModel((int)resut));
        }
        /// <summary>
        /// 删除动作
        /// </summary>
        /// <param name="iId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int iId)
        {
            ErrorEnum resut = _BedLinen.DeleteBedLinen(iId);
            return Json(resut);
        }
        /// <summary>
        /// 导出EXCEL
        /// </summary>
        /// <param name="queryModel">导出条件</param>
        /// <returns></returns>
        public FileResult Export(BedLinenWhere queryModel)
        {
            var lisData = _BedLinen.ExportBedLinenAllData(queryModel);
            DataSet ds = new DataSet();
            DataTable dt = DataTableExt.ToDataTable<dynamic>(lisData);
            ds.Tables.Add(dt);
            string sFileName = "床单送洗" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            MemoryStream stream = NPOIExcelHelper.ExportExcel(dt, NPOIExcelHelper.ExtendPropertiesType.None, "t_BedLinen");
            return File(stream, "application/vnd.ms-excel", sFileName);
        }

        public FileResult ExportSummary(BedLinenWhere queryModel)
        {
            var lisData = _BedLinen.ExportBedLinenAllData(queryModel, true);
            DataSet ds = new DataSet();
            DataTable dt = DataTableExt.ToDataTable<dynamic>(lisData);
            ds.Tables.Add(dt);
            string sFileName = "床单送洗" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            MemoryStream stream = NPOIExcelHelper.ExportExcel(dt, NPOIExcelHelper.ExtendPropertiesType.None, "t_BedLinen");
            return File(stream, "application/vnd.ms-excel", sFileName);
        }

        /// <summary>
        /// 根据宿舍汇总统计
        /// </summary>
        /// <param name="queryModel">条件</param>
        /// <returns></returns>
        public ActionResult StatsBedLinen(BedLinenWhere queryModel)
        {
            return View(_BedLinen.GetStatsBedLinen(queryModel));
        }
    }
}