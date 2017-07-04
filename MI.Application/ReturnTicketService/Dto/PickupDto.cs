using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{
    /// <summary>
    /// 接机派车查询条件
    /// </summary>
    public class PickupDto
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; } 
        /// <summary>
        /// 国籍
        /// </summary>
        public string National { get; set; } 
        /// <summary>
        /// 返菲日期
        /// </summary>
        public DateTime FromDateST { get; set; }
        /// <summary>
        /// 返菲日期
        /// </summary>
        public DateTime FromDateED { get; set; }
        /// <summary>
        /// 是否派车
        /// </summary>
        public string FromIsSendACar { get; set; }
    }
}
