using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Data.Mapping
{
    public class NoticeMap : EntityTypeConfiguration<Notice>
    {
        public NoticeMap()
        {
            this.ToTable("t_Notice");
            this.HasKey(c => c.f_Id);
        }
    }
}
