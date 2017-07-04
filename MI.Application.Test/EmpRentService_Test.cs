using MI.Application.Dto;
using MI.Core.Domain;
using MI.Data;
using NUnit.Framework;
using System.Collections.Generic;

namespace MI.Application.Test
{
    public class EmpRentService_Test : TestBase
    {
        //private IEmpRentService _employeeService; 
        private IReturnTicketService _returnTicketService;
        public override void SetUp()
        {
            base.SetUp();
            //_returnTicketService = new ReturnTicketService(new BaseRepository<ReturnTicket>(base._context));
        }

        [Test]
        public void GetEmployeeById()
        {
            //int count = 0;
            try
            {
                //int iPageIndex = 1;
                //int iPageSize = 15;
                //IList<EmpRentDto>  employee = _employeeService.GetEmpRent();
                //var list = _returnTicketService.GetSendCarByWhere(sc, iPageIndex, iPageSize);
                //IList<EmpRentDto> employee = _employeeService.GetEmpRentByWhere(iPageIndex, iPageSize,out count);
                //if (employee.Count > 0)
                //{
                //    Assert.Pass("Your first passing test");
                //}                
                //SType stype = employee.SType;
                //if (stype != null)
                //{
                //    Assert.Pass("Your first passing test");
                //}
                //SType stype = employee.SType;
                //if (stype != null)
                //{
                //    Assert.Pass("Your first passing test");
                //}
            }
            catch (System.Exception)
            {

                throw;
            }


        }
    }
}
