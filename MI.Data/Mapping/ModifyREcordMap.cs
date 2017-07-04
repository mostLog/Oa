using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Data.Mapping
{
   public class ModifyREcordMap : EntityTypeConfiguration<t_ModifyRecord>
    {
        public ModifyREcordMap()
        {
            this.ToTable("t_ModifyRecord");
            this.HasKey(m=>m.f_Id);

        }
    }
}
