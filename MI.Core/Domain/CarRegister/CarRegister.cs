using System;

namespace MI.Core.Domain
{
    /// <summary>
    /// 车辆管理类
    /// 创建人：吕秀峰
    /// </summary>
    public class CarRegister
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        public int f_ID { get; set; }
        /// <summary>
        /// 车辆类型
        /// </summary>
        public int f_CarType { get; set; }
        /// <summary>
        /// 车辆品牌
        /// </summary>
        public string f_CarBrand { get; set; }
        /// <summary>
        /// 车主
        /// </summary>
        public string f_CarOwner { get; set; } 
        /// <summary>
        /// 购置日期
        /// </summary>
        public Nullable<System.DateTime> f_PurchaseDate { get; set; }
        /// <summary>
        /// 是否有政府发行的特殊牌照
        /// </summary>
        public bool f_IsIssued { get; set; }
        /// <summary>
        /// 特殊牌照到期
        /// </summary>
        public Nullable<System.DateTime> f_IssuedDate { get; set; }
        /// <summary>
        /// 车牌号码
        /// </summary>
        public string f_CarNO { get; set; }

        public virtual SType t_sType { get; set; }
    }
}
