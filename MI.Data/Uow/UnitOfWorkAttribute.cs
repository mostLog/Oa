using Autofac.Extras.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Core;

namespace MI.Data.Uow
{
    public class UnitOfWorkAttribute : InterceptAttribute
    {
        public UnitOfWorkAttribute(Service interceptorService) : base(interceptorService)
        {

        }
        public UnitOfWorkAttribute(string interceptorServiceName= "UnitOfWork") : base(interceptorServiceName)
        {
        }

        public UnitOfWorkAttribute(Type interceptorServiceType) : base(interceptorServiceType)
        {

        }
    }
}
