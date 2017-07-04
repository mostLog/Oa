using MI.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace MI.Data.Mapping
{
    /// <summary>
    ///宿舍登记配置
    /// </summary>
    public class DormitoryMap : EntityTypeConfiguration<Dormitory>
    {
        public DormitoryMap()
        {
            this.ToTable("Dormitory");
            this.HasKey(j => j.f_DormitoryId);
            this.HasMany(c => c.t_employeeInfo).WithOptional(c => c.Dormitory).HasForeignKey(c => c.f_dormitoryId);
            this.HasMany(c=>c.ChangeRoom).WithOptional(c=>c.Dormitory).HasForeignKey(c=>c.f_NewRoomId);
            this.HasMany(c => c.ChangeRoom1).WithOptional(c => c.Dormitory1).HasForeignKey(c => c.f_OldRoomID);
            this.HasMany(c => c.Outside).WithOptional(c=>c.Dormitory).HasForeignKey(c=>c.f_DormitoryId);
            this.HasMany(c=>c.HostelClean).WithRequired(c=>c.Dormitory).HasForeignKey(c=>c.f_DormitoryId);
        }
    }
}
