using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{ 
    public class EmployeeDto
    {
        public int f_eid { get; set; }
        /// <summary>
        /// 班次
        /// </summary>
        public int? periodType_f_tid { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public int? f_department_tID { get; set; }
        /// <summary>
        ///权限等级
        /// </summary>
        public int? level_f_tid { get; set; }
        
        /// <summary>
        /// 在职状态
        /// </summary>
        public int? workStatus_f_tid { get; set; }
        /// <summary>
        /// 名字(中文名字or 护照名字or 昵称)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 菲律宾电话
        /// </summary>
        public string TelNoPH { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 楼栋
        /// </summary>
        public string BuildingOrRoomNo { get; set; }
        /// <summary>
        /// 员工ID
        /// </summary>
        public int iEid { get; set; }
        /// <summary>
        /// 要打印的员工id
        /// </summary>
        public string feid { get; set; }
        /// <summary>
        /// 抵达日期
        /// </summary>
        public DateTime arrivalDate { get; set; }
        /// <summary>
        /// 报道日期
        /// </summary>
        public DateTime rideWorkTime { get; set; }
        /// <summary>
        /// 条件是否有效
        /// </summary>
        public bool isValid => (periodType_f_tid != null && periodType_f_tid > 0) || (f_department_tID != null && f_department_tID > 0) || (level_f_tid != null && level_f_tid > 0)|| !string.IsNullOrWhiteSpace(Name) || !string.IsNullOrWhiteSpace(TelNoPH) || (workStatus_f_tid != null && (workStatus_f_tid == -2 || workStatus_f_tid > 0)) || (StartDate.ToString() != "0001/1/1 0:00:00" || EndDate.ToString() != "0001/1/1 0:00:00");
        /// <summary>
        /// 新人查询条件是否有效
        /// </summary>
        public bool isValidNewemployee => (periodType_f_tid != null && periodType_f_tid > 0) || (f_department_tID != null && f_department_tID > 0) || (level_f_tid != null && level_f_tid > 0) || (rideWorkTime != null) || !string.IsNullOrWhiteSpace(Name) || !string.IsNullOrWhiteSpace(TelNoPH) || (workStatus_f_tid != null && (workStatus_f_tid == -2 || workStatus_f_tid > 0)) || (StartDate != DateTime.MinValue || EndDate != DateTime.MinValue) || (arrivalDate != DateTime.MinValue) || !string.IsNullOrWhiteSpace(BuildingOrRoomNo) || iEid != 0;
    }
}
