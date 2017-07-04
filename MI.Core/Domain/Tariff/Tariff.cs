using System;

namespace MI.Core.Domain
{
    public partial class Tariff
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public int f_Id { get; set; }
        /// <summary>
        /// 宿舍id    外键
        /// </summary>
        public int f_DormitoryId { get; set; }
        /// <summary>
        /// 电费标准
        /// </summary>
        public decimal f_TariffStandard { get; set; }
        /// <summary>
        /// 实际账单
        /// </summary>
        public decimal f_RealBill { get; set; }
        /// <summary>
        /// 超支金额
        /// </summary>
        public decimal f_Overruns { get; set; }
        /// <summary>
        /// 超支日期
        /// </summary>
        public DateTime f_TariffDate { get; set; }
        /// <summary>
        /// 登记人
        /// </summary>
        public string f_Registrant { get; set; }
        /// <summary>
        /// 是否缴费（0=否，1=是）
        /// </summary>
        public bool f_IsPayment { get; set; }
        /// <summary>
        /// 收费人
        /// </summary>
        public string f_Rate { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string f_operator { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime? f_operatorTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string f_Remark { get; set; }
        /// <summary>
        /// 收款地点ID
        /// </summary>
        public int? f_AddressId { get; set; }
        /// <summary>
        /// 宿舍登记
        /// </summary>
        public virtual Dormitory Dormitory { get; set; }
    }
}
