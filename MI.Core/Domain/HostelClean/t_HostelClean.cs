using System;

namespace MI.Core.Domain
{
    /// <summary>
    /// 宿舍打扫类
    /// 创建人：吕秀峰
    /// </summary>
    public partial class t_HostelClean
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public int f_Id { get; set; }
        /// <summary>
        /// 宿舍Id  外键
        /// </summary>
        public int f_DormitoryId { get; set; }
        /// <summary>
        /// 星期（1=星期一，2=星期二，依此）
        /// </summary>
        public int f_Week { get; set; }
        /// <summary>
        /// 执行人
        /// </summary>
        public string f_Cleaner { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string f_operator { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime? f_operatorTime { get; set; }
        /// <summary>
        /// 宿舍登记
        /// </summary>
        public virtual t_Dormitory t_dormitory { get; set; }
    }
}
