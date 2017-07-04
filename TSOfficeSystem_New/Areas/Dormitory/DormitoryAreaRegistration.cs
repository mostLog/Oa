using System.Web.Mvc;

namespace TSOfficeSystem_New.Areas.Dormitory
{
    public class DormitoryAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Dormitory";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Dormitory_default",
                "Dormitory/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "TSOfficeSystem_New.Areas.Dormitory.Controllers" }
            );
        }
    }
}