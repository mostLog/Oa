using MI.Core.Domain;
using System;
using System.Collections.Generic;

namespace MI.Application
{
    public interface IOrderingEmployeesService
    {
        /// <summary>
        /// 更新新人订餐信息(宿舍)
        /// </summary>
        /// <param name="listModel"></param>
        void UpdateOrderingEmployees(List<OrderingEmployees> listModel);
        /// <summary>
        /// 删除所有订餐数据 根据员工ID
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
