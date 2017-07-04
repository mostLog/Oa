using MI.Core.Domain;
using System.Data.Entity.ModelConfiguration;
namespace MI.Data.Mapping
{ 
   public class CarRegisterMap: EntityTypeConfiguration<CarRegister>
    {
        /// <summary>
        /// 映射获得表数据
        /// </summary>
        public CarRegisterMap()
        {
            this.ToTable("t_CarRegister");
            this.HasKey(c => c.f_ID);

        }
    }
}
