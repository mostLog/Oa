using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{
    public class GrantPagedInputDto: PagedInputDto
    {
        /// <summary>
        /// 员工
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 辅助开始时间
        /// </summary>
        public DateTime? GrantStartDate { get; set; }
        /// <summary>
        /// 辅助结束时间
        /// </summary>
        public DateTime? GrantEndDate { get; set; }
        /// <summary>
        /// 是否发放补助
        /// </summary>
        public int IsPayment { get; set; } = 2;
    }
}
