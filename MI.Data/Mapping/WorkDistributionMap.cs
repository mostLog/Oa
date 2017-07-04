using MI.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace MI.Data.Mapping
{
    public class WorkDistributionMap : EntityTypeConfiguration<WorkDistribution>
    {
        /// <summary>
        /// 工作交接
        /// </summary>
        public WorkDistributionMap()
        {
            this.ToTable("t_WorkDistribution");
            this.HasKey(c => c.f_Id);
        }
    }
}
