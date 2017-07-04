using MI.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace MI.Data.Mapping
{
    public class CashRegisterMap: EntityTypeConfiguration<CashRegister>
    {
        public CashRegisterMap()
        {
            this.ToTable("t_CashRegister");
            this.HasKey(c=>c.f_CId);
        }
    }
}
