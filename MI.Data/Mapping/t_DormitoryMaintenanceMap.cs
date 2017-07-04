using MI.Core;
using MI.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace MI.Data.Mapping
{
    /// <summary>
    /// 宿舍维修配置
    /// </summary>
    public class t_DormitoryMaintenanceMap : EntityTypeConfiguration<DormitoryMaintenance>
    {
        public t_DormitoryMaintenanceMap()
        {
            this.ToTable("t_DormitoryMaintenance");
            this.HasKey(c => c.f_Id);
        }
    }
}
