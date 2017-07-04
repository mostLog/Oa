using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{
    /// <summary>
    /// 证件办理条件查询
    /// </summary>
    public class CertAgencyWhereDto 
    {
        /// <summary>
        /// 部门
        /// </summary>
        public int? FDepartmentId { get; set; }

        /// <summary>
        /// 姓名/昵称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 工作地点
        /// </summary>
        public int? WorkLocationId { get; set; }
        
    }
}
