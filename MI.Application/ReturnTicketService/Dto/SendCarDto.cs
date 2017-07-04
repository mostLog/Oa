using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{
    public class SendCarDto
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 国籍
        /// </summary>
        public string National { get; set; }  
        /// <summary>
        /// 返乡开始日期
        /// </summary>
        public DateTime DateStart { get; set; }   
        /// <summary>
        /// 返乡结束日期
        /// </summary>
        public DateTime DateEnd { get; set; }  
        /// <summary>
        /// 是否派车
        /// </summary>
        public string IsSendACar { get; set; }

    }
}
