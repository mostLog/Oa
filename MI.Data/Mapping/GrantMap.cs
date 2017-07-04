using MI.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace MI.Data.Mapping
{
    public class GrantMap: EntityTypeConfiguration<Grant>
    {
        public GrantMap()
        {
            this.ToTable("t_Grant");
            this.HasKey(c=>c.f_GrantId);
            //this.HasOptional(c => c.t_employeeInfo).WithMany(c => c.t_grant).HasForeignKey(c => c.f_eid);
        }
    }
}
