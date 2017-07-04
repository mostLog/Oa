using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto 
{
    public class GrantDto
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public int f_GrantId { get; set; }
        /// <summary>
        /// 补助金额
        /// </summary>
        public decimal f_amount { get; set; }
        /// <summary>
        /// 补助日期
        /// </summary>
        public System.DateTime f_GrantDate { get; set; }
        /// <summary>
        /// 员工id
        /// </summary>
        public int f_eid { get; set; }
        /// <summary>
        /// 是否已发放补助
        /// </summary>
        public bool f_IsPayment { get; set; }
        /// <summary>
        /// 发放人
        /// </summary>
        public string f_Payee { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string f_operator { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public Nullable<System.DateTime> f_operatorTime { get; set; }

        /// <summary>
        /// 关联外键   宿舍Id(f_eid)
        /// </summary>
        public virtual Employee t_employeeInfo { get; set; }
    }
}
