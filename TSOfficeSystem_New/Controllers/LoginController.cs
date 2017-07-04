using MI.Application;
using MI.Application.Dto;
using MI.Application.OASession.Dto;
using MI.Core.Extension;
using System.Web.Mvc;

namespace TSOfficeSystem_New.Controllers
{
    public class LoginController : Controller
    {
        //员工服务
        private readonly IEmployeeService _employeeService;
        public LoginController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginDto input)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", input);
            }
            var employee = _employeeService.Login(input);
            if (employee == null)
            {
                ModelState.AddModelError("error", "当前用户不存在！");
            }
            else if (employee.f_pwd.IsNullOrEmpty() || employee.f_pwd != input.Password)
            {
                ModelState.AddModelError("error", "登陆密码错误！");
            }
            else if (employee.STypeWorkStatus == null ||
              employee.STypeLevel == null ||
              !employee.STypeLevel.f_Remark.Contains("Login") ||
              !employee.STypeWorkStatus.f_Remark.Contains("true"))
            {
                //判断工作状态与工作等级
                ModelState.AddModelError("error","当前用户无登陆权限！");
            }
            else
            {
                //登陆成功
                OAUser user = new OAUser()
                {
                    Id = employee.f_eid,
                    UserName = employee.f_chineseName,
                    NickName = employee.f_nickname,
                    PassportName = employee.f_passportName,
                    IsLogin = employee.STypeWorkStatus?.f_Remark,
                    Permissions=employee.STypeLevel.f_Remark.Split(',')
                };
                Session["CurrUser"] = user;

                //FormsAuthentication.SetAuthCookie(user.UserName+"/"+user.PassportName,false);
                return RedirectToAction("Index", "Home");
            }

            return View("Index", input);
        }
        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>

        public ActionResult Layout()
        {
            Session["currUser"] = null;
            //FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }


    }
}