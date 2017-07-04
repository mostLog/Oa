using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.ChangeRoomService.Dto
{
    public class t_ChangeRoomDto
    {
        public int f_Id { get; set; }
        public int f_eid { get; set; }
        public System.DateTime f_FilingDate { get; set; }
        public Nullable<int> f_OldRoomID { get; set; }
        public Nullable<int> f_NewRoomId { get; set; }
        public string f_Remarks { get; set; }
        public string f_Progress { get; set; }
        public string f_Registrant { get; set; }
        public decimal f_SewRent { get; set; }
        public System.DateTime f_RentDate { get; set; }
        public string f_operator { get; set; }
        public Nullable<System.DateTime> f_operatorTime { get; set; }
        public Nullable<System.DateTime> f_EffectiveMonths { get; set; }
        public virtual Employee t_employeeInfo { get; set; }
        public virtual t_Dormitory t_dormitory { get; set; }
        public virtual t_Dormitory t_dormitory1 { get; set; }
    }
}
