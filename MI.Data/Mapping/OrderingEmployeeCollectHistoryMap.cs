using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Data.Mapping
{
    public class OrderingEmployeeCollectHistoryMap: EntityTypeConfiguration<OrderingEmployeeCollectHistory>
    {
        public OrderingEmployeeCollectHistoryMap()
        {
            this.ToTable("t_OrderingEmployeeCollectHistory");
            this.HasKey(c=>c.f_id);
        }
    }
}
