using MI.Core.Domain;
using MI.Data;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Test
{
    public class STypeService_Test : TestBase
    {
        private ISTypeService _sTypeService;
        public override void SetUp()
        {
            base.SetUp();
            //_sTypeService = new STypeService(new BaseRepository<SType>(base._context));
        }
        [Test]
        public void GetsTypeByWhere()
        {
            IList<SType> list=_sTypeService.GetsTypeByWhere(0);

            if (list.Count==0)
            {
                Assert.Fail();
            }
        }
    }
}
