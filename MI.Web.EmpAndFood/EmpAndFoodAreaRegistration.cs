using System.Web.Mvc;

namespace MI.Web.EmpAndFood
{
    public class EmpAndFoodAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "EmpAndFood";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "EmpAndFood_default",
                "EmpAndFood/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}