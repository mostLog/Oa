using MI.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace MI.Data.Mapping
{
    public class GetExtProByTNameMap :EntityTypeConfiguration<GetExtendProperByTabName_Result>
    {
        public GetExtProByTNameMap()
        {
            this.ToTable("GetExtendProperByTabName_Result");
            this.HasKey(m => m.colid);
        }
    }
}
