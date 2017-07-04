using MI.Application;
using MI.Application.OASession;
using MI.Application.OASession.Dto;
using MI.Core.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace MI.Web.Controllers
{
    /// <summary>
    /// 基类控制器
    /// </summary>
    public class BaseController : Controller
    {
        protected readonly ISession _session;
        protected readonly IEmployeeService _emoloyee;
        public BaseController(ISession session, IEmployeeService emoloyee)
        {
            _session = session;
            _emoloyee = emoloyee;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (_session != null)
            {
                OAUser user = _session.GetCurrUser();
                //判断当前用户会话是否到期 是否可以登录
                if (user == null ||
                    string.IsNullOrWhiteSpace(user.IsLogin) ||
                    !user.IsLogin.Contains("true") ||
                    !user.Permissions.Contains(base.RouteData.Values["Controller"].ToString())
                    )
                {
                    filterContext.Result = new RedirectResult("~/Login/Index");
                }
                else
                {
                    //权限集合
                    m_RankStr = user.Permissions;
                    m_Eid = user.Id;
                    ViewBag.levelValue = m_RankStr;
                    var empone = _emoloyee.GetEmployeeById(m_Eid);
                    ViewBag.passportDate = empone.f_passportDate?.ToString("yyyy-MM-dd") ?? "";
                    ViewBag.ICardDate = empone.f_ICardDate?.ToString("yyyy-MM-dd") ?? "";
                    ViewBag.UserName = user.UserName + "/" + user.PassportName;
                }
            }
            else
            {
                filterContext.Result = new RedirectResult("~/Login/Index");
            }
        }
        /// <summary>
        /// 权限字串
        /// </summary>
        public IList<string> m_RankStr { get; set; }
        /// <summary>
        /// 登陆用户ID
        /// </summary>
        public int m_Eid { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string m_NickName { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public int m_Department { get; set; }

        /// <summary>
        /// 图片流 转为    base64编码的文本 (自动加上头部)
        /// </summary>
        /// <param name="strUrl">图片路径</param>
        /// <param name="isLuesuo">是否略缩图</param>
        /// <param name="isEntrystamp">是否入境章</param>
        public string ToBase64String(string strUrl, bool isLuesuo = false, bool isEntrystamp = false, bool isIDCard = false)
        {
            string strbaser64 = string.Empty;
            if (!string.IsNullOrWhiteSpace(strUrl) && strUrl != "500")
            {
                strUrl = EncryptHelper.AESDecrypt(strUrl);
                string sPath = Server.MapPath("~/images/" + (isEntrystamp ? "UserES" : isIDCard ? "UserIDC" : "User") + "/" + (isLuesuo ? "Thumbnail/" : "Original/")) + strUrl;
                if (strUrl != null && System.IO.File.Exists(sPath))
                {
                    System.Drawing.Bitmap bmp = null;
                    MemoryStream ms = null;
                    try
                    {
                        bmp = new System.Drawing.Bitmap(sPath);
                        ms = new MemoryStream();
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        strbaser64 = "data:image/jpg;base64," + Convert.ToBase64String(ms.ToArray());
                    }
                    finally
                    {
                        bmp?.Dispose();
                        ms?.Close();
                        ms?.Dispose();

                    }
                }
            }
            return strbaser64;
        }
        /// <summary>
        /// 检查是否有权限上传。有员工管理的等级可以上传其他人的图片。其他等级只能上传当前登录用户的图片
        /// </summary>
        /// <returns></returns>
        public bool CheckLevel(int eid)
        {
            return ((m_RankStr != null && m_RankStr.Contains("employeeInfo")) || (m_Eid != 0 && eid == m_Eid));
        }
    }
}