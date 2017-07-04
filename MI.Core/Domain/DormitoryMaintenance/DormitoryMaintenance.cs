using MI.Core.Domain;
using System;
using System.Data.Entity;

namespace MI.Core.Domain
{
    /// <summary>
    /// 宿舍报修记录表
    /// 创建人：吕秀峰
    /// 创建时间：2017-06-27
    /// </summary>
    public partial class DormitoryMaintenance: DbContext
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public int f_Id { get; set; }
        /// <summary>
        /// 宿舍id  外键
        /// </summary>
        public int f_DormitoryId { get; set; }
        /// <summary>
        /// 登记人
        /// </summary>
        public string f_Registrant { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string f_QQNo { get; set; }
        /// <summary>
        /// 维修状态（0=已报修，已完成）
        /// </summary>
        public bool f_Fitness { get; set; }
        /// <summary>
        /// 维修内容（中）
        /// </summary>
        public string f_ContentCh { get; set; }
        /// <summary>
        /// 维修内容（英）
        /// </summary>
        public string f_ContentEn { get; set; }
        /// <summary>
        /// 对接
        /// </summary>
        public string f_Butt { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string f_Remarks { get; set; }
        /// <summary>
        /// 维修费用
        /// </summary>
        public decimal? f_MaintFee { get; set; }
        /// <summary>
        /// 确认（员工）
        /// </summary>
        public string f_IsOK_emp { get; set; }
        /// <summary>
        /// 确认（维修工）
        /// </summary>
        public string f_IsOK_main { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string f_operator { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime? f_operatorTime { get; set; }
        /// <summary>
        /// 缴费状态（1已经缴费，0未缴费）
        /// </summary>
        public bool? f_PaymentStatus { get; set; }
        /// <summary>
        /// 缴费人
        /// </summary>
        public string f_Payers { get; set; }
        /// <summary>
        /// 派件日期
        /// </summary>
        public DateTime? f_SendDate { get; set; }
        /// <summary>
        /// 最后完成日期
        /// </summary>
        public DateTime? f_LastFinishDate { get; set; }
        /// <summary>
        /// 维修方式
        /// </summary>
        public int? f_serviceWay { get; set; }
        /// <summary>
        /// 宿舍登记
        /// </summary>
        public virtual t_Dormitory t_dormitory { get; set; }
    }
}
