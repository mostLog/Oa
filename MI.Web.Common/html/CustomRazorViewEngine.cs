using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MI.Web.Common.html
{
    /// <summary>
    /// 自定义视图引擎
    /// </summary>
    public class CustomRazorViewEngine: RazorViewEngine
    {
        public CustomRazorViewEngine()
        {
            AreaViewLocationFormats = new[] {
                "~/SubProject/MI.Web.{2}/Views/{1}/{0}.cshtml",
                "~/SubProject/MI.Web.{2}/Views/Shared/{0}.cshtml",
            };
        }
    }
}
