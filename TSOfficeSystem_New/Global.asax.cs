using Autofac;
using Autofac.Integration.Mvc;
using MI.Application.Mappers;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TSOfficeSystem_New
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AutofacInit();

            new ApplicationMapperConfiguraton().Initialize();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //清理临时文件夹数据
            //ClearTempFold();
        }
        protected void Application_End()
       {

        }

        protected void AutofacInit()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyModules(Assembly.Load("MI.Core"),Assembly.Load("MI.Data"),Assembly.Load("MI.Application"));

            

            //注册Controller
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
           

            
        }
        protected void ClearTempFold()
        {
            string tempFold = HttpContext.Current.Server.MapPath("~/Temp/Downloads");
            try
            {
                DirectoryInfo dir = new DirectoryInfo(tempFold);

                if (dir.Exists)
                {
                    var files = dir.GetFileSystemInfos();
                    foreach (var file in files)
                    {
                        if (file is DirectoryInfo)
                        {
                            DirectoryInfo subDir = new DirectoryInfo(file.FullName);
                            subDir.Delete(true);
                        }
                        else
                        {
                            file.Delete();
                        }
                    }
                }else
                {
                    dir.Create();
                }
                
            }
            catch (System.Exception e)
            {

                throw;
            }
        }
    }
}
