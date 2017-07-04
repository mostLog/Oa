using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Domain
{
    /// <summary>
    /// 新人订餐类
    /// 创建人：吕秀峰
    /// </summary>
    public class NewOrderingEmployees
    {
        public int f_efID { get; set; }
        public Nullable<int> f_eID { get; set; }
        public string f_Community { get; set; }
        public string f_Building { get; set; }
        public string f_RoomNo { get; set; }
        public Nullable<int> f_LDM_tID { get; set; }
        public Nullable<int> f_type_tID { get; set; }
        public bool f_IsValid { get; set; }
        public int f_Quantity { get; set; }
        public Nullable<System.DateTime> f_EffectiveDate { get; set; }

        public virtual Employee t_employeeInfo { get; set; }
    }
}
