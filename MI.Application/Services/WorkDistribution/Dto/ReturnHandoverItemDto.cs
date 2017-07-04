using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{
    public class ReturnHandoverItemDto
    {
        public DateTime? f_StartDate { get; set; }
        public DateTime? f_EndDate { get; set; }

        public string f_chineseName { get; set; }

        public List<ReturnHandover> b { get; set; }
        public ReturnHandoverItemDto a { get; set; }
    }
}
