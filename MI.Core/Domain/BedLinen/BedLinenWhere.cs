using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Domain
{
    /// <summary>
    /// 床单送洗按照条件查询
    /// mu
    /// 2016.7.5
    /// </summary>
    public class BedLinenWhere
    {
        /// <summary>
        /// 开始登记时间
        /// </summary>
        public System.DateTime? RegistrationStarDate { get; set; }
        /// <summary>
        /// 结束登记时间
        /// </summary>
        public System.DateTime? RegistrationEndDate { get; set; }
        /// <summary>
        /// 登记人
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 社区
        /// </summary>
        public string Community { get; set; }
        /// <summary>
        /// 楼栋
        /// </summary>
        public string Building { get; set; }
        /// <summary>
        /// 房间号
        /// </summary>
        public string RoomNo { get; set; }

        public bool IsValid => RegistrationStarDate != null ||
            RegistrationEndDate != null ||
            !string.IsNullOrWhiteSpace(Name) ||
                                !string.IsNullOrWhiteSpace(Community) ||
            !string.IsNullOrWhiteSpace(Building) ||
                                !string.IsNullOrWhiteSpace(RoomNo);
    }
}
