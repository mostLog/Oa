
using MI.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace MI.Data.Mapping
{
    /// <summary>
    /// 员工外租配置
    /// </summary>
    public class OutsideMap : EntityTypeConfiguration<Outside>
    {
        /// <summary>
        /// 映射获得表数据
        /// </summary>
        public OutsideMap()
        {
            this.ToTable("Outside");
            this.HasKey(l => l.f_Id);

        }
    }
}
