using MI.Application.Dto;
using MI.Core.Domain;
using MI.Data;
using NUnit.Framework;
using System.Collections.Generic;
namespace MI.Application.Test
{
    public class WorkDistribution_Test : TestBase
    {
        private IWorkDistributionService _sTypeService;
        public override void SetUp()
        {
            base.SetUp();
            //_sTypeService = new WorkDistributionService(new BaseRepository<WorkDistribution>(base._context));
        }
        [Test]
        public void GetWorkDistribution()
        {
            //IList<WorkDistribution> list = _sTypeService.GetWorkDistribution();

            //if (list.Count == 0)
            //{
            //    Assert.Fail();
            //}
        }
    }
}
