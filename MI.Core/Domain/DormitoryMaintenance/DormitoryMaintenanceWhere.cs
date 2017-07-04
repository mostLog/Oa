using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Domain
{
    /// <summary>
    /// 宿舍维修查询条件
    /// 吕秀峰 2017.6.22
    /// </summary>
    public class DormitoryMaintenanceWhere
    {
        /// <summary>
        /// 社区
        /// </summary>
        public string f_Community { get; set; }
        /// <summary>
        /// 楼栋
        /// </summary>
        public string f_Building { get; set; }
        /// <summary>
        /// 房间号
        /// </summary>
        public string f_RoomNo { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string f_operator { get; set; }
        /// <summary>
        /// 维修状态
        /// </summary>
        public bool? f_Fitness { get; set; }
        /// <summary>
        /// 这个条件是否有效
        /// </summary>
        public int? f_serviceWay { get; set; }
        public bool isValid => (!string.IsNullOrWhiteSpace(f_Community) || !string.IsNullOrWhiteSpace(f_Building) || !string.IsNullOrWhiteSpace(f_RoomNo) || !string.IsNullOrWhiteSpace(f_operator) || f_Fitness != null) || f_serviceWay != null;
    }
    /// <summary>
    /// 维修方式
    /// </summary>
    public enum serviceWay
    {
        完成 = 0,
        送修 = 1,
        买新 = 2,
    }
}
