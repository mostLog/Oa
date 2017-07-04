using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{
    public class ReturnHandoverDto
    {
        /// <summary>
        /// 名字(中文名字or 护照名字or 昵称)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 返乡日期
        /// </summary>
        public DateTime f_ReturnDateST { get; set; }
        public DateTime f_ReturnDateED { get; set; }
        /// <summary>
        /// 代理人
        /// </summary>
        public string f_Agent { get; set; }

        public bool isValid => (!string.IsNullOrWhiteSpace(Name) || f_ReturnDateST != DateTime.MinValue || f_ReturnDateED != DateTime.MinValue);
    }
}
