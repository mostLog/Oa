using MI.Data;
using NUnit.Framework;
using System.Reflection;
using System.Configuration;
using MI.Application.Mappers;

namespace MI.Application.Test
{
    [TestFixture]
    public class TestBase
    {
        public OADbContext _context;

        [SetUp]
        public virtual void SetUp()
        {
            _context = new OADbContext(GetTestDbName());
            new ApplicationMapperConfiguraton().Initialize();
        }
      
        protected string GetTestDbName()
        {
            
            return ConfigurationManager.ConnectionStrings["Default"].ToString();
        }

    }
}
