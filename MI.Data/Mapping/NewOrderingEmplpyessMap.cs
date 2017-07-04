using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Data.Mapping
{
     public class NewOrderingEmplpyessMap: EntityTypeConfiguration<NewOrderingEmployees>
    {
        public NewOrderingEmplpyessMap()
        {
            this.ToTable("t_NewOrderingEmployees");
            this.HasKey(j => j.f_efID);
        }
    }
}
