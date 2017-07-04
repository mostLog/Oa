using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{
    public class FlightFeePagedInputDto:PagedInputDto
    {
        public string Name { get; set; }
        /// <summary>
        /// 航班日期
        /// </summary>
        public DateTime? FlightStartDate { get; set; }
        public DateTime? FlightEndDate { get; set; }
        /// <summary>
        /// 缴费状态
        /// </summary>
        public int IsPay { get; set; } = 2;

    }
}
