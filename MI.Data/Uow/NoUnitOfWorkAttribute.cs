using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Data.Uow
{
    /// <summary>
    /// 过滤不需要拦截的方法
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class NoUnitOfWorkAttribute:Attribute
    {

    }
}
