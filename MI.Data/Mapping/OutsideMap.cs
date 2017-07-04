
using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Data.Mapping
{
    /// <summary>
    /// 员工外租配置
    /// </summary>
    public class OutsideMap : EntityTypeConfiguration<t_Outside>
    {
        /// <summary>
        /// 映射获得表数据
        /// </summary>
        public OutsideMap()
        {
            this.ToTable("t_Outside");
            this.HasKey(l => l.f_Id);

        }
    }
}
