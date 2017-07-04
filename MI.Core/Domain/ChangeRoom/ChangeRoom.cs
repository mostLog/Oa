using System;

namespace MI.Core.Domain
{
    /// <summary>
    /// 员工换房类
    /// 填写人：吕秀峰
    /// </summary>
    public partial class ChangeRoom
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public int f_Id { get; set; }
        /// <summary>
        /// 员工id(人事信息表外键)
        /// </summary>
        public int f_eid { get; set; }
        /// <summary>
        /// 申请日期
        /// </summary>
        public System.DateTime f_FilingDate { get; set; }
        /// <summary>
        /// 原房间id
        /// </summary>
        public Nullable<int> f_OldRoomID { get; set; }
        /// <summary>
        /// 新房间id
        /// </summary>
        public Nullable<int> f_NewRoomId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string f_Remarks { get; set; }
        /// <summary>
        /// 处理进度
        /// </summary>
        public string f_Progress { get; set; }
        /// <summary>
        /// 登记人
        /// </summary>
        public string f_Registrant { get; set; }
        /// <summary>
        /// 补房租
        /// </summary>
        public decimal f_SewRent { get; set; }
        /// <summary>
        /// 承租日期
        /// </summary>
        public System.DateTime f_RentDate { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string f_operator { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public Nullable<System.DateTime> f_operatorTime { get; set; }
        /// <summary>
        /// 生效月份
        /// </summary>
        public Nullable<System.DateTime> f_EffectiveMonths { get; set; }
        public virtual Employee t_employeeInfo { get; set; }
        public virtual Dormitory Dormitory { get; set; }
        public virtual Dormitory Dormitory1 { get; set; }

    }
}
