using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{
    public class FlightFeeDto
    {
        /// <summary>
        /// 名字(中文名字or 护照名字or 昵称)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 航班日期
        /// </summary>
        public DateTime f_FlightDateST { get; set; }
        public DateTime f_FlightDateED { get; set; }

        public int f_IsPay { get; set; }

        public bool isValid => (!string.IsNullOrWhiteSpace(Name) || f_FlightDateST != DateTime.MinValue || f_FlightDateED != DateTime.MinValue || f_IsPay != 0);
    }
}
