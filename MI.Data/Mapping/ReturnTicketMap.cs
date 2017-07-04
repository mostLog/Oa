using MI.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace MI.Data.Mapping
{
    public class ReturnTicketMap : EntityTypeConfiguration<ReturnTicket>
    {
        public ReturnTicketMap()
        {
            this.ToTable("t_ReturnTicket");
            this.HasKey(c => c.f_Id);
        }
    }
}
