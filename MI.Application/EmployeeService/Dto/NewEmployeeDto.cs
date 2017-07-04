using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{
    /// <summary>
    /// 新员工条件查询
    /// </summary>
    public class NewEmployeeDto
    {
        /// <summary>
        /// 班次
        /// </summary>
        public int? periodType_f_tid { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public int? department_f_tid { get; set; }
        /// <summary>
        ///权限等级
        /// </summary>
        public int? level_f_tid { get; set; }
        /// <summary>
        /// 搭车时间
        /// </summary>
        public DateTime rideWorkTime { get; set; }
        /// <summary>
        /// 上班状态
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
        /// 抵达日期
        /// </summary>
        public DateTime arrivalDate { get; set; }
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
        /// 这个条件是否有效
        /// </summary>
        public bool isValid => (periodType_f_tid != null && periodType_f_tid > 0) || (department_f_tid != null && department_f_tid > 0) || (level_f_tid != null && level_f_tid > 0) || (rideWorkTime != null) || !string.IsNullOrWhiteSpace(Name) || !string.IsNullOrWhiteSpace(TelNoPH) || (workStatus_f_tid != null && (workStatus_f_tid == -2 || workStatus_f_tid > 0)) || (StartDate != DateTime.MinValue || EndDate != DateTime.MinValue) || (arrivalDate != DateTime.MinValue) || !string.IsNullOrWhiteSpace(BuildingOrRoomNo) || iEid != 0;
    }
}
