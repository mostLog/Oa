using MI.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace MI.Data.Mapping
{
    public class ReturnHandoverMap : EntityTypeConfiguration<ReturnHandover>
    {
        public ReturnHandoverMap()
        {
            this.ToTable("t_ReturnHandover");
            this.HasKey(c => c.f_Id);
        }
    }
}
