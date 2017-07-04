using MI.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace MI.Data.Mapping
{
    public class CompanyOfFoodMap: EntityTypeConfiguration<CompanyOfFood>
    {
        public CompanyOfFoodMap()
        {
            this.ToTable("t_CompanyOfFood");
            this.HasKey(m=>m.f_cID);
        }
    }
}
