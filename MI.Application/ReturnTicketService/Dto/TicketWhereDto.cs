using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{
    public class TicketWhereDto
    {
        /// <summary>
        /// 部门
        /// </summary>
        public int f_departmentId { get; set; }
        /// <summary>
        /// 国籍
        /// </summary>
        public string f_international { get; set; }
        /// <summary>
        /// 姓名/昵称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 回国日期
        /// </summary>
        public DateTime? f_ToDate { get; set; }
        /// <summary>
        /// 返菲日期
        /// </summary>
        public DateTime? f_FromDate { get; set; }
        /// <summary>
        /// 航班（回国和返菲）
        /// </summary>
        public int f_ToAirlineType_Id { get; set; }

        public int f_eId { get; set; }
        public bool isValid => f_departmentId > 0 || f_international != "0" &&f_international!=null|| !string.IsNullOrWhiteSpace(Name) || f_ToDate != null || f_FromDate != null || f_ToAirlineType_Id > 0 || f_eId > 0;
    }
}
