using MI.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace MI.Data.Mapping
{
    public class BedLinenMap : EntityTypeConfiguration<BedLinen>
    {
        public BedLinenMap()
        {
            this.ToTable("t_BedLinen");
            this.HasKey(c => c.f_Id);
        }
    }
}
