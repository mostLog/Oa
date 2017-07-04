using MI.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace MI.Data.Mapping
{
    public class DormitoryViewMap:EntityTypeConfiguration<LatticeContent>
    {
        //宿舍格子表
        public DormitoryViewMap() {

            this.ToTable("LatticeContent");
            this.HasKey(j => j.f_LId);
        }
    }
}
