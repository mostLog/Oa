using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{
    public class CashRegisterPagedInputDto:PagedInputDto
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StarDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 经手人 昵称/中文名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 据点ID
        /// </summary>
        public int TypeID { get; set; }
    }
}
