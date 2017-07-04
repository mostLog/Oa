using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Domain
{
    /// <summary>
    /// 宿舍打扫查询条件
    /// SHAWN 2016.6.28
    /// </summary>
    public class HostelCleanWhere
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
        /// 星期
        /// </summary>
        public int f_Week { get; set; }

        /// <summary>
        /// 这个条件是否有效
        /// </summary>
        public bool isValid => (!string.IsNullOrWhiteSpace(f_Community) || !string.IsNullOrWhiteSpace(f_Building) || !string.IsNullOrWhiteSpace(f_RoomNo) || f_Week != 0);
    }
}
