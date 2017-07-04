using MI.Application;
using MI.Application.OASession;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TSOfficeSystem_New.Controllers;

namespace TSOfficeSystem_New.Areas.Dormitory.Controllers
{
    public class LatticeDController : BaseController
    {
        private readonly ILatticeContentService Lattice;
        public LatticeDController(ILatticeContentService LCS, ISession session, IEmployeeService emoloyee) : base(session, emoloyee)
        {
            Lattice = LCS;
        }
        // GET: Dormitory/DormitoryView
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sCommunity">社区</param>
        /// <param name="sBuilding">楼栋</param>
        /// <returns></returns>
        public ActionResult Index(string sCommunity = "", string sBuilding = "")
        {
            //社区
            List<string> listComm = Lattice.GetCommunity();
            if (string.IsNullOrWhiteSpace(sCommunity))
            {
                sCommunity = listComm.FirstOrDefault();
            }
            //楼栋
            List<string> listFloor = Lattice.GetFloorDong(sCommunity);
            if (listFloor.Count > 0 && string.IsNullOrWhiteSpace(sBuilding))
            {
                sBuilding = listFloor.FirstOrDefault();
            }
            sBuilding = sBuilding.Trim();
            sCommunity = sCommunity.Trim();

            //社区
            ViewBag.listItemCommunity = listComm.Select(p => new SelectListItem { Value = p, Text = p, Selected = p.Trim() == sCommunity });
            //楼栋
            ViewBag.listItemFloor = listFloor.Select(p => new SelectListItem { Value = p, Text = p, Selected = p.Trim() == sBuilding });
            //格子数据
            List<MI.Application.ContentServerce.Dto.t_LatticeContentDto> listModel = Lattice.GetFloorDongCommunityID(sBuilding, sCommunity);
            //部门
            ViewBag.listTypeSector = Lattice.GetSectorName(p => p.f_type == (int)sTypeEnum.部门类型);
            ViewBag.bIsDormitory = base._session.GetCurrUser().Permissions.Contains("Dormitory");//是否有宿舍管理的权限， 如果没有就不显示宿舍详情的链接
            //楼栋规模
            string sRak = Lattice.GetFloorScale(sBuilding.Trim())?.f_Remark ?? "0*0*false";
            if (!sRak.Contains("*"))
            {
                sRak = "0*0*false";
            }
            ViewBag.sVal = sRak.Split('*').Length == 2 ? (sRak + "*false").Split('*') : sRak.Split('*');

            return View(listModel);
        }
        /// <summary>
        /// 通过社区查询楼栋
        /// </summary>
        /// <param name="sCommunity">社区</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetFloorDong(string community)
        {
            return Json(Lattice.GetFloorDong(community));
        }
        /// <summary>
        /// 一键解锁
        /// </summary>
        /// <param name="sBuilding">楼栋</param>
        /// <param name="sCommunity">社区</param>
        /// <returns></returns>
        public JsonResult AkeyaKeyUnlocked(string sBuilding, string sCommunity)
        {
            if (string.IsNullOrWhiteSpace(sBuilding) || string.IsNullOrWhiteSpace(sBuilding))
            {
                return Json("参数错误");
            }
            else
            {
                return Json(Lattice.AKeyUnlockDormitory(sBuilding, sCommunity));
            }
        }
        ///  设置楼栋规模
        /// </summary>
        /// <param name="sBuilding">楼栋(value)</param>
        /// <param name="tID2">社区(value)</param>
        /// <param name="val1">有几层 （高）</param>
        /// <param name="val2">每层有几间（宽）</param>
        /// <param name="val3">房间号是否包含字母</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetSCALE(string tID, string tID2, int val1, int val2, bool val3)
        {
            int iResult = 0;
            if (!string.IsNullOrWhiteSpace(tID2) && !string.IsNullOrWhiteSpace(tID2) && val1 != 0 && val2 != 0)
            {
                try
                {
                    var modelBuilding = Lattice.GetLoudDogn(tID.Trim());
                    var modelCommunity = Lattice.GetCommunityByValue(tID2.Trim());
                    if (modelBuilding != null && modelCommunity != null)
                    {
                        modelBuilding.f_Remark = string.Concat(val1, "*", val2, "*", val3);
                        Lattice.Update(modelBuilding);//更新保持楼栋规模设置
                        if (Lattice.GeneratingGridData(modelBuilding.f_tID, modelCommunity.f_tID, val2, val1))//生成格子数据成功
                        {
                            iResult = 1;
                        }
                    }

                }
                catch (Exception e)
                {
                    iResult = -1;
                    //写入错误日记
                    AOUnity.WriteLog(e);
                }
            }
            return Json(iResult);
        }
        /// <summary>
        /// 解锁or锁定
        /// </summary>
        /// <param name="iLID">格子ID</param>
        /// <param name="val">true为解锁，反之</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetUnlock(int iLID, bool val)
        {
            int iResult = 0;
            string sClassName = string.Empty;
            try
            {
                iResult = Lattice.SetUnlock(iLID, val, out sClassName);
            }
            catch (Exception e)
            {
                iResult = -1;
                //写入错误日记
                AOUnity.WriteLog(e);
            }

            return Json(iResult, sClassName);
        }
    }
}