using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Domain
{
    //宿舍格子表
    public class t_LatticeContent
    { 
        /// <summary>
       /// ID
       /// </summary>
        public int f_LId { get; set; }
        /// <summary>
        /// 宿舍ID
        /// </summary>
        public Nullable<int> f_tID { get; set; }
        /// <summary>
        /// 楼层
        /// </summary>
        public Nullable<int> f_floor { get; set; }
        /// <summary>
        /// 房间
        /// </summary>
        public Nullable<int> f_room { get; set; }
        /// <summary>
        /// 宿舍表id
        /// </summary>
        public Nullable<int> f_dormitoryId { get; set; }
        /// <summary>
        /// 是否解锁
        /// </summary>
        public Nullable<bool> f_isUnlock { get; set; }
        /// <summary>
        /// 社区ID
        /// </summary>
        public Nullable<int> f_tID2 { get; set; }
    }
}
