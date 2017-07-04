using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Domain
{
    public class Notice
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public int f_Id { get; set; }
        /// <summary>
        /// 公告类型,0=一般公告、1=紧急通知
        /// </summary>
        public bool f_Type { get; set; }
        /// <summary>
        /// 公告内容
        /// </summary>
        public string f_Content { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime f_AddDate { get; set; }
        /// <summary>
        /// 添加人
        /// </summary>
        public string f_Registrant { get; set; }
        /// <summary>
        /// 有效开始时间
        /// </summary>
        public DateTime f_StartDate { get; set; }
        /// <summary>
        /// 有效结束时间
        /// </summary>
        public DateTime f_EndDate { get; set; }
        /// <summary>
        /// 是否停止
        /// </summary>
        public bool f_IsTop { get; set; }
    }
}
