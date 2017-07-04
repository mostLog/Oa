using System.Web;
using System.Web.Optimization;

namespace TSOfficeSystem_New
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            //// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));
            ///这个类会在发布的时候自动压缩，这里引用不需要使用压缩版
            ///如果想不发布也压缩 请添加 BundleTable.EnableOptimizations = true;
            //////////////////////JS
            //jquery
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            //jquery.form
            bundles.Add(new ScriptBundle("~/bundles/jqueryForm").Include(
                      "~/Scripts/jquery.form.js"));

            //datetimepicker
            bundles.Add(new ScriptBundle("~/bundles/datetimepicker").Include(
                 "~/Scripts/datetimepicker/jquery.datetimepicker.full.js"
                 ));
            //bootstrap
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                  "~/Scripts/Bootstrap3.3.5/js/bootstrap.js"
                  ));
            //public.setHeightWidth()
            bundles.Add(new ScriptBundle("~/bundles/JavaScript").Include(
                  "~/Scripts/Script_v_1.0/JavaScript.js"
                  ));
            // 员工修改信息(图片上传，提交)
            bundles.Add(new ScriptBundle("~/bundles/EmployeesEdit").Include(
                  "~/Scripts/Script_v_1.0/EmployeeInfoEdit.js"
                  ));
            //拖动控件
            bundles.Add(new ScriptBundle("~/bundles/drag").Include(
                  "~/Scripts/Drag/drag.js"
                  ));
            //旋转控件
            bundles.Add(new ScriptBundle("~/bundles/jqueryRotate").Include(
                  "~/Scripts/jquery.rotate.js"
                  ));
            //////////////////////CSS
            //datetimepicker css
            bundles.Add(new StyleBundle("~/Content/datetimepicker").Include("~/Css/datetimepicker/jquery.datetimepicker.css"));
            //bootstrap
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include("~/Css/Bootstrap3.3.5/css/bootstrap.css"));

            //Responsive + Style
            bundles.Add(new StyleBundle("~/Content/ResponsiveStyle").Include("~/Css/Responsive.css", "~/Css/Style.css", "~/Css/load.css"));
            //EmployeeinfoEdit.css
            bundles.Add(new StyleBundle("~/Content/EmployeeinfoEdit").Include("~/Css/EmployeeinfoEdit.css"));
            //datetimepicker css
            bundles.Add(new StyleBundle("~/Content/datetimepicker").Include("~/Css/datetimepicker/jquery.datetimepicker.css"));
            //datetimepicker
            bundles.Add(new ScriptBundle("~/bundles/datetimepicker").Include(
               "~/Scripts/datetimepicker/jquery.datetimepicker.full.js"
               ));
        }
    }
}
