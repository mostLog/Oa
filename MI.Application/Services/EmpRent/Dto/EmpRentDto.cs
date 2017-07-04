using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{
    public class EmpRentDto
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public int f_Id { get; set; }
        /// <summary>
        /// 宿舍Id
        /// </summary>
        public Nullable<int> f_DormitoryId { get; set; }
        /// <summary>
        /// 员工id
        /// </summary>
        public int f_eid { get; set; }
        /// <summary>
        /// 租金
        /// </summary>
        public decimal f_Rent { get; set; }
        /// <summary>
        /// 补贴
        /// </summary>
        public decimal f_Grant { get; set; }
        /// <summary>
        /// 缴费日期
        /// </summary>
        public Nullable<System.DateTime> f_PaymentDate { get; set; }
        /// <summary>
        /// 应缴费
        /// </summary>
        public decimal f_Amount { get; set; }
        /// <summary>
        /// 收款人
        /// </summary>
        public bool f_IsPayment { get; set; }
        /// <summary>
        /// 是否缴费
        /// </summary>
        public string f_Payee { get; set; }
        /// <summary>
        /// 生效日期
        /// </summary>
        public Nullable<System.DateTime> f_EffectiveDate { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string f_operator { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public Nullable<System.DateTime> f_operatorTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string f_Remark { get; set; }
        /// <summary>
        /// 收款地点ID
        /// </summary>
        public Nullable<int> f_AddressId { get; set; }

        /// <summary>
        /// 关联外键   宿舍Id(f_DormitoryId) 
        /// </summary>
        public virtual Dormitory Dormitory { get; set; }
        /// <summary>
        /// 关联外键   宿舍Id(f_eid)
        /// </summary>
        public virtual Employee t_employeeInfo { get; set; } 
    }
}
