using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{
   public class CarRegisterDto
    { 
        public int f_ID { get; set; }
        public int f_CarType { get; set; }
        public string f_CarBrand { get; set; }
        public string f_CarOwner { get; set; }
        public Nullable<System.DateTime> f_PurchaseDate { get; set; }
        public bool f_IsIssued { get; set; }
        public Nullable<System.DateTime> f_IssuedDate { get; set; }
        public string f_CarNO { get; set; }

        public virtual SType t_sType { get; set; }
    }
}
