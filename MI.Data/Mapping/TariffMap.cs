using MI.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace MI.Data.Mapping
{
    public class TariffMap : EntityTypeConfiguration<Tariff>
    {
        /// <summary>
        /// 映射获得表数据
        /// </summary>
        public TariffMap()
        {
            this.ToTable("t_Tariff");
            this.HasKey(l => l.f_Id);

        }
    }
}
