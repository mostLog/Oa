using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Data.Mapping
{
    public class CoupleRegisterMap: EntityTypeConfiguration<CoupleRegister>
    {
        public CoupleRegisterMap()
        {
            this.ToTable("CoupleRegister");
            this.HasKey(c=>c.f_cId);
        }
    }
}
