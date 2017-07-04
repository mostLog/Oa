using MI.Application;
using MI.Application.OASession;
using MI.Core.Common;
using MI.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TSOfficeSystem_New.Controllers;

namespace TSOfficeSystem_New.Areas.Dormitory.Controllers
{
    /// <summary>
    /// 洗衣房控制器
    /// </summary>
    public class LaundryPwdController : BaseController
    {
        public readonly ILaundryPwdService _lps;
        public readonly ISTypeService _CRS;
        private readonly IDormitoryService _Dormitory;
        // GET: Dormitory/LaundryPwd
        public LaundryPwdController(ILaundryPwdService lps, ISTypeService CRS, IDormitoryService Dormitory, ISession session, IEmployeeService emoloyee) : base(session, emoloyee)
        {
            _lps = lps;
            _CRS = CRS;
            _Dormitory = Dormitory;
        }
        public ActionResult Index(string sNoandwpd, int iRoomtype = 0, string sCommunity = "", string sBuilding = "")
        {
            //查询所有社区
            List<string> listComm = _Dormitory.GetTariffbyCommunity();
            //获取第一个社区
            if (string.IsNullOrWhiteSpace(sCommunity))
            {
                sCommunity = listComm.FirstOrDefault();
            }
            //根据获得第一个社区查询楼栋
            List<string> listBui = _Dormitory.GetTariffbyCommunity(sCommunity);
            //不为空则绑定第一个楼栋
            if (listBui.Count > 0 && string.IsNullOrWhiteSpace(sBuilding))
            {
                sBuilding = listBui.FirstOrDefault();
            }
            //获取所有社区及第一个绑定的社区
            ViewBag.listItemCommunity = listComm.Select(p => new SelectListItem { Value = p, Text = p, Selected = p.Trim() == sCommunity});
            //获取所有楼栋及第一个绑定的楼栋
            ViewBag.listItemBui = listBui.Select(p => new SelectListItem { Value = p, Text = p, Selected = p.Trim() == sBuilding });
            //按条件查询列表
            List<t_LaundryPwd> oListla = _lps.GetLaundryPwdByWhere(u => u.f_Community == sCommunity && u.f_Building == sBuilding && (iRoomtype == 0 || u.f_RoomType == iRoomtype) && (string.IsNullOrWhiteSpace(sNoandwpd) || u.f_NoAndPwd.ToLower().Contains(sNoandwpd.ToLower())));
            //接收密码和类别（指洗衣房或晾衣房）
            ViewBag.sNoandwpd = sNoandwpd;
            ViewBag.iRoomtype = iRoomtype;
            return View(oListla);
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
        /// 添加修改视图
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Edit(int Id = 0)
        {
            ViewBag.CommunityType = _CRS.GetTypeByWhere(u =>u.f_type== (int)sTypeEnum.社区类型).ToList();
            t_LaundryPwd model;
            //新增
            model = Id == 0 ? new t_LaundryPwd { } : _lps.GetLaundryPwdById(Id);
            ViewBag.Buildings = Id == 0 ? new List<string>() : _Dormitory.GetTariffbyCommunity(model.f_Community);
            return View(model);
        }
        /// <summary>
        /// 添加修改方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UptateAndAdd(t_LaundryPwd model, string sPwd, string sNo)
        {
            ErrorEnum result = ErrorEnum.Error;
            string sReturnChar = string.Empty;
            if (model.f_Id > 0)
            {
                _lps.EditLaundryPwdOneData(model);
                result = ErrorEnum.Success;
            }
            else
            {
                _lps.AddLaundryPwdOneData(model);
                result = ErrorEnum.Success;
            }
            return Json(result);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            ErrorEnum result = ErrorEnum.Error;
            _lps.DeleteLaundryPwd(Id);
            result = ErrorEnum.Success;
            return Json(result);
        }
    }
}