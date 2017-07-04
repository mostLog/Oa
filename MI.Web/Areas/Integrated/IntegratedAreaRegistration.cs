using System.Web.Mvc;

namespace TSOfficeSystem_New.Areas.Integrated
{
    public class IntegratedAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Integrated";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Integrated_default",
                "Integrated/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}