using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{
    public class DorOrderListDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否为新人
        /// </summary>
        public bool IsNewEmployee { get; set; }
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
        /// <summary>
        /// 订餐信息
        /// </summary>
        public IList<string> Ordings { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid { get; set; }
    }
}
