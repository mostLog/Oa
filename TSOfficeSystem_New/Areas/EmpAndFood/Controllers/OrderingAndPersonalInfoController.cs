using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MI.Application.OASession;
using TSOfficeSystem_New.Controllers;
using MI.Application;
using MI.Core.Domain;
using MI.Core.Common;
using MI.Application.Dto;

namespace TSOfficeSystem_New.Areas.EmpAndFood.Controllers
{
    /// <summary>
    /// 个人资料
    /// </summary>
    public class OrderingAndPersonalInfoController : BaseController
    {
        //员工服务
        private readonly IEmployeeService _EmployeeService;
        //类型服务
        private readonly ISTypeService _STypeService;
        //宿舍服务
        private readonly IDormitoryService _DormitoryService;
        public OrderingAndPersonalInfoController(IEmployeeService EmployeeService,
            ISTypeService STypeService,
            IDormitoryService DormitoryService,
            ISession session, IEmployeeService emoloyee) : base(session, emoloyee)
        {
            _EmployeeService = EmployeeService;
            _STypeService = STypeService;
            _DormitoryService = DormitoryService;
        }

        // GET: EmpAndFood/OrderingAndPersonalInfo
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 个人资料页面数据展示
        /// </summary>
        /// <param name="iEid">当前登录用户Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PersonalProfile(int iEid=0)
        {
            //获得登陆用户Id
            int m_Eid= base._session.GetCurrUser().Id;
            iEid = iEid == 0 ? m_Eid : iEid;
            ViewBag.listData = _STypeService.GetTypeByWhere(m => m.f_type == 5 || m.f_type == 6);
            //通过用户ID获得用户信息
            Employee model = _EmployeeService.GetEmployeeById(iEid);
            ViewBag.listTypePeriodType = _STypeService.GetsTypeByWhere((int)sTypeEnum.班次类型);
            ViewBag.listStrCommunity = _DormitoryService.GetTariffbyCommunity();
            ViewBag.listTypeWorkLocation = _STypeService.GetsTypeByWhere((int)sTypeEnum.上班地点类型);
            ViewBag.listTypeworkStatus = _STypeService.GetTypeByWhere(p => p.f_type == (int)sTypeEnum.上班状态类型);
            ViewBag.listTypeLevel = _STypeService.GetTypeByWhere(p => p.f_type == (int)sTypeEnum.权限类型);
            ViewBag.listTypeLDM = _STypeService.GetTypeByWhere(p => p.f_type == (int)sTypeEnum.订餐类型);
            ViewBag.listTypeLDMType = _STypeService.GetTypeByWhere(p => p.f_type == (int)sTypeEnum.订餐要求类型);
            ViewBag.GuoJi = _STypeService.GetTypeByWhere(p => p.f_type == (int)sTypeEnum.国籍);
            ViewBag.VisaType = _STypeService.GetTypeByWhere(p => p.f_type == (int)sTypeEnum.签证类型);
            ViewBag.TpassportURL = ToBase64String(model?.f_PassportURL, true);
            ViewBag.TentrystampURL = ToBase64String(model?.f_EntrystampURL, true, true);
            ViewBag.TidCardURL = ToBase64String(model?.f_IDCardURL, true, false, true);
            ViewBag.LaundryPwd = _EmployeeService.GetLaundryPwd(int.Parse(model.t_Dormitory?.f_LaundryAndPwd ?? "0"));
            return View("IndexView",model);
        }

        /// <summary>
        ///  修改个人资料
        /// </summary>
        /// <param name="model">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditEmp(AllEmployeeInfoViewModel model)
        {
            ErrorEnum eMsg = ErrorEnum.Fail;
            eTipsEnum eTips = eTipsEnum.FailOperation;
            string sExceptionMessage = string.Empty;
            model.EmployeeInfo.f_eid = base._session.GetCurrUser().Id;
            if (model.EmployeeInfo.CheckModel(out eTips, false))
            {
                try
                {
                    //判断更新是否成功
                    _EmployeeService.UpdateEmployeeInfo(model, true);
                }
                catch(Exception e)
                {
                    sExceptionMessage = e.Message;
                    eMsg = ErrorEnum.Error;
                    eTips = eTipsEnum.Error;
                    //写入错误日记
                    AOUnity.WriteLog(e);
                }
            }
            return Json(new ResultModel((int)eMsg, eTips.Description(), sExceptionMessage));
        }

        /// <summary>
        /// 修改密码的页面展示
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifyPwd()
        {
            //获得当前登录用户ID
            int m_Eid = base._session.GetCurrUser().Id;
            if (m_Eid > 0)
            {
                return View(new ModifyPwdModel(m_Eid));
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
           
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ModifyPwd(ModifyPwdModel model)
        {
            ErrorEnum eMsg = ErrorEnum.Fail;
            eTipsEnum eTips = eTipsEnum.FailOperation;
            string sError = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    var list = _EmployeeService.GetEmployeeById(model.eid);
                    //判断通过用户id获得的密码和修改之前的旧密码是否相同
                    if (list != null && list.f_pwd == model.oldPwd)
                    {
                        //修改当前登录用户的密码
                        _EmployeeService.EditEmployeePwd(model);
                       
                    }
                }
                else
                {
                    eTips = eTipsEnum.pwdLengthError;
                }
            }
            catch (Exception e)
            {
                eMsg = ErrorEnum.Error;
                eTips = eTipsEnum.Error;
                AOUnity.WriteLog(e);
            }
            return Json(new ResultModel(eMsg, eTips, sError));
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
        public ActionResult UploadFile(string imgName, int eid = 0, string HZorRJZtype = "p")
        {
            string sResult = string.Empty;   //返回结果 默认空的结果
            if (CheckLevel(eid))
            {
                Employee oModel = _EmployeeService.GetEmployeeById(eid);
                ResultModel modelResult = Public.BLLUploadFile(imgName, eid, HZorRJZtype);
                if (eid > 0 && modelResult.result == (int)ErrorEnum.Success && !string.IsNullOrWhiteSpace(modelResult.tips))
                {
                    _EmployeeService.UpdetePassporUrl(oModel, modelResult.tips, HZorRJZtype);
                    sResult = modelResult.tips;
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
            var model = _EmployeeService.GetEmployeeById(eid);
            Public.BLLDownloadFile(model, t);
            return new EmptyResult();
        }
        #endregion

        /// <summary>
        /// post 请求加载大图片
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult getImagesbase64(string str, string strE = "", string strC = "", string HZorRJZtype = "p")
        {
            return Content(ToBase64String(HZorRJZtype == "p" ? str : HZorRJZtype == "e" ? strE : strC, false, HZorRJZtype == "e", HZorRJZtype == "c"), "text");
        }
    }
}