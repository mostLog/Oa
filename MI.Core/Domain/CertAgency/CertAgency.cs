using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Domain
{
    public class CertAgency
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public int f_Id { get; set; }
        /// <summary>
        /// 员工id  外键
        /// </summary>
        public int? f_eId { get; set; }
        /// <summary>
        /// 证件类型ID
        /// </summary>
        public int? f_CertificateTypeId { get; set; }
        /// <summary>
        /// 办理进度ID
        /// </summary>
        public int? f_ProgressId { get; set; }
        /// <summary>
        /// 处理人
        /// </summary>
        public string f_Operator { get; set; }
        /// <summary>
        /// 最后一次操作时间
        /// </summary>
        public DateTime? f_OperatorTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string f_Remark { get; set; }
        /// <summary>
        /// 文件资料名称
        /// </summary>
        public string f_FileName { get; set; }
        /// <summary>
        /// 资料下载是否提示  false不提示
        /// </summary>
        public bool f_DownTips { get; set; }
    }
}
