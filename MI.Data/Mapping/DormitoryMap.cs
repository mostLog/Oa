using MI.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace MI.Data.Mapping
{
    /// <summary>
    ///宿舍登记配置
    /// </summary>
    public class DormitoryMap : EntityTypeConfiguration<t_Dormitory>
    {
        public DormitoryMap()
        {
            this.ToTable("t_dormitory");
            this.HasKey(j => j.f_DormitoryId);
            this.HasMany(c => c.t_employeeInfo).WithOptional(c => c.t_Dormitory).HasForeignKey(c => c.f_dormitoryId);
            this.HasMany(c=>c.t_ChangeRoom).WithOptional(c=>c.t_dormitory).HasForeignKey(c=>c.f_NewRoomId);
            this.HasMany(c => c.t_ChangeRoom1).WithOptional(c => c.t_dormitory1).HasForeignKey(c => c.f_OldRoomID);
            this.HasMany(c => c.t_Outside).WithOptional(c=>c.t_dormitory).HasForeignKey(c=>c.f_DormitoryId);
            this.HasMany(c=>c.t_HostelClean).WithRequired(c=>c.t_dormitory).HasForeignKey(c=>c.f_DormitoryId);
        }
    }
}
