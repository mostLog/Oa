using MI.Core;
using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;

namespace MI.Data.Mapping
{
    /// <summary>
    /// 宿舍打扫配置
    /// </summary>
    public class t_HostelCleanMap:EntityTypeConfiguration<t_HostelClean>
    {
        public t_HostelCleanMap()
        {
            this.ToTable("t_HostelClean");
            this.HasKey(c => c.f_Id);
        }
    }
}
