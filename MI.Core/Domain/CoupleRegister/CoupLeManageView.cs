using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Domain
{
    /// <summary>
    /// 情侣关系 试图模型
    ///  显示Name
    /// </summary>
    public class CoupLeManageView
    {
        public int iCid { get; set; }

        public int iEid1 { get; set; }
        public int iEid2 { get; set; }
        public string ChineseName1 { get; set; }
        public string ChineseName2 { get; set; }
        
    }
}
