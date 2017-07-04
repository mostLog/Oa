using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{
    public class FlightFeeListDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 员工名称
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// 机票差额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 航班日期
        /// </summary>
        public DateTime? FlightDate { get; set; }
        /// <summary>
        /// 航班
        /// </summary>
        public string Flight { get; set; }
        /// <summary>
        /// 起止地点
        /// </summary>
        public string StartEndAdd { get; set; }
        /// <summary>
        /// 缴费状态
        /// </summary>
        public bool IsPay { get; set; }
        /// <summary>
        /// 收款地点
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator{get;set;}
        /// <summary>
        /// 操作日期
        /// </summary>
        public DateTime OperatorTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
