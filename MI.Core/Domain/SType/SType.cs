using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Domain
{
    public class SType
    {
        public SType()
        {
            this.EmployeePeriodType = new HashSet<Employee>();
            this.EmployeeDepartment = new HashSet<Employee>();
            this.EmployeeWorkStatus = new HashSet<Employee>();
            this.EmployeeWorkLocation = new HashSet<Employee>();
            this.EmployeeLevel = new HashSet<Employee>();
        }
        /// <summary>
        /// 类型ID
        /// </summary>
        public int f_tID { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string f_value { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string f_Remark { get; set; }
        /// <summary>
        /// 类型(权限类型 = 0,班次类型 = 1,部门类型 = 2,上班状态类型 = 3,上班地点类型 = 4,订餐类型 = 5,订餐要求类型 = 6,公司用餐统计部门班次类型 = 20,公司用餐时间类型 = 21,
        /// 房间类型 = 22,车辆类型 = 23,工作类别 = 24,国籍 = 25,航班类型 = 26,社区类型 = 27,楼栋类型 = 28,签证类型 = 29,证件类型 = 30,办理进度 = 31,床位类型 = 32,
        /// 银行储值类型 = 33,考勤能见部门 = 34,月均儲值兌獎 = 35,金流客服等级类型 = 36,金流客服权限控制 = 37,组别类型 = 39,汇率类型 = 40,
        /// 上车地点配置 = 51,宿舍wifi配置 = 52,行政部联系方式配置 = 53,宿舍订餐领便当处 = 54,宿舍饮水订购方式 = 55,宿舍瓦斯订购方式 = 56,宿舍维修对接 = 57)
        /// </summary>
        public byte f_type { get; set; }

        public virtual ICollection<Employee> EmployeePeriodType { get; set; }
        public virtual ICollection<Employee> EmployeeDepartment { get; set; }
        public virtual ICollection<Employee> EmployeeWorkStatus { get; set; }
        public virtual ICollection<Employee> EmployeeWorkLocation { get; set; }
        public virtual ICollection<Employee> EmployeeLevel { get; set; }

        public virtual ICollection<CarRegister> EmployeeCarType { get; set; }

        public virtual ICollection<Employee> t_employeeInfo { get; set; }
        /// <summary>
        /// 公司用餐统计表
        /// </summary>
        public virtual ICollection<CompanyOfFood> CompanyOfFoods { get; set; }
        public virtual ICollection<t_Outside> t_Outside { get; set; }
    }
}
