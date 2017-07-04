using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace MI.Core.Domain
{
    /// <summary>
    /// 住宿汇总的ViewModel
    /// 创建人：吕秀峰
    /// 创建时间：2017-06-26
    public class DormitorySummaryViewModel
    {
        private static readonly int iQuota = int.Parse(WebConfigurationManager.AppSettings["_Quota"] ?? "5000");
        /// <summary>
        /// ID序号
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string Department { get; set; }
        /// <summary>
        /// 社区
        /// </summary>
        public string Community { get; set; }
        /// <summary>
        /// 楼栋
        /// </summary>
        public string Building { get; set; }
        /// <summary>
        /// 房号
        /// </summary>
        public string RoomNo { get; set; }

        /// <summary>
        /// 住户 
        /// </summary>
        public Dictionary<int, string> Names => listEmployeeInfo.ToDictionary(e => e.f_eid, e => e.f_chineseName + "&" + e.f_IsNewEmp + "&" + e.f_eid.ToString());
        /// <summary>
        /// 房型 
        /// </summary>
        public string RoomType { get; set; }
        /// <summary>
        /// 住宿上限 最多住多少人
        /// </summary>
        public int? TotalOfPeople { get; set; }

        /// <summary>
        /// 现在已经住了多少人
        /// </summary>
        public int SumPeople { get; set; }
        /// <summary>
        /// 房租差额合计 
        /// </summary>
        public int Balance => TotalOfPeople == null ? 0 : ((TotalOfPeople ?? 0 - SumPeople) * iQuota);
        /// <summary>
        /// 备注 
        /// </summary>
        public string Remarks { get; set; }

        public ICollection<Employee> listEmployeeInfo { private get; set; }
    }
}
