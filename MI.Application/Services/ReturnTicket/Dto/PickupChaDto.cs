using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{
    public class PickupChaDto
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 国籍
        /// </summary>
        public string International { get; set; }
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
        /// <summary>
        /// 是否查询
        /// </summary>
        public bool isValid => (!string.IsNullOrWhiteSpace(Name) || (!string.IsNullOrWhiteSpace(International) && International != "0") || (!string.IsNullOrWhiteSpace(FromIsSendACar) && FromIsSendACar != "2") || FromDateST != DateTime.MinValue || FromDateED != DateTime.MinValue);
    }
}
