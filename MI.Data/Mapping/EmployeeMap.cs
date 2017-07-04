using MI.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace MI.Data.Mapping
{
    public class EmployeeMap : EntityTypeConfiguration<Employee>
    {
        public EmployeeMap()
        {
            this.ToTable("t_employeeInfo");
            this.HasKey(c => c.f_eid);
            this.HasMany(c => c.EmpRents).WithRequired(c => c.t_employeeInfo).HasForeignKey(c => c.f_eid);
            this.HasMany(c => c.WorkDistribution).WithRequired(c => c.t_employeeInfo).HasForeignKey(c => c.f_Treat);
            this.HasMany(c => c.t_grant).WithRequired(c => c.t_employeeInfo).HasForeignKey(c => c.f_eid);
            this.HasMany(c => c.t_ReturnTicket).WithRequired(c => c.t_employeeInfo).HasForeignKey(c => c.f_eId);
            this.HasMany(c => c.ReturnHandover).WithRequired(c => c.t_employeeInfo).HasForeignKey(c => c.f_eid);
            this.HasMany(c => c.OrderingEmployees).WithRequired(c => c.t_Employee).HasForeignKey(c => c.f_eID);
            this.HasMany(c => c.NewOrderingEmoloyees).WithRequired(c => c.t_employeeInfo).HasForeignKey(c => c.f_eID);
            this.HasMany(c => c.ChangeRoom).WithRequired(c => c.t_employeeInfo).HasForeignKey(c => c.f_eid);
            this.HasMany(c => c.FlightFee).WithRequired(c => c.t_employeeInfo).HasForeignKey(c => c.f_eid);
            this.HasMany(c=>c.Outside).WithRequired(c=>c.t_employeeInfo).HasForeignKey(c=>c.f_eid);
            this.HasMany(c => c.Outside1).WithRequired(c => c.t_employeeInfo1).HasForeignKey(c => c.f_LeadId);
        }
    }
}
