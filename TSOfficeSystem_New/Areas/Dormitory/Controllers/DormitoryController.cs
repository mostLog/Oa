using MI.Application;
using MI.Application.OASession;
using MI.Core.Common;
using MI.Core.Domain;
using MI.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using TSOfficeSystem_New.Controllers;

namespace TSOfficeSystem_New.Areas.Dormitory.Controllers
{
    /// <summary>
    /// 宿舍登记控制器
    /// </summary>
    public class DormitoryController : BaseController
    {
        public IDormitoryService _Dormitory;
        public readonly ISTypeService _Icrs;
        public readonly ILaundryPwdService _lps;
        private readonly IDbContext _IDbContext;
        public DormitoryController(IDormitoryService Dormitory, ISTypeService icrs, ILaundryPwdService lps, IDbContext IDbContext, ISession session, IEmployeeService emoloyee) : base(session, emoloyee)
        {
            _Dormitory = Dormitory;
            _Icrs = icrs;
            _lps = lps;
            _IDbContext = IDbContext;
        }
        public ActionResult Index(DormitoryWhere dorm,int iPageIndex = 1, int iPageSize = 15)
        {
            int count = 0;
            var list = dorm.IsValid ? _Dormitory.GetConditionByWhere(u =>
             (string.IsNullOrWhiteSpace(dorm.f_Community) || u.f_Community.ToUpper().Contains(dorm.f_Community.ToUpper().Trim())) &&
             (string.IsNullOrWhiteSpace(dorm.f_Building) || u.f_Building.ToUpper().Contains(dorm.f_Building.ToUpper().Trim())) &&
             (string.IsNullOrWhiteSpace(dorm.f_RoomNo) || u.f_RoomNo.Contains(dorm.f_RoomNo.Trim())) &&
             ((string.IsNullOrWhiteSpace(dorm.IsBerth) || (dorm.IsBerth == "2")) || (u.f_IsBerth == (dorm.IsBerth == "1"))) &&
             ((dorm.f_ContractDateST == DateTime.MinValue) || (u.f_ContractDate >= dorm.f_ContractDateST)) &&
             ((dorm.f_ContractDateED == DateTime.MinValue) || (u.f_ContractDate <= dorm.f_ContractDateED))
            , iPageIndex, iPageSize, out count).ToList():_Dormitory.GetConditionByWhere(iPageIndex,iPageSize, out count);
            ViewBag.iPageIndex = iPageIndex;
            ViewBag.ipageSize = iPageSize;
            ViewBag.iCount = count;
            ViewBag.Dorm = dorm;
            return View(list);
        }
        public ActionResult Edit_sel(int Id = 0)
        {
            t_Dormitory model;
            //新增
            if (Id == 0)
            {
                model = new t_Dormitory { f_ContractDate = DateTime.Now, f_Term = 0, f_DueDate = DateTime.Now };
            }
            //修改
            else
            {
                model = _Dormitory.GetDormitoryById(Id);
            }
            //社区下拉框
            ViewBag.Community = _Dormitory.GetTariffbyCommunity().Select(u => new SelectListItem { Text = u, Value = u, Selected = Id != 0 && u == model.f_Community });
            //楼栋下拉框
            ViewBag.building = _Dormitory.GetTariffbyCommunity(model.f_Community).Select(u => new SelectListItem { Text = u, Value = u, Selected = Id != 0 && u == model.f_Building });
            ViewBag.NoandPwd = _lps.GetTariffbyBuilding(model.f_Community, model.f_Building);
            //房间类型
            ViewBag.listTypeDormitory = _Icrs.GetTypeByWhere(p => p.f_type == (int)sTypeEnum.房间类型);
            ViewBag.listTypeDepartment = _Icrs.GetTypeByWhere(p => p.f_type == (int)sTypeEnum.部门类型);
            return View(model);
        }
        /// <summary>
        /// 根据社区查询楼栋
        /// </summary>
        /// <param name="community">社区</param>
        [HttpPost]
        public ActionResult Community(string community)
        {
            return Json(_Dormitory.GetTariffbyCommunity(community, true));
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
            return Json(_Dormitory.GetTariffbyBuilding(community, building, true));
        }
        /// <summary>
        ///  根据社区，楼栋查询社区的所有洗衣房
        /// </summary>
        /// <param name="sCommunity"></param>
        /// <param name="sBuilding"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Buildings(string sCommunity, string sBuilding)
        {
            IList<t_LaundryPwd> list = _lps.GetTariffbyBuilding(sCommunity, sBuilding);
            return Json(list);
        }
        [HttpPost]
        public ActionResult UptateAndAdd(t_Dormitory model)
        {
            ErrorEnum resut = ErrorEnum.Error;
            String eReturn = string.Empty;
            model.f_operator = m_NickName;
            model.f_operatorTime = System.DateTime.Now;
            model.f_Community = model.f_Community.Trim(); //社区和楼栋统一都大写去首尾空格
            model.f_Building = model.f_Building.Trim();//社区和楼栋统一都大写去首尾空格

            //先去检查是否有相同的社区(忽略大小写)
            string sComm = _Icrs.GetReplaceCommunity(model.f_Community);
            if (string.IsNullOrWhiteSpace(sComm))  //没有就新增
            {
                SType typemodel = new SType();
                typemodel.f_value = model.f_Community;
                typemodel.f_Remark = "社区类型";
                typemodel.f_type = (int)sTypeEnum.社区类型;
                _Icrs.AddsTypeOneData(typemodel);
            }
            else   //有就把替换掉,避免大小写导致重复
            {
                model.f_Community = sComm;
            }

            string sBuil = _Icrs.GetReplaceBuilding(model.f_Building);
            if (string.IsNullOrWhiteSpace(sBuil))
            {
                SType typemodel = new SType();
                typemodel.f_value = model.f_Building;
                typemodel.f_Remark = "楼栋类型";
                typemodel.f_type = (int)sTypeEnum.楼栋类型;
                _Icrs.AddsTypeOneData(typemodel);
            }
            else
            {
                model.f_Building = sBuil;
            }
            //验证洗衣房是否存在
            if (!string.IsNullOrWhiteSpace(model.f_LaundryAndPwd) && !"0".Equals(model.f_LaundryAndPwd))
            {
                int iLaundryAndPwd = 0;
                if (!int.TryParse(model.f_LaundryAndPwd, out iLaundryAndPwd) || _lps.GetLaundryPwdByWhere(p => iLaundryAndPwd == p.f_Id).Count == 0)
                {
                    resut = ErrorEnum.Error;
                    eReturn = "洗衣房不存在";
                    return Json(new ResultModel((int)resut, eReturn));
                }
            }
            if (model.f_DormitoryId > 0)
            {
                if (_Dormitory.EditDormitoryOneData(model)>0) {                  
                        resut = ErrorEnum.Success;
                };
            }
            else
            {
                //新增房间号不能重复
                if (_Dormitory.GetTariffbyRoomNo(model.f_Community, model.f_Building, model.f_RoomNo) != null)
                {
                    resut = ErrorEnum.Error;
                    eReturn = "房间号不能重复";
                    return Json(new ResultModel((int)resut, eReturn));
                }
                if (_Dormitory.AddDormitoryOneData(model) > 0) {
                    resut = ErrorEnum.Success;
                    //添加f_type=51的社区上车地点配置   2017-02-27
                   SType type = _Icrs.GetTypeByWhere(p => p.f_type == (int)sTypeEnum.社区类型 && p.f_value.Equals(sComm)).FirstOrDefault();
                    if (type != null && _Icrs.GetTypeByWhere(p => p.f_type == (int)sTypeEnum.上车地点配置 && p.f_value.Equals(type.f_tID.ToString())).FirstOrDefault() == null)
                    {
                        SType typemodel = new SType();
                        typemodel.f_value = type.f_tID.ToString();
                        typemodel.f_type = (int)sTypeEnum.上车地点配置;
                        _Icrs.AddsTypeOneData(typemodel);
                    }
                    resut = ErrorEnum.Success;
                };
            }
            return Json(resut);
        }
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            ErrorEnum eResult;
            eTipsEnum eTips;
            //stateError=1;表示其他表有数据依赖t_dormitory（宿舍登记表）
            int stateError = _Dormitory.DeleteDormitory(Id);
            //自己定义的，stateError为2表示删除成功！
            if (stateError == 2)
            {
                eResult = ErrorEnum.Success;
                eTips = eTipsEnum.DeleteSuccess;
            }
            else {
                if (stateError == 1)
                {
                    eResult = ErrorEnum.Fail;
                    eTips = eTipsEnum.DelError;
                }
                else
                {
                    eResult = ErrorEnum.Fail;
                    eTips = eTipsEnum.FailOperation;
                }
            }
            return Json(new ResultModel((int)eResult, eTips.Description()));
        }
        /// <summary>
        /// 导出EXCEL
        /// </summary>
        /// <param name="dorm"></param>
        /// <returns></returns>
        public FileResult Export(DormitoryWhere dorm)
        {
            var list = dorm.IsValid ? _Dormitory.GetConditionByWhereExportData(u =>
          (string.IsNullOrWhiteSpace(dorm.f_Community) || u.f_Community.ToUpper().Contains(dorm.f_Community.ToUpper().Trim())) &&
          (string.IsNullOrWhiteSpace(dorm.f_Building) || u.f_Building.ToUpper().Contains(dorm.f_Building.ToUpper().Trim())) &&
          (string.IsNullOrWhiteSpace(dorm.f_RoomNo) || u.f_RoomNo.Contains(dorm.f_RoomNo.Trim())) &&
            ((string.IsNullOrWhiteSpace(dorm.IsBerth) || (dorm.IsBerth == "2")) || (u.f_IsBerth == (dorm.IsBerth == "1"))) &&
          ((dorm.f_ContractDateST == DateTime.MinValue) || (u.f_ContractDate >= dorm.f_ContractDateST)) &&
          ((dorm.f_ContractDateED == DateTime.MinValue) || (u.f_ContractDate <= dorm.f_ContractDateED))
           ).ToList() : _Dormitory.GetDormitoryAllExportData();

            DataSet ds = new DataSet();
            DataTable dt = DataTableExt.ToDataTable<dynamic>(list);

            #region 也可以在这里指定要导出的列
            //  string[] columns =new string[] {"f_DormitoryId", "f_Community", "f_Building", "f_RoomNo", "f_ContractDate", "f_Term", "f_DueDate", "f_Landlady", "f_Rent"};
            // DataTable dt = DataTableExt.ToDataTable<t_dormitory>(list, columns);
            #endregion

            ds.Tables.Add(dt);
            var data = _IDbContext.GetExtendProperByTabNames("t_dormitory").ToList();
            DataTable dtColumns = DataTableExt.ToDataTable(data);
            string fileName = "";
            if (fileName == "")
            {
                fileName = "Export_宿舍登记" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            }
            if (dt.Rows.Count > 0)
            {
                MemoryStream stream = NPOIExcelHelper.ExportExcel(dt, NPOIExcelHelper.ExtendPropertiesType.Inner, "t_dormitory", dtColumns);
                return File(stream, "application/vnd.ms-excel", fileName);
            }
            else
            {
                MemoryStream stream = NPOIExcelHelper.ExportExcel(dt, NPOIExcelHelper.ExtendPropertiesType.None, "t_dormitory");
                return File(stream, "application/vnd.ms-excel", fileName);
            }


        }
    }
}