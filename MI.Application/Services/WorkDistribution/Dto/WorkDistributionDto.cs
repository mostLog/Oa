using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{
    public class WorkDistributionDto
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
        public bool? f_IsComplete { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public Nullable<System.DateTime> f_CompleteDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string f_Remarks { get; set; }
        /// <summary>
        /// 人事信息
        /// </summary>
        public virtual Employee t_employeeInfo { get; set; }
        /// <summary>
        /// 权限类别
        /// </summary>
        public virtual SType sType { get; set; }
        /// <summary>
        /// 这个条件是否有效
        /// </summary>
        public bool isValid
        {
            get
            {
                return (f_Treat != null || f_IsComplete != null || f_WorkType != 0 || (!string.IsNullOrWhiteSpace(f_Registered)) || f_Id != 0 || (!string.IsNullOrWhiteSpace(f_Handover)));
            }
        }
    }
}
