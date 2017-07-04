using MI.Application;
using MI.Application.Dto;
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

namespace TSOfficeSystem_New.Areas.Tickets.Controllers
{
    public class ReturnTicketController : BaseController
    {
        /// <summary>
        /// 机票服务
        /// </summary>
        public readonly IReturnTicketService _iReturnService;
        /// <summary>
        /// 人员表服务
        /// </summary>
        public readonly IEmployeeService _iEmployeeService;
        public ReturnTicketController(IReturnTicketService ireturnService,IEmployeeService iemployeeService, ISession session) : base(session, iemployeeService)
        {
            _iReturnService = ireturnService;
            _iEmployeeService = iemployeeService;
        }
        // GET: Tickets/ReturnTicket
        public ActionResult Index(TicketWhereDto ticketDto,int iPageIndex=1,int iPageSize=15)
        {
            int count;
            var list = ticketDto.isValid ? (_iReturnService.GetReturnTicketByWhere(ticketDto, iPageIndex, iPageSize, out count)) : (_iReturnService.GetReturnTicketAllData(iPageIndex, iPageSize, out count));
            ViewBag.iPageIndex = iPageIndex;
            ViewBag.iPageSize = iPageSize;
            ViewBag.iCount = count;
            ViewBag.ticketDto = ticketDto;
            ViewBag.listTypeDepartment = _iReturnService.GetNameByStype(p => p.f_type == (int)sTypeEnum.部门类型);
            ViewBag.listTypeInternational = _iReturnService.GetNameByStype(p => p.f_type == (int)sTypeEnum.国籍);
            ViewBag.AirlineType = _iReturnService.GetNameByStype(p => p.f_type == (int)sTypeEnum.航班类型);
            ViewBag.Eid = m_Eid;
            return View(list);
        }
        /// <summary>
        /// 新增与修改
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Edit(int Id = 0)
        {
            ReturnTicket model = new ReturnTicket();
            //新增
            model = Id == 0 ? new ReturnTicket() : _iReturnService.GetReturnTicketById(Id);
            ViewBag.AirlineType = _iReturnService.GetNameByStype(p => p.f_type == (int)sTypeEnum.航班类型);
            return View(model);
        }
        /// <summary>
        /// 新增修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UptateAndAdd(ReturnTicket model)
        {
            ErrorEnum resut = ErrorEnum.Error;
            string sReturnChar = string.Empty;
            if (AOUnity.ValidationLength("返乡地点", model.f_FromAddress, 50, out sReturnChar) || AOUnity.ValidationLength("返菲地点", model.f_FromAddress, 50, out sReturnChar) || AOUnity.ValidationLength("返乡备注", model.f_ToRemark, 100, out sReturnChar) || AOUnity.ValidationLength("返菲备注", model.f_FromRemark, 100, out sReturnChar))
            {
                resut = ErrorEnum.LengthOutRange;
                return Json(new ResultModel((int)resut, sReturnChar));
            }
            model.f_ToTerminal = _iReturnService.GetNameByStype(u => u.f_tID == model.f_ToAirlineType_Id).FirstOrDefault()?.f_Remark ?? "";

            model.f_FromTerminal = _iReturnService.GetNameByStype(u => u.f_tID == model.f_FromAirlineType_Id).FirstOrDefault()?.f_Remark ?? "";
            model.f_IsNewEmp = false;
            if (model.f_Id > 0)
            {
                //修改
                if (_iReturnService.EditReturnTicketOneData(model) > 0)
                    resut =ErrorEnum.Success;
            }
            else
            {
                //昵称
                model.f_Operation = base._session.GetCurrUser().NickName;
                model.f_OperationId = m_Eid;
                model.f_OperationDate = DateTime.Now.Date;
                if (base._session.GetCurrUser().NickName != null)
                {
                    //新增
                    if (_iReturnService.AddReturnTicketOneData(model)> 0)
                        resut = ErrorEnum.Success;
                }
                else
                {
                    return Json(new ResultModel((int)ErrorEnum.NickNameIsNull));
                }
            }
            return Json(new ResultModel((int)resut));
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id">id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            ErrorEnum resut = ErrorEnum.Error;
            if (_iReturnService.Deletet_ReturnTicket(Id)> 0)
                resut = ErrorEnum.Success;
            return Json(resut);
        }
        /// <summary>
        /// 导出EXCEL
        /// </summary>
        /// <param name="ticketDto">查询条件</param>
        /// <returns></returns>
        public FileResult Export(TicketWhereDto ticketDto)
        {
            var lisData = ticketDto.isValid ? _iReturnService.ExportDataByWhere(ticketDto) : _iReturnService.ExportData();
            DataSet ds = new DataSet();
            DataTable dt = DataTableExt.ToDataTable<dynamic>(lisData);
            ds.Tables.Add(dt);
            string sFileName = "机票登记统计表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            MemoryStream stream = NPOIExcelHelper.ExportExcel(dt, NPOIExcelHelper.ExtendPropertiesType.None, "t_ReturnTicket");
            return File(stream, "application/vnd.ms-excel", sFileName);
        }
        /// <summary>
        /// 下载 
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <returns></returns>
        public ActionResult Filedown(string FileName)
        {

            if (string.IsNullOrWhiteSpace(FileName))
            {
                return null;
            }
            FileName = EncryptHelper.AESDecrypt(FileName);
            string sPath = Server.MapPath("~/TicketFile/") + FileName;
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
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8));
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
                    if (fs != null)
                    {
                        fs.Dispose();
                    }
                }
            }
            return new EmptyResult();
        }
        /// <summary>
        /// 检查文件后缀名是否符合要求
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool checkFileExtension(string fileName)
        {
            //获取文件后缀
            string oExtension = Path.GetExtension(fileName).ToLower();
            if (oExtension != null)
                oExtension = oExtension.ToLower();
            else
                return false;
            if (!oExtension.Contains(".jpeg") && !oExtension.Contains(".jpg") && !oExtension.Contains(".png") && !oExtension.Contains(".doc") && !oExtension.Contains(".xls") && !oExtension.Contains(".pdf") && !oExtension.Contains(".rar"))
                return false;
            return true;
        }
        /// <summary>
        /// 快速获取人员信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetNames(string query)
        {
            List<Employee> emp = _iEmployeeService.GetNames(query);
            var rst = emp.Select(u => new { id = u.f_eid, name = String.Format("{0}--{1}--{2}", u.f_chineseName, u.f_nickname, u.f_passportName) });
            return Json(new { success = true, data = rst });
        }
        /// <summary>
        /// 上传资料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FileSave(string ToOrFrom, int f_eId = 0)
        {

            ErrorEnum result = ErrorEnum.Error;
            if (f_eId > 0)
            {
                try
                {
                    ReturnTicket model = _iReturnService.GetReturnTicketById(f_eId);
                    if (Request.Files.Count > 0)
                    {
                        HttpPostedFileBase file = Request.Files["imgFile"];
                        if (!checkFileExtension(file.FileName))
                        {
                            return Json(ErrorEnum.ImageFormat);//文件为不可上传的类型;
                        }
                        if (file.FileName.Length > 35)//文件名长度不能查过18，否则加密后长度超过100  报错 mu 2017.1.5
                        {
                            return Json(ErrorEnum.LengthOutRange);
                        }
                        string uploadPath = Server.MapPath("~/TicketFile/");
                        //检查路径
                        if (!Directory.Exists(uploadPath))
                            Directory.CreateDirectory(uploadPath);
                        file.SaveAs(uploadPath + System.IO.Path.GetFileName(file.FileName));
                        string filename = EncryptHelper.AESEncrypt(System.IO.Path.GetFileName(file.FileName));
                        if (ToOrFrom == "f_ToFile")
                        {
                            model.f_ToFile = filename;
                            model.f_ToIsTips = true;
                        }
                        else if (ToOrFrom == "f_FromFile")
                        {
                            model.f_FromFile = filename;
                            model.f_FromIsTips = true;
                        }
                        if(_iReturnService.EditReturnTicketOneData(model, null, false)>0)
                            result = ErrorEnum.Success;
                    }

                }
                catch (Exception e)
                {
                    //写入错误日记
                    AOUnity.WriteLog(e);
                }
            }
            return Json(result);
        }
    }
}