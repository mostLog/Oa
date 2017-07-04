using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{
    public class EmpRentListDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 员工名称
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; }
        /// <summary>
        /// 宿舍名称
        /// </summary>
        public string DormitoryName { get; set; }
        /// <summary>
        /// 租金
        /// </summary>
        public decimal Rent { get; set; }
        /// <summary>
        /// 补贴
        /// </summary>
        public decimal Grant { get; set; }
        /// <summary>
        /// 应缴费
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 缴费日期
        /// </summary>
        public DateTime? PaymentDate { get; set; }
        /// <summary>
        /// 是否收款
        /// </summary>
        public bool IsPayment { get; set; }
        /// <summary>
        /// 收款人
        /// </summary>
        public string Payee { get; set; }
        /// <summary>
        /// 收款地点
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 收款日期
        /// </summary>
        public DateTime? EffectiveDate { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime? OperatorTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
