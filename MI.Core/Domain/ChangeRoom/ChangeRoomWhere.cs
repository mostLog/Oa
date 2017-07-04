using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Domain
{
    /// <summary>
    /// 员工换房查询条件
    /// SHAWN 2016.6.28
    /// </summary>
    public class ChangeRoomWhere
    {
        /// <summary>
        /// 名字(中文名字or 护照名字or 昵称)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 处理进度
        /// </summary>
        public string Progress { get; set; }
        /// <summary>
        /// 承租日期
        /// </summary>
        public DateTime f_RentDateST { get; set; }
        public DateTime f_RentDateED { get; set; }

        public bool isValid => (!string.IsNullOrWhiteSpace(Name) || (!string.IsNullOrWhiteSpace(Progress) && !"全部".Equals(Progress)) || f_RentDateST != DateTime.MinValue || f_RentDateED != DateTime.MinValue);
    }
}
