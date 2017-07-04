using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Domain
{
    /// <summary>
    /// 夫妻关系登记类
    /// 创建人：吕秀峰
    /// 创建时间：2017-06-27
    /// </summary>
    public partial class t_CoupleRegister
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int f_cId { get; set; }
        /// <summary>
        /// 员工1的id
        /// </summary>
        public int f_eId1 { get; set; }
        /// <summary>
        /// 员工2的id
        /// </summary>
        public int f_eId2 { get; set; }
    }
}
