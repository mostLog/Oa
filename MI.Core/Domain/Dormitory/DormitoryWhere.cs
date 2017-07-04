using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Domain
{
    /// <summary>
    /// 宿舍登记查询条件
    /// SHAWN 2016.6.28
    /// </summary>
    public class DormitoryWhere
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
        /// 签约日期
        /// </summary>
        public DateTime f_ContractDateST { get; set; }
        public DateTime f_ContractDateED { get; set; }
        /// <summary>
        /// 是否有停车位
        /// </summary>
        public string IsBerth { get; set; }

        /// <summary>
        /// 这个条件是否有效
        /// </summary>
        public bool IsValid => (!string.IsNullOrWhiteSpace(f_Community) || !string.IsNullOrWhiteSpace(f_Building) || !string.IsNullOrWhiteSpace(f_RoomNo) || f_ContractDateST != DateTime.MinValue || f_ContractDateED != DateTime.MinValue || (!string.IsNullOrWhiteSpace(IsBerth) && IsBerth != "2"));
    }
}
