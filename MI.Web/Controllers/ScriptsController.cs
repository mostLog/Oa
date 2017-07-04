using MI.Core.Proxy;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace MI.Web.Controllers
{
    /// <summary>
    /// 生成Ajax方法
    /// 
    /// 小白-2017-6-21
    /// </summary>
    public class ScriptsController : Controller
    {
        public JavaScriptResult GetScripts()
        {
            return JavaScript(GenerateAjaxJs());
        }
        /// <summary>
        /// 生成ajax js方法
        /// </summary>
        protected string GenerateAjaxJs()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            //
            var types = assembly
                .GetTypes()
                .Where(t => t.BaseType != null && t.GetCustomAttribute(typeof(AjaxFuncAttribute)) != null).ToList();

            StringBuilder builder = new StringBuilder();
            foreach (var type in types)
            {
                //服务名
                string name = type.Name.Replace("Controller", "");
                //
                var attr = (AjaxFuncAttribute)type.GetCustomAttribute(typeof(AjaxFuncAttribute));
                var methods = type.GetMethods().Where(f => f.GetCustomAttribute(typeof(AjaxGetOrPostAttribute)) != null).ToList();

                builder.AppendLine("(function(){");
                builder.AppendLine("  var service=oa.util.createService('" + name.ToLower() + "service');");
                for (int i = 0; i < methods.Count(); i++)
                {
                    var method = methods[i];
                    //方法名称
                    string mName = method.Name;
                    string p = FormatParameters(method.GetParameters());
                    builder.AppendLine("  service." + ToFirstLetterLower(mName) + "=function(" + p + "){");
                    builder.AppendLine("    return $.ajax({");
                    builder.AppendLine("      type: 'POST',");
                    builder.AppendLine("      url: '" + attr.Url + "/" + name + "/" + mName + "',");
                    if (!string.IsNullOrEmpty(p))
                    {
                        builder.AppendLine("      data: " + p);
                    }
                    builder.AppendLine("    });");
                    builder.AppendLine("  };");
                }
                builder.AppendLine("})();");

            }
            return builder.ToString();
        }
        protected string FormatParameters(ParameterInfo[] parameters)
        {
            IList<string> pList = new List<string>();
            foreach (var parameter in parameters)
            {
                pList.Add(parameter.Name);
            }
            return string.Join(",", pList);
        }
        /// <summary>
        /// 首字母转换为小写
        /// </summary>
        /// <returns></returns>
        protected string ToFirstLetterLower(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            return value.Substring(0, 1).ToLower() + value.Substring(1);
        }
    }
}