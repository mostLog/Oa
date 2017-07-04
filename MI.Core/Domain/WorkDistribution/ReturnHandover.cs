using System;

namespace MI.Core.Domain
{
    public class ReturnHandover
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        public int f_Id { get; set; }
        /// <summary>
        /// 员工ID 外键
        /// </summary>
        public int f_eid { get; set; }
        /// <summary>
        /// 返乡起始时间
        /// </summary>
        public DateTime? f_StartDate { get; set; }
        /// <summary>
        /// 返乡结束时间
        /// </summary>
        public DateTime? f_EndDate { get; set; }
        /// <summary>
        /// 工作事项序号
        /// </summary>
        public int? f_SerialNO { get; set; }
        /// <summary>
        /// 工作任务
        /// </summary>
        public string f_WorkItem { get; set; }
        /// <summary>
        /// 当前移交进度
        /// </summary>
        public string f_CurrentProgress { get; set; }
        /// <summary>
        /// 代理人
        /// </summary>
        public string f_Agent { get; set; }
        /// <summary>
        /// 代理人处理进度及结果
        /// </summary>
        public string f_AgentProcess { get; set; }
        /// <summary>
        /// 跨行数
        /// </summary>
        public int? f_Rowspan { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime? f_operatorTime { get; set; }
        /// <summary>
        /// 代理人id
        /// </summary>
        public int? f_AgentEid { get; set; }
        /// <summary>
        /// 人事信息
        /// </summary>
        public virtual Employee t_employeeInfo { get; set; }
    }
}
