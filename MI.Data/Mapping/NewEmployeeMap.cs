using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Data.Mapping
{
    public class NewEmployeeMap: EntityTypeConfiguration<NewEmployee>
    {
        public NewEmployeeMap()
        {
            this.ToTable("t_newEmployeeInfo");
            this.HasKey(c=>c.f_nID);
            this.HasMany(c => c.Employees).WithOptional(c => c.NewEmployee).HasForeignKey(c => c.f_nid);

        }
    }
}
