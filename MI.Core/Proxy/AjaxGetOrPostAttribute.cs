using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Proxy
{
    /// <summary>
    /// 生成get ajax请求
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AjaxGetOrPostAttribute:Attribute
    {
        public Type R { get; set; }
        public AjaxGetOrPostAttribute(Type r)
        {
            R = r;
        }
    }
    public enum Type
    {
        Get=1,
        Post
    }
}
