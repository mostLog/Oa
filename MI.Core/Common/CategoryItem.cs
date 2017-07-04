using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Common
{
    /// <summary>
    /// 操作记录操作项目的选项
    /// </summary>
    public enum CategoryItem : int
    {
        /// <summary>
        /// 车辆管理
        /// </summary>
        CarRegister = 1,
        /// <summary>
        /// 宿舍管理
        /// </summary>
        Dormitory = 2,
        /// <summary>
        /// 餐饮管理
        /// </summary>
        CompanyOfFood = 3,
        /// <summary>
        /// 工作交接
        /// </summary>
        WorkDistribution = 4,
        /// <summary>
        /// 员工管理
        /// </summary>
        EmployeeInfo = 5,
        /// <summary>
        /// 个人资料
        /// </summary>
        OrderingAndPersonalInfo = 6,
        /// <summary>
        /// 公告管理
        /// </summary>
        Notice = 7,
        /// <summary>
        /// 类别管理
        /// </summary>
        Category = 8,
        /// <summary>
        /// 新人登记
        /// </summary>
        NewEmployeeInfo = 9,
        /// <summary>
        /// 机票登记
        /// </summary>
        ReturnTicket = 10,
        /// <summary>
        /// 现金登记
        /// </summary>
        CashRegister = 11,
        /// <summary>
        /// 派车安排
        /// </summary>
        SendACar = 12,
        /// <summary>
        /// 费用管理
        /// </summary>
        MoneyManage = 13,

    }
    /// <summary>
    /// 操作记录操作类型选项
    /// </summary>
    public enum ActionItem : int
    {
        /// <summary>
        /// 新增
        /// </summary>
        Add = 1,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 2,
        /// <summary>
        /// 修改
        /// </summary>
        Update = 3,
    }
}
