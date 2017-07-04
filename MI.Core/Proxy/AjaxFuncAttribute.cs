using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Proxy
{
    /// <summary>
    /// 是否为生成前端ajax 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AjaxFuncAttribute:Attribute
    {
        public string Url { get; set; }
        /// <summary>
        /// 服务名称
        /// </summary>
        /// <param name="serviceName"></param>
        public AjaxFuncAttribute(string url)
        {
            Url = url;
        }
    }
}
