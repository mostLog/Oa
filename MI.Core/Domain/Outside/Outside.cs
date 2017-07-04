using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Domain
{
    /// <summary>
    /// 员工外租类
    /// 创建人：吕秀峰
    /// 创建时间：2017-06-23
    /// </summary>
    public partial class Outside
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public int f_Id { get; set; }
        /// <summary>
        /// 部门id
        /// </summary>
        public int f_DeptId { get; set; }
        /// <summary>
        /// 员工id
        /// </summary>
        public int f_eid { get; set; }
        /// <summary>
        /// 申请日期
        /// </summary>
        public System.DateTime f_FilingDate { get; set; }
        /// <summary>
        /// 批准干部id
        /// </summary>
        public int f_LeadId { get; set; }
        /// <summary>
        /// 原宿舍id
        /// </summary>
        public Nullable<int> f_DormitoryId { get; set; }
        /// <summary>
        /// 社区大楼名
        /// </summary>
        public string f_CommunityName { get; set; }
        /// <summary>
        /// 外宿地址
        /// </summary>
        public string f_OutsideAddress { get; set; }
        /// <summary>
        ///房号
        /// </summary>
        public string f_RoomNo { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        public string f_Reason { get; set; }
        /// <summary>
        /// 处理进度
        /// </summary>
        public string f_Progress { get; set; }
        /// <summary>
        /// 登记人
        /// </summary>
        public string f_Registrant { get; set; }
        /// <summary>
        /// 房东联系方式
        /// </summary>
        public string f_LandladyTel { get; set; }
        /// <summary>
        ///操作人
        /// </summary>
        public string f_operator { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public Nullable<System.DateTime> f_operatorTime { get; set; }

        public virtual Dormitory Dormitory { get; set; }
        public virtual SType t_sType { get; set; }
        public virtual Employee t_employeeInfo { get; set; }
        public virtual Employee t_employeeInfo1 { get; set; }
    }
}
