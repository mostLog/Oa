using MI.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace MI.Data.Mapping
{
    public class STypeMap: EntityTypeConfiguration<SType>
    {
        public STypeMap()
        {
            this.ToTable("t_sType");
            this.HasKey(c => c.f_tID);
            this.HasMany(c => c.EmployeePeriodType).WithOptional(c => c.STypePeriodType).HasForeignKey(c => c.f_periodType_tID);
            this.HasMany(c => c.EmployeeDepartment).WithRequired(c => c.STypeDepartment).HasForeignKey(c => c.f_department_tID);//部门
            this.HasMany(c => c.EmployeeWorkStatus).WithOptional(c => c.STypeWorkStatus).HasForeignKey(c => c.f_workStatus_tID);
            this.HasMany(c => c.EmployeeWorkLocation).WithOptional(c => c.STypeWorkLocation).HasForeignKey(c => c.f_workLocation_tID);
            this.HasMany(c => c.EmployeeLevel).WithRequired(c => c.STypeLevel).HasForeignKey(c => c.f_level_tID);
            this.HasMany(c => c.EmployeeCarType).WithRequired(c=>c.t_sType).HasForeignKey(c=>c.f_CarType);
            this.HasMany(c => c.t_employeeInfo).WithOptional(c => c.t_sType).HasForeignKey(c => c.f_periodType_tID);//班别
            this.HasMany(c=>c.Outside).WithRequired(c=>c.t_sType).HasForeignKey(c=>c.f_DeptId);//关联部门
            this.HasMany(c => c.CompanyOfFoods).WithRequired(c => c.SType).HasForeignKey(c=>c.f_type_tID);//
        }
    }
}
