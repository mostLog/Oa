using MI.Application;
using MI.Application.OASession;
using MI.Core.Common;
using MI.Core.Domain;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSOfficeSystem_New.Controllers;

namespace TSOfficeSystem_New.Areas.Dormitory.Controllers
{
    /// <summary>
    /// 住宿汇总控制器
    /// 创建人：吕秀峰
    /// 创建时间：2017-06-26
    /// </summary>
    public class DormitorySummaryController :BaseController
    {
        private readonly IDormitorySummaryService _DormitorySummary;
        private readonly IDormitoryService _Dormitory;
        private readonly IEmployeeService _Employee;
        public DormitorySummaryController(IDormitorySummaryService DormitorySummary, IDormitoryService Dormitory, IEmployeeService Employee, ISession session, IEmployeeService emoloyee) : base(session, emoloyee)
        {
            _DormitorySummary = DormitorySummary;
            _Dormitory = Dormitory;
            _Employee = Employee;
        }
        // GET: Dormitory/DormitorySummary
        /// <summary>
        /// 主页显示
        /// </summary>
        /// <param name="sCommunity">社区</param>
        /// <param name="sBuilding">楼栋</param>
        /// <returns></returns>
        public ActionResult Index(string sCommunity = "", string sBuilding = "")
        {
            _DormitorySummary.DeleteCoupleNotServ();
            //社区集合
            List<string> listComm = _Dormitory.GetTariffbyCommunity();
            if (string.IsNullOrWhiteSpace(sCommunity))
            {
                sCommunity = listComm.FirstOrDefault();
            }
            //楼栋集合
            List<string> listBui = _Dormitory.GetTariffbyCommunity(sCommunity);
            if (listBui.Count > 0 && string.IsNullOrWhiteSpace(sBuilding))
            {
                sBuilding = listBui.FirstOrDefault();
            }
            //去空格
            sBuilding = sBuilding.Trim();
            sCommunity = sCommunity.Trim();
            ViewBag.listItemCommunity = listComm.Select(p => new SelectListItem { Value = p, Text = p, Selected = p.Trim() == sCommunity });
            ViewBag.listItemBui = listBui.Select(p => new SelectListItem { Value = p, Text = p, Selected = p.Trim() == sBuilding });
            ViewBag.listCoupleRegister = _DormitorySummary.GetCoupleRegister();
            //取每个房间的数据
            List<DormitorySummaryViewModel> listModel = _DormitorySummary.getDormitorySummary(sCommunity, sBuilding);
            return View(listModel);
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
        /// 打开修改页面(情侣关系)
        /// </summary>
        /// <param name="当前宿舍ID">ID</param>
        public ActionResult Edit()
        {
            List<t_CoupleRegister> list = new List<t_CoupleRegister> { new t_CoupleRegister(), new t_CoupleRegister() };
            return View(list);
        }
        /// <summary>
        /// JObject 扩展方法
        /// </summary>
        public class JSONNetResult : ActionResult
        {
            private readonly JObject _data;
            /// <summary>
            /// add 2016-9-28 by ale
            /// </summary>
            /// <param name="data"></param>
            public JSONNetResult(JObject data)
            {
                _data = data;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                var response = context.HttpContext.Response;
                response.ContentType = "application/json";
                response.Write(_data.ToString(Newtonsoft.Json.Formatting.None));
            }
        }
        /// <summary>
        /// 提交新增/修改
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditCP(List<t_CoupleRegister> list)
        {
            JObject json = new JObject();
            int iIndex = 1;
            foreach (var tCoupleRegister in list)
            {
                string msg = _DormitorySummary.CheckCoupleRegister(tCoupleRegister);
                if (msg == "OK")
                {
                    int result =_DormitorySummary.AddCoupleRegister(tCoupleRegister) ;
                    json.Add(iIndex.ToString(),result>0 ? "添加成功" : "添加失败");
                }
                else
                {
                    json.Add(iIndex.ToString(), msg);
                }
                iIndex++;
            }
            return new JSONNetResult(json);
        }

        /// <summary>
        /// 打开管理页面(情侣关系)
        /// </summary>
        /// <param name="keyWords"></param>
        /// <param name="iPageIndex"></param>
        /// <param name="iPageSize"></param>
        public ActionResult ManageCP(string keyWords, int iPageIndex = 1, int iPageSize = 10)
        {
            int o_Count;
            List<CoupLeManageView> listCP = _DormitorySummary.GetCoupleRegister(keyWords, iPageIndex, 10,
                out o_Count);
            ViewBag.iCount = o_Count;
            ViewBag.sKeyWords = keyWords;
            ViewBag.iPageIndex = iPageIndex;
            ViewBag.iPageSize = 10;
            return View(listCP);
        }

        /// <summary>
        /// 删除情侣关系
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public ActionResult DeleteCP(int cID)
        {
            int msg = 0;
            if (cID <= 0)
            {
                return Json(0);
            }
           
            if (_DormitorySummary.DeleteCoupleRegister(cID) > 0)
            {
                msg = 1;
            }
            return Json(msg);
        }
        /// <summary>
        /// 更新情侣关系
        /// </summary>
        /// <param name="CoupleRegister"></param>
        /// <returns></returns>
        public ActionResult UpdateCP(t_CoupleRegister CoupleRegister)
        {
            string msg = _DormitorySummary.CheckCoupleRegister(CoupleRegister);
            if (msg != "OK") return Json(msg);
            if (_DormitorySummary.UpdateCoupleRegister(CoupleRegister) > 0)
            {
                msg = "1";
            }
            else
            {
                msg = "0";
            }
            return Json(msg);
        }

        /// <summary>
        /// 快速获取人员信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetNames(string query)
        {
            List<Employee> emp = _Employee.GetNames(query);
            var rst = emp.Select(u => new
            {
                id = u.f_eid,
                name =
                $"{u.f_chineseName}--{u.f_nickname}--{u.f_passportName}"
            });
            return Json(new { success = true, data = rst });
        }
    }
}