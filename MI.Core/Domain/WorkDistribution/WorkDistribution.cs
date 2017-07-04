using System;
using System.Data.Entity;

namespace MI.Core.Domain
{
    public class WorkDistribution
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        public int f_Id { get; set; }
        /// <summary>
        /// 登记时间
        /// </summary>
        public System.DateTime f_RegisterDate { get; set; }
        /// <summary>
        /// 登记人
        /// </summary>
        public string f_Registered { get; set; }
        /// <summary>
        /// 工作类别
        /// </summary>
        public int f_WorkType { get; set; }
        /// <summary>
        /// 交接事项
        /// </summary>
        public string f_Handover { get; set; }
        /// <summary>
        /// 待处理人
        /// 外键
        /// </summary>
        public Nullable<int> f_Treat { get; set; }
        /// <summary>
        /// 紧急事项处理期限
        /// </summary>
        public Nullable<System.DateTime> f_UrgentDate { get; set; }
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool f_IsComplete { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public Nullable<System.DateTime> f_CompleteDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string f_Remarks { get; set; }
        /// <summary>
        /// 人事信息表
        /// </summary>
        public virtual Employee t_employeeInfo { get; set; }

    }
}
