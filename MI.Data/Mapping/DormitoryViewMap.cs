using MI.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace MI.Data.Mapping
{
    public class DormitoryViewMap:EntityTypeConfiguration<t_LatticeContent>
    {
        //宿舍格子表
        public DormitoryViewMap() {

            this.ToTable("t_LatticeContent");
            this.HasKey(j => j.f_LId);
        }
    }
}
