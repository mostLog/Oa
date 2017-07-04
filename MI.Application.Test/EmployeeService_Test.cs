using MI.Application;
using MI.Core.Domain;
using MI.Data;
using NUnit.Framework;
using System;

namespace MI.Application.Test
{
    public class EmployeeService_Test: TestBase
    {
        private IEmployeeService _employeeService;
        public override void SetUp()
        {
            base.SetUp();
            //_employeeService = new EmployeeService(new BaseRepository<Employee>(base._context));
        }

        [Test]
        public void GetEmployeeById()
        {

            string t = "9:00";
            var d= DateTime.Parse(t);

           
        }
    }
}
