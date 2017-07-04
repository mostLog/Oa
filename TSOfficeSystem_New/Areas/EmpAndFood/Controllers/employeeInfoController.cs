using ICSharpCode.SharpZipLib.Zip;
using MI.Application;
using MI.Application.Dto;
using MI.Application.OASession;
using MI.Core.Common;
using MI.Core.Domain;
using MI.Data.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSOfficeSystem_New.Controllers;


namespace TSOfficeSystem_New.Areas.EmpAndFood.Controllers
{
    public class employeeInfoController : BaseController
    {
        /// <summary>
        /// 人事管理服务
        /// </summary>
        public readonly IEmployeeService IEmololyee;
        /// <summary>
        /// 宿舍管理服务
        /// </summary>
        public readonly IDormitoryService IDormitory;
        public employeeInfoController(IEmployeeService ies,IDormitoryService ids,ISession session, IEmployeeService emoloyee) : base(session, emoloyee)
        {
            IEmololyee = ies;
            IDormitory = ids;
        }
        public ActionResult Index(EmployeeDto employeedto, int iPageIndex = 1, int iPageSize = 15)
        {
            int count = 0;
            List<Employee> employeeData;
            //部门
            ViewBag.listTypeSector = IEmololyee.GetSectorName(p => p.f_type == (int)sTypeEnum.部门类型);
            //在职状态
            ViewBag.listTypeworkStatus = IEmololyee.GetInServiceStatus(p => p.f_type == (int)sTypeEnum.上班状态类型);
            //权限等级
            ViewBag.listPrivilegeLevel = IEmololyee.GetPrivilegeLevel(p => p.f_type == (int)sTypeEnum.权限类型);
            //订餐类型
            ViewBag.listorderType = IEmololyee.GetOrderType(p => p.f_type == (int)sTypeEnum.订餐类型);
            ViewBag.employee = employeedto;
            if (employeedto.isValid)
            {
                //条件查询
                employeeData = IEmololyee.GetEmployeeByWhere(employeedto,iPageIndex, iPageSize, out count).ToList();
            }
            else {
                //无条件分页查询
                employeeData = IEmololyee.GetEmployeeByWhere(iPageIndex, iPageSize, out count).ToList();
            }
            ViewBag.iPageIndex = iPageIndex;
            ViewBag.iPageSize = iPageSize;
            ViewBag.iCount = count;
            return View(employeeData);
        }
        /// <summary>
        /// 新员工数据
        /// iPageIndex  iPageSize 对应PageListFor扩展分页方法里的参数
        /// </summary>
        /// <returns></returns>
        public ActionResult NewIndex(EmployeeDto employeedto, int iPageIndex = 1, int iPageSize = 15)
        {
            int iCount;
            ViewBag.LDM = IEmololyee.GetOrderType(p => p.f_type == (int)sTypeEnum.订餐类型).ToList();//查出中中晚宵夜的tID
            ViewBag.typeList = IEmololyee.GetOrderType(p => p.f_type == (int)sTypeEnum.订餐要求类型).ToList();//查出中中晚宵夜的tID
            ViewBag.listTypeDepartment = IEmololyee.GetSectorName(p => p.f_type == (int)sTypeEnum.部门类型);
            ViewBag.listTypeFlight = IEmololyee.GetOrderType(p => p.f_type == (int)sTypeEnum.航班类型);
            List<Employee> newEmployeeData=employeedto.isValidNewemployee? IEmololyee.GetEmployeeByWhere(employeedto, iPageIndex, iPageSize, out iCount, true).ToList(): IEmololyee.GetEmployeeByWhere(iPageIndex, iPageSize, out iCount, true).ToList();
            ViewBag.employee = employeedto;
            ViewBag.QueryModel = employeedto;
            ViewBag.iPageIndex = iPageIndex;
            ViewBag.iPageSize = iPageSize;
            ViewBag.iCount = iCount;
            return View(newEmployeeData);
        }
        /// <summary>
        /// 移走新人
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Removal(int id)
        {
            ErrorEnum eResult;
            eTipsEnum eTips;
            try
            {
                //移走
                if (IEmololyee.RemovalEmployeeInfoById(id) > 0)
                {
                    eResult = ErrorEnum.Success;
                    eTips = eTipsEnum.SuccessfulOperation;
                }
                else
                {
                    eResult = ErrorEnum.Fail;
                    eTips = eTipsEnum.FailOperation;
                }
            }
            catch (Exception e)
            {
                eResult = ErrorEnum.Error;
                eTips = eTipsEnum.Error;
                //写入错误日记
                AOUnity.WriteLog(e);
            }
            return Json(new ResultModel((int)eResult, eTips.Description()));
        }

        /// <summary>
        /// 查询已被移走的新人数据
        /// </summary>
        /// <param name="employeedto">查询条件</param>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">每页多少数据</param>
        /// <param name="o_iCount">总数</param>
        /// <returns>返回t_employeeInfo集合</returns>
        public ActionResult NewEmpRemove(EmployeeDto employeedto=null, int iPageIndex = 1, int iPageSize = 15)
        {
            int iCount;
            var lisData = IEmololyee.GetNewEmpRemove(employeedto, iPageIndex, iPageSize, out iCount);
            ViewBag.LDM = IEmololyee.GetOrderType(p => p.f_type == (int)sTypeEnum.订餐类型).ToList();//查出早中晚宵夜的tID
            ViewBag.listTypeFlight = IEmololyee.GetOrderType(p => p.f_type == (int)sTypeEnum.航班类型);
            ViewBag.employee = employeedto;
            ViewBag.iPageIndex = iPageIndex;
            ViewBag.iPageSize = iPageSize;
            ViewBag.iCount = iCount;

            ViewBag.listTypeDepartment = IEmololyee.GetOrderType(p => p.f_type == (int)sTypeEnum.部门类型);
            return View(lisData);
        }
        /// <summary>
        /// 打印以移走新人数据
        /// </summary>
        /// <param name="employeedto">条件model</param>
        /// <returns></returns>
        public ActionResult ExportRemoveNewEmp(EmployeeDto employeedto)
        {
            var lisData = IEmololyee.ExportRemoveNewEmp(employeedto);
            DataSet ds = new DataSet();
            DataTable dt = DataTableExt.ToDataTable(lisData);
            ds.Tables.Add(dt);
            string sFileName = "已移走新人" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            MemoryStream stream = NPOIExcelHelper.ExportExcel(dt, NPOIExcelHelper.ExtendPropertiesType.None, "t_employeeInfo");
            return File(stream, "application/vnd.ms-excel", sFileName);
        }
        ////// <summary>
        /// 新人须知表
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public ActionResult NewEmpPrintView(string eId)
        {
            string[] arrId = eId.Split('÷');
            int[] intArray = Array.ConvertAll<string, int>(arrId, s => !string.IsNullOrWhiteSpace(s) ? int.Parse(s) : 0);
            List<Employee> sList = IEmololyee.GetemployeeInfoByWhere(p => intArray.Contains(p.f_eid)).OrderBy(p => p.NewEmployee?.f_flightDate).ThenBy(p => p.f_eid).ToList();
            ViewBag.sTypeItem = IEmololyee.GetSectorName(p => p.f_tID != 0).ToList();
            ViewBag.LaundryPwdList = IEmololyee.GetLaundryPwdAllData();
            return View(sList);
        }
        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="feid">员工id</param>
        /// <param name="value">删除口令</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteByID(int pk, string value)
        {
            ErrorEnum error;
            eTipsEnum etienum;
            try
            {
                int departmentID = IEmololyee.GetEmployeeById(pk)?.f_department_tID ?? 0;
                //当前登录的id是否有删除其他部门的权限，否则只能删自己部门的
                if ((!base._session.GetCurrUser().Permissions.Contains("Department") && IEmololyee.GetEmployeeById(base._session.GetCurrUser().Id).f_department_tID != departmentID)
               )
                {
                    error = ErrorEnum.Error;
                    etienum = eTipsEnum.InsufficientPrivileges;
                }
                else
                {
                    if (IEmololyee.GetEmployeeById(pk) == null)
                    {
                        error = ErrorEnum.Fail;
                        string eReturn = "员工不存在或已被删除";
                        return Json(new ResultModel((int)error, eReturn));
                    }
                    if (string.IsNullOrWhiteSpace(value) || AOUnity.GetDelPwd != value.Trim())
                    {
                        error = ErrorEnum.Fail;
                        string eReturn = "删除失败，删除口令不正确！";
                        return Json(new ResultModel((int)error, eReturn));
                    }
                    //删除
                    if (IEmololyee.DeleteEmoloyeeByfeid(pk) > 0)
                    {
                        error = ErrorEnum.Success;
                        etienum = eTipsEnum.DeleteSuccess;
                    }
                    else
                    {
                        error = ErrorEnum.Fail;
                        etienum = eTipsEnum.FailOperation;
                    }
                }
            }
            catch (Exception e)
            {
                error = ErrorEnum.Error;
                etienum = eTipsEnum.Error;
                //写入错误日记
                AOUnity.WriteLog(e);
            }
            return Json(new ResultModel((int)error, etienum.Description()));
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="employeedto">人员表参数</param>
        /// <returns></returns>
        public FileResult Export(EmployeeDto employeedto)
        {
            var excelData = employeedto.isValid ? IEmololyee.ExportData(employeedto) : IEmololyee.ExportData();
            DataSet ds = new DataSet();
            DataTable dt = DataTableExt.ToDataTable(excelData);
            ds.Tables.Add(dt);
            string sFileName = "员工信息表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            MemoryStream stream = NPOIExcelHelper.ExportExcel(dt, NPOIExcelHelper.ExtendPropertiesType.None, "t_employeeInfo");
            return File(stream, "application/vnd.ms-excel", sFileName);
        }
        /// <summary>
        /// 新增跳转
        /// </summary>
        /// <param name="isNew"></param>
        /// <returns></returns>
        public ActionResult GetCreate(bool isNew = false)
        {
            ViewBag.listTypeDepartment = IEmololyee.GetSectorName(p => p.f_type == (int)sTypeEnum.部门类型);
            ViewBag.listTypePeriodType = IEmololyee.GetSectorName(p => p.f_type == (int)sTypeEnum.班次类型);
            ViewBag.listTypeWorkStatus = IEmololyee.GetInServiceStatus(p => p.f_type == (int)sTypeEnum.上班状态类型);
            ViewBag.listTypeWorkLocation = IEmololyee.GetSectorName(p => p.f_type == (int)sTypeEnum.上班地点类型);
            foreach (var item in m_RankStr)
            {
                if (item.Contains("Rights1"))
                {
                    ViewBag.listTypeLevel = IEmololyee.GetSectorName(p => p.f_type == (int)sTypeEnum.权限类型 && "普通员工" == p.f_value || "普通員工" == p.f_value);
                }
                else
                {
                    ViewBag.listTypeLevel = IEmololyee.GetSectorName(p => p.f_type == (int)sTypeEnum.权限类型);
                }
            }
            ViewBag.listTypeLDM = IEmololyee.GetSectorName(p => p.f_type == (int)sTypeEnum.订餐类型);
            ViewBag.listTypeLDMType = IEmololyee.GetSectorName(p => p.f_type == (int)sTypeEnum.订餐要求类型);
            ViewBag.GuoJi = IEmololyee.GetSectorName(p => p.f_type == (int)sTypeEnum.国籍);
            ViewBag.JLStage = IEmololyee.GetSectorName(u => u.f_type == (int)sTypeEnum.金流客服等级类型).OrderBy(u => u.f_tID).ToList();//金流客服等级
            ViewBag.JLlevel = IEmololyee.GetSectorName(u => u.f_type == (int)sTypeEnum.金流客服权限控制).OrderBy(u => u.f_tID).ToList();//金流客服权限
            ViewBag.CurrentEmp = IEmololyee.GetEmployeeById(m_Eid);
            if (isNew)
            {
                //新人独有
                ViewBag.listTypeFlight = IEmololyee.GetSectorName(p => p.f_type == (int)sTypeEnum.航班类型);
            }
            else
            {
                //人事总表独有
                ViewBag.listTypeVisa = IEmololyee.GetSectorName(p => p.f_type == (int)sTypeEnum.签证类型);
            }
            ViewBag.listStrCommunity = IDormitory.GetTariffbyCommunity();
            ViewBag.IsCreate = true;
            ViewBag.isNew = isNew;
            return View("Edit", new Employee());
        }
        /// <summary>
        /// 新增实现
        /// </summary>
        /// <param name="allEmployeeInfoViewModel">员工信息模型</param>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult Create(AllEmployeeInfoViewModel allEmployeeInfoViewModel)
        {
            ErrorEnum eResult = ErrorEnum.Fail;
            eTipsEnum eTips = eTipsEnum.FailOperation;
            string sExceptionMessage = string.Empty;
            allEmployeeInfoViewModel.EmployeeInfo.f_IsNewEmp = allEmployeeInfoViewModel.IsnewEmployeeInfo;
           bool a= allEmployeeInfoViewModel.EmployeeInfo.CheckModel(out eTips);
            if (allEmployeeInfoViewModel.EmployeeInfo.CheckModel(out eTips) && (!allEmployeeInfoViewModel.IsnewEmployeeInfo || allEmployeeInfoViewModel.EmployeeInfo.NewEmployee.checkModel(out eTips)))
            {
                //判断登录帐号是否具有查看其他部门信息的权限，没有权限则只能添加自己部门的人员信息&&
                //判断登录帐号的部门ID是否和添加的部门id相同 
                if ((!base._session.GetCurrUser().Permissions.Contains("Department") && IEmololyee.GetEmployeeById(base._session.GetCurrUser().Id).f_department_tID != allEmployeeInfoViewModel.EmployeeInfo.f_department_tID)
                 )
                {
                    eResult = ErrorEnum.Error;
                    eTips = eTipsEnum.InsufficientPrivileges;
                }
                else
                {
                    try
                    {
                        allEmployeeInfoViewModel.EmployeeInfo.f_CreateDate = DateTime.Now.Date;
                        allEmployeeInfoViewModel.EmployeeInfo.f_AccountName = IEmololyee.GetMaxAccountName(allEmployeeInfoViewModel.EmployeeInfo.f_department_tID);
                        //添加
                        if (IEmololyee.AddEmployeeInfo(allEmployeeInfoViewModel) != 0)
                        {
                            eResult = ErrorEnum.Success;
                            eTips = eTipsEnum.CreateSuccess;
                        }
                    }
                    catch (Exception e)
                    {
                        sExceptionMessage = e.Message;
                        eResult = ErrorEnum.Error;
                        eTips = eTipsEnum.Error;
                        //写入错误日记
                        AOUnity.WriteLog(e);
                    }
                }
            }
            return Json(new ResultModel((int)eResult, eTips.Description(), sExceptionMessage));
        }
        /// <summary>
        /// 修改跳转
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult getEdit(int id = 0)
        {
            ViewBag.listTypeDepartment = IEmololyee.GetSectorName(p => p.f_type == (int)sTypeEnum.部门类型);
            ViewBag.listTypePeriodType = IEmololyee.GetSectorName(p => p.f_type == (int)sTypeEnum.班次类型);
            ViewBag.listTypeWorkStatus = IEmololyee.GetInServiceStatus(p => p.f_type == (int)sTypeEnum.上班状态类型);
            ViewBag.listTypeWorkLocation = IEmololyee.GetSectorName(p => p.f_type == (int)sTypeEnum.上班地点类型);
            ViewBag.listTypeLevel = IEmololyee.GetSectorName(p => p.f_type == (int)sTypeEnum.权限类型);
            ViewBag.listTypeLDM = IEmololyee.GetSectorName(p => p.f_type == (int)sTypeEnum.订餐类型);
            ViewBag.listTypeLDMType = IEmololyee.GetSectorName(p => p.f_type == (int)sTypeEnum.订餐要求类型);
            ViewBag.GuoJi = IEmololyee.GetSectorName(p => p.f_type == (int)sTypeEnum.国籍);
            ViewBag.listStrCommunity = IDormitory.GetTariffbyCommunity();
            ViewBag.IsCreate = false;
            ViewBag.JLStage = IEmololyee.GetSectorName(u => u.f_type == (int)sTypeEnum.金流客服等级类型).OrderBy(u => u.f_tID).ToList();//金流客服等级
            ViewBag.JLlevel = IEmololyee.GetSectorName(u => u.f_type == (int)sTypeEnum.金流客服权限控制).OrderBy(u => u.f_tID).ToList();//金流客服权限
            ViewBag.CurrentEmp = IEmololyee.GetEmployeeById(m_Eid);
            var model = IEmololyee.GetEmployeeById(id);
            ViewBag.listStrBuilding = IDormitory.GetTariffbyCommunity(model.t_Dormitory?.f_Community ?? "");
            ViewBag.listStrRoom = IDormitory.GetTariffbyBuilding(model.t_Dormitory?.f_Community ?? "", model.t_Dormitory?.f_Building ?? "");
            ViewBag.isNew = model?.f_IsNewEmp;
            if (model?.f_IsNewEmp ?? false)
            {
                ViewBag.listTypeFlight = IEmololyee.GetSectorName(p => p.f_type == (int)sTypeEnum.航班类型);
            }
            else
            {
                //人事总表独有
                ViewBag.listTypeVisa = IEmololyee.GetSectorName(p => p.f_type == (int)sTypeEnum.签证类型);
            }
            ViewBag.TpassportURL = ToBase64String(model?.f_PassportURL, true);
            ViewBag.TentrystampURL = ToBase64String(model?.f_EntrystampURL, true, true);
            ViewBag.TidCardURL = ToBase64String(model?.f_IDCardURL, true, false, true);
            return View("Edit", model);
        }

        /// <summary>
        /// 修改实现
        /// </summary>
        /// <param name="allEmployeeInfoViewModel">参数，需要修改的内容</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(AllEmployeeInfoViewModel allEmployeeInfoViewModel)
        {
            ErrorEnum eResult = ErrorEnum.Fail;
            eTipsEnum eTips = eTipsEnum.FailOperation;
            string sExceptionMessage = string.Empty;
            //判断登录帐号是否具有查看其他部门信息的权限，没有权限则只能修改自己部门的人员信息&&
            //判断登录帐号的部门ID是否和被修改者的部门id相同 
            if((!base._session.GetCurrUser().Permissions.Contains("Department") && IEmololyee.GetEmployeeById(base._session.GetCurrUser().Id).f_department_tID!= allEmployeeInfoViewModel.EmployeeInfo.f_department_tID)
                )
            {
                eResult = ErrorEnum.Error;
                eTips = eTipsEnum.InsufficientPrivileges;
            }
            else
            {
                allEmployeeInfoViewModel.EmployeeInfo.f_IsNewEmp = allEmployeeInfoViewModel.IsnewEmployeeInfo;
                if (allEmployeeInfoViewModel.EmployeeInfo.CheckModel(out eTips))
                {
                    try
                    {
                        if (IEmololyee.UpdateEmployeeInfo(allEmployeeInfoViewModel) > 0)
                        {
                            eResult = ErrorEnum.Success;
                            eTips = eTipsEnum.EditSuccess;
                        }
                    }
                    catch (Exception e)
                    {
                        sExceptionMessage = e.Message;
                        eResult = ErrorEnum.Error;
                        eTips = eTipsEnum.Error;
                        //写入错误日记
                        AOUnity.WriteLog(e);
                    }
                }
            }
            return Json(new ResultModel((int)eResult, eTips.Description(), sExceptionMessage));
        }
        /// <summary>
        /// 人员统计
        /// </summary>
        /// <returns></returns>
        public ActionResult PersonnelStatistics()
        {

            ViewBag.listTypeDepartment = IEmololyee.GetOrderType(p => p.f_type == (int)sTypeEnum.部门类型);
            ViewBag.listTypeWorkLocation = IEmololyee.GetOrderType(p => p.f_type == (int)sTypeEnum.上班地点类型);
            ViewBag.GuoJi = IEmololyee.GetOrderType(p => p.f_type == (int)sTypeEnum.国籍);
            ViewBag.listTypeWorkStatus = IEmololyee.GetOrderType(p => p.f_type == (int)sTypeEnum.上班状态类型);
            //社区
            ViewBag.Community = IDormitory.GetTariffbyCommunity();
            //统计在职数据
            return View(IEmololyee.GetPersonnelData());
        }
        /// <summary>
        /// 新人登记打印数据
        /// </summary>
        /// <returns></returns>
        public ActionResult NewIndexView(EmployeeDto employeedto)
        {
            List<Employee> newEmployeeData = employeedto.isValidNewemployee ? IEmololyee.GetPageListexcel(employeedto,true).ToList() : IEmololyee.GetPageListexcel(employeedto).ToList();
            ViewBag.LDM = IEmololyee.GetOrderType(p => p.f_type == (int)sTypeEnum.订餐类型).ToList();//查出中中晚宵夜的tID
            ViewBag.listTypeFlight = IEmololyee.GetOrderType(p => p.f_type == (int)sTypeEnum.航班类型);
            return View(newEmployeeData);
        }
        /// <summary>
        /// 根据社区，楼栋查询社区的所有楼栋
        /// </summary>
        /// <param name="community">社区</param>
        /// <param name="building">楼栋</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Building(string community, string building)
        {
            return Json(IDormitory.GetTariffbyBuilding(community, building));
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
            return Json(IDormitory.GetTariffbyRoomNo(community, building, roomno).f_DormitoryId);
        }
        /// <summary>
        /// 根据社区查询楼栋
        /// </summary>
        /// <param name="community">社区</param>
        [HttpPost]
        public ActionResult Communits(string community)
        {
            return Json(IDormitory.GetTariffbyCommunity(community));
        }

        #region 上传图片
        /// <summary>
        /// 上传图片逻辑更改： 图片上传到服务器，数据库中存储图片名字。显示使用数据库中的图片名字拼接完整路径然后在把服务器中的图片转为base64 呈现
        /// 图存在硬盘， 数据中存路径
        /// </summary>
        /// <param name="imgName"></param>
        /// <param name="eid"></param>
        /// <param name="HZorRJZtype">p=护照 || e=入境章</param>
        /// <returns></returns>
        [HttpPost]
        // ReSharper disable once InconsistentNaming
        public ActionResult UploadFile(string imgName, int eid = 0, string HZorRJZtype = "p")
        {
            Employee oModel = IEmololyee.GetEmployeeById(eid);
            string sResult = string.Empty;   //返回结果 默认空的结果
            if (CheckLevel(eid))
            {
                //判断登录帐号是否具有查看其他部门信息的权限，没有权限则只能修改自己部门的人员信息&&
                //判断登录帐号的部门ID是否和被修改者的部门id相同 
                if ((!base._session.GetCurrUser().Permissions.Contains("Department") && IEmololyee.GetEmployeeById(base._session.GetCurrUser().Id).f_department_tID != (oModel?.f_department_tID ?? 0)))
                {
                    return Json(sResult);
                }
                ResultModel modelResult = Public.BLLUploadFile(imgName, eid, HZorRJZtype);
                if (modelResult.result == (int)ErrorEnum.Success && !string.IsNullOrWhiteSpace(modelResult.tips))
                {
                    if (eid > 0)
                        IEmololyee.UpdetePassporUrl(oModel, modelResult.tips, HZorRJZtype);
                    sResult = modelResult.tips;
                }
                else
                {
                    sResult = modelResult.result.ToString();
                }
            }
            return Json(sResult);
        }
        /// <summary>
        /// 下载图片，默认下载护照
        /// </summary>
        /// <param name="eid"></param>
        /// <param name="t">p=护照 || e=入境章</param>
        /// <returns></returns>
        public ActionResult DownloadFile(int eid = 0, string t = "p")
        {
            if (!CheckLevel(eid))
            {
                return null;
            }
            var oModel = IEmololyee.GetEmployeeById(eid);
            //判断登录帐号是否具有查看其他部门信息的权限，没有权限则只能修改自己部门的人员信息&&
            //判断登录帐号的部门ID是否和被修改者的部门id相同 
            if ((!base._session.GetCurrUser().Permissions.Contains("Department") && IEmololyee.GetEmployeeById(base._session.GetCurrUser().Id).f_department_tID != (oModel?.f_department_tID ?? 0)))
            {
                return null;
            }
            Public.BLLDownloadFile(oModel, t);
            return new EmptyResult();
        }
        #endregion
        /// <summary>
        /// 根据部门下载IDCardURL（身份证图片）
        /// </summary>
        /// <returns></returns>
        public ActionResult downloadImg(int sDepartmentId)
        {
            //服务器存储图片的路径
            string sPath = Server.MapPath("~/images/User/Original/");
            DirectoryInfo directoryInfo = new DirectoryInfo(sPath);
            //根据部门获取员工数据
            List<Employee> sList = IEmololyee.GetemployeeInfoByWhere(p => p.f_department_tID == sDepartmentId).ToList();
            Directory.CreateDirectory(System.Environment.SystemDirectory);
            FileInfo[] serFiles = directoryInfo.GetFiles();
            MemoryStream ms = new MemoryStream();
            byte[] buffer = null;
            using (ZipFile file = ZipFile.Create(ms))
            {
                file.BeginUpdate();
                foreach (var item in sList)
                {
                    //如果这个人没有f_IDCardURL身份证图片跳过
                    if (string.IsNullOrWhiteSpace(item.f_IDCardURL))
                    {
                        continue;
                    }
                    string ImgUrl = EncryptHelper.AESDecrypt(item.f_IDCardURL.ToString());
                    //如果这个人f_IDCardURL身份证图片没在服务器路径下跳过
                    if (serFiles.Where(p => p.Name == ImgUrl).Count() <= 0)
                    {
                        continue;
                    }
                    //根据部门筛选出的图片复制到ImgDownloadFile文件
                    file.Add(Server.MapPath(Path.Combine("/images/UserIDC/Original", ImgUrl)), (item.f_AccountName ?? "*") + "_" + (item.f_chineseName ?? "*") + "_" + (item.f_passportName ?? "*") + ".jpg");
                }
                file.CommitUpdate();
                buffer = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(buffer, 0, buffer.Length);
            }
            Response.AddHeader("content-disposition", "attachment;filename=图片.zip");
            Response.BinaryWrite(buffer);
            Response.Flush();
            Response.End();
            return new EmptyResult();
        }
    }
}