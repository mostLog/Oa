using MI.Core.Domain.GetExtendProperByTabName_Result;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
