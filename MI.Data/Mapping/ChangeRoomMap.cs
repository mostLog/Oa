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
    /// 员工换房配置
    /// </summary>
   public class ChangeRoomMap : EntityTypeConfiguration<t_ChangeRoom>
    {
        /// <summary>
        /// 映射获得表数据
        /// </summary>
        public ChangeRoomMap()
        {
            this.ToTable("t_ChangeRoom");
            this.HasKey(l => l.f_Id);

        }
    }
}
