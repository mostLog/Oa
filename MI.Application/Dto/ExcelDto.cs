using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{
    public class ExcelDto
    {
        public ExcelDto(string fileName)
        {
            FileName = fileName;
        }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
      
    }
}
