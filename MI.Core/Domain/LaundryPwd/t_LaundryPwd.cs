using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Domain
{
    /// <summary>
    /// 洗衣房类
    /// 创建人：吕秀峰
    /// 创建时间：2017-06
    /// </summary>
    public class t_LaundryPwd
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public int f_Id { get; set; }
        /// <summary>
        /// 社区（社区外键）
        /// </summary>
        public string f_Community { get; set; }
        /// <summary>
        /// 房间类型洗衣房/晾衣房
        /// </summary>
        public int f_RoomType { get; set; }
        /// <summary>
        /// 楼栋
        /// </summary>
        public string f_Building { get; set; }
        /// <summary>
        /// 房间号密码
        /// </summary>
        public string f_NoAndPwd { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string f_Remark { get; set; }
    }
}
