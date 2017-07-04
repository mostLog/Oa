using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{
    public class TariffDto
    {
        /// <summary>
        /// 社区
        /// </summary>
        public string f_Community { get; set; }
        /// <summary>
        /// 楼栋
        /// </summary>
        public string f_Building { get; set; }
        /// <summary>
        /// 房间号
        /// </summary>
        public string f_RoomNo { get; set; }
        /// <summary>
        /// 超支日期
        /// </summary>
        public DateTime f_TariffDateST { get; set; }
        public DateTime f_TariffDateED { get; set; }
        /// <summary>
        /// 是否缴费
        /// </summary>
        public string IsPayment { get; set; }
        /// <summary>
        /// 这个条件是否有效
        /// </summary>
        public bool IsValid => (!string.IsNullOrWhiteSpace(f_Community) || !string.IsNullOrWhiteSpace(f_Building) || !string.IsNullOrWhiteSpace(f_RoomNo)
                                || f_TariffDateST != DateTime.MinValue || f_TariffDateED != DateTime.MinValue || (!string.IsNullOrWhiteSpace(IsPayment) && IsPayment != "2"));
    }
}
