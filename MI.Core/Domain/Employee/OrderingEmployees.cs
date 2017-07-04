using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Domain
{
    /// <summary>
    /// 员工订餐表
    /// </summary>
    public class OrderingEmployees
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int f_efID { get; set; }
        /// <summary>
        /// 外键
        /// </summary>
        public Nullable<int> f_eID { get; set; }
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
        /// 中/晚/宵   
        /// </summary>
        public Nullable<int> f_LDM_tID { get; set; }
        /// <summary>
        /// 餐类型
        /// </summary>
        public Nullable<int> f_type_tID { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool f_IsValid { get; set; }
        /// <summary>
        /// 分量
        /// </summary>
        public int f_Quantity { get; set; }
        /// <summary>
        /// 有效日期字段
        /// </summary>
        public Nullable<System.DateTime> f_EffectiveDate { get; set; }
        /// <summary>
        /// 关联员工表
        /// </summary>
        public virtual Employee t_Employee { get; set; }
    }
}
