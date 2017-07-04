using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{
    public class DorOrderPagedInputDto:PagedInputDto
    {
        public DorOrderPagedInputDto()
        {
            base.iPageSize = 30;
        }
        /// <summary>
        /// 中文名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 社区/楼栋/房间号
        /// </summary>
        public string Dormitory { get; set; }
    }
}
