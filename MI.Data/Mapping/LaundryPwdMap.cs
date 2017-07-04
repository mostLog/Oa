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
    /// 洗衣房配置
    /// </summary>
   public class LaundryPwdMap: EntityTypeConfiguration<t_LaundryPwd>
    {
        /// <summary>
        /// 映射获得表数据
        /// </summary>
        public LaundryPwdMap()
        {
            this.ToTable("t_LaundryPwd");
            this.HasKey(l => l.f_Id);
          //  this.HasMany(l => l.Dormitory).WithRequired(l => l.t_pwd).HasForeignKey(l=> l.f_LaundryAndPwd);

        }
    }
}
