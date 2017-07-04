using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application
{
    public interface INewOrderingEmployeesService
    {
        /// <summary>
        /// 更新新人订餐信息(宿舍)
        /// </summary>
        /// <param name="listModel"></param>
        void UpdateOrderingEmployees(List<NewOrderingEmployees> listModel);
        /// <summary>
        /// 新人登記未訂餐時，添加工作交接記錄
        /// <param name="employee">員工表</param>
        /// </summary>
        void UpdateWorkDistribution(Employee employee);
        /// <summary>
        /// 根据员工ID删除所有订餐数据 
        /// </summary>
        /// <param name="eId"></param>
        void DeleteAllClear(int eId);

        /// <summary>
        /// 删除所有订餐数据 根据员工ID
        /// </summary>
        /// <param name="eId"></param>
        void AllClear(int eId);
    }
}
