using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{
    /// <summary>
    /// 汇率输入Dto
    /// </summary>
    public class RateInputDto
    {
        /// <summary>
        /// 美元
        /// </summary>
        public decimal Dollar { get; set; }
        /// <summary>
        /// RMB
        /// </summary>
        public decimal RMB { get; set; }
        /// <summary>
        /// 台币
        /// </summary>
        public decimal Taiwan { get; set; }

    }
}
