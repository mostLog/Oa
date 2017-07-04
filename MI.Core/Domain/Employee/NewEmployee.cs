using System;
using System.Collections.Generic;

namespace MI.Core.Domain
{
    /// <summary>
    /// 新人
    /// </summary>
    public class NewEmployee
    {
        public NewEmployee()
        {
            this.Employees = new HashSet<Employee>();
        }

        public int f_nID { get; set; }
        public Nullable<System.DateTime> f_flightDate { get; set; }
        public Nullable<int> f_flightTypt_tID { get; set; }
        public Nullable<System.DateTime> f_flightArrivalTime { get; set; }
        public Nullable<int> f_LoanAmount { get; set; }
        public Nullable<int> f_GiftCard { get; set; }
        public Nullable<int> f_TelCatd { get; set; }
        public Nullable<bool> f_signature { get; set; }
        public string f_Remark { get; set; }
        public string f_TrainingDirector { get; set; }
        public Nullable<bool> f_isInform { get; set; }
        public Nullable<System.DateTime> f_RegistrationTime { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
