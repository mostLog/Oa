using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Domain
{
    public class CompanyOfFood
    {
        public int f_cID { get; set; }
        /// <summary>
        /// 早晨
        /// </summary>
        public int f_breakfast { get; set; }
        /// <summary>
        /// 中餐
        /// </summary>
        public int f_lunch { get; set; }
        /// <summary>
        /// 晚餐
        /// </summary>
        public int f_dinner { get; set; }
        /// <summary>
        /// 宵夜
        /// </summary>
        public int f_Midnight { get; set; }
        /// <summary>
        /// 字典表id
        /// </summary>
        public int f_type_tID { get; set; }
        /// <summary>
        /// 字典表
        /// </summary>

        public virtual SType SType { get; set; }
    }
}
