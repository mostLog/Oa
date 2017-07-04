using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MI.Core.Domain;

namespace MI.Application.ISType
{
    public interface ISTypeService
    {
        IList<SType> GetsType();

        /// <summary>
        /// 根据条件查询类型
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>返回一个t_sType的list集合</returns>
        List<SType> QureyByCondition(Func<SType, bool> predicate);

        /// <summary>
        /// 添加类型
        /// </summary>
        /// <param name="sType"></param>
        /// <returns></returns>
        void AddsType(SType sType);
    }
}
