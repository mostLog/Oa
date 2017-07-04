using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Common
{
    /// <summary>
    /// 员工信息 视图模型
    /// </summary>
    public class AllEmployeeInfoViewModel
    {
      
        /// <summary>
        /// 员工信息
        /// </summary>
        public Employee EmployeeInfo { get; set; }
        /// <summary>
        /// 是否新人信息资料
        /// </summary>
        public bool IsnewEmployeeInfo { get; set; }
        /// <summary>
        /// 是否订餐
        /// </summary>
        public bool IsOrderingEmployees { get; set; }
        /// <summary>
        /// 订餐数组，中/晚/宵夜ID的数组
        /// </summary>
        public string[] LDM_tID { get; set; }
        /// <summary>
        /// 订餐日期（新人订餐）
        /// </summary>
        public string[] observationDate { get; set; }
        /// <summary>
        /// 代表对应订餐数组中的 数量
        /// 只计算>0的
        /// </summary>
        public int[] LDM_Quantity { get; set; }
        /// <summary>
        /// 订餐类型
        /// </summary>
        public int LDMType_tID { get; set; }
    }

}

