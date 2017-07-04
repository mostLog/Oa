using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{
    public class CertAgencyDto
    {
        /// <summary>
        /// id
        /// </summary>
        public int f_Id { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string Sector { get; set; }
        /// <summary>
        /// 员工名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 员工id
        /// </summary>
        public int Eid { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 工作地点
        /// </summary>
        public string WorkLocation { get; set; }
        /// <summary>
        /// 办理进度
        /// </summary>
        public string f_Progress { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public string f_CertificateType { get; set; }
        /// <summary>
        /// 处理人
        /// </summary>
        public string f_Operator { get; set; }
        /// <summary>
        /// 最后操作时间
        /// </summary>
        public Nullable<System.DateTime> f_OperatorTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string f_Remark { get; set; }
        /// <summary>
        /// 文件资料名称
        /// </summary>
        public string f_FileName { get; set; }
        /// <summary>
        /// 资料下载是否提示
        /// </summary>
        public bool f_DownTips { get; set; }
    }
}
