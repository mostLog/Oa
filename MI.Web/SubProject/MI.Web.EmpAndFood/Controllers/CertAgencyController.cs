using MI.Application;
using MI.Application.Dto;
using MI.Application.OASession;
using MI.Core.Common;
using MI.Core.Domain;
using MI.Web.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MI.Web.EmpAndFood.Controllers
{
    public class CertAgencyController : BaseController
    {
        //证件办理服务
        private readonly ICertAgencyService _CertAgencyService; 
        //类型服务
        private readonly ISTypeService _STypeService;
        //人员服务
        private readonly IEmployeeService _EmployeeService;
        public CertAgencyController(
            ICertAgencyService certAgencyService,
            ISTypeService STypeService,
            IEmployeeService EmployeeService,
            ISession session, IEmployeeService emoloyee) : base(session, emoloyee)
        {
            _CertAgencyService = certAgencyService;
            _STypeService = STypeService;
            _EmployeeService = EmployeeService;
        }
        // GET: EmpAndFood/CertAgency
        public ActionResult Index(CertAgencyWhereDto cwd, int iPageIndex = 1,int iPageSize=15)
        {
            int o_Count = 0;
            var list = _CertAgencyService.GetCertAgencyByWhere(cwd, iPageIndex, iPageSize, out o_Count);
            ViewBag.listTypeDepartment = _STypeService.GetsTypeByWhere((int)sTypeEnum.部门类型);
            ViewBag.listWorkLocation = _STypeService.GetsTypeByWhere((int)sTypeEnum.上班地点类型);
            ViewBag.listCertType = _STypeService.GetsTypeByWhere((int)sTypeEnum.证件类型);
            ViewBag.listProcess = _STypeService.GetsTypeByWhere((int)sTypeEnum.办理进度);
            ViewBag.listEmployee = _EmployeeService.GetAllEmployeeData();
            ViewBag.CWD = cwd;
            ViewBag.iPageIndex = iPageIndex;
            ViewBag.iPageSize = iPageSize;
            ViewBag.iCount = o_Count;
            return View(list);
        }

        public ActionResult Edit(int iId=0)
        {
            CertAgency model = new CertAgency();
            string sChineseName = string.Empty;
            if (iId != 0)
            {
                model = _CertAgencyService.GetCertAgencyById(iId);
                if (model?.f_eId != null)
                {
                    var emp = _EmployeeService.GetEmployeeById(model.f_eId.Value);
                    sChineseName = emp.f_chineseName;
                }
            }
            ViewBag.f_chineseName = sChineseName;
            ViewBag.listCertType = _STypeService.GetsTypeByWhere((int)sTypeEnum.证件类型);
            ViewBag.listProcess = _STypeService.GetsTypeByWhere((int)sTypeEnum.办理进度);

            return View(model);
        }

        /// <summary>
        /// 修改编辑所要执行的方法
        /// </summary>
        /// <param name="model">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditOrAdd(CertAgency model)
        {
            int result = 0;
            ErrorEnum eMsg = ErrorEnum.Error;
            string sMsg = string.Empty;
            if (AOUnity.ValidationLength("备注", model.f_Remark,50, out sMsg))
            {
                eMsg = ErrorEnum.LengthOutRange;
                return Json(new ResultModel((int)eMsg, sMsg));
            }
            model.f_Operator = base._session.GetCurrUser().NickName;
            model.f_OperatorTime = DateTime.Now;
            if (model.f_Id > 0)
            {
                _CertAgencyService.EditCertAgencyData(model);
            }
            else
            {
                _CertAgencyService.AddCertAgencyData(model);
            }
            if (result > 0)
            {
                eMsg = ErrorEnum.Success;
            }
            return Json(new ResultModel((int)eMsg));
        }

        /// <summary>
        /// 删除所要执行的方法
        /// </summary>
        /// <param name="Id">条件</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            ErrorEnum eMsg = ErrorEnum.Error;
            _CertAgencyService.DeleteCertAgency(Id);
           
            return Json(eMsg);
        }

        /// <summary>
        /// 快速获取人员信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetNames(string query)
        {
            IList<Employee> emp = _EmployeeService.GetEmployeeByName(query);
            var rst = emp.Select(u => new {
                id = u.f_eid,
                name =
                $"{u.f_chineseName}--{u.f_nickname}--{u.f_passportName}--{u.f_AccountName}"
            });
            return Json(new { success = true, data = rst });
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="hidFile">文件名称</param>
        /// <param name="f_Id">证件办理ID</param>
        /// <returns></returns>
        public ActionResult FileSave(string hidFile, int f_Id = 0)
        {
            ErrorEnum eMsg = ErrorEnum.Error;
            if (f_Id > 0)
            {
                try
                {
                    //根据Id获得证件信息
                    CertAgency model = _CertAgencyService.GetCertAgencyById(f_Id);
                    if (Request.Files.Count > 0)
                    {
                        HttpPostedFileBase file= Request.Files["imgFile"];
                        if (file != null && !CheckFileSuffixName(file.FileName))
                        {
                            return Json(ErrorEnum.ImageFormat);
                        }
                        string sUploadPath= Server.MapPath("~/File/CertAgencyFile/");
                        //判断文件下载路径是否存在
                        if (!Directory.Exists(sUploadPath))
                        {
                            Directory.CreateDirectory(sUploadPath);
                        }
                        file.SaveAs(sUploadPath + Path.GetFileName(file.FileName));
                        //获得加密后的文件名称
                        string strFileName = EncryptHelper.AESEncrypt(Path.GetFileName(file.FileName));
                        model.f_FileName = strFileName;
                        model.f_DownTips = true;
                        //修改证件办理信息
                        _CertAgencyService.EditCertAgencyData(model);
                       
                    }
                }
                catch(Exception e)
                {
                    AOUnity.WriteLog(e);
                }
            }
            return Json(eMsg);
        }

        /// <summary>
        /// 检查文件后缀名是否符合要求
        /// </summary>
        /// <param name="sFileName">文件名称</param>
        /// <returns></returns>
        private bool CheckFileSuffixName(string sFileName)
        {
            List<string> lstRes = new List<string> { ".jpeg", ".jpg", ".png", ".doc", ".xls", ".pdf", ".rar" };
            string sSuffixName = Path.GetExtension(sFileName);
            if (sSuffixName != null)
            {
                //判断文件后缀名是否包含在集合中
                if(!lstRes.Contains(sSuffixName.ToLower()))
                {
                    return false;
                }                    
            }
            return true;
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="sFileName">文件名称</param>
        /// <returns></returns>
        public ActionResult FileDown(string fileName)
        {
            //判断文件名称是否存在
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return null;
            }
            string sAesFileName= EncryptHelper.AESDecrypt(fileName); 
            string sPath = Server.MapPath("~/File/CertAgencyFile/") + sAesFileName;
            //判断指定的文件路径是否存在
            if (System.IO.File.Exists(sPath))
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(sPath, FileMode.OpenOrCreate);
                    byte[] bytes = new byte[(int)fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                    fs.Close();
                    Response.ContentType = "application/octet-stream";
                    //通知浏览器下载文件而不是打开
                    Response.AddHeader("Content-Disposition",
                        "attachment;   filename=" + HttpUtility.UrlEncode(sAesFileName, System.Text.Encoding.UTF8));
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    Response.End();

                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    fs?.Dispose();
                }
            }
            return new EmptyResult();
        }
    }
}