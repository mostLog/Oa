using MI.Core.Domain;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Data.Mapping
{
    public class EmpRentMap : EntityTypeConfiguration<EmpRent>
    {
        public EmpRentMap() 
        {
            this.ToTable("t_EmpRent");
            this.HasKey(c => c.f_Id);
            
        }
    }
}
