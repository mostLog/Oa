using MI.Application;
using MI.Application.OASession;
using MI.Web.Common;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MI.Web.Controllers
{
    public class HomeController : BaseController
    {
        private INoticeService _CRS;
        private IWorkDistributionService _CWD;
        private IRegistTipService _RTS;
        private ISTypeService _ITS;
        public HomeController(ISession session,
            INoticeService CRS,
            IWorkDistributionService CWD,
            IRegistTipService RTS,
            ISTypeService ITS, IEmployeeService emoloyee) : base(session, emoloyee)
        {
            _CRS = CRS;
            _CWD = CWD;
            _RTS = RTS;
            _ITS = ITS;
        }
        public ActionResult Index()
        {
            Session["notice"] = 0;
            Session["RegistTips"] = 0;
            var noticelist = _CRS.GetConditionByWhere(u => u.f_Type && u.f_StartDate <= DateTime.Now.Date && u.f_EndDate >= DateTime.Now.Date).OrderBy(u => u.f_EndDate).ToList();
            int count = _CRS.GetConditionByWhere(u => u.f_Type == false && u.f_StartDate <= DateTime.Now.Date && u.f_EndDate >= DateTime.Now.Date).ToList().Count;
            var worklist = _CWD.GetWorkDistributionByWhere(u => u.f_UrgentDate >= DateTime.Now.Date).OrderBy(u => u.f_UrgentDate).ToList();
            ViewBag.typeList = _ITS.QueryByCondition(p => p.f_type == (int)sTypeEnum.工作类别).ToList();
            if (count > 0)
            {
                Session["notice"] = count;
            }
            int iCount = _RTS.TipsCount(m_Eid);
            Session["RegistTips"] = iCount;
            return View(Tuple.Create(noticelist, worklist));
        }
    }
}