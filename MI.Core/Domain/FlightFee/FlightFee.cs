using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Domain
{
    public class FlightFee
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public int f_ID { get; set; }
        /// <summary>
        /// 员工id 外键
        /// </summary>
        public int f_eid { get; set; }
        /// <summary>
        /// 机票差额
        /// </summary>
        public decimal f_Amount { get; set; }
        /// <summary>
        /// 单位（默认Peso）
        /// </summary>
        public string f_PUnit { get; set; }
        /// <summary>
        /// 航班日期
        /// </summary>
        public System.DateTime f_FlightDate { get; set; }
        /// <summary>
        /// 航班
        /// </summary>
        public string f_Flight { get; set; }
        /// <summary>
        /// 起止地点
        /// </summary>
        public string f_StartEndAdd { get; set; }
        /// <summary>
        /// 缴费状态
        /// </summary>
        public bool f_IsPay { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string f_operator { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public System.DateTime f_operatorTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string f_Remark { get; set; }
        /// <summary>
        /// 收款地点ID
        /// </summary>
        public Nullable<int> f_AddressId { get; set; }
        /// <summary>
        /// 员工信息
        /// </summary>
        public virtual Employee t_employeeInfo { get; set; }
    }
}
